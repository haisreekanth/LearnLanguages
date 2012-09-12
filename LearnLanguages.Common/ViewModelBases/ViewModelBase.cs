using System;
using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;
using System.Windows;

namespace LearnLanguages.Common.ViewModelBases
{
  public abstract class ViewModelBase : Screen, IViewModelBase
  {
    public ViewModelBase()
    {
      //Services.Container.SatisfyImportsOnce(this);
    }

    /// <summary>
    /// Use this to configure the ViewModel's properties.  
    /// The intended format (I haven't implemented it yet) is key= property name, value = property value as string.
    /// Return true if query string was loaded correctly.
    /// Default base implementation just returns true, ignoring the queryString parameter.
    /// </summary>
    public virtual bool LoadFromUri(Uri uri)
    {
      return true;
    }

    public virtual void OnImportsSatisfied()
    {
      //do nothing by default.
    }
    ///// <summary>
    ///// Publish Parts Satisfied to EventAggregator.
    ///// </summary>
    //public virtual void OnImportsSatisfied()
    //{
    //  Services.EventAggregator.Publish(
    //    new EventMessages.PartSatisfiedEventMessage(ViewModelBase.GetCoreViewModelName(GetType().Name, false)));
    //}

    ///// <summary>
    ///// Retrieves core test of ViewModel's type name, using convention "[CoreText]ViewModel".  
    ///// Returns [Text], optionally inserting spaces
    ///// before any capital letters if withSpaces == true.  
    ///// E.g. AddNewViewModel, withSpaces = false returns "AddNew".
    ///// AddNewViewModel, withSpaces = true returns "Add New".
    ///// </summary>
    ///// <returns>
    ///// Returns [Text], optionally inserting spaces
    ///// before any capital letters if withSpaces == true.  
    ///// E.g. AddNewViewModel, withSpaces = false returns "AddNew".
    ///// AddNewViewModel, withSpaces = true returns "Add New".
    ///// </returns>
    //public static string GetCoreViewModelName(bool withSpaces = false)
    //  where TViewModel : IViewModelBase
    //{
    //  return GetCoreViewModelName(typeof(TViewModel).Name, withSpaces);
    //}
    public static string GetCoreViewModelName(Type viewModelType, bool withSpaces = false)
    {
      return GetCoreViewModelName(viewModelType.Name, withSpaces);
    }

    private static string GetCoreViewModelName(string viewModelTypeName, bool withSpaces = false)
    {
      string retCoreViewModelName = "";

      //var thisTypeName = this.GetType().Name;
      var thisTypeName = viewModelTypeName;

      var thisTypeMinusViewModelText =
        thisTypeName.Replace(@"ViewModel", "");
      //var thisTypeMinusViewModelText = 
      //thisTypeName.Substring(0, (thisTypeName.Length - @"ViewModel".Length));

      retCoreViewModelName = thisTypeMinusViewModelText;

      if (withSpaces)
      {
        //ADD SPACES PRECEDING CAPITAL LETTERS
        var textStringWithSpaces = "";
        textStringWithSpaces += thisTypeMinusViewModelText[0];//The first letter has no preceding space
        for (int i = 1; i < thisTypeMinusViewModelText.Length; i++)
        {
          var character = thisTypeMinusViewModelText[i];
          if (Char.IsLower(character))
            textStringWithSpaces += character;
          else
            textStringWithSpaces += " " + character;
        }
        retCoreViewModelName = textStringWithSpaces;
      }

      return retCoreViewModelName;
    }

    private bool _ShowGridLines = bool.Parse(CommonResources.ShowGridLines);
    public bool ShowGridLines
    {
      get { return _ShowGridLines; }
      set
      {
        if (value != _ShowGridLines)
        {
          _ShowGridLines = value;
          NotifyOfPropertyChange(() => ShowGridLines);
        }
      }
    }

    private Visibility _ViewModelVisibility = Visibility.Visible;
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
