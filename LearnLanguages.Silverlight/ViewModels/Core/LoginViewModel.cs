using System.ComponentModel.Composition;
using LearnLanguages.Business.Security;
using LearnLanguages.Common.ViewModelBases;
using System.ComponentModel;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LoginViewModel))]
  [ViewModelMetadata("Login")]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class LoginViewModel : ViewModelBase
  {
    private string _Username;
    public string Username
    {
      get { return _Username; }
      set
      {
        if (value != _Username)
        {
          _Username = value;
          NotifyOfPropertyChange(() => Username);
          NotifyOfPropertyChange(() => CanLogin);
        }
      }
    }

    private string _Password;
    public string Password
    {
      get { return _Password; }
      set
      {
        if (value != _Password)
        {
          _Password = value;
          NotifyOfPropertyChange(() => Password);
          NotifyOfPropertyChange(() => CanLogin);
        }
      }
    }

    public void Login()
    {
      LoggingIn = true;
      UserPrincipal.BeginLogin(Username, Password, (e) =>
        {
          if (e != null)
            throw e;
          //Navigation.Publish.AuthenticationChanged();
          //Services.EventAggregator.Publish(new EventMessages.AuthenticationChangedEventMessage());
          EventMessages.AuthenticationChangedEventMessage.Publish();

          if (Csla.ApplicationContext.User.Identity.IsAuthenticated)
          {
            //LOGIN SUCCESSFUL
            LoggingIn = false;
          }
          else
          {
            //LOGIN UNSUCCESSFUL
            //PENALIZE FOR NOT LOGGING IN PROPERLY.
            //QUICK AND DIRTY PENALTY.
            //HACK: LOGIN FAILED PENALTY EASILY CIRCUMVENTED.  THIS REALLY NEEDS TO BE DONE ON THE SERVER SIDE.
            System.Windows.MessageBox.Show(AppResources.LoginFailedMessage);
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, eventArg) =>
              {
                System.Threading.Thread.Sleep(int.Parse(AppResources.LoginFailedPenaltyInMilliseconds));
                LoggingIn = false;
              };
            worker.RunWorkerAsync();
          }
        });
    }
    public bool CanLogin
    {
      get
      {
        return (!string.IsNullOrEmpty(Username) && 
                !string.IsNullOrEmpty(Password) &&
                !LoggingIn);
      }
    }

    private bool _LoggingIn = false;
    public bool LoggingIn
    {
      get { return _LoggingIn; }
      set
      {
        if (value != _LoggingIn)
        {
          _LoggingIn = value;
          NotifyOfPropertyChange(() => LoggingIn);
          NotifyOfPropertyChange(() => CanLogin);
        }
      }
    }
  }
}
