using System;
using Csla;
using System.ComponentModel;
using Csla.Serialization;
using Csla.DataPortalClient;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.DataAccess;
using LearnLanguages.Business.Security;
using System.Collections.Generic;


namespace LearnLanguages.Business
{
  /// <summary>
  /// Song, Poem, Lyric Sheet, Scene Lines...Anything that has a list of ordered lines.
  /// </summary>
  [Serializable]
  public class MultiLineTextEdit : Common.CslaBases.BusinessBase<MultiLineTextEdit, MultiLineTextDto>, IHaveId
  {
    #region Ctors and Init
    public MultiLineTextEdit()
    {
      Lines = LineList.NewLineListNewedUpOnly();
    }
    #endregion
    
    #region Factory Methods

    #region Wpf Factory Methods
#if !SILVERLIGHT

    /// <summary>
    /// Creates a new MultiLineTextEdit sync
    /// </summary>
    public static MultiLineTextEdit NewMultiLineTextEdit()
    {
      return DataPortal.Create<MultiLineTextEdit>();
    }
    /// <summary>
    /// Creates a new MultiLineTextEdit sync with given id.
    /// </summary>
    public static MultiLineTextEdit NewMultiLineTextEdit(Guid id)
    {
      return DataPortal.Create<MultiLineTextEdit>(id);
    }
    /// <summary>
    /// Fetches a MultiLineTextEdit sync with given id
    /// </summary>
    public static MultiLineTextEdit GetMultiLineTextEdit(Guid id)
    {
      return DataPortal.Fetch<MultiLineTextEdit>(id);
    }

#endif
    #endregion

    #region Silverlight Factory Methods
#if SILVERLIGHT

    public static void NewMultiLineTextEdit(EventHandler<DataPortalResult<MultiLineTextEdit>> callback)
    {
      DataPortal.BeginCreate<MultiLineTextEdit>(callback);
    }

    ///// <summary>
    ///// This happens DataPortal.ProxyModes.LocalOnly
    ///// </summary>
    //public static void NewMultiLineTextEdit(Guid id, EventHandler<DataPortalResult<MultiLineTextEdit>> callback)
    //{
    //  DataPortal.BeginCreate<MultiLineTextEdit>(id, callback, DataPortal.ProxyModes.LocalOnly);
    //  //DataPortal.BeginCreate<MultiLineTextEdit>(id, callback);
    //}

    public static void GetMultiLineTextEdit(Guid id, EventHandler<DataPortalResult<MultiLineTextEdit>> callback)
    {
      DataPortal.BeginFetch<MultiLineTextEdit>(id, callback);
    }

#endif
    #endregion
    #endregion

    #region Business Properties

    //PHRASES
    //#region public ICollection<Guid> LineIds
    //public static readonly PropertyInfo<ICollection<Guid>> LineIdsProperty =
    //  RegisterProperty<ICollection<Guid>>(c => c.LineIds);
    //public ICollection<Guid> LineIds
    //{
    //  get { return GetProperty(LineIdsProperty); }
    //  set { SetProperty(LineIdsProperty, value); }
    //}
    //#endregion
    //SCALAR PROPERTIES

