﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.Interfaces
{
  public interface ICreateChildAsync
  {
    Result<IAsyncStub> CreateChild(Args.CreateChildArgs args);
  }
}
