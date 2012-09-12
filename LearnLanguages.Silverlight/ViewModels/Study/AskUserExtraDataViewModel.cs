using System;
using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;
using System.Windows;
using LearnLanguages.Common.ViewModelBases;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AskUserExtraDataViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AskUserExtraDataViewModel : ViewModelBase<StudyDataEdit, StudyDataDto>,
                                           IHandle<EventMessages.NativeLanguageChangedEventMessage>
  {
    public AskUserExtraDataViewModel()
    {
      Languages = Services.Container.GetExportedValue<LanguageSelectorViewModel>();
      Languages.SelectedItemChanged += HandleLanguageChanged;
      if (!bool.Parse(ViewViewModelResources.ShowInstructions))
        InstructionsVisibility = Visibility.Collapsed;
      else
        InstructionsVisibility = Visibility.Visible;

      StudyDataRetriever.CreateNew((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          Model = r.Object.StudyData;
        });
    }

    public string Instructions { get { return ViewViewModelResources.InstructionsAskUserExtraData; } }

    #region Native Language

    private LanguageSelectorViewModel _Languages;
    public LanguageSelectorViewModel Languages
    {
      get { return _Languages; }
      set
      {
        if (value != _Languages)
        {
          _Languages = value;
          NotifyOfPropertyChange(() => Languages);
        }
      }
    }

    private void HandleLanguageChanged(object sender, EventArgs e)
    {
      if (sender != null)
        Model.NativeLanguageText = ((LanguageEditViewModel)sender).Model.Text;
    }

    public string LabelNativeLanguageText { get { return ViewViewModelResources.LabelAskUserExtraDataNativeLanguageText; } }
    public string InstructionsSelectNativeLanguageText 
    {
      get { return ViewViewModelResources.InstructionsAskUserExtraDataSelectNativeLanguageText; } 
    }

    #endregion

    private Visibility _InstructionsVisibility;
    public Visibility InstructionsVisibility
    {
      get { return _InstructionsVisibility; }
      set
      {
        if (value != _InstructionsVisibility)
        {
          _InstructionsVisibility = value;
          NotifyOfPropertyChange(() => InstructionsVisibility);
        }
      }
    }

    //public bool CanSelectLanguage
    //{
    //  get
    //  {
    //    return true;
    //  }
    //}
    //public void SelectLanguage()
    //{
    //  var selectViewModel = Services.Container.GetExportedValue<SelectNativeLanguageViewModel>();
    //  Services.WindowManager.ShowDialog(selectViewModel);
    //}

    #region Base

    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }
    public void OnImportsSatisfied()
    {
      //
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

    #endregion

    public void Handle(EventMessages.NativeLanguageChangedEventMessage message)
    {
      Model.NativeLanguageText = message.NewNativeLanguageText;
      NotifyOfPropertyChange(() => Model);
      NotifyOfPropertyChange(() => CanSave);

      if (!Model.IsSavable)
        throw new Exception();

      Model.BeginSave((s, r) =>
        {
          if (r.Error != null)
          {
            if (_Callback != null)
            {
              var result = new Common.ResultArgs<StudyDataEdit>(r.Error);
              _Callback(this, result);
              _Callback = null;
            }
            else
              throw r.Error;
          }

          if (_Callback != null)
          {
            var result = new Common.ResultArgs<StudyDataEdit>(Model);
            _Callback(this, result);
            _Callback = null;
          }

          TryClose();
        });
    }

    private AsyncCallback<StudyDataEdit> _Callback;
    public void ShowModal(AsyncCallback<StudyDataEdit> callback)
    {
      _Callback = callback;
      Services.WindowManager.ShowDialog(this);
    }

    public override bool CanSave
    {
      get
      {
        return (base.CanSave &&
                !string.IsNullOrEmpty(Model.NativeLanguageText));
      }
    }
    public override void Save()
    {
      Model.BeginSave((s, r) =>
      {
        if (r.Error != null)
          throw r.Error;
        Model = (StudyDataEdit)r.NewObject;
        EventMessages.NativeLanguageChangedEventMessage.Publish(Model.NativeLanguageText);
        DispatchSavedEvent();
        NotifyOfPropertyChange(() => CanSave);
        if (_Callback != null)
          _Callback(this, new Common.ResultArgs<StudyDataEdit>(Model));
        TryClose();
      });
    }
  }
}
