using System;
using System.Linq;
using System.Net;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using Csla.Core;

namespace LearnLanguages.Business
{

  /// <summary>
   //Takes a list of phraseTexts, and prunes that list to phraseTexts that do NOT have an 
   //exactly corresponding phrase in the data store, and stores those in PrunedPhraseTexts.  
   //The PhraseTextsCriteria can have duplicates, as this will remove those duplicates 
   //when checking against the data store.
  /// </summary>
  [Serializable]
  public class RemoveAlreadyExistingPhraseTextsCommand : CommandBase<RemoveAlreadyExistingPhraseTextsCommand>
  {
    #region Ctors and Init

    public RemoveAlreadyExistingPhraseTextsCommand() { /* Required for MobileFormatter */   }

    public RemoveAlreadyExistingPhraseTextsCommand(string languageText, IList<string> phraseTexts)
    {
      OriginalPhraseTexts = new MobileList<string>(phraseTexts);
    }

    #endregion

    #region Factory Methods

#if !SILVERLIGHT
    public static bool Execute(string languageText, IList<string> phraseTexts) 
    { 
      var cmd = new RemoveAlreadyExistingPhraseTextsCommand(languageText, phraseTexts); 
      cmd.BeforeServer(); 
      cmd = DataPortal.Execute<RemoveAlreadyExistingPhraseTextsCommand>(cmd); 
      cmd.AfterServer(); 
      return cmd.Result; 
    } 
#endif

    public static void BeginExecute(string languageText, IList<string> phraseTexts, EventHandler<DataPortalResult<RemoveAlreadyExistingPhraseTextsCommand>> callback)
    {
      var cmd = new RemoveAlreadyExistingPhraseTextsCommand(languageText, phraseTexts);
      cmd.BeforeServer();
      DataPortal.BeginExecute<RemoveAlreadyExistingPhraseTextsCommand>(cmd, (o, e) =>
      {
        if (e.Error != null)
          throw e.Error;

        e.Object.AfterServer();
        callback(o, e);
      });
    }

    #endregion

    #region Properties

    public static readonly PropertyInfo<string> LanguageTextProperty = RegisterProperty<string>(c => c.LanguageText);
    public string LanguageText
    {
      get { return ReadProperty(LanguageTextProperty); }
      private set { LoadProperty(LanguageTextProperty, value); }
    }

    public static readonly PropertyInfo<MobileList<string>> PrunedPhraseTextsProperty = 
      RegisterProperty<MobileList<string>>(c => c.PrunedPhraseTexts);
    public MobileList<string> PrunedPhraseTexts
    {
      get { return ReadProperty(PrunedPhraseTextsProperty); }
      private set { LoadProperty(PrunedPhraseTextsProperty, value); }
    }

    public static readonly PropertyInfo<MobileList<string>> OriginalPhraseTextsProperty =
      RegisterProperty<MobileList<string>>(c => c.OriginalPhraseTexts);
    public MobileList<string> OriginalPhraseTexts
    {
      get { return ReadProperty(OriginalPhraseTextsProperty); }
      private set { LoadProperty(OriginalPhraseTextsProperty, value); }
    }

    public static PropertyInfo<bool> ResultProperty = RegisterProperty<bool>(c => c.Result);
    private bool Result
    {
      get { return ReadProperty(ResultProperty); }
      set { LoadProperty(ResultProperty, value); }
    }

    #endregion
    
    #region Execute

    private void BeforeServer()
    {
      // TODO: implement code to run on client 
      // before server is called 
    }

    private void AfterServer()
    {
      // TODO: implement code to run on client 
      // after server is called 
    }

#if !SILVERLIGHT
    protected override void DataPortal_Execute()
    {
      Result = false;
      try
      {
        var phrasesExist = PhraseEdit.PhrasesExist(LanguageText, OriginalPhraseTexts);

        if (PrunedPhraseTexts == null)
          PrunedPhraseTexts = new MobileList<string>();
        var noDuplicatesOriginalPhraseTexts = OriginalPhraseTexts.Distinct();
        foreach (var phraseText in noDuplicatesOriginalPhraseTexts)
        {
          if (phrasesExist.ExistenceDictionary[phraseText] == false)
            PrunedPhraseTexts.Add(phraseText);
        }
        Result = true;
      }
      catch (Exception)
      {
        Result = false;
        throw;
      }
    }
#endif

    #endregion
  }
}
