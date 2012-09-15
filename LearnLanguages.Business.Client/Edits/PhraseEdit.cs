using System;
using System.ComponentModel;
using System.Collections.Generic;
using Csla;
using Csla.Serialization;
using Csla.DataPortalClient;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.DataAccess;
using LearnLanguages.Business.Security;

namespace LearnLanguages.Business
{
  [Serializable]
  public class PhraseEdit : LearnLanguages.Common.CslaBases.BusinessBase<PhraseEdit, PhraseDto>, IHaveId
  {
    #region Factory Methods
    #region Wpf Factory Methods
#if !SILVERLIGHT

    /// <summary>
    /// Creates a new PhraseEdit sync
    /// </summary>
    public static PhraseEdit NewPhraseEdit()
    {
      return DataPortal.Create<PhraseEdit>();
    }
    ///// <summary>
    ///// Creates a new PhraseEdit sync with given id.
    ///// </summary>
    //public static PhraseEdit NewPhraseEdit(Guid id)
    //{
    //  return DataPortal.Create<PhraseEdit>(id);
    //}
    /// <summary>
    /// Fetches a PhraseEdit sync with given id
    /// </summary>
    public static PhraseEdit GetPhraseEdit(Guid id)
    {
      return DataPortal.Fetch<PhraseEdit>(id);
    }

    public static PhrasesExistRetriever PhrasesExist(string languageText, IList<string> phraseTexts)
    {
      var criteria = new Criteria.PhraseTextsCriteria(languageText, phraseTexts);
      var retriever = PhrasesExistRetriever.CreateNew(criteria);
      return retriever;
    }

#endif
    #endregion

    #region Silverlight Factory Methods
#if SILVERLIGHT

    public static void NewPhraseEdit(EventHandler<DataPortalResult<PhraseEdit>> callback)
    {
      //DataPortal.BeginCreate<PhraseEdit>(callback, DataPortal.ProxyModes.LocalOnly);
      DataPortal.BeginCreate<PhraseEdit>(callback);
    }

    public static void NewPhraseEdit(string languageText, EventHandler<DataPortalResult<PhraseEdit>> callback)
    {
      DataPortal.BeginCreate<PhraseEdit>(languageText, callback);
    }

    public static void GetPhraseEdit(Guid id, EventHandler<DataPortalResult<PhraseEdit>> callback)
    {
      DataPortal.BeginFetch<PhraseEdit>(id, callback);
    }

#endif
    #endregion
    #endregion

    #region Business Properties & Methods

    //PHRASE
    #region public string Text
    public static readonly PropertyInfo<string> TextProperty = RegisterProperty<string>(c => c.Text);
    public string Text
    {
      get { return GetProperty(TextProperty); }
      set { SetProperty(TextProperty, value); }
    }
    #endregion

    //LANGUAGE
    #region public Guid LanguageId
    public static readonly PropertyInfo<Guid> LanguageIdProperty = RegisterProperty<Guid>(c => c.LanguageId);
    //[Display(ResourceType = typeof(ModelResources), Name = "Phrase_Language_DisplayName")]
    //[Required(ErrorMessageResourceType=typeof(ModelResources), ErrorMessageResourceName="Phrase_Language_RequiredErrorMessage")]
    public Guid LanguageId
    {
      get { return GetProperty(LanguageIdProperty); }
      set { SetProperty(LanguageIdProperty, value); }
    }
    #endregion
    #region public LanguageEdit Language
    public static readonly PropertyInfo<LanguageEdit> LanguageProperty =
      RegisterProperty<LanguageEdit>(c => c.Language, RelationshipTypes.Child);
    public LanguageEdit Language
    {
      get { return GetProperty(LanguageProperty); }
      set 
      {
        SetProperty(LanguageProperty, value);
        
        if (value != null)
          LanguageId = value.Id;
        else
          LanguageId = Guid.Empty;
      }
    }
    #endregion

    //USER
    #region public Guid UserId
    public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(c => c.UserId);
    public Guid UserId
    {
      get { return GetProperty(UserIdProperty); }
      set { SetProperty(UserIdProperty, value); }
    }
    #endregion
    #region public string Username
    public static readonly PropertyInfo<string> UsernameProperty = RegisterProperty<string>(c => c.Username);
    public string Username
    {
      get { return GetProperty(UsernameProperty); }
      set { SetProperty(UsernameProperty, value); }
    }
    #endregion
    #region public UserIdentity User
    public static readonly PropertyInfo<UserIdentity> UserProperty =
      RegisterProperty<UserIdentity>(c => c.User, RelationshipTypes.Child);
    public UserIdentity User
    {
      get { return GetProperty(UserProperty); }
      private set { LoadProperty(UserProperty, value); }
    }
    #endregion

