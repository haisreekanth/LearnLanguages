using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnLanguages.Common;

namespace LearnLanguages.DataAccess.Ef
{
  public class CustomIdentityDal : CustomIdentityDalBase
  {
    protected override bool? VerifyUserImpl(string username, string password)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from userData in ctx.ObjectContext.UserDatas
                      where userData.Username == username
                      select userData;

        if (results.Count() == 1)
        {
          var user = results.First();
          var authenticated = (SaltedHashedPassword.GetHashedPasswordValue(password, user.Salt) == user.SaltedHashedPasswordValue);

          //RETURNS SUCCESS IF VALIDATION IS AUTHENTICATED OR NOT.  DOES *NOT* THROW EXCEPTION
          //IF CREDENTIALS ARE INVALID.
          return authenticated;
        }
        else
        {
          if (results.Count() == 0)
            return false;  //FALSE BECAUSE USER NOT FOUND
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple users with the same username.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException, 
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }

    protected override UserDto GetUserImpl(string username)
    {
      int maxTries = int.Parse(EfResources.MaxDeadlockAttempts);
      for (int i = 0; i < maxTries; i++)
      {
        try
        {
          using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
          {
            var results = from userData in ctx.ObjectContext.UserDatas
                          where userData.Username == username
                          select userData;

            if (results.Count() == 1)
            {
              var userDto = EfHelper.ToDto(results.First());
              return userDto;
            }
            else if (results.Count() == 0)
            {
              return null;
            }
            else
            {
#if DEBUG
              System.Diagnostics.Debugger.Break();
#endif

              //RESULTS.COUNT IS NOT ONE OR ZERO.  EITHER IT'S NEGATIVE, WHICH WOULD BE FRAMEWORK ABSURD, OR ITS MORE THAN ONE,
              //WHICH MEANS THAT WE HAVE MULTIPLE USERS WITH THE SAME USERNAME.  THIS IS VERY BAD.
              var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                           DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
              throw new Exceptions.VeryBadException(errorMsg);
            }
          }
        }
        catch (Exception ex)
        {
          if (ex is System.Data.EntityCommandExecutionException && 
              ex.InnerException is System.Data.SqlClient.SqlException &&
              ex.InnerException.Message.Contains("Rerun the transaction"))
          {
            //"Transaction (Process ID 55) was deadlocked on lock resources with another process and has been chosen as the deadlock victim. Rerun the transaction."
            //DO NOTHING IF THE ERROR IS A DEADLOCK. WE HAVE THIS IN A FOR LOOP THAT WILL RETRY UP TO A MAX NUMBER OF ATTEMPTS
          }
          else
          {
            System.Diagnostics.Debugger.Break();
            //RETHROW THIS EXCEPTION
            throw;
          }
        }
      }

      //IF WE REACH THIS POINT, THEN WE HAVE TRIED OUR MAX TRIES AT BREAKING A SQL DEADLOCK.
#if DEBUG
      //if (retRoles == null)
      System.Diagnostics.Debugger.Break();
#endif
      var errorMsg2 = string.Format(DalResources.ErrorMsgVeryBadException,
                                   DalResources.ErrorMsgVeryBadExceptionDetail_DeadlockRetriesExceededMaxTries);
      throw new Exceptions.VeryBadException(errorMsg2);
    }

    protected override ICollection<RoleDto> GetRolesImpl(string username)
    {
      try
      {

      //GET USER
      var userDto = GetUserImpl(username);
      if (username == null)
        throw new Exceptions.UsernameNotFoundException(username);
      ICollection<RoleDto> retRoles = new List<RoleDto>();
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        //GET ROLES FOR THAT USER
        var results = from roleData in ctx.ObjectContext.RoleDatas
                      where userDto.RoleIds.Contains(roleData.Id)
                      select roleData;

        foreach (var roleData in results)
        {
          var dto = EfHelper.ToDto(roleData);
          retRoles.Add(dto);
        }
      }

#if DEBUG
      if (retRoles == null)
        System.Diagnostics.Debugger.Break();
#endif
       
        return retRoles;
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debugger.Break();
        
        throw;
      }
    }

