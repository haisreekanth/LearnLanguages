using System;
using System.Linq;

using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using LearnLanguages.DataAccess;
using LearnLanguages.Business.Security;
using LearnLanguages.DataAccess.Exceptions;

namespace LearnLanguages.Business
{
  [Serializable]
  public class PhraseBeliefList : Common.CslaBases.BusinessListBase<PhraseBeliefList, PhraseBeliefEdit, PhraseBeliefDto>
  {
    #region Factory Methods

    public static void GetAll(EventHandler<DataPortalResult<PhraseBeliefList>> callback)
    {
      DataPortal.BeginFetch<PhraseBeliefList>(callback);
    }

    /// <summary>
    /// Gets all beliefs about a given phrase, using phrase.Id
    /// </summary>
    public static void GetBeliefsAboutPhrase(Guid phraseId, 
      EventHandler<DataPortalResult<PhraseBeliefList>> callback)
    {
      var criteria = new Criteria.PhraseIdCriteria(phraseId);
      DataPortal.BeginFetch<PhraseBeliefList>(criteria, callback);
    }
    
    /// <summary>
    /// Just news up a PhraseBeliefList.
    /// </summary>
    public static PhraseBeliefList NewPhraseBeliefListNewedUpOnly()
    {
      return new PhraseBeliefList();
    }

#if SILVERLIGHT
    /// <summary>
    /// Runs locally.
    /// </summary>
    /// <param name="callback"></param>
    public static void NewPhraseBeliefList(EventHandler<DataPortalResult<PhraseBeliefList>> callback)
    {
      DataPortal.BeginCreate<PhraseBeliefList>(callback, DataPortal.ProxyModes.LocalOnly);
    }
#else
    /// <summary>
    /// Runs locally.
    /// </summary>
    [RunLocal]
    public static void NewPhraseBeliefList(EventHandler<DataPortalResult<PhraseBeliefList>> callback)
    {
      DataPortal.BeginCreate<PhraseBeliefList>(callback);
    }
#endif

    #endregion

    #region Data Portal methods (including child)

#if !SILVERLIGHT
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch(Criteria.PhraseIdCriteria phraseIdCriteria)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();

        var phraseId = phraseIdCriteria.PhraseId;
        Result<ICollection<PhraseBeliefDto>> result = beliefDal.FetchAllRelatedToPhrase(phraseId);
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
        var fetchedPhraseBeliefDtos = result.Obj;
        LoadDtos(fetchedPhraseBeliefDtos);
      }
    }

    private void LoadDtos(ICollection<PhraseBeliefDto> dtos)
    {
      Items.Clear();
      foreach (var beliefDto in dtos)
      {
        //var PhraseBeliefEdit = DataPortal.CreateChild<PhraseBeliefEdit>(PhraseBeliefDto);
        var beliefEdit = DataPortal.FetchChild<PhraseBeliefEdit>(beliefDto);
        this.Add(beliefEdit);
      }
    }
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();

        Result<ICollection<PhraseBeliefDto>> result = beliefDal.GetAll();
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
        var allPhraseBeliefDtos = result.Obj;
        LoadDtos(allPhraseBeliefDtos);
        //foreach (var PhraseBeliefDto in allPhraseBeliefDtos)
        //{
        //  //var PhraseBeliefEdit = DataPortal.CreateChild<PhraseBeliefEdit>(PhraseBeliefDto);
        //  var PhraseBeliefEdit = DataPortal.FetchChild<PhraseBeliefEdit>(PhraseBeliefDto);
        //  this.Add(PhraseBeliefEdit);
        //}
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
    public void Child_Fetch(Criteria.PhraseIdCriteria phraseIdCriteria)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();

        var phraseId = phraseIdCriteria.PhraseId;
        Result<ICollection<PhraseBeliefDto>> result = beliefDal.FetchAllRelatedToPhrase(phraseId);
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
        var fetchedPhraseBeliefDtos = result.Obj;
        LoadDtos(fetchedPhraseBeliefDtos);
      }
    }
#endif

    #endregion

    #region AddNewCore

#if SILVERLIGHT
    protected override void AddNewCore()
    {
      AddedNew += PhraseBeliefList_AddedNew; 
      base.AddNewCore();
      AddedNew -= PhraseBeliefList_AddedNew;
    }

    private void PhraseBeliefList_AddedNew(object sender, Csla.Core.AddedNewEventArgs<PhraseBeliefEdit> e)
    {
      //UserIdentity.CheckAuthentication();
      var beliefEdit = e.NewObject;
      beliefEdit.LoadCurrentUser();
      //var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
      //beliefEdit.UserId = identity.UserId;
      //beliefEdit.Username = identity.Name;
    }
#else //SERVER
    protected override PhraseBeliefEdit AddNewCore()
    {
      var beliefEdit = base.AddNewCore();
      beliefEdit.LoadCurrentUser();
      return beliefEdit;
    }
#endif

    #endregion

    protected override void OnChildChanged(Csla.Core.ChildChangedEventArgs e)
    {
      base.OnChildChanged(e);
      //if (e.ChildObject != null)
      //  (Csla.Core.BusinessBase)e.ChildObject.BusinessRules.CheckRules();
    }

  }
}