    public override void LoadFromDtoBypassPropertyChecksImpl(PhraseDto dto)
    {
      using (BypassPropertyChecks)
      {
        LoadProperty<Guid>(IdProperty, dto.Id);
        LoadProperty<string>(TextProperty, dto.Text);
        LoadProperty<Guid>(LanguageIdProperty, dto.LanguageId);
        if (dto.LanguageId != Guid.Empty)
          Language = DataPortal.FetchChild<LanguageEdit>(dto.LanguageId);
        LoadProperty<Guid>(UserIdProperty, dto.UserId);
        LoadProperty<string>(UsernameProperty, dto.Username);

        //if (!string.IsNullOrEmpty(dto.Username))
        //  User = DataPortal.FetchChild<UserIdentity>(dto.Username);
      }
    }
    public override PhraseDto CreateDto()
    {
      PhraseDto retDto = new PhraseDto(){
                                          Id = this.Id,
                                          Text = this.Text,
                                          LanguageId = this.LanguageId,
                                          UserId = this.UserId,
                                          Username = this.Username
                                        };
      return retDto;
    }

    /// <summary>
    /// Begins to persist object
    /// </summary>
    public override void BeginSave(bool forceUpdate, EventHandler<Csla.Core.SavedEventArgs> handler, object userState)
    {
      base.BeginSave(forceUpdate, handler, userState);
    }

    /// <summary>
    /// Loads the default properties, including generating a new Id, inside of a using (BypassPropertyChecks) block.
    /// </summary>
    private void LoadDefaults()
    {
      using (BypassPropertyChecks)
      {
        Id = Guid.NewGuid();
        //LanguageId = LanguageEdit.GetLanguageEdit
        Text = DalResources.DefaultNewPhraseText;
        Language = DataPortal.FetchChild<LanguageEdit>(LanguageId);
        UserId = Guid.Empty;
        Username = DalResources.DefaultNewPhraseUsername;
      }
      BusinessRules.CheckRules();
    }

    public int GetEditLevel()
    {
      return EditLevel;
    }

    /// <summary>
    /// Does CheckAuthentication().  Then Loads the phrase with the current user, userid, username.
    /// </summary>
    internal void LoadCurrentUser()
    {
      UserIdentity.CheckAuthentication();
      var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
      UserId = identity.UserId;
      Username = identity.Name;
      User = identity;
    }

    #endregion

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();

      // TODO: add validation rules
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(IdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(LanguageIdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(TextProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UserIdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UsernameProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MinLength(TextProperty, 1));
    }

    #endregion

    #region Authorization Rules

    public static void AddObjectAuthorizationRules()
    {
      // TODO: PhraseEdit Authorization: add object-level authorization rules
      // Csla.Rules.CommonRules.Required
    }

    #endregion

    #region Data Access (This is run on the server, unless run local set)

    #region Wpf DP_XYZ
#if !SILVERLIGHT

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Create()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var result = phraseDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        PhraseDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Create(string languageText)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        //LanguageEdit languageEdit = LanguageEdit.GetLanguageEdit(languageText);
        //LanguageEdit languageEdit = DataPortal.FetchChild<LanguageEdit>(languageText);

        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var result = phraseDal.New(languageText);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        PhraseDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        Result<PhraseDto> result = phraseDal.Fetch(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(result.Msg);
        }
        PhraseDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Insert()
    {
      //WE DO EXTRA CHECKING FOR PHRASE.TEXT AND PHRASE.LANGUAGE.TEXT,
      //SO WE DECIDE BETWEEN INSERT AND UPDATE WITHIN THIS METHOD.
      //THIS AS OPPOSED TO LETTING CSLA HANDLE ALL THESE DECISIONS WITH 
      //META INFORMATION.

      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var dto = CreateDto();
        var result = phraseDal.Insert(dto);

        ////IF THIS PHRASE ALREADY EXISTS IN DB (SAME TEXT AND LANGUAGETEXT)
        ////THEN WE WILL CALL AN UPDATE, OTHERWISE CONTINUE WITH INSERT.
        //PhraseDto dto = CreateDto();
        //Result<PhraseDto> result = null;

        //var retriever = PhrasesByTextAndLanguageRetriever.CreateNew(this);
        //if (retriever.RetrievedSinglePhrase != null)
        //{
        //  //PERFORM AN UPDATE
        //  //REPLACE THE DTO ID
        //  dto.Id = retriever.RetrievedSinglePhrase.Id;
        //  result = phraseDal.Update(dto);
        //}
        //else
        //{
        //  //PERFORM AN INSERT
        //  result = phraseDal.Insert(dto);
        //}

        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new InsertFailedException(result.Msg);
        }
        //SetIdBypassPropertyChecks(result.Obj.Id);
        //Loading the whole Dto now because I think the insert may affect LanguageId and UserId, and the object
        //may need to load new LanguageEdit child, new languageId, etc.
        //HACK: possible optimization available here (loading from dto instead of just setting id.)  I think a lot of these methods could benefit from this but we'll see.
        LoadFromDtoBypassPropertyChecks(result.Obj);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var dto = CreateDto();
        Result<PhraseDto> result = phraseDal.Update(dto);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new UpdateFailedException(result.Msg);
        }
        //SetIdBypassPropertyChecks(result.Obj.Id);
        //Loading the whole Dto now because I think the insert may affect LanguageId and UserId, and the object
        //may need to load new LanguageEdit child, new languageId, etc.
        LoadFromDtoBypassPropertyChecks(result.Obj);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_DeleteSelf()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var result = phraseDal.Delete(ReadProperty<Guid>(IdProperty));
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DeleteFailedException(result.Msg);
        }
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Delete(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var result = phraseDal.Delete(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DeleteFailedException(result.Msg);
        }
      }
    }

