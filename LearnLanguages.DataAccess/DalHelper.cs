using System.Text.RegularExpressions;

namespace LearnLanguages.DataAccess
{
  public static class DalHelper
  {
    public static void CheckAuthorizationToAddUser()
    {
      if (!IsInRoleToAddUser())
        throw new Exceptions.UserNotAuthorizedException(DalResources.ErrorMsgAttemptedToAddUser, 0);
    }
    public static bool IsInRoleToAddUser()
    {
      var isInAdminRole = Csla.ApplicationContext.User.IsInRole(DalResources.RoleAdmin);
      return isInAdminRole;
    }

    public static void CheckAuthorizationToDeleteUser()
    {
      if (!IsInRoleToDeleteUser())
        throw new Exceptions.UserNotAuthorizedException(DalResources.ErrorMsgAttemptedToDeleteUser, 0);
    }
    public static bool IsInRoleToDeleteUser()
    {
      var isInAdminRole = Csla.ApplicationContext.User.IsInRole(DalResources.RoleAdmin);
      return isInAdminRole;
    }

    public static void CheckAuthorizationToGetAllUsers()
    {
      if (!IsInRoleToGetAllUsers())
        throw new Exceptions.UserNotAuthorizedException(DalResources.ErrorMsgAttemptedToGetAllUsers, 0);
    }
    public static bool IsInRoleToGetAllUsers()
    {
      var isInAdminRole = Csla.ApplicationContext.User.IsInRole(DalResources.RoleAdmin);
      return isInAdminRole;
    }

    public static void CheckAuthorizationMustRunOnServer()
    {
#if SILVERLIGHT
      throw new Exceptions.MustRunOnServerException();
#endif
    }
  }
}
