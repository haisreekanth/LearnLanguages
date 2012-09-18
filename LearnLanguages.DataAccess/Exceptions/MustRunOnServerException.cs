using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class MustRunOnServerException : Exception
  {
    public MustRunOnServerException() 
      : base()
    {
    }

    public MustRunOnServerException(string errorMsg)
      : base(errorMsg)
    {

    }

    public MustRunOnServerException(Exception innerException)
      : base(string.Format(DalResources.ErrorMsgMustRunOnServerException, innerException.Message), innerException)
    {

    }

  }
}