#endif
    #endregion //Wpf DP_XYZ
    
    #region Child DP_XYZ
    
#if !SILVERLIGHT
    
    //[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    //public void Child_Fetch(Guid id)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    var phraseDal = dalManager.GetProvider<IPhraseDal>();
    //    var result = phraseDal.Fetch(id);
    //    if (result.IsError)
    //      throw new FetchFailedException(result.Msg);
    //    PhraseDto dto = result.Obj;
    //    LoadFromDtoBypassPropertyChecks(dto);
    //  }
    //}

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void Child_Create()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var result = phraseDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        PhraseDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(PhraseDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);

      //using (var dalManager = DalFactory.GetDalManager())
      //{
      //  var phraseDal = dalManager.GetProvider<IPhraseDal>();
      //  var result = phraseDal.Fetch(id);
      //  if (result.IsError)
      //    throw new FetchFailedException(result.Msg);
      //  PhraseDto dto = result.Obj;
      //  LoadFromDtoBypassPropertyChecks(dto);
      //}
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var result = phraseDal.Fetch(id);
        if (result.IsError)
          throw new FetchFailedException(result.Msg);
        PhraseDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Insert()
    {
      //WE DO EXTRA CHECKING FOR PHRASE.TEXT AND PHRASE.LANGUAGE.TEXT,
      //SO WE DECIDE BETWEEN INSERT AND UPDATE WITHIN THIS METHOD.
      //THIS AS OPPOSED TO LETTING CSLA HANDLE ALL THESE DECISIONS WITH 
      //META INFORMATION.

      using (var dalManager = DalFactory.GetDalManager())
      {
        FieldManager.UpdateChildren(this);
        if (Language != null)
          LanguageId = Language.Id;

        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        using (BypassPropertyChecks)
        {
          var dto = CreateDto();
          var result = phraseDal.Insert(dto);
          ////IF THIS PHRASE ALREADY EXISTS IN DB (SAME TEXT AND LANGUAGETEXT)
          ////THEN WE WILL CALL AN UPDATE, OTHERWISE CONTINUE WITH INSERT.
          //PhraseDto dto = CreateDto();
          //Result<PhraseDto> result = null;

          //var retriever = PhrasesByTextAndLanguageRetriever.CreateNew(this);
          //if (retriever.RetrievedSinglePhrase != null)
          //{
          //  //PERFORM AN UPDATE
          //  //REPLACE THE DTO ID
          //  dto.Id = retriever.RetrievedSinglePhrase.Id;
          //  result = phraseDal.Update(dto);
          //}
          //else
          //{
          //  //PERFORM AN INSERT
          //  result = phraseDal.Insert(dto);
          //}

          if (!result.IsSuccess)
          {
            Exception error = result.GetExceptionFromInfo();
            if (error != null)
              throw error;
            else
              throw new InsertFailedException(result.Msg);
          }
          LoadFromDtoBypassPropertyChecks(result.Obj);
        }
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        FieldManager.UpdateChildren(this);
        LanguageId = Language.Id;

        var phraseDal = dalManager.GetProvider<IPhraseDal>();

        var dto = CreateDto();
        var result = phraseDal.Update(dto);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new UpdateFailedException(result.Msg);
        }
        LoadFromDtoBypassPropertyChecks(result.Obj);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_DeleteSelf()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();

        var result = phraseDal.Delete(Id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DeleteFailedException(result.Msg);
        }
      }
    }

#endif

    #endregion

    #endregion
  }
}
