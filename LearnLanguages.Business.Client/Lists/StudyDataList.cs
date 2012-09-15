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
  public class StudyDataList : 
    Common.CslaBases.BusinessListBase<StudyDataList, StudyDataEdit, StudyDataDto>
  {
    #region Factory Methods

    public static void GetAll(EventHandler<DataPortalResult<StudyDataList>> callback)
    {
      DataPortal.BeginFetch<StudyDataList>(callback);
    }

    public static void NewStudyDataList(ICollection<Guid> StudyDataIds, EventHandler<DataPortalResult<StudyDataList>> callback)
    {
      DataPortal.BeginFetch<StudyDataList>(StudyDataIds, callback);
    }

    public static StudyDataList NewStudyDataList()
    {
      return new StudyDataList();
    }
    
#if SILVERLIGHT
    /// <summary>
    /// Runs locally.
    /// </summary>
    /// <param name="callback"></param>
    public static void NewStudyDataList(EventHandler<DataPortalResult<StudyDataList>> callback)
    {
      DataPortal.BeginCreate<StudyDataList>(callback, DataPortal.ProxyModes.LocalOnly);
    }
#else
    /// <summary>
    /// Runs locally.
    /// </summary>
    [RunLocal]
    public static void NewStudyDataList(EventHandler<DataPortalResult<StudyDataList>> callback)
    {
      DataPortal.BeginCreate<StudyDataList>(callback);
    }
#endif

    #endregion

    #region Data Portal methods (including child)

#if !SILVERLIGHT
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public void DataPortal_Fetch(ICollection<Guid> StudyDataIds)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    var StudyDataDal = dalManager.GetProvider<IStudyDataDal>();

    //    Result<ICollection<StudyDataDto>> result = StudyDataDal.Fetch(StudyDataIds);
    //    if (!result.IsSuccess || result.IsError)
    //    {
    //      if (result.Info != null)
    //      {
    //        var ex = result.GetExceptionFromInfo();
    //        if (ex != null)
    //          throw new FetchFailedException(ex.Message);
    //        else
    //          throw new FetchFailedException();
    //      }
    //      else
    //        throw new FetchFailedException();
    //    }

    //    //RESULT WAS SUCCESSFUL
    //    var fetchedStudyDataDtos = result.Obj;
    //    foreach (var StudyDataDto in fetchedStudyDataDtos)
    //    {
    //      //var StudyDataEdit = DataPortal.CreateChild<StudyDataEdit>(StudyDataDto);
    //      var StudyDataEdit = DataPortal.FetchChild<StudyDataEdit>(StudyDataDto);
    //      this.Add(StudyDataEdit);
    //    }
    //  }
    //}

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var StudyDataDal = dalManager.GetProvider<IStudyDataDal>();

        Result<ICollection<StudyDataDto>> result = StudyDataDal.GetAll();
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
        var allStudyDataDtos = result.Obj;
        foreach (var StudyDataDto in allStudyDataDtos)
        {
          //var StudyDataEdit = DataPortal.CreateChild<StudyDataEdit>(StudyDataDto);
          var StudyDataEdit = DataPortal.FetchChild<StudyDataEdit>(StudyDataDto);
          this.Add(StudyDataEdit);
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
    public void Child_Fetch(ICollection<Guid> StudyDataIds)
    {
      Items.Clear();
      foreach (var id in StudyDataIds)
      {
        var StudyDataEdit = DataPortal.FetchChild<StudyDataEdit>(id);
        Items.Add(StudyDataEdit);
      }
    }
#endif

    #endregion

    #region AddNewCore

#if SILVERLIGHT
    protected override void AddNewCore()
    {
      AddedNew += StudyDataList_AddedNew; 
      base.AddNewCore();
      AddedNew -= StudyDataList_AddedNew;
    }

    private void StudyDataList_AddedNew(object sender, Csla.Core.AddedNewEventArgs<StudyDataEdit> e)
    {
      //UserIdentity.CheckAuthentication();
      var StudyDataEdit = e.NewObject;
      StudyDataEdit.LoadCurrentUser();
      //var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
      //StudyDataEdit.UserId = identity.UserId;
      //StudyDataEdit.Username = identity.Name;
    }
#else
    protected override StudyDataEdit AddNewCore()
    {
      //SERVER
      var StudyDataEdit = base.AddNewCore();
      StudyDataEdit.LoadCurrentUser();
      return StudyDataEdit;
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
