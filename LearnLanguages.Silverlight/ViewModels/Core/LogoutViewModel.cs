using LearnLanguages.Business.Security;
using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LogoutViewModel))]
  [ViewModelMetadata("Logout")]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class LogoutViewModel : ViewModelBase
  {
    private string _LogoutMessage = AppResources.LogoutMessage;
    public string LogoutMessage
    {
      get { return _LogoutMessage; }
      set
      {
        if (value != _LogoutMessage)
        {
          _LogoutMessage = value;
          NotifyOfPropertyChange(() => LogoutMessage);
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
