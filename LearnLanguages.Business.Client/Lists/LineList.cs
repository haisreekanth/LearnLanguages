using System;
using System.Linq;

using LearnLanguages.DataAccess;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using LearnLanguages.DataAccess.Exceptions;
using System.ComponentModel;
using LearnLanguages.Business.Security;

namespace LearnLanguages.Business
{
  [Serializable]
  public class LineList : Common.CslaBases.BusinessListBase<LineList, LineEdit, LineDto>
  {
    #region Factory Methods

    public static void GetAll(EventHandler<DataPortalResult<LineList>> callback)
    {
      DataPortal.BeginFetch<LineList>(callback);
    }

    public static void NewLineList(ICollection<Guid> lineIds, EventHandler<DataPortalResult<LineList>> callback)
    {
      DataPortal.BeginFetch<LineList>(lineIds, callback);
    }

    public static void NewLineList(Criteria.LineInfosCriteria lineInfos, EventHandler<DataPortalResult<LineList>> callback)
    {
      DataPortal.BeginCreate<LineList>(lineInfos, callback);
    }

    /// <summary>
    /// Just news up a LineList.
    /// </summary>
    public static LineList NewLineListNewedUpOnly()
    {
      return new LineList();
    }

#if SILVERLIGHT
    /// <summary>
    /// Runs locally.
    /// </summary>
    /// <param name="callback"></param>
    public static void NewLineList(EventHandler<DataPortalResult<LineList>> callback)
    {
      DataPortal.BeginCreate<LineList>(callback, DataPortal.ProxyModes.LocalOnly);
    }
#else
    /// <summary>
    /// Runs locally.
    /// </summary>
    [RunLocal]
    public static void NewLineList(EventHandler<DataPortalResult<LineList>> callback)
    {
      DataPortal.BeginCreate<LineList>(callback);
    }
#endif

    #endregion

    #region Data Portal methods (including child)

#if !SILVERLIGHT
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Create(Criteria.LineInfosCriteria lineInfosCriteria)
    {
      if (lineInfosCriteria.LineInfos.Count == 0)
        throw new ArgumentException("lineInfosCriteria");
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageText = lineInfosCriteria.LanguageText;
        var language = DataPortal.FetchChild<LanguageEdit>(languageText);
        //var languageDal = dalManager.GetProvider<ILanguageDal>();
        //var languageResult = languageDal.Fetch(languageText);
        //LanguageEdit language = null;
        //if (!languageResult.IsSuccess)
        //{
        //  var exception = languageResult.GetExceptionFromInfo();
        //  if (exception == null)
        //    throw new LanguageTextNotFoundException(languageText);
        //  else if (exception.Message.Contains(LanguageTextNotFoundException.GetDefaultErrorMessage(languageText)))
        //  {
        //    //IF THE LANGUAGE TEXT DOES NOT EXIST, CREATE IT
        //    //CAN'T CHECK FOR DATA EXCEPTION TYPE BECAUSE IT WILL BE A GENERAL CSLA DATAPORTAL EXCEPTION
        //    language = LanguageEdit.NewLanguageEdit();
        //    language.Text = languageText;
        //    //language = language.Save();
        //  }
        //  else
        //    throw new CreateFailedException(DataAccess.DalResources.ErrorMsgLanguageTextProblemWhileCreatingLineInfos);
        //}

        //var languageDto = languageResult.Obj;
        //language = LanguageEdit.NewLanguageEdit(languageDto);
        //language.LoadFromDtoBypassPropertyChecks(languageDto);

        //WE NOW HAVE OUR LANGUAGEEDIT THAT WILL BE USED FOR ALL PHRASE TEXTS.
        var LineDal = dalManager.GetProvider<ILineDal>();

        //LineList newLineList = LineList.NewLineList();
        foreach (var lineInfo in lineInfosCriteria.LineInfos)
        {
          //FOREACH LINEINFO, CREATE A PHRASE, THEN CREATE A LINE CONTAINING THAT PHRASE WITH GIVEN LINE NUMBER
          var lineNumber = lineInfo.Key;
          var lineText = lineInfo.Value;
          if (string.IsNullOrEmpty(lineText) || lineNumber < 0)
            continue;

          PhraseEdit phraseEdit = DataPortal.CreateChild<PhraseEdit>();
          phraseEdit.Language = language;
          phraseEdit.Text = lineText;

          LineEdit lineEdit = DataPortal.CreateChild<LineEdit>();
          lineEdit.Phrase = phraseEdit;
          lineEdit.LineNumber = lineNumber;

          Add(lineEdit);
        }
      }
    }
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch(ICollection<Guid> lineIds)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var lineDal = dalManager.GetProvider<ILineDal>();

