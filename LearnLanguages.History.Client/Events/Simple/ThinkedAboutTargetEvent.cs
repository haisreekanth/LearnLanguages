using System;
namespace LearnLanguages.History.Events
{
  /// <summary>
  /// This event is used to tell history that we have thinked about some target, 
  /// and that it should update progress indicators accordingly.
  ///   
  /// If you are just pinging an update (not completed thinking about the target),
  /// publish this with the thinkId = guid.empty.
  /// </summary>
  public class ThinkedAboutTargetEvent : Bases.ThinkingEventBase
  {

    public ThinkedAboutTargetEvent(Guid thinkId)
      : base(thinkId)
    {
    }

    public Guid TargetId
    {
      get
      {
        return GetDetail<Guid>(HistoryResources.Key_TargetId);
      }
    }

    /// <summary>
    /// This event is used to tell history that we have thinked about some target, 
    /// and that it should update progress indicators accordingly.
    ///   
    /// If you are just pinging an update (not completed thinking about the target),
    /// publish this with the thinkId = guid.empty.
    /// </summary>
    public static void Publish(Guid targetId)
    {
      History.HistoryPublisher.Ton.PublishEvent(new ThinkedAboutTargetEvent(targetId));
    }
  }
}
