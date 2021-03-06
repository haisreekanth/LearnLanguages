﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.Common;

namespace LearnLanguages.DataAccess.Mock
{
  public class CustomIdentityDal : ICustomIdentityDal
  {
    //private Guid _TestValidUserId = new Guid("89991D3B-0435-4167-8691-455D3D5000BC");
    private Guid _TestValidUserId = SeedData.Ton.DefaultTestValidUserId;
    private string _TestValidUsername = "user";
    private static Guid _TestRoleId = new Guid("4E7DACEC-2EE7-4201-8657-694D51AA0487");
    private RoleDto _TestRole = new RoleDto()
    {
      Id = _TestRoleId,
      Text = DalResources.RoleAdmin
    };
    private string _TestValidPassword = "password";
    private string _TestSaltedHashedPassword = @"瞌訖ꎚ壿喐ຯ缟㕧";
    private int _TestSalt = -54623530;
    private string _TestInvalidUsername = "ImNotAValidUser";
    private string _TestInvalidPassword = "ImNotAValidPassword";

    /// <summary>
    /// Returns Success(true) if verify user is valid, Success(false) if invalid. Throws exceptions if something bad happens.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public Result<bool?> VerifyUser(string username, string password)
    {
      //old
      //return Result<bool?>.Success(username == _TestValidUsername && password == _TestValidPassword);
      Result<bool?> retResult = Result<bool?>.Undefined(null);
      try
      {
        var results = from u in SeedData.Ton.Users
                      where u.Username == username
                      select u;

        if (results.Count() == 1)
        {
          //USERNAME FOUND. CHECK PASSWORD
          var userDto = results.First();
          SaltedHashedPassword saltedHashedPasswordObj = 
            new SaltedHashedPassword(password, userDto.Salt);
          if (string.Compare(userDto.SaltedHashedPasswordValue,
                             saltedHashedPasswordObj.Value,
                             StringComparison.InvariantCulture) == 0)
          {
            //PASSWORDS MATCH
            retResult = Result<bool?>.Success(true);
          }
          else
          {
            //PASSWORDS DO *NOT* MATCH
            retResult = Result<bool?>.Success(false);
          }
        }
        else if (results.Count() == 0)
        {
          //USERNAME NOT FOUND.
          retResult = Result<bool?>.Success(false);
        }
        else
        {
          //?? VERY BAD EXCEPTION. MULTIPLE USERS WITH THAT USERNAME FOUND?
          throw new Exceptions.VeryBadException();
        }

      }
      catch (Exception ex)
      {
        retResult = Result<bool?>.FailureWithInfo(null, ex);
      }

      return retResult;
    }
    public Result<UserDto> GetUser(string username)
    {
      //if (username != _TestValidUsername)
      //  throw new GeneralDataAccessException("GetUser(username) not found");
      var results = from u in SeedData.Ton.Users
                    where u.Username == username
                    select u;

      if (results.Count() == 1)
        return Result<UserDto>.Success(results.First());
      else if (results.Count() == 0)
        throw new Exceptions.UsernameNotFoundException(username);
      else
        throw new Exceptions.VeryBadException();

      //UserDto dto = new UserDto()
      //{
      //  Id = SeedData.Ton.DefaultTestValidUserId,//_TestValidUserId,
      //  Salt = SeedData.Ton.TestSalt, //_TestSalt
      //  SaltedHashedPasswordValue = SeedData.Ton.TestSaltedHashedPassword,// _TestSaltedHashedPassword,
      //  Username = SeedData.Ton.TestValidUsername//_TestValidUsername
      //};
      //return Result<UserDto>.Success(dto);
    }
    public Result<ICollection<RoleDto>> GetRoles(string username)
    {
      Result<ICollection<RoleDto>> retResult = Result<ICollection<RoleDto>>.Undefined(null);
      try
      {
        var roleDtos = new List<RoleDto>();

        var userResults = (from user in SeedData.Ton.Users
                           where user.Username == username
                           select user);

        if (userResults.Count() == 1)
        {
          var roleIds = userResults.First().RoleIds.ToList();
          for (int i = 0; i < roleIds.Count; i++)
          //foreach (var roleId in roleIds)
          {
            var roleId = roleIds[i];
            var roleResults = (from role in SeedData.Ton.Roles
                               where role.Id == roleId
                               select role);
            if (roleResults.Count() == 1)
            {
              roleDtos.Add(roleResults.First());
            }
            else if (roleResults.Count() == 0)
              throw new Exceptions.IdNotFoundException(roleId);
            else
              throw new Exceptions.VeryBadException();
          }
        }
        else if (userResults.Count() == 0)
          throw new Exceptions.UsernameNotFoundException(username);
        else
          throw new Exceptions.VeryBadException();

        retResult = Result<ICollection<RoleDto>>.Success(roleDtos);
      }
      catch (Exception ex)
      {
        retResult = Result<ICollection<RoleDto>>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
    public Result<UserDto> AddUser(string newUsername, string password)
    {
      Result<UserDto> retResult = Result<UserDto>.Undefined(null);
      try
      {
        //VALIDATE WE ARE IN ROLE TO ADD A USER
        bool isInRoleToAddUser = DalHelper.IsInRoleToAddUser();
        if (!isInRoleToAddUser)
          throw new DataAccess.Exceptions.UserNotAuthorizedException(DalResources.ErrorMsgAttemptedToAddUser, 0);

        //VALIDATE USERNAME
        bool usernameIsValid = CommonHelper.UsernameIsValid(newUsername);
        if (!usernameIsValid)
          throw new DataAccess.Exceptions.InvalidUsernameException(newUsername);

        //VALIDATE USER DOESN'T ALREADY EXIST
        if (SeedData.Ton.ContainsUsername(newUsername))
          throw new DataAccess.Exceptions.UsernameAlreadyExistsException(newUsername);
        
        //VALIDATE PASSWORD
        bool passwordIsValid = CommonHelper.PasswordIsValid(password);
        if (!passwordIsValid)
          throw new DataAccess.Exceptions.InvalidPasswordException(password);

        //GENERATE UNIQUE SALT 
        bool saltAlreadyExists = true;
        int salt = -1;
        Random r = new Random(DateTime.Now.Millisecond * DateTime.Now.Minute * DateTime.Now.Month);
        int maxTries = int.Parse(DalResources.MaxTriesGenerateSalt);
        int tries = 0;
        do
        {
          salt = r.Next(int.Parse(DataAccess.DalResources.MaxSaltValue));
          saltAlreadyExists = SeedData.Ton.ContainsSalt(salt);
          tries++;
          if (tries > maxTries)
            throw new DataAccess.Exceptions.GeneralDataAccessException("MaxTries for generating salt reached.");
        } while (saltAlreadyExists);

        //GENERATE SALTEDHASHEDPASSWORD
        var saltedHashedPasswordObj = new Common.SaltedHashedPassword(password, salt);
        string saltedHashedPasswordString = saltedHashedPasswordObj.Value;

        //GET ROLEID FOR PLAIN USER (NOT ADMIN)
        var roleId = SeedData.Ton.UserRoleId;

        //CREATE ACTUAL USERDTO
        UserDto newUserDto = new UserDto()
        {
          Id = Guid.NewGuid(),
          Username = newUsername,
          Salt = salt,
          SaltedHashedPasswordValue = saltedHashedPasswordString,
          RoleIds = new List<Guid>() { roleId }
        };

        //ADD THE USER TO THE SEEDDATA (WE ARE IN THE MOCK DAL)
        SeedData.Ton.Users.Add(newUserDto);

        //ASSIGN SUCCESFUL RESULT WITH USERDTO
        retResult = Result<UserDto>.Success(newUserDto);
      }
      catch (Exception ex)
      {
        //WRAP EXCEPTION IN FAILURE WITH INFO RESULT
        retResult = Result<UserDto>.FailureWithInfo(null, ex);
      }

      //RETURN RESULT
      return retResult;
    }
    public Result<bool?> DeleteUser(string username)
    {
      Result<bool?> retResult = Result<bool?>.Undefined(null);
      try
      {
        //VALIDATE WE ARE IN ROLE TO ADD A USER
        bool isInRoleToAddUser = DalHelper.IsInRoleToAddUser();
        if (!isInRoleToAddUser)
          throw new DataAccess.Exceptions.UserNotAuthorizedException(DalResources.ErrorMsgAttemptedToDeleteUser, 0);

        bool containsUsername = false;
        var results = from user in SeedData.Ton.Users
                      where user.Username == username
                      select user;

        if (results.Count() == 1)
        {
          //return Result<bool?>.Success(results.First());
          var userToRemove = results.First();
          SeedData.Ton.Users.Remove(userToRemove);
          retResult = Result<bool?>.Success(true);
        }
        else if (results.Count() == 0)
          throw new Exceptions.UsernameNotFoundException(username);
        else
          throw new Exceptions.VeryBadException();
      }
      catch (Exception ex)
      {
        //WRAP EXCEPTION IN FAILURE WITH INFO RESULT
        retResult = Result<bool?>.FailureWithInfo(null, ex);
      }

      //RETURN RESULT
      return retResult;
    }
  }
}


