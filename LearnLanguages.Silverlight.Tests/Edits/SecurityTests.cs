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
using Microsoft.Silverlight.Testing;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("sec")]
  public class SecurityTests : Microsoft.Silverlight.Testing.SilverlightTest
  {

    //private void OutputExecutionLocations()
    //{
    //  string msg = "ExecutionLocation: " + Csla.ApplicationContext.ExecutionLocation.ToString();
    //  msg = msg + "\r\nLogicalExecutionLocation: " + Csla.ApplicationContext.LogicalExecutionLocation.ToString();
    //  MessageBox.Show(msg);
    //}


    //[ClassInitialize]
    //public void Initialize()
    //{
    //  _TestUsername = "user";
    //  _TestRole = "Users";
    //  _TestPassword = "password";
    //  _TestSaltedHashedPassword = new SaltedHashedPassword(_TestPassword);
    //  _TestSalt = _TestSaltedHashedPassword.Salt;

    //  MockDb.Users.Add(new User()
    //    {
    //      Username = _TestUsername,
    //      SaltedHashedPasswordValue = _TestSaltedHashedPassword.Value,
    //      Salt = _TestSalt
    //    });

    //  MockDb.UserRoles.Add(new UserRole() 
    //    { 
    //      Username = _TestUsername, 
    //      Role = _TestRole 
    //    });


    //}

    private string _TestValidUsername = "user";
    private string _TestRole = "Admin";
    private string _TestValidPassword = "password";
    //private SaltedHashedPassword _TestSaltedHashedPassword;
    private string _TestSaltedHashedPassword = @"瞌訖ꎚ壿喐ຯ缟㕧";
    private int _TestSalt = -54623530;

    private string _TestInvalidUsername = "ImNotAValidUser";
    private string _TestInvalidPassword = "ImNotAValidPassword";

    [TestMethod]
    [Asynchronous]
    public void TEST_CUSTOM_PRINCIPAL_BEGIN_LOGIN_SUCCESS_VALID_USERNAME_VALID_PASSWORD()
    {
      var isInitialized = false;

      UserPrincipal.BeginLogin(_TestValidUsername, _TestValidPassword, (e) =>
      {
        if (e != null)
          throw e;
        isInitialized = true;
      });

      EnqueueConditional(() => isInitialized);

      EnqueueCallback(() => { Assert.IsTrue(Csla.ApplicationContext.User.IsInRole(_TestRole)); },
                      () => { Assert.AreEqual(_TestValidUsername, Csla.ApplicationContext.User.Identity.Name); },
                      () => { Assert.IsTrue(Csla.ApplicationContext.User.Identity.IsAuthenticated); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void TEST_CUSTOM_PRINCIPAL_BEGIN_LOGIN_FAIL_INVALID_USERNAME_INVALID_PASSWORD()
    {
      var isInitialized = false;

      UserPrincipal.BeginLogin(_TestInvalidUsername, _TestInvalidPassword, (e) =>
      {
        if (e != null)
          throw e;
        isInitialized = true;
      });

      EnqueueConditional(() => isInitialized);

      EnqueueCallback(() => { Assert.IsFalse(Csla.ApplicationContext.User.IsInRole(_TestRole)); },
                      () => { Assert.AreNotEqual(_TestValidUsername, Csla.ApplicationContext.User.Identity.Name); },
                      () => { Assert.IsFalse(Csla.ApplicationContext.User.Identity.IsAuthenticated); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void TEST_CUSTOM_PRINCIPAL_BEGIN_LOGIN_FAIL_VALID_USERNAME_INVALID_PASSWORD()
    {
      var isInitialized = false;

      UserPrincipal.BeginLogin(_TestValidUsername, _TestInvalidPassword, (e) =>
      {
        if (e != null)
          throw e;
        isInitialized = true;
      });

      EnqueueConditional(() => isInitialized);

      EnqueueCallback(() => { Assert.IsFalse(Csla.ApplicationContext.User.IsInRole(_TestRole)); },
                      () => { Assert.AreNotEqual(_TestValidUsername, Csla.ApplicationContext.User.Identity.Name); },
                      () => { Assert.IsFalse(Csla.ApplicationContext.User.Identity.IsAuthenticated); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void TEST_CUSTOM_PRINCIPAL_BEGIN_LOGIN_FAIL_INVALID_USERNAME_VALID_PASSWORD()
    {
      var isInitialized = false;

      UserPrincipal.BeginLogin(_TestInvalidUsername, _TestValidPassword, (e) =>
      {
        if (e != null)
          throw e;
        isInitialized = true;
      });

      EnqueueConditional(() => isInitialized);

      EnqueueCallback(() => { Assert.IsFalse(Csla.ApplicationContext.User.IsInRole(_TestRole)); },
                      () => { Assert.AreNotEqual(_TestValidUsername, Csla.ApplicationContext.User.Identity.Name); },
                      () => { Assert.IsFalse(Csla.ApplicationContext.User.Identity.IsAuthenticated); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void TEST_ADD_USER()
    {
      var isCreated = false;

      var testUsername = "user2";
      var testPassword = "password2";
      var criteria = new Csla.Security.UsernameCriteria(testUsername, testPassword);
      Business.NewUserCreator.CreateNew(criteria, (s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          isCreated = true;
        });


      EnqueueConditional(() => isCreated);

      EnqueueCallback(() => { Assert.IsTrue(Csla.ApplicationContext.User.IsInRole(DataAccess.SeedData.Ton.AdminRoleText)); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    [Tag("current")]
    public void TEST_ADD_20_RANDOM_USERS_RANDOM_PASSWORDS_MUST_CLEAN_SOLUTION_FIRST()
    {
      int numToAdd = 20;
      var creationAttempts = 0;
      var creationSuccesses = 0;

      for (int i = 0; i < numToAdd; i++)
      {
        string randomUsername = GenerateRandomUsername();
        string randomPassword = GenerateRandomPassword();
        var criteria = new Csla.Security.UsernameCriteria(randomUsername, randomPassword);

        Business.NewUserCreator.CreateNew(criteria, (s, r) =>
        {
          creationAttempts++;
          //if (r.Error != null)
          //  throw r.Error;
          if (r.Error == null)
            creationSuccesses++;
        });
      }

      EnqueueConditional(() => creationAttempts == numToAdd);

      EnqueueCallback(() => { Assert.IsTrue(Csla.ApplicationContext.User.IsInRole(DataAccess.SeedData.Ton.AdminRoleText)); },
                      () => { Assert.AreEqual(numToAdd, creationAttempts); },
                      () => { Assert.AreEqual(numToAdd, creationSuccesses); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void TEST_ADD_THEN_DELETE_RANDOM_USER()
    {
      bool wasAdded = false;
      bool wasDeleted = false;

      string randomUsername = GenerateRandomUsername();
      string randomPassword = GenerateRandomPassword();
      var criteria = new Csla.Security.UsernameCriteria(randomUsername, randomPassword);

      Business.NewUserCreator.CreateNew(criteria, (s, r) =>
      {
        if (r.Error != null)
          throw r.Error;
        wasAdded = true;
        

        //criteria = new Business.Criteria.UserInfoCriteria(randomUsername, randomPassword);
        Business.DeleteUserReadOnly.CreateNew(randomUsername, (s2, r2) =>
          {
            if (r2.Error != null)
              throw r2.Error;

            wasDeleted = true;
          });
      });


      EnqueueConditional(() => wasAdded);
      EnqueueConditional(() => wasDeleted);

      EnqueueCallback(
                      () => { Assert.IsTrue(Csla.ApplicationContext.User.IsInRole(DataAccess.SeedData.Ton.AdminRoleText)); }
                     );

      EnqueueTestComplete();
    }


    [TestMethod]
    [Asynchronous]
    [ExpectedException(typeof(ExpectedException))]
    public void TEST_DELETE_USER_THAT_DOESNT_EXIST_EXPECT_EXPECTED_EXCEPTION()
    {
      bool wasDeleted = false;

      string randomUsername = GenerateRandomUsername();

      Business.DeleteUserReadOnly.CreateNew(randomUsername, (s2, r2) =>
      {
        if (r2.Error != null)// &&
          //WHY DOES THIS THROW (*UN*EXPECTED) EXCEPTION AND JUST CHECKING FOR != NULL THROWS EXPECTED EXCEPTION?
          //r2.Error is Csla.DataPortalException) //&&
          //r2.Error.Message.Contains(@"UsernameNotFoundException"))
        {
          throw new ExpectedException(r2.Error);
        }
        else
        {
          throw new Exception();
        }
      });


      EnqueueConditional(() => wasDeleted);

      //EnqueueCallback(
      //                () => { Assert.IsTrue(Csla.ApplicationContext.User.IsInRole(DataAccess.SeedData.Ton.AdminRoleText)); }
      //               );

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void GET_ALL_USERS()
    {
      bool wasGotten = false;


      EnqueueConditional(() => wasGotten);

      EnqueueCallback(
                      () => { Assert.IsTrue(Csla.ApplicationContext.User.IsInRole(DataAccess.SeedData.Ton.AdminRoleText)); }
                     );

      EnqueueTestComplete();
    }


    #region Helpers

    private string GenerateRandomPassword()
    {
      int minValidPasswordLength = 6;
      int maxValidPasswordLength = 15;
      string validPasswordChars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+";
      string randomPassword = "";
      bool randomPasswordIsValid = false;
      do
      {
        randomPassword =
          GenerateRandomString(validPasswordChars, minValidPasswordLength, maxValidPasswordLength);
        //randomPasswordIsValid = Regex.IsMatch(randomPassword, CommonResources.PasswordValidationRegex);
        randomPasswordIsValid = Common.CommonHelper.PasswordIsValid(randomPassword);
      } while (!randomPasswordIsValid);

      return randomPassword;
    }

    #region private List<string> _usernames

    private object ___usernamesLock = new object();
    private List<string> __usernames = new List<string>();
    private List<string> _usernames
    {
      get
      {
        lock (___usernamesLock)
        {
          var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
          return __usernames;
        }
      }
      set
      {
        lock (___usernamesLock)
        {
          __usernames = value;
        }
      }
    }

    #endregion

    private string GenerateRandomUsername()
    {
      int minValidUsernameLength = 3;
      int maxValidUsernameLength = 30;
      string validUsernameChars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_";
      string randomUsername = "";
      bool randomUsernameIsValid = false;

      do
      {
        randomUsername =
          GenerateRandomString(validUsernameChars, minValidUsernameLength, maxValidUsernameLength);
        //randomUsernameIsValid = Regex.IsMatch(randomUsername, CommonResources.UsernameValidationRegex);
        randomUsernameIsValid = Common.CommonHelper.UsernameIsValid(randomUsername);
        if (randomUsernameIsValid)
        {
          var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
          if (_usernames.Contains(randomUsername))
            randomUsernameIsValid = false;
        }
      } while (!randomUsernameIsValid);

      _usernames.Add(randomUsername);
      return randomUsername;
    }

    private static string GenerateRandomString(string validChars, int minLength, int maxLength)
    {
      System.Threading.Thread.Sleep(20); //for randomizer
      Random r = new Random(DateTime.Now.Millisecond * DateTime.Now.Second + DateTime.Now.Minute);
      int length = r.Next(minLength, maxLength + 1);
      string generatedString = "";
      for (int i = 0; i < length; i++)
      {
        int randomCharIndex = r.Next(0, validChars.Length);
        char randomChar = validChars[randomCharIndex];
        generatedString += randomChar;
      }

      return generatedString;
    }

    #endregion
  }
}