        Result<ICollection<LineDto>> result = lineDal.Fetch(lineIds);
        if (!result.IsSuccess || result.IsError)
        {
          if (result.Info != null)
          {
            var ex = result.GetExceptionFromInfo();
            if (ex != null)
              throw new FetchFailedException(ex.Message);
            else
              throw new FetchFailedException();
          }
          else
            throw new FetchFailedException();
        }

        //RESULT WAS SUCCESSFUL
        var fetchedLineDtos = result.Obj;
        foreach (var lineDto in fetchedLineDtos)
        {
          //var LineEdit = DataPortal.CreateChild<LineEdit>(LineDto);
          var lineEdit = DataPortal.FetchChild<LineEdit>(lineDto);
          this.Add(lineEdit);
        }
      }
    }
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var lineDal = dalManager.GetProvider<ILineDal>();

        Result<ICollection<LineDto>> result = lineDal.GetAll();
        if (!result.IsSuccess || result.IsError)
        {
          if (result.Info != null)
          {
            var ex = result.GetExceptionFromInfo();
            if (ex != null)
              throw new FetchFailedException(ex.Message);
            else
              throw new FetchFailedException();
          }
          else
            throw new FetchFailedException();
        }

        //RESULT WAS SUCCESSFUL
        var allLineDtos = result.Obj;
        foreach (var LineDto in allLineDtos)
        {
          //var LineEdit = DataPortal.CreateChild<LineEdit>(LineDto);
          var LineEdit = DataPortal.FetchChild<LineEdit>(LineDto);
          this.Add(LineEdit);
        }
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        base.Child_Update();
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Child_Fetch(ICollection<Guid> lineIds)
    {
      Items.Clear();
      foreach (var id in lineIds)
      {
        var lineEdit = DataPortal.FetchChild<LineEdit>(id);
        Items.Add(lineEdit);
      }
    }
#endif

    #endregion

    #region AddNewCore

#if SILVERLIGHT
    protected override void AddNewCore()
    {
      AddedNew += LineList_AddedNew; 
      base.AddNewCore();
      AddedNew -= LineList_AddedNew;
    }

    private void LineList_AddedNew(object sender, Csla.Core.AddedNewEventArgs<LineEdit> e)
    {
      //UserIdentity.CheckAuthentication();
      var lineEdit = e.NewObject;
      lineEdit.LoadCurrentUser();
      //var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
      //lineEdit.UserId = identity.UserId;
      //lineEdit.Username = identity.Name;
    }
#else
    protected override LineEdit AddNewCore()
    {
      //SERVER
      var lineEdit = base.AddNewCore();
      lineEdit.LoadCurrentUser();
      return lineEdit;
    }
#endif

    #endregion

    protected override void OnChildChanged(Csla.Core.ChildChangedEventArgs e)
    {
      base.OnChildChanged(e);
      //if (e.ChildObject != null)
      //  (Csla.Core.BusinessBase)e.ChildObject.BusinessRules.CheckRules();
    }

    /// <summary>
    /// LineNumbers are zero-based index in line list.
    /// </summary>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public LineEdit GetLine(int lineNumber)
    {
      if (lineNumber > Items.Count - 1)
        return null;

      var lineFound = (from line in Items
                       where line.LineNumber == lineNumber
                       select line).FirstOrDefault();

      return lineFound;
    }
  }
}
