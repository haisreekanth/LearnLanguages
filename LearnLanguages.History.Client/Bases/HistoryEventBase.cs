﻿using System;
using System.Collections.Generic;
using Csla.Serialization;
using Csla.Core;

namespace LearnLanguages.History.Bases
{
  [Serializable]
  public abstract class HistoryEventBase : Common.Interfaces.IHistoryEvent
  {
    /// <summary>
    /// Does NOT create the dictionaries Ids, Strings, Ints, Doubles.
    /// </summary>
    public HistoryEventBase()
    {
      HistoryEventId = Guid.NewGuid();
      TimeStamp = DateTime.UtcNow;
    }

    /// <summary>
    /// DOES not create the dictionaries Ids, Strings, Ints, Doubles, even if these are not utilized.
    /// Create your own ctor if you want to keep from creating these would-be empty dictionaries.
    /// </summary>
    /// <param name="duration">event duration</param>
    /// <param name="text">text describing the event</param>
    /// <param name="details">key/value pairs of details of event.  e.g. ("ChildId", 9579019B-A958-40EF-9021-D622FCD17DE7), ("ParentLanguage", "English")</param>
    public HistoryEventBase(TimeSpan duration, params KeyValuePair<string, object>[] details)
      : this()
    {
      Duration = duration;

      Ids = new MobileDictionary<string, Guid>();
      Strings = new MobileDictionary<string, string>();
      Ints = new MobileDictionary<string, int>();
      Doubles = new MobileDictionary<string, double>();

      AddDetails(details);
    }

    protected void AddDetails(KeyValuePair<string, object>[] details)
    {
      if (details != null && details.Length > 0)
      {
        foreach (var detail in details)
        {
          if (detail.Value is Guid)
            Ids.Add(detail.Key, (Guid)detail.Value);
          else if (detail.Value is string)
            Strings.Add(detail.Key, (string)detail.Value);
          else if (detail.Value is int)
            Ints.Add(detail.Key, (int)detail.Value);
          else if (detail.Value is double)
            Doubles.Add(detail.Key, (double)detail.Value);
          else
            Strings.Add(detail.Key, detail.Value.ToString());
        }
      }
    }

    public Guid HistoryEventId { get; protected set; }

    public DateTime TimeStamp { get; protected set; }
    public TimeSpan Duration { get; protected set; }

    /// <summary>
    /// Default virtual implementation of GetText is return this.GetType().FullName;
    /// </summary>
    public string Text { get { return GetText(); } }

    protected virtual string GetText()
    {
      return this.GetType().FullName;
    }

    public MobileDictionary<string, Guid> Ids { get; protected set; }
    public MobileDictionary<string, string> Strings { get; protected set; }
    public MobileDictionary<string, int> Ints { get; protected set; }
    public MobileDictionary<string, double> Doubles { get; protected set; }
  }
}