using System;
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
  public class UserList : Common.CslaBases.BusinessListBase<UserList, UserEdit, UserDto>
  {
    #region Factory Methods

    public static void GetAll(EventHandler<DataPortalResult<UserList>> callback)
    {
      DataPortal.BeginFetch<UserList>(callback);
    }

    public static void GetAllContainingText(string text, EventHandler<DataPortalResult<UserList>> callback)
    {
      DataPortal.BeginFetch<UserList>(text, callback);
    }

    public static void NewUserList(ICollection<Guid> phraseIds, EventHandler<DataPortalResult<UserList>> callback)
    {
      DataPortal.BeginFetch<UserList>(phraseIds, callback);
    }

    public static void NewUserList(Criteria.UserTextsCriteria phraseTextsCriteria,
      EventHandler<DataPortalResult<UserList>> callback)
    {
      DataPortal.BeginCreate<UserList>(phraseTextsCriteria, callback);
    }

    public static UserList NewUserList()
    {
      return new UserList();
    }

#if SILVERLIGHT
    /// <summary>
    /// Runs locally.
    /// </summary>
    /// <param name="callback"></param>
    public static void NewUserList(EventHandler<DataPortalResult<UserList>> callback)
    {
      DataPortal.BeginCreate<UserList>(callback, DataPortal.ProxyModes.LocalOnly);
    }
#else
    /// <summary>
    /// Runs locally.
    /// </summary>
    [RunLocal]
    public static void NewUserList(EventHandler<DataPortalResult<UserList>> callback)
    {
      DataPortal.BeginCreate<UserList>(callback);
    }

    public static UserList GetAll()
    {
      return DataPortal.Fetch<UserList>();
    }

    public static UserList GetAllContainingText(string text)
    {
      return DataPortal.Fetch<UserList>(text);
    }

#endif

    #endregion

    #region Data Portal methods (including child)

#if !SILVERLIGHT
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Create(Criteria.UserTextsCriteria phraseTextsCriteria)
    {
      var sep = " ||| ";
      var msg = DateTime.Now.ToShortTimeString() +
                   "UserList.DP_Create" + sep +
                   "ThreadID = " +
                   System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + sep +
                   "UserTexts.Count = " + phraseTextsCriteria.UserTexts.Count.ToString() + sep +
                   "1st User = " + phraseTextsCriteria.UserTexts[0];
      System.Diagnostics.Trace.WriteLine(msg);
      //Services.Log(msg, LogPriority.Low, LogCategory.Information);


      if (phraseTextsCriteria.UserTexts.Count == 0)
        throw new ArgumentException("phraseTextsCriteria");
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageText = phraseTextsCriteria.LanguageText;
        var language = DataPortal.FetchChild<LanguageEdit>(languageText);
       
        //WE NOW HAVE OUR LANGUAGEEDIT THAT WILL BE USED FOR ALL PHRASE TEXTS.
        var UserDal = dalManager.GetProvider<IUserDal>();

        //UserList newUserList = UserList.NewUserList();
        for (int i = 0; i < phraseTextsCriteria.UserTexts.Count; i++)
        {
        //foreach (var phraseText in phraseTextsCriteria.UserTexts)
          var phraseText = phraseTextsCriteria.UserTexts[i];
          if (string.IsNullOrEmpty(phraseText))
            continue;
          UserEdit phraseEdit = DataPortal.CreateChild<UserEdit>();
          phraseEdit.Language = language;
          phraseEdit.Text = phraseText;
          Add(phraseEdit);
        }
      }
    }

    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch(ICollection<Guid> phraseIds)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var UserDal = dalManager.GetProvider<IUserDal>();

        Result<ICollection<UserDto>> result = UserDal.Fetch(phraseIds);
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
        var fetchedUserDtos = result.Obj;
        foreach (var phraseDto in fetchedUserDtos)
        {
          //var UserEdit = DataPortal.CreateChild<UserEdit>(UserDto);
          var phraseEdit = DataPortal.FetchChild<UserEdit>(phraseDto);
          this.Add(phraseEdit);
        }
      }
    }

    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var UserDal = dalManager.GetProvider<IUserDal>();

        Result<ICollection<UserDto>> result = UserDal.GetAll();
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
        var allUserDtos = result.Obj;
        foreach (var UserDto in allUserDtos)
        {
          //var UserEdit = DataPortal.CreateChild<UserEdit>(UserDto);
          var UserEdit = DataPortal.FetchChild<UserEdit>(UserDto);
          this.Add(UserEdit);
        }
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch(string text)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var UserDal = dalManager.GetProvider<IUserDal>();

        Result<ICollection<UserDto>> result = UserDal.Fetch(text);
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
        var phraseDtos = result.Obj;
        foreach (var UserDto in phraseDtos)
        {
          //var UserEdit = DataPortal.CreateChild<UserEdit>(UserDto);
          var UserEdit = DataPortal.FetchChild<UserEdit>(UserDto);
          this.Add(UserEdit);
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
    public void Child_Fetch(ICollection<Guid> phraseIds)
    {
      Items.Clear();
      foreach (var id in phraseIds)
      {
        var phraseEdit = DataPortal.FetchChild<UserEdit>(id);
        Items.Add(phraseEdit);
      }
    }
#endif

    #endregion

    #region AddNewCore

#if SILVERLIGHT
    protected override void AddNewCore()
    {
      AddedNew += UserList_AddedNew; 
      base.AddNewCore();
      AddedNew -= UserList_AddedNew;
    }

    private void UserList_AddedNew(object sender, Csla.Core.AddedNewEventArgs<UserEdit> e)
    {
      //CustomIdentity.CheckAuthentication();
      var phraseEdit = e.NewObject;
      phraseEdit.LoadCurrentUser();
      //var identity = (CustomIdentity)Csla.ApplicationContext.User.Identity;
      //phraseEdit.UserId = identity.UserId;
      //phraseEdit.Username = identity.Name;
    }
#else
    protected override UserEdit AddNewCore()
    {
      //SERVER
      var phraseEdit = base.AddNewCore();
      phraseEdit.LoadCurrentUser();
      return phraseEdit;
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
