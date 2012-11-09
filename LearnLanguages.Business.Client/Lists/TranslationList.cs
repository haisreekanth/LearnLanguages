using System;
using LearnLanguages.DataAccess;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using LearnLanguages.DataAccess.Exceptions;
using System.ComponentModel;

namespace LearnLanguages.Business
{
  [Serializable]
  public class TranslationList : Common.CslaBases.BusinessListBase<TranslationList, TranslationEdit, TranslationDto>
  {
    #region Factory Methods

    public static void GetAll(EventHandler<DataPortalResult<TranslationList>> callback)
    {
      DataPortal.BeginFetch<TranslationList>(callback);
    }
    
    /// <summary>
    /// Gets all of the translations that contain the given phrase, using that phrase's id.  This in 
    /// contrast to searching through all translations' texts and matching up the phrase.text or regex, etc.
    /// THIS DOES NOT THROW AN EXCEPTION IF THE PHRASE IS NOT FOUND.
    /// </summary>
    public static void GetAllTranslationsContainingPhraseById(PhraseEdit phrase, 
      EventHandler<DataPortalResult<TranslationList>> callback)
    {
      var criteria = new Criteria.PhraseCriteria(phrase);
      DataPortal.BeginFetch<TranslationList>(criteria, callback);
    }

    public static void NewTranslationList(ICollection<Guid> translationIds, EventHandler<DataPortalResult<TranslationList>> callback)
    {
      DataPortal.BeginFetch<TranslationList>(translationIds, callback);
    }

    #endregion

#if !SILVERLIGHT

    #region Factory Methods

    /// <summary>
    /// Gets all of the translations that contain the given phrase, using that phrase's id.  This in 
    /// contrast to searching through all translations' texts and matching up the phrase.text or regex, etc.
    /// THIS DOES NOT THROW AN EXCEPTION IF THE PHRASE IS NOT FOUND.
    /// </summary>
    public static TranslationList GetAllTranslationsContainingPhraseById(PhraseEdit phrase)
    {
      var criteria = new Criteria.PhraseCriteria(phrase);
      return DataPortal.Fetch<TranslationList>(criteria);
    }

    #endregion

    #region DP_xyz (including child)

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected void DataPortal_Fetch(ICollection<Guid> translationIds)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var TranslationDal = dalManager.GetProvider<ITranslationDal>();

        Result<ICollection<TranslationDto>> result = TranslationDal.Fetch(translationIds);
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
        var fetchedTranslationDtos = result.Obj;
        foreach (var translationDto in fetchedTranslationDtos)
        {
          //var TranslationEdit = DataPortal.CreateChild<TranslationEdit>(TranslationDto);
          var translationEdit = DataPortal.FetchChild<TranslationEdit>(translationDto);
          this.Add(translationEdit);
        }
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var TranslationDal = dalManager.GetProvider<ITranslationDal>();

        Result<ICollection<TranslationDto>> result = TranslationDal.GetAll();
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
        var allTranslationDtos = result.Obj;
        foreach (var translationDto in allTranslationDtos)
        {
          //var TranslationEdit = DataPortal.CreateChild<TranslationEdit>(TranslationDto);
          var translationEdit = DataPortal.FetchChild<TranslationEdit>(translationDto);
          this.Add(translationEdit);
        }
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Fetch(Criteria.PhraseCriteria phraseCriteria)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var TranslationDal = dalManager.GetProvider<ITranslationDal>();
        Result<ICollection<TranslationDto>> result = TranslationDal.FetchByPhraseId(phraseCriteria.Phrase.Id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(result.Msg);
        }
        
        //RESULT WAS SUCCESSFUL
        var translationDtos = result.Obj;
        foreach (var translationDto in translationDtos)
        {
          //var TranslationEdit = DataPortal.CreateChild<TranslationEdit>(TranslationDto);
          var translationEdit = DataPortal.FetchChild<TranslationEdit>(translationDto);
          this.Add(translationEdit);
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
    protected void Child_Fetch(ICollection<Guid> translationIds)
    {
      Items.Clear();
      foreach (var id in translationIds)
      {
        var translationEdit = DataPortal.FetchChild<TranslationEdit>(id);
        Items.Add(translationEdit);
      }
    }

    #endregion

#endif
  }
}