    protected override UserDto AddUserImpl(string username, string password)
    {
      //RECHECK AUTHORIZATION.
      DalHelper.CheckAuthorizationToAddUser();

      //THIS FOR LOOP IS FOR RETRYING DUE TO EF DB DEADLOCK
      int maxTries = int.Parse(EfResources.MaxDeadlockAttempts);
      for (int i = 0; i < maxTries; i++)
      {
        try
        {
          using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
          {
            //VALIDATE USERNAME
            bool usernameIsValid = CommonHelper.UsernameIsValid(username);
            if (!usernameIsValid)
              throw new DataAccess.Exceptions.InvalidUsernameException(username);

            //VALIDATE USER DOESN'T ALREADY EXIST
            var alreadyExists = (from userData in ctx.ObjectContext.UserDatas
                                 where userData.Username == username
                                 select userData).Count() > 0;
            if (alreadyExists)
              throw new DataAccess.Exceptions.UsernameAlreadyExistsException(username);

            //VALIDATE PASSWORD
            bool passwordIsValid = CommonHelper.PasswordIsValid(password);
            if (!passwordIsValid)
              throw new DataAccess.Exceptions.InvalidPasswordException(password);

            //GENERATE UNIQUE SALT 
            bool saltAlreadyExists = true;
            int salt = -1;
            Random r = new Random(DateTime.Now.Millisecond * DateTime.Now.Minute * DateTime.Now.Month);
            int maxSaltTries = int.Parse(DalResources.MaxTriesGenerateSalt);
            int tries = 0;
            do
            {
              salt = r.Next(int.Parse(DataAccess.DalResources.MaxSaltValue));

              saltAlreadyExists = (from userData in ctx.ObjectContext.UserDatas
                                   where userData.Salt == salt
                                   select userData).Count() > 0;

              tries++;
              if (tries > maxSaltTries)
                throw new DataAccess.Exceptions.GeneralDataAccessException("MaxTries for generating salt reached.");
            } while (saltAlreadyExists);

            //GENERATE SALTEDHASHEDPASSWORD
            var saltedHashedPasswordObj = new Common.SaltedHashedPassword(password, salt);
            string saltedHashedPasswordString = saltedHashedPasswordObj.Value;

            //GET ROLEID FOR PLAIN USER (NOT ADMIN)
            var userRoleData = (from roleData in ctx.ObjectContext.RoleDatas
                                where roleData.Text == DalResources.RoleUser
                                select roleData).First();
            var roleId = userRoleData.Id;

            //CREATE NEW USERDATA
            var newUserData = ctx.ObjectContext.UserDatas.CreateObject();

            //ASSIGN PROPERTIES
            newUserData.Username = username;
            newUserData.Salt = salt;
            newUserData.SaltedHashedPasswordValue = saltedHashedPasswordString;
            newUserData.RoleDatas.Add(userRoleData);

            //ADD TO CONTEXT
            ctx.ObjectContext.UserDatas.AddObject(newUserData);

            //SAVE CHANGES
            ctx.ObjectContext.SaveChanges();

            //CREATE RETURN USERDTO
            var retUserDto = EfHelper.ToDto(newUserData);

            //RETURN IT, WE'RE DONE.
            return retUserDto;
          }
        }
        catch (Exception ex)
        {
          if (ex is System.Data.EntityCommandExecutionException &&
              ex.InnerException is System.Data.SqlClient.SqlException &&
              ex.InnerException.Message.Contains("Rerun the transaction"))
          {
            //"Transaction (Process ID 55) was deadlocked on lock resources with another process and has been chosen as the deadlock victim. Rerun the transaction."
            //DO NOTHING IF THE ERROR IS A DEADLOCK. WE HAVE THIS IN A FOR LOOP THAT WILL RETRY UP TO A MAX NUMBER OF ATTEMPTS
          }
          else
          {
            System.Diagnostics.Debugger.Break();
            //RETHROW THIS EXCEPTION
            throw;
          }
        }
      }

      //IF WE REACH THIS POINT, THEN WE HAVE TRIED OUR MAX TRIES AT BREAKING A SQL DEADLOCK.
#if DEBUG
      //if (retRoles == null)
      System.Diagnostics.Debugger.Break();
#endif
      var errorMsg2 = string.Format(DalResources.ErrorMsgVeryBadException,
                                   DalResources.ErrorMsgVeryBadExceptionDetail_DeadlockRetriesExceededMaxTries);
      throw new Exceptions.VeryBadException(errorMsg2);
    }

