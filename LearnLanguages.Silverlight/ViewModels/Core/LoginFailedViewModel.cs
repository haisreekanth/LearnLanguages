using LearnLanguages.Business.Security;
using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LoginFailedViewModel))]
  [ViewModelMetadata("Logout")]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class LoginFailedViewModel : ViewModelBase
  {
    private string _LoginFailedMessage = AppResources.LoginFailedMessage;
    public string LoginFailedMessage
    {
      get { return _LoginFailedMessage; }
      set
      {
        if (value != _LoginFailedMessage)
        {
          _LoginFailedMessage = value;
          NotifyOfPropertyChange(() => LoginFailedMessage);
        }
      }
    }

    //Logout() functionality is included in the LogoutNavigationButtonViewModel

    //public void Logout()
    //{
    //  UserPrincipal.Logout();
    //  Services.EventAggregator.Publish(new Events.AuthenticationChangedEventMessage());
    //}

    //public bool CanLogout
    //{
    //  get
    //  {
    //    return (Csla.ApplicationContext.User.Identity.IsAuthenticated);
    //  }
    //}
  }
}
