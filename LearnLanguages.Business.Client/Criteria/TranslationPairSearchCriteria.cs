
using System;
using System.Net;
using Csla.Serialization;
using Csla;

namespace LearnLanguages.Business.Criteria
{
  /// <summary>
  /// Use this criteria for conducting translation searches.
  /// </summary>
  [Serializable]
  public class TranslationPairSearchCriteria : CriteriaBase<TranslationPairSearchCriteria>
  {
    /// <summary>
    /// DON'T USE THIS CTOR.  THIS IS A READ ONLY CRITERIA CLASS.  THIS CTOR IS ONLY HERE
    /// BECAUSE SERIALIZATION REQUIRES IT.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public TranslationPairSearchCriteria()
    {
      //required for serialization
    }

    public TranslationPairSearchCriteria(PhraseEdit phraseA, PhraseEdit phraseB, string contextText = "")
    {
      PhraseA = phraseA;
      PhraseB = phraseB;
      ContextText = contextText;
    }

    public static readonly PropertyInfo<PhraseEdit> PhraseAProperty = RegisterProperty<PhraseEdit>(c => c.PhraseA);
    public PhraseEdit PhraseA
    {
      get { return ReadProperty(PhraseAProperty); }
      private set { LoadProperty(PhraseAProperty, value); }
    }

    public static readonly PropertyInfo<PhraseEdit> PhraseBProperty = RegisterProperty<PhraseEdit>(c => c.PhraseB);
    public PhraseEdit PhraseB
    {
      get { return ReadProperty(PhraseBProperty); }
      private set { LoadProperty(PhraseBProperty, value); }
    }

    public static readonly PropertyInfo<string> ContextTextProperty =
      RegisterProperty<string>(c => c.ContextText);
    public string ContextText
    {
      get { return ReadProperty(ContextTextProperty); }
      private set { LoadProperty(ContextTextProperty, value); }
    }
  }
}
