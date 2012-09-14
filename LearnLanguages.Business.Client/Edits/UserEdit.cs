﻿using System;
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
  public class UserEdit : LearnLanguages.Common.CslaBases.BusinessBase<UserEdit, UserDto>, IHaveId
  {
    #region Factory Methods
    #region Wpf Factory Methods
#if !SILVERLIGHT

    /// <summary>
    /// Creates a new UserEdit sync
    /// </summary>
    public static UserEdit NewUserEdit()
    {
      return DataPortal.Create<UserEdit>();
    }

    /// <summary>
    /// Fetches a UserEdit sync with given id
    /// </summary>
    public static UserEdit GetUserEdit(Guid id)
    {
      return DataPortal.Fetch<UserEdit>(id);
    }

#endif
    #endregion

    #region Silverlight Factory Methods
#if SILVERLIGHT

    public static void NewUserEdit(EventHandler<DataPortalResult<UserEdit>> callback)
    {
      //DataPortal.BeginCreate<UserEdit>(callback, DataPortal.ProxyModes.LocalOnly);
      DataPortal.BeginCreate<UserEdit>(callback);
    }

    public static void NewUserEdit(string languageText, EventHandler<DataPortalResult<UserEdit>> callback)
    {
      DataPortal.BeginCreate<UserEdit>(languageText, callback);
    }

    public static void GetUserEdit(Guid id, EventHandler<DataPortalResult<UserEdit>> callback)
    {
      DataPortal.BeginFetch<UserEdit>(id, callback);
    }

#endif
    #endregion
    #endregion

    #region Business Properties & Methods

    //USER
    #region public Guid UserId
    public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(c => c.UserId);
    public Guid UserId
    {
      get { return GetProperty(UserIdProperty); }
      set { SetProperty(UserIdProperty, value); }
    }
    #endregion

    //USERNAME
    #region public string Username
    public static readonly PropertyInfo<string> UsernameProperty = RegisterProperty<string>(c => c.Username);
    public string Username
    {
      get { return GetProperty(UsernameProperty); }
      set { SetProperty(UsernameProperty, value); }
    }
    #endregion

    
    #endregion

    public override void LoadFromDtoBypassPropertyChecksImpl(UserDto dto)
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
        //  User = DataPortal.FetchChild<CustomIdentity>(dto.Username);
      }
    }
    public override UserDto CreateDto()
    {
      UserDto retDto = new UserDto(){
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
        Text = DalResources.DefaultNewUserText;
        Language = DataPortal.FetchChild<LanguageEdit>(LanguageId);
        UserId = Guid.Empty;
        Username = DalResources.DefaultNewUserUsername;
      }
      BusinessRules.CheckRules();
    }

    public int GetEditLevel()
    {
      return EditLevel;
    }

    /// <summary>
    /// Does CheckAuthentication().  Then Loads the user with the current user, userid, username.
    /// </summary>
    internal void LoadCurrentUser()
    {
      CustomIdentity.CheckAuthentication();
      var identity = (CustomIdentity)Csla.ApplicationContext.User.Identity;
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
      // TODO: UserEdit Authorization: add object-level authorization rules
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
        var userDal = dalManager.GetProvider<IUserDal>();
        var result = userDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        UserDto dto = result.Obj;
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

        var userDal = dalManager.GetProvider<IUserDal>();
        var result = userDal.New(languageText);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        UserDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        Result<UserDto> result = userDal.Fetch(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(result.Msg);
        }
        UserDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Insert()
    {
      //WE DO EXTRA CHECKING FOR USER.TEXT AND USER.LANGUAGE.TEXT,
      //SO WE DECIDE BETWEEN INSERT AND UPDATE WITHIN THIS METHOD.
      //THIS AS OPPOSED TO LETTING CSLA HANDLE ALL THESE DECISIONS WITH 
      //META INFORMATION.

      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        var dto = CreateDto();
        var result = userDal.Insert(dto);

        ////IF THIS USER ALREADY EXISTS IN DB (SAME TEXT AND LANGUAGETEXT)
        ////THEN WE WILL CALL AN UPDATE, OTHERWISE CONTINUE WITH INSERT.
        //UserDto dto = CreateDto();
        //Result<UserDto> result = null;

        //var retriever = UsersByTextAndLanguageRetriever.CreateNew(this);
        //if (retriever.RetrievedSingleUser != null)
        //{
        //  //PERFORM AN UPDATE
        //  //REPLACE THE DTO ID
        //  dto.Id = retriever.RetrievedSingleUser.Id;
        //  result = userDal.Update(dto);
        //}
        //else
        //{
        //  //PERFORM AN INSERT
        //  result = userDal.Insert(dto);
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
        var userDal = dalManager.GetProvider<IUserDal>();
        var dto = CreateDto();
        Result<UserDto> result = userDal.Update(dto);
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
        var userDal = dalManager.GetProvider<IUserDal>();
        var result = userDal.Delete(ReadProperty<Guid>(IdProperty));
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
        var userDal = dalManager.GetProvider<IUserDal>();
        var result = userDal.Delete(id);
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
    //    var userDal = dalManager.GetProvider<IUserDal>();
    //    var result = userDal.Fetch(id);
    //    if (result.IsError)
    //      throw new FetchFailedException(result.Msg);
    //    UserDto dto = result.Obj;
    //    LoadFromDtoBypassPropertyChecks(dto);
    //  }
    //}

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void Child_Create()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        var result = userDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        UserDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(UserDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);

      //using (var dalManager = DalFactory.GetDalManager())
      //{
      //  var userDal = dalManager.GetProvider<IUserDal>();
      //  var result = userDal.Fetch(id);
      //  if (result.IsError)
      //    throw new FetchFailedException(result.Msg);
      //  UserDto dto = result.Obj;
      //  LoadFromDtoBypassPropertyChecks(dto);
      //}
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var userDal = dalManager.GetProvider<IUserDal>();
        var result = userDal.Fetch(id);
        if (result.IsError)
          throw new FetchFailedException(result.Msg);
        UserDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Insert()
    {
      //WE DO EXTRA CHECKING FOR USER.TEXT AND USER.LANGUAGE.TEXT,
      //SO WE DECIDE BETWEEN INSERT AND UPDATE WITHIN THIS METHOD.
      //THIS AS OPPOSED TO LETTING CSLA HANDLE ALL THESE DECISIONS WITH 
      //META INFORMATION.

      using (var dalManager = DalFactory.GetDalManager())
      {
        FieldManager.UpdateChildren(this);
        if (Language != null)
          LanguageId = Language.Id;

        var userDal = dalManager.GetProvider<IUserDal>();
        using (BypassPropertyChecks)
        {
          var dto = CreateDto();
          var result = userDal.Insert(dto);
          ////IF THIS USER ALREADY EXISTS IN DB (SAME TEXT AND LANGUAGETEXT)
          ////THEN WE WILL CALL AN UPDATE, OTHERWISE CONTINUE WITH INSERT.
          //UserDto dto = CreateDto();
          //Result<UserDto> result = null;

          //var retriever = UsersByTextAndLanguageRetriever.CreateNew(this);
          //if (retriever.RetrievedSingleUser != null)
          //{
          //  //PERFORM AN UPDATE
          //  //REPLACE THE DTO ID
          //  dto.Id = retriever.RetrievedSingleUser.Id;
          //  result = userDal.Update(dto);
          //}
          //else
          //{
          //  //PERFORM AN INSERT
          //  result = userDal.Insert(dto);
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

        var userDal = dalManager.GetProvider<IUserDal>();

        var dto = CreateDto();
        var result = userDal.Update(dto);
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
        var userDal = dalManager.GetProvider<IUserDal>();

        var result = userDal.Delete(Id);
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
