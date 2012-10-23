using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Linq;

using LearnLanguages.Common;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;
using LearnLanguages.History;
using LearnLanguages.History.Events;

namespace LearnLanguages.Study.ViewModels
{
  [Export(typeof(StudyLineOrderTimedQuestionAnswerViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class StudyLineOrderTimedQuestionAnswerViewModel : StudyItemViewModelBase
  {
    #region Ctors and Init

    public StudyLineOrderTimedQuestionAnswerViewModel()
    {
      StudyItemTitle = StudyResources.StudyLineTimedQuestionAnswerStudyItemTitle;
      //Services.EventAggregator.Subscribe(this);
    }

    #endregion

    #region Properties

    private string _Question;
    public string Question
    {
      get { return _Question; }
      set
      {
        if (value != _Question)
        {
          _Question = value;
          NotifyOfPropertyChange(() => Question);
          NotifyOfPropertyChange(() => QuestionHeader);
        }
      }
    }

    private string _Answer;
    public string Answer
    {
      get { return _Answer; }
      set
      {
        if (value != _Answer)
        {
          _Answer = value;
          NotifyOfPropertyChange(() => Answer);
          NotifyOfPropertyChange(() => AnswerHeader);
        }
      }
    }

    private LineEdit _Line;
    public LineEdit Line
    {
      get { return _Line; }
      set
      {
        if (value != _Line)
        {
          _Line = value;
          NotifyOfPropertyChange(() => Line);
        }
      }
    }

    private MultiLineTextEdit _MultiLineText;
    public MultiLineTextEdit MultiLineText
    {
      get { return _MultiLineText; }
      set
      {
        if (value != _MultiLineText)
        {
          _MultiLineText = value;
          NotifyOfPropertyChange(() => MultiLineText);
        }
      }
    }
    private bool _HidingAnswer;
    public bool HidingAnswer
    {
      get { return _HidingAnswer; }
      set
      {
        if (value != _HidingAnswer)
        {
          _HidingAnswer = value;
          NotifyOfPropertyChange(() => HidingAnswer);
        }
      }
    }

    private Visibility _QuestionVisibility = Visibility.Visible;
    public Visibility QuestionVisibility
    {
      get { return _QuestionVisibility; }
      set
      {
        if (value != _QuestionVisibility)
        {
          _QuestionVisibility = value;
          NotifyOfPropertyChange(() => QuestionVisibility);
        }
      }
    }

    private Visibility _AnswerVisibility = Visibility.Collapsed;
    public Visibility AnswerVisibility
    {
      get { return _AnswerVisibility; }
      set
      {
        if (value != _AnswerVisibility)
        {
          _AnswerVisibility = value;
          NotifyOfPropertyChange(() => AnswerVisibility);
        }
      }
    }

    public string QuestionHeader
    {
      get
      {
        if (Question == null)
          return StudyResources.ErrorMsgQuestionIsNull;

        return string.Format(StudyResources.StudyLineOrderQuestionHeader, Line.LineNumber);
      }
    }
    public string AnswerHeader
    {
      get
      {
        if (Answer == null)
          return StudyResources.ErrorMsgAnswerIsNull;

        return StudyResources.StudyLineOrderAnswerHeader;
      }
    }

    private int _QuestionDurationInMilliseconds;
    public int QuestionDurationInMilliseconds
    {
      get { return _QuestionDurationInMilliseconds; }
      set
      {
        if (value != _QuestionDurationInMilliseconds)
        {
          _QuestionDurationInMilliseconds = value;
          NotifyOfPropertyChange(() => QuestionDurationInMilliseconds);
        }
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Executes callback when answer is shown, or when exception is thrown.
    /// </summary>
    /// <param name="question">Question LineEdit</param>
    /// <param name="answer">Answer LineEdit</param>
    /// <param name="questionDurationInMilliseconds"></param>
    /// <param name="callback"></param>
    protected void AskQuestion(int questionDurationInMilliseconds,
                               ExceptionCheckCallback callback)
    {
      try
      {
        HideAnswer();
        BackgroundWorker timer = new BackgroundWorker();
        timer.DoWork += (s, e) =>
        {
          try
          {
            System.Threading.Thread.Sleep(questionDurationInMilliseconds);
            //if (AnswerVisibility == Visibility.Collapsed)
              
            ShowAnswer();
            callback(null);
          }
          catch (Exception ex)
          {
            callback(ex);
          }
        };

        timer.RunWorkerAsync();
      }
      catch (Exception ex)
      {
        callback(ex);
      }
    }

    public void Initialize(LineEdit line, MultiLineTextEdit multiLineText)
    {
      HideAnswer();
      Line = line;
      MultiLineText = multiLineText;
      PopulateQuestionAndAnswer();
    }

    protected virtual void PopulateQuestionAndAnswer()
    {
      var sb = new System.Text.StringBuilder();
      LineEdit prevLine = null;
      if (Line.LineNumber > 0)
      {
        //PREVIOUS LINE
        prevLine = (from line in MultiLineText.Lines
                    where line.LineNumber == Line.LineNumber - 1
                    select line).FirstOrDefault();
        //prevLine = MultiLineText.Lines[Line.LineNumber - 1];
        sb.AppendLine(StudyResources.StudyLineOrderLineNumberPrefix +
                      prevLine.LineNumber.ToString() +
                      StudyResources.StudyLineOrderSeparatorBetweenLineNumberAndLineText +
                      prevLine.Phrase.Text);

        //BLANK REPRESENTING THE LINE IN QUESTION
        sb.AppendLine(StudyResources.StudyLineOrderQuestionBlank);
      }
      else
      {
        //NO PREVIOUS LINE BECAUSE WE'RE STUDYING THE 1ST LINE IN MLT
        sb.AppendLine(StudyResources.StudyLineOrderQuestionWhatIsFirstLine);
      }

      Question = sb.ToString();
      Answer = Line.Phrase.Text;
    }

    public override void Show(ExceptionCheckCallback callback)
    {
      _DateTimeQuestionShown = DateTime.Now;
      ViewModelVisibility = Visibility.Visible;
      DispatchShown();
      var eventViewing = new History.Events.ViewingPhraseOnScreenEvent(Line.Phrase);
      History.HistoryPublisher.Ton.PublishEvent(eventViewing);
      var reviewingLineOrderEvent = new History.Events.ReviewingLineOrderEvent(Line, MultiLineText, GetReviewMethodId());
      HistoryPublisher.Ton.PublishEvent(reviewingLineOrderEvent);
      QuestionDurationInMilliseconds = int.Parse(StudyResources.DefaultTimeLineOrderQuestionInMilliseconds);
      AskQuestion(QuestionDurationInMilliseconds, (e) =>
        {
          if (e != null)
          {
            callback(e);
          }
          else
          {
            //WAIT FOR ALOTTED TIME FOR USER TO THINK ABOUT ANSWER.
            System.Threading.Thread.Sleep(int.Parse(StudyResources.DefaultThinkAboutAnswerTime));
            callback(null);
          }
        });
    }

    public override void Abort()
    {
      QuestionVisibility = Visibility.Collapsed;
      AnswerVisibility = Visibility.Collapsed;
      if (_Callback != null)
        _Callback(null);
    }
    
    private void HideAnswer()
    {
      HidingAnswer = true;
      AnswerVisibility = Visibility.Collapsed;
    }

    public bool CanShowAnswer
    {
      get
      {
        return (Answer != null);
      }
    }
    public void ShowAnswer()
    {
      AnswerVisibility = Visibility.Visible;
      HidingAnswer = false;

      _DateTimeAnswerShown = DateTime.Now;
      var duration = _DateTimeAnswerShown - _DateTimeQuestionShown;
      HistoryPublisher.Ton.PublishEvent(new ViewedPhraseOnScreenEvent(Line.Phrase, duration));
      //HistoryPublisher.Ton.PublishEvent(new ViewingPhraseOnScreenEvent(Line.Phrase));
      //HistoryPublisher.Ton.PublishEvent(new ViewedPhraseOnScreenEvent(Line.Phrase, duration));
    }

    protected override Guid GetReviewMethodId()
    {
      return Guid.Parse(StudyResources.ReviewMethodIdStudyLineOrderTimedQA);
    }

    #endregion
  }
}
