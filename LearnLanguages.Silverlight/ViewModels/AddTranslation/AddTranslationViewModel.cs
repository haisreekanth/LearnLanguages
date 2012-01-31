﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using LearnLanguages.Business;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddTranslationViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddTranslationViewModel : ViewModelBase
  {
    public AddTranslationViewModel()
    {
      BlankTranslationRetriever.CreateNew((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;
          var retriever = r.Object;

          TranslationViewModel = Services.Container.GetExportedValue<AddTranslationTranslationEditViewModel>();
          TranslationViewModel.Model = retriever.Translation;
          HookInto(TranslationViewModel);
        });
    }

    private AddTranslationTranslationEditViewModel _TranslationViewModel;
    public AddTranslationTranslationEditViewModel TranslationViewModel
    {
      get { return _TranslationViewModel; }
      set
      {
        if (value != _TranslationViewModel)
        {
          _TranslationViewModel = value;
          NotifyOfPropertyChange(() => TranslationViewModel);
        }
      }
    }


    public bool CanSave
    {
      get
      {
        return (TranslationViewModel != null &&
                TranslationViewModel.Model != null &&
                TranslationViewModel.Model.IsSavable);
      }
    }
    public void Save()
    {
      TranslationViewModel.Model.BeginSave((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          TranslationViewModel.Model = (TranslationEdit)r.NewObject;
          //NotifyOfPropertyChange(() => CanSave);
        });
    }

    private void HookInto(AddTranslationTranslationEditViewModel translationViewModel)
    {
      translationViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(translationViewModel_PropertyChanged);
    }

    void translationViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => CanSave);
    }
  }
}
