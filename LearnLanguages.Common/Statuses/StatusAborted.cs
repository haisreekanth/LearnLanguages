﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Statuses
{
  public class StatusAborted : Status
  {
    public override string GetMessage()
    {
      return CommonResources.StatusAborted;
    }
  }
}