    #region public string Title
    public static readonly PropertyInfo<string> TitleProperty = RegisterProperty<string>(c => c.Title);
    public string Title
    {
      get { return GetProperty(TitleProperty); }
      set { SetProperty(TitleProperty, value); }
    }
    #endregion
    #region public string AdditionalMetadata
    public static readonly PropertyInfo<string> AdditionalMetadataProperty = RegisterProperty<string>(c => c.AdditionalMetadata);
    public string AdditionalMetadata
    {
      get { return GetProperty(AdditionalMetadataProperty); }
      set { SetProperty(AdditionalMetadataProperty, value); }
    }
    #endregion
    #region public LineList Lines (child)
    public static readonly PropertyInfo<LineList> LinesProperty = 
      RegisterProperty<LineList>(c => c.Lines, RelationshipTypes.Child);
    public LineList Lines
    {
      get { return GetProperty(LinesProperty); }
      set { SetProperty(LinesProperty, value); }
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

    #endregion

    #region Business Methods

    /// <summary>
    /// If AdditionalMetadata does not already contain key/value, then it adds it.
    /// </summary>
    /// <param name="key">Metadata key, e.g. "type" in "type=song"</param>
    /// <param name="value">Metadata value, e.g. "song" in "type=song"</param>
    public void AddMetadata(string key, string value)
    {
      var entry = BuildMetadataEntry(key, value);
      if (string.IsNullOrEmpty(AdditionalMetadata))
        AdditionalMetadata = entry;
      else
      {
        //is not null or empty
        if (AdditionalMetadata.Contains(entry))
          return;//already contains entry
        else
          //append
          AdditionalMetadata += BusinessResources.MetadataSeparator + entry;
      }
    }

    public override void LoadFromDtoBypassPropertyChecksImpl(MultiLineTextDto dto)
    {
      using (BypassPropertyChecks)
      {
        LoadProperty<Guid>(IdProperty, dto.Id);
        LoadProperty<string>(TitleProperty, dto.Title);
        LoadProperty<string>(AdditionalMetadataProperty, dto.AdditionalMetadata);
        if (dto.LineIds != null && dto.LineIds.Count > 0)
        {
          //LineIds = dto.LineIds;
          Lines = DataPortal.FetchChild<LineList>(dto.LineIds);
        }
        LoadProperty<Guid>(UserIdProperty, dto.UserId);
        LoadProperty<string>(UsernameProperty, dto.Username);
        if (!string.IsNullOrEmpty(dto.Username))
          User = DataPortal.FetchChild<UserIdentity>(dto.Username);
      }

      BusinessRules.CheckRules();
    }
    public override MultiLineTextDto CreateDto()
    {
      var _lineIds = new List<Guid>();
      if (Lines != null)
      {
        foreach (var line in Lines)
        {
          _lineIds.Add(line.Id);
        }
      }
      MultiLineTextDto retDto = new MultiLineTextDto()
      {
        Id = this.Id,
        Title = this.Title,
        AdditionalMetadata = this.AdditionalMetadata,
        LineIds = _lineIds,
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
        Lines = null;
        UserId = Guid.Empty;
        Username = DalResources.DefaultNewMultiLineTextUsername;
      }
      BusinessRules.CheckRules();
    }

    public int GetEditLevel()
    {
      return EditLevel;
    }

    public void AddLine(LineEdit line)
    {
      //LineIds.Add(line.Id);
      if (_LineAdding != null)
        throw new Exception();
      _LineAdding = line;
      Lines.AddedNew += Lines_AddedNew;
      Lines.AddNew();
      Lines.AddedNew -= Lines_AddedNew;
      BusinessRules.CheckRules();
    }
    private LineEdit _LineAdding;

    private void Lines_AddedNew(object sender, Csla.Core.AddedNewEventArgs<LineEdit> e)
    {
      var line = e.NewObject;
      var lineDto = _LineAdding.CreateDto();
      _LineAdding = null;
      line.LoadFromDtoBypassPropertyChecks(lineDto);
    }

    protected override void OnChildChanged(Csla.Core.ChildChangedEventArgs e)
    {
      base.OnChildChanged(e);
      BusinessRules.CheckRules(LinesProperty);
    }

    /// <summary>
    /// Builds an entry using the key and value given.  
    /// At time of writing, this is:
    /// key + BusinessResources.MetadataEquals + value
    /// </summary>
    /// <param name="key">AdditionalMetadata key</param>
    /// <param name="value">AdditionalMetadata value</param>
    /// <returns>Properly formed AdditionalMetadata entry</returns>
    public static string BuildMetadataEntry(string key, string value)
    {
      return key + BusinessResources.MetadataEquals + value;
    }

    public static string MetadataEntrySong
    {
      get
      {
        return BuildMetadataEntry(BusinessResources.MetadataKeyType, BusinessResources.MetadataValueTypeSong);
      }
    }

    public static string MetadataEntryPoem
    {
      get
      {
        return BuildMetadataEntry(BusinessResources.MetadataKeyType, BusinessResources.MetadataValueTypePoem);
      }
    }

    #endregion

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();

      // TODO: add validation rules
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(IdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(LinesProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UserIdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UsernameProperty));


      //multiLineText must have 2 lineids to be valid
      BusinessRules.AddRule(
        new Rules.CollectionMinimumCountBusinessRule(LinesProperty, 
                                                     int.Parse(BusinessResources.SongMinimumLineCount)));
      //BusinessRules.AddRule(new CollectionCountsAreEqualBusinessRule(LinesProperty, LineIdsProperty));
    }

    #endregion

    #region Authorization Rules

    public static void AddObjectAuthorizationRules()
    {
      // TODO: MultiLineTextEdit Authorization: add object-level authorization rules
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
        var multiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();
        var result = multiLineTextDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        MultiLineTextDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
        var dummy = this;
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var multiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();
        Result<MultiLineTextDto> result = multiLineTextDal.Fetch(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(result.Msg);
        }
        MultiLineTextDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Insert()
    {
      //Dal is responsible for setting new Id
      //MultiLineTextDto dto = new MultiLineTextDto()
      //{
      //  Id = this.Id,
      //  LanguageId = this.LanguageId,
      //  Text = this.Text
      //};
      using (var dalManager = DalFactory.GetDalManager())
      {
        FieldManager.UpdateChildren(this);
        //DataPortal.UpdateChild(Lines);

        var multiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();
        var dto = CreateDto();
        var result = multiLineTextDal.Insert(dto);
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
        FieldManager.UpdateChildren(this);

        var multiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();
        var dto = CreateDto();
        Result<MultiLineTextDto> result = multiLineTextDal.Update(dto);
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
        var multiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();
        var result = multiLineTextDal.Delete(ReadProperty<Guid>(IdProperty));
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
        var multiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();
        var result = multiLineTextDal.Delete(id);
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
    //    var MultiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();
    //    var result = MultiLineTextDal.Fetch(id);
    //    if (result.IsError)
    //      throw new FetchFailedException(result.Msg);
    //    MultiLineTextDto dto = result.Obj;
    //    LoadFromDtoBypassPropertyChecks(dto);
    //  }
    //}

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(MultiLineTextDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);

      //using (var dalManager = DalFactory.GetDalManager())
      //{
      //  var MultiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();
      //  var result = MultiLineTextDal.Fetch(id);
      //  if (result.IsError)
      //    throw new FetchFailedException(result.Msg);
      //  MultiLineTextDto dto = result.Obj;
      //  LoadFromDtoBypassPropertyChecks(dto);
      //}
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var multiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();
        Result<MultiLineTextDto> result = multiLineTextDal.Fetch(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(result.Msg);
        }
        MultiLineTextDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }


    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Insert()
    {   
      using (var dalManager = DalFactory.GetDalManager())
      {
        FieldManager.UpdateChildren(this);

        var multiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();
        using (BypassPropertyChecks)
        {
          var dto = CreateDto();
          var result = multiLineTextDal.Insert(dto);
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

        var multiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();

        var dto = CreateDto();
        var result = multiLineTextDal.Update(dto);
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
        var multiLineTextDal = dalManager.GetProvider<IMultiLineTextDal>();

        var result = multiLineTextDal.Delete(Id);
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
