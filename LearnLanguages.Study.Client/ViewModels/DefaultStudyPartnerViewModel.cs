using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.ViewModels;
using LearnLanguages.DataAccess;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Study.ViewModels
{
  [Export(typeof(DefaultStudyPartnerViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class DefaultStudyPartnerViewModel : ViewModelBase
  {
    #region Ctors and Init

    public DefaultStudyPartnerViewModel()
    {
      
    }

    #endregion

    #region Properties

    private string _Instructions = StudyResources.InstructionsDefaultStudyPartner;
    public string Instructions
    {
      get { return _Instructions; }
      set
      {
        if (value != _Instructions)
        {
          _Instructions = value;
          NotifyOfPropertyChange(() => Instructions);
        }
      }
    }

    private string _StudyTitle = StudyResources.DefaultStudyTitle;
    public string StudyTitle
    {
      get { return _StudyTitle; }
      set
      {
        if (value != _StudyTitle)
        {
          _StudyTitle = value;
          NotifyOfPropertyChange(() => StudyTitle);
        }
      }
    }

    private IViewModelBase _StudyItemViewModel;
    public IViewModelBase StudyItemViewModel
    {
      get { return _StudyItemViewModel; }
      set
      {
        if (value != _StudyItemViewModel)
        {
          _StudyItemViewModel = value;
          if (_StudyItemViewModel is Interfaces.IStudyItemViewModelBase)
          {
            var studyItemTitle = ((Interfaces.IStudyItemViewModelBase)_StudyItemViewModel).StudyItemTitle;
            //IF THE STUDYITEMTITLE ISN'T EMPTY, THEN WE ASSIGN IT TO OUR STUDY TITLE. 
            //ELSE WE USE THE DEFAULT
            StudyTitle = studyItemTitle != "" ? studyItemTitle : StudyResources.DefaultStudyTitle;
          }
          NotifyOfPropertyChange(() => StudyItemViewModel);
        }
      }
    }

    private string _FeedbackTitle;
    public string FeedbackTitle
    {
      get { return _FeedbackTitle; }
      set
      {
        if (value != _FeedbackTitle)
        {
          _FeedbackTitle = value;
          NotifyOfPropertyChange(() => FeedbackTitle);
        }
      }
    }

    private IFeedbackViewModelBase _FeedbackViewModel;
    public IFeedbackViewModelBase FeedbackViewModel
    {
      get { return _FeedbackViewModel; }
      set
      {
        if (value != _FeedbackViewModel)
        {
          _FeedbackViewModel = value;
          NotifyOfPropertyChange(() => FeedbackViewModel);
        }
      }
    }

    #endregion

    #region Methods

    

    #endregion

    #region Commands

    #endregion
  }
}
