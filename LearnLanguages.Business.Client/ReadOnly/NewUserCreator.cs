using System;
using System.Net;
using Csla;
using Csla.Serialization;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class creates a new phrase, loads it with the default language.
  /// </summary>
  [Serializable]
  public class NewUserCreator : Common.CslaBases.ReadOnlyBase<NewUserCreator>
  {
    #region Factory Methods

    public static void CreateNew(Csla.Security.UsernameCriteria criteria, 
      EventHandler<DataPortalResult<NewUserCreator>> callback)
    {
      DataPortal.BeginCreate<NewUserCreator>(criteria, callback);
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
    public void DataPortal_Create(Csla.Security.UsernameCriteria criteria)
    {
      RetrieverId = Guid.NewGuid();
      WasSuccessful = false;

      using (var dalManager = DalFactory.GetDalManager())
      {
        var customIdentityDal = dalManager.GetProvider<IUserDal>();
        var result = customIdentityDal.AddUser(criteria);
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
