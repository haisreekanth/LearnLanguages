using System;
using Csla;
using System.ComponentModel;
using Csla.Serialization;
using Csla.DataPortalClient;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.DataAccess;
using LearnLanguages.Business.Security;


namespace LearnLanguages.Business
{
  [Serializable]
  public class StudyDataEdit : Common.CslaBases.BusinessBase<StudyDataEdit, StudyDataDto>,
                               IHaveId
  {
    #region Factory Methods
    #region Wpf Factory Methods
#if !SILVERLIGHT

    /// <summary>
    /// Creates a new StudyDataEdit sync
    /// </summary>
    public static StudyDataEdit NewStudyDataEdit()
    {
      return DataPortal.Create<StudyDataEdit>();
    }
    ///// <summary>
    ///// Fetches a StudyDataEdit sync with given id
    ///// </summary>
    //public static StudyDataEdit GetStudyDataEditForCurrentUser()
    //{
    //  return DataPortal.Fetch<StudyDataEdit>();
    //}

#endif
    #endregion

    #region Silverlight Factory Methods
#if SILVERLIGHT

    //public static void NewStudyDataEdit(EventHandler<DataPortalResult<StudyDataEdit>> callback)
    //{
    //  //DataPortal.BeginCreate<StudyDataEdit>(callback, DataPortal.ProxyModes.LocalOnly);
    //  DataPortal.BeginCreate<StudyDataEdit>(callback);
    //}

    //public static void GetStudyDataEdit(Guid id, EventHandler<DataPortalResult<StudyDataEdit>> callback)
    //{
    //  DataPortal.BeginFetch<StudyDataEdit>(id, callback);
    //}

    //public static void GetStudyDataEditForCurrentUser(EventHandler<DataPortalResult<StudyDataEdit>> callback)
    //{
    //  DataPortal.BeginFetch<StudyDataEdit>(callback);
    //}
#endif
    #endregion
    #endregion

    #region Business Properties & Methods
    //NATIVE LANGUAGE TEXT
    #region public string NativeLanguageText
    public static readonly PropertyInfo<string> NativeLanguageTextProperty = RegisterProperty<string>(c => c.NativeLanguageText);
    public string NativeLanguageText
    {
      get { return GetProperty(NativeLanguageTextProperty); }
      set { SetProperty(NativeLanguageTextProperty, value); }
    }
    #endregion
    
    //USER
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

    public override void LoadFromDtoBypassPropertyChecksImpl(StudyDataDto dto)
    {
      using (BypassPropertyChecks)
      {
        LoadProperty<Guid>(IdProperty, dto.Id);
        LoadProperty<string>(NativeLanguageTextProperty, dto.NativeLanguageText);
        LoadProperty<string>(UsernameProperty, dto.Username);
        if (!string.IsNullOrEmpty(dto.Username))
          User = DataPortal.FetchChild<UserIdentity>(dto.Username);
      }
    }
    public override StudyDataDto CreateDto()
    {
      StudyDataDto retDto = new StudyDataDto(){
                                          Id = this.Id,
                                          NativeLanguageText = this.NativeLanguageText,
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
        NativeLanguageText = BusinessResources.DefaultNewStudyDataNativeLanguageText;
        LoadCurrentUser();
      }
      BusinessRules.CheckRules();
    }

    public int GetEditLevel()
    {
      return EditLevel;
    }

    /// <summary>
    /// Does CheckAuthentication().  Then Loads the StudyData with the current user, userid, username.
    /// </summary>
    internal void LoadCurrentUser()
    {
      UserIdentity.CheckAuthentication();
      var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
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
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(NativeLanguageTextProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UsernameProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MinLength(NativeLanguageTextProperty, 
                                                                 int.Parse(BusinessResources.MinNativeLanguageTextLength)));

      BusinessRules.AddRule(new Csla.Rules.CommonRules.RegExMatch(UsernameProperty, CommonResources.UsernameValidationRegex));
    }

    #endregion

    #region Authorization Rules

    public static void AddObjectAuthorizationRules()
    {
      // TODO: StudyDataEdit Authorization: add object-level authorization rules
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
        var studyDataDal = dalManager.GetProvider<IStudyDataDal>();
        var result = studyDataDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        StudyDataDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    //[Transactional(TransactionalTypes.TransactionScope)]
    //protected void DataPortal_Fetch(Guid id)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    var studyDataDal = dalManager.GetProvider<IStudyDataDal>();
    //    Result<StudyDataDto> result = studyDataDal.Fetch(id);
    //    if (!result.IsSuccess)
    //    {
    //      Exception error = result.GetExceptionFromInfo();
    //      if (error != null)
    //        throw error;
    //      else
    //        throw new FetchFailedException(result.Msg);
    //    }
    //    StudyDataDto dto = result.Obj;
    //    LoadFromDtoBypassPropertyChecks(dto);
    //  }
    //}

    protected void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var studyDataDal = dalManager.GetProvider<IStudyDataDal>();
        Result<bool> resultExists = studyDataDal.StudyDataExistsForCurrentUser();
        if (!resultExists.IsSuccess)
        {
          Exception error = resultExists.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(resultExists.Msg);
        }
        var userHasStudyData = resultExists.Obj;

        //POPULATE OUR STUDY DATA DTO
        StudyDataDto dto = null;

        if (userHasStudyData)
        {
          //THE USER HAS STUDY DATA, SO FETCH IT
          Result<StudyDataDto> resultFetch = studyDataDal.FetchForCurrentUser();
          if (!resultFetch.IsSuccess)
          {
            Exception error = resultFetch.GetExceptionFromInfo();
            if (error != null)
              throw error;
            else
              throw new FetchFailedException(resultFetch.Msg);
          }

          dto = resultFetch.Obj;
        }
        else
        {
          //THE USER DOESN'T HAVE STUDY DATA
          //SET PROPS ACCORDINGLY
          dto = new StudyDataDto()
          {
            Id = Guid.Empty,
            NativeLanguageText = "",
            Username = Csla.ApplicationContext.User.Identity.Name
          };
        }

        //OUR DTO IS NOW POPULATED
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Insert()
    {
      //Dal is responsible for setting new Id
      //StudyDataDto dto = new StudyDataDto()
      //{
      //  Id = this.Id,
      //  LanguageId = this.LanguageId,
      //  Text = this.Text
      //};
      using (var dalManager = DalFactory.GetDalManager())
      {
        var studyDataDal = dalManager.GetProvider<IStudyDataDal>();
        var dto = CreateDto();
        var result = studyDataDal.Insert(dto);
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
        var studyDataDal = dalManager.GetProvider<IStudyDataDal>();
        var dto = CreateDto();
        Result<StudyDataDto> result = studyDataDal.Update(dto);
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
        var studyDataDal = dalManager.GetProvider<IStudyDataDal>();
        var result = studyDataDal.Delete(ReadProperty<Guid>(IdProperty));
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
        var studyDataDal = dalManager.GetProvider<IStudyDataDal>();
        var result = studyDataDal.Delete(id);
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
    //    var StudyDataDal = dalManager.GetProvider<IStudyDataDal>();
    //    var result = StudyDataDal.Fetch(id);
    //    if (result.IsError)
    //      throw new FetchFailedException(result.Msg);
    //    StudyDataDto dto = result.Obj;
    //    LoadFromDtoBypassPropertyChecks(dto);
    //  }
    //}

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(StudyDataDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);

      //using (var dalManager = DalFactory.GetDalManager())
      //{
      //  var StudyDataDal = dalManager.GetProvider<IStudyDataDal>();
      //  var result = StudyDataDal.Fetch(id);
      //  if (result.IsError)
      //    throw new FetchFailedException(result.Msg);
      //  StudyDataDto dto = result.Obj;
      //  LoadFromDtoBypassPropertyChecks(dto);
      //}
    }

    //[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    //public void Child_Fetch(Guid id)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    var studyDataDal = dalManager.GetProvider<IStudyDataDal>();
    //    var result = studyDataDal.Fetch(id);
    //    if (result.IsError)
    //      throw new FetchFailedException(result.Msg);
    //    StudyDataDto dto = result.Obj;
    //    LoadFromDtoBypassPropertyChecks(dto);
    //  }
    //}

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Insert()
    {   
      using (var dalManager = DalFactory.GetDalManager())
      {
        var studyDataDal = dalManager.GetProvider<IStudyDataDal>();
        using (BypassPropertyChecks)
        {
          var dto = CreateDto();
          var result = studyDataDal.Insert(dto);
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
        var studyDataDal = dalManager.GetProvider<IStudyDataDal>();

        var dto = CreateDto();
        var result = studyDataDal.Update(dto);
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
        var studyDataDal = dalManager.GetProvider<IStudyDataDal>();

        var result = studyDataDal.Delete(Id);
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
