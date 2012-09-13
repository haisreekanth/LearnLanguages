using System;
using System.Linq;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using Caliburn.Micro;
using System.Collections.Specialized;
using System.Windows;
using System.Collections.Generic;
using LearnLanguages.Common.Interfaces;
using System.ComponentModel;

namespace LearnLanguages.Silverlight.ViewModels
{

  /// <summary>
  /// This contains Items that each represent a Translation.  
  /// A Translation contains Phrases, so each Item in this collection is itself a collection.
  /// </summary>
  [Export(typeof(ViewTranslationsViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ViewTranslationsViewModel : Conductor<ViewTranslationsItemViewModel>.Collection.AllActive, 
                                           IViewModelBase
  {
    #region Ctors and Init

    public ViewTranslationsViewModel()
    {
      ViewModelVisibility = Visibility.Visible;
      _InitiateDeleteVisibility = Visibility.Visible;
      _FinalizeDeleteVisibility = Visibility.Collapsed;
      TranslationList.GetAll((s, r) =>
      {
        if (r.Error != null)
          throw r.Error;

        var allTranslations = r.Object;
        ModelList = allTranslations;
        PopulateViewModels(allTranslations);
      });
    }
    
    #endregion

    #region Properties

    private TranslationList _ModelList;
    public TranslationList ModelList
    {
      get { return _ModelList; }
      set
      {
        if (value != _ModelList)
        {
          UnhookFrom(_ModelList);
          _ModelList = value;
          HookInto(_ModelList);
          NotifyOfPropertyChange(() => ModelList);
          NotifyOfPropertyChange(() => CanSave);
        }
      }
    }

    private Visibility _InitiateDeleteVisibility;
    public Visibility InitiateDeleteVisibility
    {
      get { return _InitiateDeleteVisibility; }
      set
      {
        if (value != _InitiateDeleteVisibility)
        {
          _InitiateDeleteVisibility = value;
          NotifyOfPropertyChange(() => InitiateDeleteVisibility);
        }
      }
    }

    private Visibility _FinalizeDeleteVisibility;
    public Visibility FinalizeDeleteVisibility
    {
      get { return _FinalizeDeleteVisibility; }
      set
      {
        if (value != _FinalizeDeleteVisibility)
        {
          _FinalizeDeleteVisibility = value;
          NotifyOfPropertyChange(() => FinalizeDeleteVisibility);
        }
      }
    }

    private string _FilterLabel = ViewViewModelResources.LabelFilter;
    public string FilterLabel
    {
      get { return _FilterLabel; }
      set
      {
        if (value != _FilterLabel)
        {
          _FilterLabel = value;
          NotifyOfPropertyChange(() => FilterLabel);
        }
      }
    }

    private string _FilterText = "";
    public string FilterText
    {
      get { return _FilterText; }
      set
      {
        if (value != _FilterText)
        {
          _FilterText = value;
          //PopulateViewModels(ModelList);
          NotifyOfPropertyChange(() => FilterText);
        }
      }
    }

    private Visibility _ProgressVisibility = Visibility.Collapsed;
    public Visibility ProgressVisibility
    {
      get { return _ProgressVisibility; }
      set
      {
        if (value != _ProgressVisibility)
        {
          _ProgressVisibility = value;
          NotifyOfPropertyChange(() => ProgressVisibility);
        }
      }
    }

    private double _ProgressValue;
    public double ProgressValue
    {
      get { return _ProgressValue; }
      set
      {
        //if (value != _ProgressValue)
        //{
        _ProgressValue = value;
        NotifyOfPropertyChange(() => ProgressValue);
        //}
      }
    }

    private double _ProgressMaximum;
    public double ProgressMaximum
    {
      get { return _ProgressMaximum; }
      set
      {
        //if (value != _ProgressMaximum)
        //{
        _ProgressMaximum = value;
        NotifyOfPropertyChange(() => ProgressMaximum);
        //}
      }
    }

    private string _ButtonLabelApplyFilter = ViewViewModelResources.ButtonLabelApplyFilter;
    public string ButtonLabelApplyFilter
    {
      get { return _ButtonLabelApplyFilter; }
      set
      {
        if (value != _ButtonLabelApplyFilter)
        {
          _ButtonLabelApplyFilter = value;
          NotifyOfPropertyChange(() => ButtonLabelApplyFilter);
        }
      }
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

    private bool _IsPopulating;
    public bool IsPopulating
    {
      get { return _IsPopulating; }
      set
      {
        if (value != _IsPopulating)
        {
          _IsPopulating = value;
          NotifyOfPropertyChange(() => IsPopulating);
          NotifyOfPropertyChange(() => CanStopPopulating);
          NotifyOfPropertyChange(() => CanApplyFilter);
          if (_IsPopulating)
            ProgressVisibility = Visibility.Visible;
          else
            ProgressVisibility = Visibility.Collapsed;
        }
      }
    }

    private bool _AbortIsFlagged;
    public bool AbortIsFlagged
    {
      get { return _AbortIsFlagged; }
      set
      {
        if (value != _AbortIsFlagged)
        {
          _AbortIsFlagged = value;
          NotifyOfPropertyChange(() => AbortIsFlagged);
          NotifyOfPropertyChange(() => CanStopPopulating);
        }
      }
    }

    #endregion

    #region Methods

    private IEnumerable<TranslationEdit> FilterTranslations(TranslationList translations)
    {
      if (string.IsNullOrEmpty(FilterLabel))
        return translations;

      var results = from translation in translations
                    where (from phrase in translation.Phrases
                           where phrase.Text.Contains(FilterText)
                           select phrase).Count() > 0
                    select translation;

      return results;
    }

    private void PopulateViewModels(TranslationList allTranslations)
    {
      if (IsPopulating && AbortIsFlagged)
        return;

      BackgroundWorker worker = new BackgroundWorker();
      worker.DoWork += ((s, r) =>
        {
          //IsBusy = true;
          IsPopulating = true;

          //CLEAR OUR ITEMS (VIEWMODELS)
          Items.Clear();

          //FILTER OUR TRANSLATIONS
          var filteredTranslations = FilterTranslations(allTranslations);

          //PREPARE FOR COMING UP FOR AIR IN TIGHT LOOP
          int counter = 0;
          int iterationsBetweenComeUpForAir = 20;
          int totalCount = filteredTranslations.Count();
          ProgressMaximum = totalCount;

          //TELL HISTORY WE'RE THINKING
          Guid thinkId = Guid.NewGuid();
          History.Events.ThinkingAboutTargetEvent.Publish(thinkId);
        
          //POPULATE NEW VIEWMODELS WITH FILTERED RESULTS IN TIGHT LOOP
          foreach (var translationEdit in filteredTranslations)
          {
            var itemViewModel = Services.Container.GetExportedValue<ViewTranslationsItemViewModel>();
            itemViewModel.PropertyChanged +=
              new System.ComponentModel.PropertyChangedEventHandler(HandleItemViewModelChanged);
            itemViewModel.Model = translationEdit;
            Items.Add(itemViewModel);

            //UPDATE COMING UP FOR AIR
            counter++;
            ProgressValue = counter;
            if (counter % iterationsBetweenComeUpForAir == 0)
            {
              if (AbortIsFlagged)
              {
                AbortIsFlagged = false;
                ProgressValue = 0;
                break;
              }
            }

            //PING THINKING
            History.Events.ThinkedAboutTargetEvent.Publish(Guid.Empty);
          }

          //TELL HISTORY WE'RE DONE THINKING
          History.Events.ThinkedAboutTargetEvent.Publish(thinkId);

          IsPopulating = false;
        }); //END WORKER THREAD

      worker.RunWorkerAsync();
    }

    protected virtual void HookInto(TranslationList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    protected virtual void UnhookFrom(TranslationList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    protected virtual void HandleChildChanged(object sender, Csla.Core.ChildChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.ModelList);
      //Model.BeginSave((s2, r2) =>
      //{
      //  if (r2.Error != null)
      //    throw r2.Error;
      //  Model = (TranslationList)r2.NewObject;
      //});
      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);

      //NotifyOfPropertyChange(() => CanCancelEdit);
    }
    protected virtual void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.ModelList);
      NotifyOfPropertyChange(() => CanSave);
      //NotifyOfPropertyChange(() => CanCancelEdit);
    }
    protected void HandleItemViewModelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }

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
      //Publish.PartSatisfied("ViewTranslations");
    }

    #endregion

    #region Events

    #endregion

    #region Commands

    public bool CanSave
    {
      get
      {
        return (ModelList != null && ModelList.IsSavable);
      }
    }
    public virtual void Save()
    {
      ModelList.BeginSave((s, r) =>
      {
        if (r.Error != null)
          throw r.Error;

        ModelList = (TranslationList)r.NewObject;
        //propagate new TranslationEdits to their ViewModels
        PopulateViewModels(ModelList);

        NotifyOfPropertyChange(() => CanSave);
      });
    }

    public bool CanInitiateDeleteChecked
    {
      get
      {
        bool somethingIsChecked = (from viewModel in Items
                                   where ((ViewTranslationsItemViewModel)viewModel).IsChecked
                                   select viewModel).Count() > 0;

        return somethingIsChecked;
      }
    }
    public void InitiateDeleteChecked()
    {
      InitiateDeleteVisibility = Visibility.Collapsed;
      FinalizeDeleteVisibility = Visibility.Visible;
    }
    public void FinalizeDeleteChecked()
    {
      var checkedForDeletion = from viewModel in Items
                               where viewModel.IsChecked
                               select viewModel;

      foreach (var toDelete in checkedForDeletion)
      {
        ModelList.Remove(toDelete.Model);
      }

      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => ModelList);
      if (CanSave)
        Save();

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
    }
    public void CancelDeleteChecked()
    {
      foreach (var item in Items)
      {
        var vm = (ViewTranslationsItemViewModel)item;
        vm.IsChecked = false;
      }

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }

    public bool CanApplyFilter
    {
      get
      {
        return (!IsPopulating &&
                !AbortIsFlagged &&
                 ModelList != null &&
                 ModelList.Count > 0);
      }
    }
    public void ApplyFilter()
    {
      PopulateViewModels(ModelList);
    }

    public bool CanStopPopulating
    {
      get
      {
        //if abort is already flagged, then we're already trying to stop populating.
        return (IsPopulating && !AbortIsFlagged);
      }
    }
    public void StopPopulating()
    {
      AbortIsFlagged = true;
    }

    #endregion
  }
}
