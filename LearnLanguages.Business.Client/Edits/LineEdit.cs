using System;
using Csla;
using System.ComponentModel;
using Csla.Serialization;
using Csla.DataPortalClient;
using LearnLanguages.Common.Interfaces;
#if !SILVERLIGHT
using LearnLanguages.DataAccess.Exceptions;
#endif
using LearnLanguages.DataAccess;
using LearnLanguages.Business.Security;


namespace LearnLanguages.Business
{
  [Serializable]
  public class LineEdit : LearnLanguages.Common.CslaBases.BusinessBase<LineEdit, LineDto>, IHaveId
  {
    #region Factory Methods
    #region Wpf Factory Methods
#if !SILVERLIGHT

    /// <summary>
    /// Creates a new LineEdit sync
    /// </summary>
    public static LineEdit NewLineEdit()
    {
      return DataPortal.Create<LineEdit>();
    }
    ///// <summary>
    ///// Creates a new LineEdit sync with given id.
    ///// </summary>
    //public static LineEdit NewLineEdit(Guid id)
    //{
    //  return DataPortal.Create<LineEdit>(id);
    //}
    /// <summary>
    /// Fetches a LineEdit sync with given id
    /// </summary>
    public static LineEdit GetLineEdit(Guid id)
    {
      return DataPortal.Fetch<LineEdit>(id);
    }

#endif
    #endregion

    #region Silverlight Factory Methods
#if SILVERLIGHT

    public static void NewLineEdit(EventHandler<DataPortalResult<LineEdit>> callback)
    {
      //DataPortal.BeginCreate<LineEdit>(callback, DataPortal.ProxyModes.LocalOnly);
      DataPortal.BeginCreate<LineEdit>(callback);
    }

    public static void NewLineEdit(string languageText, EventHandler<DataPortalResult<LineEdit>> callback)
    {
      DataPortal.BeginCreate<LineEdit>(languageText, callback);
    }

    public static void GetLineEdit(Guid id, EventHandler<DataPortalResult<LineEdit>> callback)
    {
      DataPortal.BeginFetch<LineEdit>(id, callback);
    }

#endif
    #endregion
    #endregion

    #region Business Properties & Methods
    
    //PHRASE
    #region public Guid PhraseId
    public static readonly PropertyInfo<Guid> PhraseIdProperty = RegisterProperty<Guid>(c => c.PhraseId);
    //[Display(ResourceType = typeof(ModelResources), Name = "Line_Phrase_DisplayName")]
    //[Required(ErrorMessageResourceType=typeof(ModelResources), ErrorMessageResourceName="Line_Phrase_RequiredErrorMessage")]
    public Guid PhraseId
    {
      get { return GetProperty(PhraseIdProperty); }
      set { SetProperty(PhraseIdProperty, value); }
    }
    #endregion
    #region public PhraseEdit Phrase
    public static readonly PropertyInfo<PhraseEdit> PhraseProperty =
      RegisterProperty<PhraseEdit>(c => c.Phrase, RelationshipTypes.Child);
    public PhraseEdit Phrase
    {
      get { return GetProperty(PhraseProperty); }
      set 
      {
        LoadProperty(PhraseProperty, value);

        if (value != null)
          PhraseId = value.Id;
        else
          PhraseId = Guid.Empty;
      }
    }
    #endregion

    //LINE NUMBER
    #region public int LineNumber
    public static readonly PropertyInfo<int> LineNumberProperty = RegisterProperty<int>(c => c.LineNumber);
    public int LineNumber
    {
      get { return GetProperty(LineNumberProperty); }
      set { SetProperty(LineNumberProperty, value); }
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

    public override void LoadFromDtoBypassPropertyChecksImpl(LineDto dto)
    {
      using (BypassPropertyChecks)
      {
        //ID
        LoadProperty<Guid>(IdProperty, dto.Id);

        //PHRASE
        LoadProperty<Guid>(PhraseIdProperty, dto.PhraseId);
        if (dto.PhraseId != Guid.Empty)
          Phrase = DataPortal.FetchChild<PhraseEdit>(dto.PhraseId);

        //LINE NUMBER
        LoadProperty<int>(LineNumberProperty, dto.LineNumber);

        //USER
        LoadProperty<Guid>(UserIdProperty, dto.UserId);
        LoadProperty<string>(UsernameProperty, dto.Username);
        if (!string.IsNullOrEmpty(dto.Username))
          User = DataPortal.FetchChild<UserIdentity>(dto.Username);
      }
    }
    public override LineDto CreateDto()
    {
      LineDto retDto = new LineDto(){
                                          Id = this.Id,
                                          PhraseId = this.PhraseId,
                                          LineNumber = this.LineNumber,
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
        PhraseId = Guid.Empty;
        LineNumber = int.Parse(DalResources.DefaultLineNumber);
        UserId = Guid.Empty;
        Username = DalResources.DefaultNewLineUsername;
      }
      BusinessRules.CheckRules();
    }

    public int GetEditLevel()
    {
      return EditLevel;
    }

    /// <summary>
    /// Does CheckAuthentication().  Then Loads the line with the current user, userid, username.
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
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(PhraseIdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UserIdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UsernameProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MinValue<int>(LineNumberProperty, 0));
    }