    protected override bool? DeleteUserImpl(string username)
    {
      //RECHECK AUTHORIZATION
      DalHelper.CheckAuthorizationToDeleteUser();

      //THIS FOR LOOP IS FOR RETRYING DUE TO EF DB DEADLOCK
      int maxTries = int.Parse(EfResources.MaxDeadlockAttempts);
      for (int i = 0; i < maxTries; i++)
      {
        try
        {
          using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
          {
            var results = from userData in ctx.ObjectContext.UserDatas
                          where userData.Username == username
                          select userData;

            if (results.Count() == 1)
            {
              //PERFORM THE DELETE
              var userData = results.First();
              ctx.ObjectContext.UserDatas.DeleteObject(userData);
              
              //SAVE THE CHANGES
              ctx.ObjectContext.SaveChanges();

              //RETURN TRUE TO INDICATE THE USER WAS DELETED
              return true;
            }
            else if (results.Count() == 0)
            {
              throw new Exceptions.UsernameNotFoundException(username);
            }
            else
            {
              //COUNT > 1 OR SOME OTHER PROBLEM. BOTH OF WHICH ARE VERY BAD PROBLEMS.
#if DEBUG
              System.Diagnostics.Debugger.Break();
#endif

              //RESULTS.COUNT IS NOT ONE OR ZERO.  EITHER IT'S NEGATIVE, WHICH WOULD BE FRAMEWORK ABSURD, OR ITS MORE THAN ONE,
              //WHICH MEANS THAT WE HAVE MULTIPLE USERS WITH THE SAME USERNAME.  THIS IS VERY BAD.
              var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                           DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
              throw new Exceptions.VeryBadException(errorMsg);
            }
          }
        }
        catch (Exception ex)
        {
          if (ex is System.Data.EntityCommandExecutionException &&
              ex.InnerException is System.Data.SqlClient.SqlException &&
              ex.InnerException.Message.Contains("Rerun the transaction"))
          {
            //"Transaction (Process ID 55) was deadlocked on lock resources with another process and has been chosen as the deadlock victim. Rerun the transaction."
            //DO NOTHING IF THE ERROR IS A DEADLOCK. WE HAVE THIS IN A FOR LOOP THAT WILL RETRY UP TO A MAX NUMBER OF ATTEMPTS
          }
          else
          {
            System.Diagnostics.Debugger.Break();
            //RETHROW THIS EXCEPTION
            throw;
          }
        }
      }

      //IF WE REACH THIS POINT, THEN WE HAVE TRIED OUR MAX TRIES AT BREAKING A SQL DEADLOCK.
#if DEBUG
      //if (retRoles == null)
      System.Diagnostics.Debugger.Break();
#endif
      var errorMsg2 = string.Format(DalResources.ErrorMsgVeryBadException,
                                   DalResources.ErrorMsgVeryBadExceptionDetail_DeadlockRetriesExceededMaxTries);
      throw new Exceptions.VeryBadException(errorMsg2);
    }

    protected override ICollection<UserDto> GetAllUsersImpl()
    {
      //RECHECK AUTHORIZATION
      DalHelper.CheckAuthorizationToGetAllUsers();

      //THIS FOR LOOP IS FOR RETRYING DUE TO EF DB DEADLOCK
      int maxTries = int.Parse(EfResources.MaxDeadlockAttempts);
      for (int i = 0; i < maxTries; i++)
      {
        try
        {
          using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
          {
            var results = from userData in ctx.ObjectContext.UserDatas
                          select userData;

            if (results.Count() > 0)
            {
              var allUserDatas = results.ToList();
              var allUserDtos = new List<UserDto>();
              for (int j = 0; j < allUserDatas.Count; j++)
              {
                var userDto = EfHelper.ToDto(allUserDatas[j]);
                allUserDtos.Add(userDto);
              }

              return allUserDtos;
            }
            else
            {
              //COUNT == 0 OR SOME OTHER PROBLEM. BOTH OF WHICH ARE VERY BAD PROBLEMS.
#if DEBUG
              System.Diagnostics.Debugger.Break();
#endif

              //RESULTS.COUNT IS NOT ONE OR ZERO.  EITHER IT'S NEGATIVE, WHICH WOULD BE FRAMEWORK ABSURD, OR ITS MORE THAN ONE,
              //WHICH MEANS THAT WE HAVE MULTIPLE USERS WITH THE SAME USERNAME.  THIS IS VERY BAD.
              var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                           DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
              throw new Exceptions.VeryBadException(errorMsg);
            }
          }
        }
        catch (Exception ex)
        {
          if (ex is System.Data.EntityCommandExecutionException &&
              ex.InnerException is System.Data.SqlClient.SqlException &&
              ex.InnerException.Message.Contains("Rerun the transaction"))
          {
            //"Transaction (Process ID 55) was deadlocked on lock resources with another process and has been chosen as the deadlock victim. Rerun the transaction."
            //DO NOTHING IF THE ERROR IS A DEADLOCK. WE HAVE THIS IN A FOR LOOP THAT WILL RETRY UP TO A MAX NUMBER OF ATTEMPTS
          }
          else
          {
            System.Diagnostics.Debugger.Break();
            //RETHROW THIS EXCEPTION
            throw;
          }
        }
      }

      //IF WE REACH THIS POINT, THEN WE HAVE TRIED OUR MAX TRIES AT BREAKING A SQL DEADLOCK.
#if DEBUG
      //if (retRoles == null)
      System.Diagnostics.Debugger.Break();
#endif
      var errorMsg2 = string.Format(DalResources.ErrorMsgVeryBadException,
                                   DalResources.ErrorMsgVeryBadExceptionDetail_DeadlockRetriesExceededMaxTries);
      throw new Exceptions.VeryBadException(errorMsg2);
    }
  }
}
