
using System;
using System.Net;
using Csla.Serialization;
using Csla;
using System.Collections.Generic;
using Csla.Core;

namespace LearnLanguages.Business.Criteria
{
  /// <summary>
  /// I use this in making LineList
  /// </summary>
  [Serializable]
  public class LineInfosCriteria : CriteriaBase<LineInfosCriteria>
  {
    /// <summary>
    /// For Serialization.  Don't use this.
    /// </summary>
    public LineInfosCriteria()
    {
      //required for serialization
    }
    
    /// <summary>
    /// Use this if you have a list of lines (linenumber, text) in a given language.
    /// </summary>
    public LineInfosCriteria(string languageText, IDictionary<int, string> lineInfos)
    {
      LanguageText = languageText;
      LineInfos = new MobileDictionary<int, string>();
      foreach (var line in lineInfos)
      {
        LineInfos.Add(line.Key, line.Value);
      }
    }

    public static readonly PropertyInfo<IDictionary<int, string>> LineInfosProperty = 
      RegisterProperty<IDictionary<int, string>>(c => c.LineInfos);
    public IDictionary<int, string> LineInfos
    {
      get { return ReadProperty(LineInfosProperty); }
      private set { LoadProperty(LineInfosProperty, value); }
    }

    public static readonly PropertyInfo<string> LanguageTextProperty = RegisterProperty<string>(c => c.LanguageText);
    public string LanguageText
    {
      get { return ReadProperty(LanguageTextProperty); }
      private set { LoadProperty(LanguageTextProperty, value); }
    }
  }
}
