using System;
using System.Linq;
using System.Collections.Generic;
using Csla.Security;
using Csla;
using Csla.Serialization;
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Business.Security
{
  [Serializable]
  public class UserIdentity : CslaIdentityBase<UserIdentity>
  {
    #region Factory Methods

#if !SILVERLIGHT
    public static UserIdentity GetUserIdentity(string username, string clearUnsaltedPassword)
    {
      var criteria = new UsernameCriteria(username, clearUnsaltedPassword);
      return DataPortal.Fetch<UserIdentity>(criteria);
    }

    internal static UserIdentity GetUserIdentity(string username)
    {
      return DataPortal.Fetch<UserIdentity>(username);
    }

#endif

    public static void GetUserIdentity(string username,
                                         string clearUnsaltedPassword,
                                         EventHandler<DataPortalResult<UserIdentity>> callback)
    {
      var criteria = new UsernameCriteria(username, clearUnsaltedPassword);
      DataPortal.BeginFetch<UserIdentity>(criteria, callback);
    }

    #endregion

    #region public int Salt
    public static readonly PropertyInfo<int> SaltProperty = RegisterProperty<int>(c => c.Salt);
    public int Salt
    {
      get { return ReadProperty(SaltProperty); }
      private set { LoadProperty(SaltProperty, value); }
    }
    #endregion

    #region public Guid UserId
    public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(c => c.UserId);
    public Guid UserId
    {
      get { return GetProperty(UserIdProperty); }
      private set { LoadProperty(UserIdProperty, value); }
    }
    #endregion

    /// <summary>
    /// If current user is not authenticated, throws a UserNotAuthenticatedException.
    /// </summary>
    /// <exception cref="LearnLanguages.DataAccess.Exceptions.UserNotAuthenticatedException">Thrown when current user is not authenticated.</exception>
    public static void CheckAuthentication()
    {
      if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
        throw new Common.Exceptions.UserNotAuthenticatedException();
    }

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      
      //USERNAME REGEX
      string usernameRegexMatch = CommonResources.UsernameValidationRegex;
      BusinessRules.AddRule(new Csla.Rules.CommonRules.RegExMatch(NameProperty, usernameRegexMatch));
    }

#if !SILVERLIGHT
    /// <summary>
    /// This should run ONLY on the server.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="dal"></param>
    private void LoadUserData(string username, IUserDal dal)
    {
      var result = dal.Fetch(username);
      if (!result.IsSuccess)
      {
        var ex = result.GetExceptionFromInfo();
        if (ex != null)
          throw ex;
        else
          throw new DataAccess.Exceptions.LoadUserDataException(result.Msg, username);
      }

      var userDto = result.Obj;
      
      
      IsAuthenticated = (userDto != null);
      if (IsAuthenticated)
      {
        //PROPERTIES
        Name = userDto.Username;
        Salt = userDto.Salt;
        UserId = userDto.Id;
        
        //ROLES
        var resultRoles = dal.GetRoles(Name);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new GeneralDataAccessException(resultRoles.Msg);
        }

        if (resultRoles.Obj.Count == 0)
          throw new VeryBadException("Roles.Count == 0.  Every user should have at least one role.");
        Roles = new Csla.Core.MobileList<string>();
        foreach (var roleDto in resultRoles.Obj)
        {
          Roles.Add(roleDto.Text);
        }
      }
    }

    private void DataPortal_Fetch(UsernameCriteria criteria)
    {
      AuthenticationType = DalResources.AuthenticationTypeString;
      using (var dalManager = DataAccess.DalFactory.GetDalManager())
      {
        var dal = dalManager.GetProvider<IUserDal>();

        var verifyResult = dal.VerifyUser(criteria.Username, criteria.Password);
        if (!verifyResult.IsSuccess)
        {
          Exception error = verifyResult.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(verifyResult.Msg);
        }
        bool? userIsVerified = verifyResult.Obj;
        if (userIsVerified == null)
          userIsVerified = false;

        if (userIsVerified == true)
          LoadUserData(criteria.Username, dal);
      }
    }
    private void DataPortal_Fetch(string username)
    {
      AuthenticationType = DalResources.AuthenticationTypeString;
      using (var dalManager = DataAccess.DalFactory.GetDalManager())
      {
        var dal = dalManager.GetProvider<IUserDal>();
        LoadUserData(username, dal);
      }
    }
    private void Child_Fetch(string username)
    {
      AuthenticationType = DalResources.AuthenticationTypeString;
      using (var dalManager = DataAccess.DalFactory.GetDalManager())
      {
        var dal = dalManager.GetProvider<IUserDal>();
        LoadUserData(username, dal);
      }
    }
#endif
  }
}
