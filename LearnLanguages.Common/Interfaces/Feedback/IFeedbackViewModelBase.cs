﻿using System;
using Csla.Serialization;

namespace LearnLanguages.Common.Interfaces
{
  public interface IFeedbackViewModelBase : IViewModelBase, IGetFeedback, IHaveFeedback
  {
    bool IsEnabled { get; set; }
  }
}
