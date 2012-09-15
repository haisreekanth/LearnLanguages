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
using System.ComponentModel.Composition;
using LearnLanguages.Business.Security;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LogoutNavigationButtonViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class LogoutNavigationButtonViewModel : NavigationButtonViewModelBase
  {
    public override bool CanNavigateImpl()
    {
      return Csla.ApplicationContext.User.Identity.IsAuthenticated;
    }

    public override void Navigate()
    {
      UserPrincipal.Logout();
      EventMessages.AuthenticationChangedEventMessage.Publish();
      //Services.EventAggregator.Publish(new Events.AuthenticationChangedEventMessage());
      base.Navigate();
    }
  }
}
