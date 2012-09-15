using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business.Security;
using LearnLanguages.DataAccess;
using Microsoft.Silverlight.Testing;

namespace LearnLanguages.Silverlight.Tests
{
  /// <summary>
  /// Logs in as the valid user with roles Admin and User.
  /// </summary>
  [TestClass]
  public class InitializeCleanupClass : Microsoft.Silverlight.Testing.SilverlightTest
  {
    [AssemblyInitialize]
    [Asynchronous]
    public void InitializeAllTesting()
    {
      bool loggedIn = false;
      UserPrincipal.BeginLogin(SeedData.Ton.TestValidUsername, SeedData.Ton.TestValidPassword, (e) =>
        {
          if (e != null)
            throw e;

          loggedIn = true;
        });

      EnqueueConditional(() => loggedIn);
      EnqueueCallback(() => { Assert.IsInstanceOfType(Csla.ApplicationContext.User.Identity, typeof(UserIdentity)); },
                      () => { Assert.IsTrue(Csla.ApplicationContext.User.Identity.IsAuthenticated); });

      EnqueueTestComplete();
    }

    [AssemblyCleanup]
    public void CleanUpAllTesting()
    {
      UserPrincipal.Logout();
    }
  }
}
