using System;
using System.Net;
using Csla;
using Csla.Serialization;

namespace LearnLanguages.Business
{

  /// <summary>
  /// This class takes a given phrase and loads it with the default language.
  /// </summary>
  [Serializable]
  public class PhraseDefaultSetterCommand : CommandBase<PhraseDefaultSetterCommand>
  {
    public PhraseDefaultSetterCommand() { /* Required for MobileFormatter */   }

    public PhraseDefaultSetterCommand(PhraseEdit phrase)
    {
      Phrase = phrase;
    }


#if !SILVERLIGHT 
    public static bool Execute(PhraseEdit phrase) 
    { 
      var cmd = new PhraseDefaultSetterCommand(phrase); 
      cmd.BeforeServer(); 
      cmd = DataPortal.Execute<PhraseDefaultSetterCommand>(cmd); 
      cmd.AfterServer(); 
      return cmd.Result; 
    } 
#endif

    public static void BeginExecute(PhraseEdit phrase, EventHandler<DataPortalResult<PhraseDefaultSetterCommand>> callback)
    {
      var cmd = new PhraseDefaultSetterCommand(phrase);
      cmd.BeforeServer();
      DataPortal.BeginExecute<PhraseDefaultSetterCommand>(cmd, (o, e) =>
      {
        if (e.Error != null)
          throw e.Error;

        e.Object.AfterServer();
        callback(o, e);
      });
    }

    public static readonly PropertyInfo<PhraseEdit> PhraseProperty = RegisterProperty<PhraseEdit>(c => c.Phrase);
    public PhraseEdit Phrase
    {
      get { return ReadProperty(PhraseProperty); }
      private set { LoadProperty(PhraseProperty, value); }
    }

    public static PropertyInfo<bool> ResultProperty = RegisterProperty<bool>(c => c.Result);
    private bool Result
    {
      get { return ReadProperty(ResultProperty); }
      set { LoadProperty(ResultProperty, value); }
    }

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
      //CURRENT USER
      Phrase.LoadCurrentUser();

      //DEFAULT LANGUAGE
      var languageId = LanguageEdit.GetDefaultLanguageId();
      Phrase.LanguageId = languageId;
      Phrase.Language = DataPortal.FetchChild<LanguageEdit>(Phrase.LanguageId);
      Result = true;
    }
#endif
  }
}
