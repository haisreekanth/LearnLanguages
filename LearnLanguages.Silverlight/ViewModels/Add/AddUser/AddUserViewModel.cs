using System.Windows;
using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using System.Text.RegularExpressions;
using System;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddUserViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddUserViewModel : ViewModelBase
  {
    public AddUserViewModel()
    {
    }

    private string _NewUsername;
    public string NewUsername
    {
      get { return _NewUsername; }
      set
      {
        if (value != _NewUsername)
        {
          _NewUsername = value;
          NotifyOfPropertyChange(() => NewUsername);
          NotifyOfPropertyChange(() => CanAddUser);
        }
      }
    }

    private string _NewPassword;
    public string NewPassword
    {
      get { return _NewPassword; }
      set
      {
        if (value != _NewPassword)
        {
          _NewPassword = value;
          NotifyOfPropertyChange(() => NewPassword);
          NotifyOfPropertyChange(() => CanAddUser);
        }
      }
    }

    private string _ConfirmNewPassword;
    public string ConfirmNewPassword
    {
      get { return _ConfirmNewPassword; }
      set
      {
        if (value != _ConfirmNewPassword)
        {
          _ConfirmNewPassword = value;
          NotifyOfPropertyChange(() => ConfirmNewPassword);
          NotifyOfPropertyChange(() => CanAddUser);
        }
      }
    }

    public bool CanAddUser
    {
      get
      {
        //PASSWORDS MATCH
        bool passwordsMatch = NewPassword == ConfirmNewPassword;

        //PASSWORD IS VALID
        bool newPasswordIsValid = Common.CommonHelper.PasswordIsValid(NewPassword);

        //USERNAME IS VALID
        bool newUsernameIsValid = Common.CommonHelper.UsernameIsValid(NewUsername);

        bool canAddUser = DataAccess.DalHelper.IsInRoleToAddUser() &&
                          passwordsMatch &&
                          newUsernameIsValid &&
                          newPasswordIsValid;
        
        return canAddUser;
      }
    }
    public void AddUser()
    {
      var thinkingId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(thinkingId);
      var criteria = new Csla.Security.UsernameCriteria(NewUsername, NewPassword);
      Business.NewUserCreator.CreateNew(criteria, (s, r) =>
        {
          History.Events.ThinkedAboutTargetEvent.Publish(thinkingId);
          if (r.Error != null)
            throw r.Error;
          else
            MessageBox.Show(string.Format(ViewViewModelResources.MessageNewUserAdded, NewUsername));
        });
    }
  }
}
