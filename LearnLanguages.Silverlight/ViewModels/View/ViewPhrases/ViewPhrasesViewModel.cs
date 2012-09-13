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
  [Export(typeof(ViewPhrasesViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ViewPhrasesViewModel : Conductor<ViewPhrasesItemViewModel>.Collection.AllActive, 
                                      IViewModelBase
  {
    #region Ctors and Init

    public ViewPhrasesViewModel()
    {
      _ProgressValue = 1;
      ViewModelVisibility = Visibility.Visible;
      _InitiateDeleteVisibility = Visibility.Visible;
      _FinalizeDeleteVisibility = Visibility.Collapsed;
      _ProgressVisibility = Visibility.Collapsed;

      
      var thinkingId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(thinkingId);
      PhraseList.GetAll((s, r) =>
      {
        History.Events.ThinkedAboutTargetEvent.Publish(thinkingId);
        if (r.Error != null)
          throw r.Error;

        var allPhrases = r.Object;
        ModelList = allPhrases;
        History.Events.ThinkingAboutTargetEvent.Publish(thinkingId);
        PopulateItems(allPhrases);
        History.Events.ThinkedAboutTargetEvent.Publish(thinkingId);
      });
    }

    #endregion

    #region Fields

    #endregion

    #region Properties

    private PhraseList _ModelList;
    public PhraseList ModelList
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

    #region Progress

    private Visibility _ProgressVisibility;
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

    #endregion

    #region Filter
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
    #endregion

    #region Delete
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
    #endregion

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

    #region Methods

    private void PopulateItems(PhraseList phrases)
    {
      if (IsPopulating && AbortIsFlagged)
        return;

      BackgroundWorker worker = new BackgroundWorker();
      worker.DoWork += ((s, r) =>
      {
        //IsBusy = true;
        IsPopulating = true;


        Items.Clear();
        var filteredPhrases = FilterPhrases(phrases);

        #region //Sort (not in use...too slow)

        //#region Sort PhraseEdit by Text Comparison (Comparison<PhraseEdit> comparison = (a, b) =>)

        //Comparison<PhraseEdit> comparison = (a, b) =>
        //{
        //  //WE'RE GOING TO TEST CHAR ORDER IN ALPHABET
        //  string aText = a.Text.ToLower();
        //  string bText = b.Text.ToLower();

        //  //IF THEY'RE THE SAME TITLES IN LOWER CASE, THEN THEY ARE EQUAL
        //  if (aText == bText)
        //    return 0;

        //  //ONLY NEED TO TEST CHARACTERS UP TO LENGTH
        //  int shorterTitleLength = aText.Length;
        //  bool aIsShorter = true;
        //  if (bText.Length < shorterTitleLength)
        //  {
        //    shorterTitleLength = bText.Length;
        //    aIsShorter = false;
        //  }

        //  int result = 0; //assume a and b are equal (though we know they aren't if we've reached this point)

        //  for (int i = 0; i < shorterTitleLength; i++)
        //  {
        //    if (aText[i] < bText[i])
        //    {
        //      result = -1;
        //      break;
        //    }
        //    else if (aText[i] > bText[i])
        //    {
        //      result = 1;
        //      break;
        //    }
        //  }

        //  //IF THEY ARE STILL EQUAL, THEN THE SHORTER PRECEDES THE LONGER
        //  if (result == 0)
        //  {
        //    if (aIsShorter)
        //      result = -1;
        //    else
        //      result = 1;
        //  }

        //  return result;
        //};

        //#endregion
        //filteredPhrases.Sort(comparison);
        #endregion

        int counter = 0;
        int iterationsBetweenComeUpForAir = 20;
        int totalCount = filteredPhrases.Count();
        ProgressMaximum = totalCount;

        Guid thinkId = Guid.NewGuid();
        History.Events.ThinkingAboutTargetEvent.Publish(thinkId);
        foreach (var phraseEdit in filteredPhrases)
        {
          var itemViewModel = Services.Container.GetExportedValue<ViewPhrasesItemViewModel>();
          itemViewModel.PropertyChanged +=
            new System.ComponentModel.PropertyChangedEventHandler(HandleItemViewModelChanged);
          itemViewModel.Model = phraseEdit;
          Items.Add(itemViewModel);
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

        IsPopulating = false;

        //TELL HISTORY WE'RE DONE THINKING
        History.Events.ThinkedAboutTargetEvent.Publish(thinkId);

        //if (batch.Count > 0)
        //Items.AddRange(batch);
      });

      worker.RunWorkerAsync();
    }

    private IEnumerable<PhraseEdit> FilterPhrases(PhraseList phrases)
    {
      if (string.IsNullOrEmpty(FilterText))
        return phrases;//.ToList();

      var results = from phrase in phrases
                    where phrase.Text.Contains(FilterText)
                    select phrase;

      return results;//.ToList();
    }

    private void HandleItemViewModelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }
    protected virtual void HookInto(PhraseList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    protected virtual void UnhookFrom(PhraseList modelList)
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
      //  Model = (PhraseList)r2.NewObject;
      //});
      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
      NotifyOfPropertyChange(() => CanApplyFilter);

      //NotifyOfPropertyChange(() => CanCancelEdit);
    }
    protected virtual void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.ModelList);
      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => CanApplyFilter);
      //NotifyOfPropertyChange(() => CanCancelEdit);
    }

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
      //LearnLanguages.Navigation.Publish.PartSatisfied("ViewPhrases");
    }
    #endregion

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

        ModelList = (PhraseList)r.NewObject;
        //propagate new PhraseEdits to their ViewModels
        PopulateItems(ModelList);

        NotifyOfPropertyChange(() => CanSave);
      });
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
      PopulateItems(ModelList);
    }

    public bool CanInitiateDeleteChecked
    {
      get
      {
        bool somethingIsChecked = (from viewModel in Items
                                   where viewModel.IsChecked
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
      foreach (var phraseItemViewModel in Items)
      {
        phraseItemViewModel.IsChecked = false;
      }

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }


    #endregion
  }
}
