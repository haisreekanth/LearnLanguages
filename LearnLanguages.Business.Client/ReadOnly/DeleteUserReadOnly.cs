using System;
using System.Net;
using Csla;
using Csla.Serialization;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This readonly deletes a user and all his associated data from database.
  /// </summary>
  [Serializable]
  public class DeleteUserReadOnly : Common.CslaBases.ReadOnlyBase<DeleteUserReadOnly>
  {
    #region Factory Methods

    public static void CreateNew(Criteria.UserInfoCriteria criteria, 
      EventHandler<DataPortalResult<DeleteUserReadOnly>> callback)
    {
      DataPortal.BeginCreate<DeleteUserReadOnly>(criteria, callback);
    }

    #endregion

    #region Properties

    public static readonly PropertyInfo<Guid> RetrieverIdProperty = RegisterProperty<Guid>(c => c.RetrieverId);
    public Guid RetrieverId
    {
      get { return ReadProperty(RetrieverIdProperty); }
      private set { LoadProperty(RetrieverIdProperty, value); }
    }

    public static readonly PropertyInfo<bool> WasSuccessfulProperty = RegisterProperty<bool>(c => c.WasSuccessful);
    public bool WasSuccessful
    {
      get { return ReadProperty(WasSuccessfulProperty); }
      private set { LoadProperty(WasSuccessfulProperty, value); }
    }

    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public void DataPortal_Create(Criteria.UserInfoCriteria criteria)
    {
      RetrieverId = Guid.NewGuid();
      WasSuccessful = false;

      using (var dalManager = DalFactory.GetDalManager())
      {
        var customIdentityDal = dalManager.GetProvider<IUserIdentityDal>();
        var result = customIdentityDal.Delete(criteria.Username);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DataAccess.Exceptions.AddUserFailedException(result.Msg);
        }
        WasSuccessful = true;
      }
    }
#endif

    #endregion
  }
}
