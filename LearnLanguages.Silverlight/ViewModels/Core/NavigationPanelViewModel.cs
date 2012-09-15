using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Interfaces;
using LearnLanguages.Business.Security;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using System.Windows;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(NavigationPanelViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class NavigationPanelViewModel : Conductor<IViewModelBase>.Collection.AllActive,
                                          IHandle<EventMessages.AuthenticationChangedEventMessage>,
                                          IHandle<EventMessages.ExpandedChangedEventMessage>,
                                          IViewModelBase
  {
    public NavigationPanelViewModel()
    {
      Services.EventAggregator.Subscribe(this);
      PopulatePanel();
    }

    #region Populate Panel
    
    private void PopulatePanel()
    {
      Items.Clear();
      var user = Csla.ApplicationContext.User.Identity;
      if (!user.IsAuthenticated)
        AddUnauthenticatedItems();
      else
        AddAuthenticatedItems((UserIdentity)user);

      //sort the items
      var tmp = new List<IViewModelBase>(Items);
      Comparison<IViewModelBase> comparison = (a, b) =>
        {
          //put Logout last always
          if (a is LogoutNavigationButtonViewModel)
            return 1;
          else if (b is LogoutNavigationButtonViewModel)
            return -1;
          else if (a.GetType().Name[0] < b.GetType().Name[0])
            return -1;
          else
            return 1;
        };
      tmp.Sort(comparison);
      Items.Clear();
      for (int i = 0; i < tmp.Count; i++)
      {
        Items.Insert(i, tmp[i]);
      }

      
    }

    private void AddAuthenticatedItems(UserIdentity user)
    {
      if (Csla.ApplicationContext.User.IsInRole(DataAccess.DalResources.RoleAdmin))
        AddAdminItems();
      if (Csla.ApplicationContext.User.IsInRole(DataAccess.DalResources.RoleUser))
        AddUserItems();

      var logoutNavButtonViewModel = Services.Container.GetExportedValue<LogoutNavigationButtonViewModel>();
      Items.Add(logoutNavButtonViewModel);
    }
    private void AddUnauthenticatedItems()
    {
      var loginNavButtonViewModel = Services.Container.GetExportedValue<LoginNavigationButtonViewModel>();
      Items.Add(loginNavButtonViewModel);
    }
    
    private void AddAdminItems()
    {
      //INITIALIZE NAVIGATION ITSELF
      var adminNavigationSet = Services.Container.GetExportedValue<NavigationSetViewModel>();
      var adminTitle = Services.Container.GetExportedValue<AdminNavigationSetTitleViewModel>();
      adminNavigationSet.TitleControl = adminTitle;
      adminNavigationSet.ShowItems = adminTitle.IsExpanded;

      //CREATE BUTTONS TO GO INTO NAVIGATION SET
      var authStatusNavButtonViewModel = 
        Services.Container.GetExportedValue<AuthenticationStatusNavigationButtonViewModel>();
      var addUserNavButtonViewModel = Services.Container.GetExportedValue<AddUserNavigationButtonViewModel>();

      //INSERT BUTTONS IN LIST ***IN THE ORDER IN WHICH YOU WANT THEM TO APPEAR***
      List<ViewModelBase> orderedAdminButtons = new List<ViewModelBase>();
      orderedAdminButtons.Insert(0, authStatusNavButtonViewModel);
      orderedAdminButtons.Insert(1, addUserNavButtonViewModel);

      //ADD ORDERED LIST OF NAV BUTTONS TO NAVIGATION SET
      adminNavigationSet.AddControls(orderedAdminButtons);

      //ADD THE SET TO THE ITEMS
      Items.Add(adminNavigationSet);
    }
    private void AddUserItems()
    {
      AddStudyNavigationSet();
      AddEditNavigationSet();
      AddTestNavigationSet();
      AddReviewNavigationSet();
      AddProgressNavigationSet();
      AddChugNavigationSet();

      //var study = Services.Container.GetExportedValue<StudyNavigationButtonViewModel>();
      //Items.Add(study);
      //var iWantToLearn = Services.Container.GetExportedValue<IWantToLearnNavigationButtonViewModel>();
      //Items.Add(iWantToLearn);
      //var addSong = Services.Container.GetExportedValue<AddSongNavigationButtonViewModel>();
      //Items.Add(addSong);
    }

    private void AddStudyNavigationSet()
    {
      //INITIALIZE NAVIGATION ITSELF
      var studyNavigationSet = Services.Container.GetExportedValue<NavigationSetViewModel>();
      var studyTitle = Services.Container.GetExportedValue<StudyNavigationSetTitleViewModel>();
      studyNavigationSet.ShowItems = studyTitle.IsExpanded;
      studyNavigationSet.TitleControl = studyTitle;

      //CREATE BUTTONS TO GO INTO NAVIGATION SET
      var studyASong = Services.Container.GetExportedValue<StudyASongNavigationButtonViewModel>();

      //INSERT BUTTONS IN LIST ***IN THE ORDER IN WHICH YOU WANT THEM TO APPEAR***
      List<ViewModelBase> orderedStudyButtons = new List<ViewModelBase>();
      orderedStudyButtons.Insert(0, studyASong);
      //orderedStudyButtons.Insert(1, addPhrase);
      //orderedStudyButtons.Insert(2, viewPhrases);
      //orderedStudyButtons.Insert(3, addTranslation);
      //orderedStudyButtons.Insert(4, viewTranslations);
      //orderedStudyButtons.Insert(5, addLanguage);
      //orderedStudyButtons.Insert(6, viewLanguages);

      //ADD ORDERED LIST OF NAV BUTTONS TO NAVIGATION SET
      studyNavigationSet.AddControls(orderedStudyButtons);

      //ADD THE SET TO THE ITEMS
      Items.Add(studyNavigationSet);
    }
    private void AddEditNavigationSet()
    {
      //INITIALIZE NAVIGATION ITSELF
      var editNavigationSet = Services.Container.GetExportedValue<NavigationSetViewModel>();
      var editTitle = Services.Container.GetExportedValue<EditNavigationSetTitleViewModel>();
      editNavigationSet.ShowItems = editTitle.IsExpanded;
      editNavigationSet.TitleControl = editTitle;

      //CREATE BUTTONS TO GO INTO NAVIGATION SET
      var addLanguage = Services.Container.GetExportedValue<AddLanguageNavigationButtonViewModel>();
      var viewLanguages = Services.Container.GetExportedValue<ViewLanguagesNavigationButtonViewModel>();
      var viewTranslations = Services.Container.GetExportedValue<ViewTranslationsNavigationButtonViewModel>();
      var viewPhrases = Services.Container.GetExportedValue<ViewPhrasesNavigationButtonViewModel>();
      var addTranslation = Services.Container.GetExportedValue<AddTranslationNavigationButtonViewModel>();
      var addPhrase = Services.Container.GetExportedValue<AddPhraseNavigationButtonViewModel>();
      var addSong = Services.Container.GetExportedValue<AddSongNavigationButtonViewModel>();

      //INSERT BUTTONS IN LIST ***IN THE ORDER IN WHICH YOU WANT THEM TO APPEAR***
      List<ViewModelBase> orderedEditButtons = new List<ViewModelBase>();
      orderedEditButtons.Insert(0, addSong);
      orderedEditButtons.Insert(1, addPhrase);
      orderedEditButtons.Insert(2, viewPhrases);
      orderedEditButtons.Insert(3, addTranslation);
      orderedEditButtons.Insert(4, viewTranslations);
      orderedEditButtons.Insert(5, addLanguage);
      orderedEditButtons.Insert(6, viewLanguages);

      //ADD ORDERED LIST OF NAV BUTTONS TO NAVIGATION SET
      editNavigationSet.AddControls(orderedEditButtons);

      //ADD THE SET TO THE ITEMS
      Items.Add(editNavigationSet);
    }
    private void AddTestNavigationSet()
    {
      //throw new NotImplementedException();
    }
    private void AddReviewNavigationSet()
    {
      //throw new NotImplementedException();
    }
    private void AddProgressNavigationSet()
    {
      //throw new NotImplementedException();
    }
    private void AddChugNavigationSet()
    {
      //throw new NotImplementedException();
    }

    #endregion

    public void Handle(EventMessages.AuthenticationChangedEventMessage message)
    {
      PopulatePanel();
    }
    public void Handle(EventMessages.ExpandedChangedEventMessage message)
    {
      PopulatePanel();
    }
    
    public void OnImportsSatisfied()
    {
      //var coreViewModelName = ViewModelBase.GetCoreViewModelName(typeof(NavigationPanelViewModel));
      //Services.EventAggregator.Publish(new Events.PartSatisfiedEventMessage(coreViewModelName));
    }
    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }
    private Visibility _ViewModelVisibility;
    public Visibility ViewModelVisibility
    {
      get { return _ViewModelVisibility; }
      set
      {
        if (value != _ViewModelVisibility)
        {
          _ViewModelVisibility = value;
          NotifyOfPropertyChange(() => ViewModelVisibility);
        }
      }
    }
  }
}
