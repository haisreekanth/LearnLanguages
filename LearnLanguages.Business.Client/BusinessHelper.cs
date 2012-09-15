using System;
using LearnLanguages.Business.Security;

namespace LearnLanguages.Business
{
  public static class BusinessHelper
  {
    public static string GetCurrentUsername()
    {
      var currentUsername = ((UserIdentity)(Csla.ApplicationContext.User.Identity)).Name;
      return currentUsername;
    }

    public static Guid GetCurrentUserId()
    {
      var currentUserId = ((UserIdentity)(Csla.ApplicationContext.User.Identity)).UserId;
      return currentUserId;
    }
  }
}