    #endregion

    #region Authorization Rules

    public static void AddObjectAuthorizationRules()
    {
      // TODO: LineEdit Authorization: add object-level authorization rules
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
        var lineDal = dalManager.GetProvider<ILineDal>();
        var result = lineDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        LineDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);

        //PHRASE CHILD
        Phrase = DataPortal.CreateChild<PhraseEdit>();
      }
    }
    //[Transactional(TransactionalTypes.TransactionScope)]
    //protected void DataPortal_Create(string languageText)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    //LanguageEdit languageEdit = LanguageEdit.GetLanguageEdit(languageText);
    //    //LanguageEdit languageEdit = DataPortal.FetchChild<LanguageEdit>(languageText);

    //    var lineDal = dalManager.GetProvider<ILineDal>();
    //    var result = lineDal.New(languageText);
    //    if (!result.IsSuccess)
    //    {
    //      Exception error = result.GetExceptionFromInfo();
    //      if (error != null)
    //        throw error;
    //      else
    //        throw new CreateFailedException(result.Msg);
    //    }
    //    LineDto dto = result.Obj;
    //    LoadFromDtoBypassPropertyChecks(dto);
    //  }
    //}

    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var lineDal = dalManager.GetProvider<ILineDal>();
        Result<LineDto> result = lineDal.Fetch(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(result.Msg);
        }
        LineDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Insert()
    {
      //Dal is responsible for setting new Id
      //LineDto dto = new LineDto()
      //{
      //  Id = this.Id,
      //  LanguageId = this.LanguageId,
      //  Text = this.Text
      //};
      using (var dalManager = DalFactory.GetDalManager())
      {
        //NEED TO UPDATE CHILDREN FIRST
        FieldManager.UpdateChildren(this);

        //UPDATE CHILDREN WILL INSERT/UPDATE PHRASE AS NEEDED AND THUS CHANGE PHRASE ID
        PhraseId = Phrase.Id;

        //UPDATE OUR LINE ITSELF
        var lineDal = dalManager.GetProvider<ILineDal>();
        var dto = CreateDto();
        var result = lineDal.Insert(dto);
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
        LoadFromDtoBypassPropertyChecks(result.Obj);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var lineDal = dalManager.GetProvider<ILineDal>();
        var dto = CreateDto();
        Result<LineDto> result = lineDal.Update(dto);
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
        var lineDal = dalManager.GetProvider<ILineDal>();
        var result = lineDal.Delete(ReadProperty<Guid>(IdProperty));
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
        var lineDal = dalManager.GetProvider<ILineDal>();
        var result = lineDal.Delete(id);
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
    
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void Child_Create()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var lineDal = dalManager.GetProvider<ILineDal>();
        var result = lineDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        LineDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(LineDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);

      //using (var dalManager = DalFactory.GetDalManager())
      //{
      //  var lineDal = dalManager.GetProvider<ILineDal>();
      //  var result = lineDal.Fetch(id);
      //  if (result.IsError)
      //    throw new FetchFailedException(result.Msg);
      //  LineDto dto = result.Obj;
      //  LoadFromDtoBypassPropertyChecks(dto);
      //}
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var lineDal = dalManager.GetProvider<ILineDal>();
        var result = lineDal.Fetch(id);
        if (result.IsError)
          throw new FetchFailedException(result.Msg);
        LineDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Insert()
    {   
      using (var dalManager = DalFactory.GetDalManager())
      {
        var lineDal = dalManager.GetProvider<ILineDal>();
        using (BypassPropertyChecks)
        {
          FieldManager.UpdateChildren(this);
          PhraseId = Phrase.Id;

          var dto = CreateDto();
          var result = lineDal.Insert(dto);
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
        PhraseId = Phrase.Id;

        var lineDal = dalManager.GetProvider<ILineDal>();

        var dto = CreateDto();
        var result = lineDal.Update(dto);
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
        var lineDal = dalManager.GetProvider<ILineDal>();

        var result = lineDal.Delete(Id);
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
