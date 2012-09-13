using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Interfaces;
using LearnLanguages.Business.Security;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using System.Windows;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ThinkingPanelViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ThinkingPanelViewModel : ViewModelBase,
                                        IHandle<History.Events.ThinkingAboutTargetEvent>,
                                        IHandle<History.Events.ThinkedAboutTargetEvent>
                                        
  {
    public ThinkingPanelViewModel()
    {
      History.HistoryPublisher.Ton.SubscribeToEvents(this);
      Thoughts = new List<Guid>();
      ThinkingText = "Ready";
    }

    private List<Guid> Thoughts { get; set; }

    public void Handle(History.Events.ThinkingAboutTargetEvent message)
    {
      //IF WE AREN'T ALREADY TRACKING THIS TargetId, THEN ADD IT TO OUR THOUGHTS
      if (!(Thoughts.Contains(message.TargetId)))
      {
        Thoughts.Add(message.TargetId);
      }

      UpdateThinkingText();
    }

    public void Handle(History.Events.ThinkedAboutTargetEvent message)
    {
      //IF THE TARGET ID IS GUID.EMPTY, THEN WE'RE JUST PINGING A THOUGHT, NO NEED TO TRACK
      //IF WE ARE TRACKING THIS TargetId, THEN REMOVE IT FROM OUR THOUGHTS
      if (message.TargetId != Guid.Empty && 
          Thoughts.Contains(message.TargetId))
      {
        Thoughts.Remove(message.TargetId);
      }

      UpdateThinkingText();
    }

    private void UpdateThinkingText()
    {
      //todo: updatethinkingtext strings to resx
      string thinkingDots = GenerateRandomThinkingDots();
      var thoughtCount = Thoughts.Count;
      if (thoughtCount > 1)
        ThinkingText = "Thinking About " + thoughtCount.ToString() + " Thing(s)" + thinkingDots;
      else if (thoughtCount == 1)
        ThinkingText = "Thinking About 1 Thing" + thinkingDots;
      else
        ThinkingText = "Ready. Give me something to think about!" ;
    }

    private string GenerateRandomThinkingDots()
    {
      string retThinkingDotsString = "";
      int maxLength = int.Parse(ViewViewModelResources.MaxThinkingDots);
      Random r = new Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute);
      int numDots = r.Next(1, maxLength + 1);
      for (int i = 0; i < numDots; i++)
      {
        retThinkingDotsString += ".";
      }
      return retThinkingDotsString;
    }

    private string _ThinkingText;
    public string ThinkingText
    {
      get { return _ThinkingText; }
      set
      {
        if (value != _ThinkingText)
        {
          _ThinkingText = value;
          NotifyOfPropertyChange(() => ThinkingText);
        }
      }
    }

    public void OnImportsSatisfied()
    {
      //var coreViewModelName = ViewModelBase.GetCoreViewModelName(typeof(ThinkingPanelViewModel));
      //Services.EventAggregator.Publish(new Events.PartSatisfiedEventMessage(coreViewModelName));
    }
    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }
    private Visibility _ViewModelVisibility;
    public Visibility ViewModelVisibility
    {
      get { return _ViewModelVisibility; }
      set
      {
        if (value != _ViewModelVisibility)
        {
          _ViewModelVisibility = value;
          NotifyOfPropertyChange(() => ViewModelVisibility);
        }
      }
    }
  }
}
