using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using LearnLanguages.Common;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;
using LearnLanguages.History;
using LearnLanguages.History.Events;

namespace LearnLanguages.Study.ViewModels
{
  [Export(typeof(StudyPhraseTimedQuestionAnswerViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class StudyPhraseTimedQuestionAnswerViewModel : StudyItemViewModelBase
  {
    #region Ctors and Init

    public StudyPhraseTimedQuestionAnswerViewModel()
    {
      StudyItemTitle = StudyResources.StudyPhraseTimedQuestionAnswerStudyItemTitle;
      Services.EventAggregator.Subscribe(this);
    }

    #endregion

    #region Properties

    private PhraseEdit _Question;
    public PhraseEdit Question
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

    private PhraseEdit _Answer;
    public PhraseEdit Answer
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

        if (Question.Language == null)
          return StudyResources.ErrorMsgLanguageIsNull;

        if (string.IsNullOrEmpty(Question.Language.Text))
          return StudyResources.ErrorMsgLanguageTextIsNullOrEmpty;

        return Question.Language.Text;
      }
    }
    public string AnswerHeader
    {
      get
      {
        if (Answer == null)
          return StudyResources.ErrorMsgAnswerIsNull;

        if (Answer.Language == null)
          return StudyResources.ErrorMsgLanguageIsNull;

        if (string.IsNullOrEmpty(Answer.Language.Text))
          return StudyResources.ErrorMsgLanguageTextIsNullOrEmpty;

        return Answer.Language.Text;
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
    /// <param name="question">Question PhraseEdit</param>
    /// <param name="answer">Answer PhraseEdit</param>
    /// <param name="questionDurationInMilliseconds"></param>
    /// <param name="callback"></param>
    protected void AskQuestion(PhraseEdit question,
                            PhraseEdit answer,
                            int questionDurationInMilliseconds,
                            ExceptionCheckCallback callback)
    {
      try
      {
        //BingTranslatorService.LanguageServiceClient client = new BingTranslatorService.LanguageServiceClient();
        //client.SpeakCompleted += client_SpeakCompleted;
        //client.SpeakAsync(StudyResources.BingAppId, question.Text, BingTranslateHelper.GetLanguageCode(question.Language.Text), string.Empty, string.Empty);

        HideAnswer();
        Question = question;
        Answer = answer;
        //QuestionDurationInMilliseconds = questionDurationInMilliseconds;
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

    public void Initialize(PhraseEdit question, PhraseEdit answer, ExceptionCheckCallback callback)
    {
      _InitialQuestionText = question.Text;
      _ModifiedQuestionText = "";
      _InitialAnswerText = answer.Text;
      _ModifiedAnswerText = "";

      Question = question;
      Answer = answer;

      //FINISH UP
      var words = Question.Text.ParseIntoWords();
      var durationMilliseconds = words.Count * (int.Parse(StudyResources.DefaultMillisecondsTimePerWordInQuestion));
      QuestionDurationInMilliseconds = durationMilliseconds;
      HideAnswer();
      callback(null);

      //if (!question.IsValid)
      //  callback(new ArgumentException("question is not a valid phrase", "question"));
      //if (!answer.IsValid)
      //  callback(new ArgumentException("answer is not a valid phrase", "answer"));


      ////THE POINT OF THIS METHOD IS TO ASSIGN QUESTION, ANSWER, AND CALCULATE TIMING
      ////WE GET THE ACTUAL PHRASEEDITS FROM THE DB
      ////get these anew from database, so we know that we are NOT dealing
      ////with any children, to make things smoother when we are editing/saving them.
      

      //var criteria = new Business.Criteria.ListOfPhrasesCriteria(question, answer);
      //Business.PhrasesByTextAndLanguageRetriever.CreateNew(criteria, (s, r) =>
      //  {
      //    if (r.Error != null)
      //    {
      //      callback(r.Error);
      //      return;
      //    }

      //    var retriever = r.Object;

      //    Question = retriever.RetrievedPhrases[question.Id];
      //    Answer = retriever.RetrievedPhrases[answer.Id];

      //    if (Question == null)
      //      callback(new ArgumentException("phrase not found in DB", "question"));
      //    if (Answer == null)
      //      callback(new ArgumentException("phrase not found in DB", "answer"));

      //    //FINISH UP
      //    var words = Question.Text.ParseIntoWords();
      //    var durationMilliseconds = words.Count * (int.Parse(StudyResources.DefaultMillisecondsTimePerWordInQuestion));
      //    QuestionDurationInMilliseconds = durationMilliseconds;
      //    HideAnswer();
      //    callback(null);
      //  });
    }

    public override void Show(ExceptionCheckCallback callback)
    {
      _DateTimeQuestionShown = DateTime.Now;
      ViewModelVisibility = Visibility.Visible;
      DispatchShown();
      var eventViewing = new History.Events.ViewingPhraseOnScreenEvent(Question);
      History.HistoryPublisher.Ton.PublishEvent(eventViewing);
      AskQuestion(Question, Answer, QuestionDurationInMilliseconds, (e) =>
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
      UpdateEditAnswerButtonVisibilities();
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
      UpdateEditAnswerButtonVisibilities();
      HidingAnswer = false;

      _DateTimeAnswerShown = DateTime.Now;
      var duration = _DateTimeAnswerShown - _DateTimeQuestionShown;
      HistoryPublisher.Ton.PublishEvent(new ViewedPhraseOnScreenEvent(Question, duration));
      HistoryPublisher.Ton.PublishEvent(new ViewingPhraseOnScreenEvent(Answer));
      HistoryPublisher.Ton.PublishEvent(new ViewedPhraseOnScreenEvent(Answer, duration));
    }

    protected override Guid GetReviewMethodId()
    {
      return Guid.Parse(StudyResources.ReviewMethodIdStudyPhraseTimedQA);
    }

    #endregion

    #region Edit/Save Question Stuff

    #region Question

    private Visibility _EditQuestionButtonVisibility = Visibility.Visible;
    public Visibility EditQuestionButtonVisibility
    {
      get { return _EditQuestionButtonVisibility; }
      set
      {
        if (value != _EditQuestionButtonVisibility)
        {
          _EditQuestionButtonVisibility = value;
          NotifyOfPropertyChange(() => EditQuestionButtonVisibility);
        }
      }
    }

    private Visibility _SaveQuestionButtonVisibility = Visibility.Collapsed;
    public Visibility SaveQuestionButtonVisibility
    {
      get { return _SaveQuestionButtonVisibility; }
      set
      {
        if (value != _SaveQuestionButtonVisibility)
        {
          _SaveQuestionButtonVisibility = value;
          NotifyOfPropertyChange(() => SaveQuestionButtonVisibility);
        }
      }
    }

    public string ButtonLabelEditQuestion { get { return StudyResources.ButtonLabelEdit; } }
    public string ButtonLabelSaveQuestion { get { return StudyResources.ButtonLabelSave; } }

    private bool _QuestionIsReadOnly = true;
    public bool QuestionIsReadOnly
    {
      get { return _QuestionIsReadOnly; }
      set
      {
        if (value != _QuestionIsReadOnly)
        {
          _QuestionIsReadOnly = value;
          NotifyOfPropertyChange(() => QuestionIsReadOnly);
        }
      }
    }

    public bool CanEditQuestion
    {
      get
      {
        //if the question is read only, then we are NOT currently editing the question.
        //therefore, we can press the edit question button which is what CanEditQuestion is asking.
        return QuestionIsReadOnly;
      }
    }
    public void EditQuestion()
    {
      QuestionIsReadOnly = false;
      //hack: UpdateEditButtonVisibilities method instead of visibility converter
      UpdateEditQuestionButtonVisibilities();
    }

    public bool CanSaveQuestion
    {
      get
      {
        //return Question != null && Question.IsSavable && SaveQuestionButtonIsEnabled;
        return Question != null && Question.IsValid && SaveQuestionButtonIsEnabled;
      }
    }
    public void SaveQuestion()
    {
      QuestionIsReadOnly = true;
      SaveQuestionButtonIsEnabled = false;

      #region Get the DB version of question, then modify it and save

      //STORE OUR MODIFIED TEXT
      _ModifiedQuestionText = Question.Text;

      //PREPARE QUESTION TO BE SEARCHED FOR IN DB
      Question.Text = _InitialQuestionText;

      #region SEARCH IN DB FOR QUESTION (WITH INITIAL TEXT)
      Business.PhrasesByTextAndLanguageRetriever.CreateNew(Question, (s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          var dbQuestion = r.Object.RetrievedSinglePhrase;
          if (dbQuestion == null)
          {
            //QUESTION NOT IN DB, BUT QUESTION IS A CHILD
            if (Question.IsChild)
            {
              //CANNOT SAVE THE QUESTION DIRECTLY BECAUSE IT IS A CHILD

              #region CREATE NEW PHRASE EDIT, LOAD FROM QUESTION DTO, SAVE QUESTION
              PhraseEdit.NewPhraseEdit((s3, r3) =>
                {
                  if (r3.Error != null)
                    throw r3.Error;

                  var newQuestionObj = r3.Object;

                  Question.Text = _ModifiedQuestionText;
                  var dto = Question.CreateDto();
                  dto.Id = Guid.NewGuid();

                  newQuestionObj.LoadFromDtoBypassPropertyChecks(dto);
                  Question = newQuestionObj;

                  #region Save Question
                  Question.BeginSave((s4, r4) =>
                    {
                      if (r4.Error != null)
                        throw r4.Error;

                      Question = (PhraseEdit)r4.NewObject;
                      _InitialQuestionText = Question.Text;
                      _ModifiedQuestionText = "";
                      SaveQuestionButtonIsEnabled = true;
                      QuestionIsReadOnly = true;
                      UpdateEditQuestionButtonVisibilities();
                    });
                  #endregion
                });
              #endregion
            }
            //QUESTION NOT IN DB, BUT QUESTION IS NOT A CHILD
            else
            {
              #region SAVE THE QUESTION DIRECTLY, B/C IT IS NOT A CHILD
              Question.BeginSave((s2, r2) =>
                {
                  if (r2.Error != null)
                    throw r2.Error;

                  Question = (PhraseEdit)r2.NewObject;
                  _InitialQuestionText = Question.Text;
                  _ModifiedQuestionText = "";
                  SaveQuestionButtonIsEnabled = true;
                  QuestionIsReadOnly = true;
                  UpdateEditQuestionButtonVisibilities();
                });
            #endregion
            }
          }
          //QUESTION WAS FOUND IN DB
          else
          {
            #region REASSIGN THE QUESTION WITH THE DBQUESTION, SAVE WITH THE MODIFIED TEXT
            Question = dbQuestion;
            Question.Text = _ModifiedQuestionText;
            Question.BeginSave((s5, r5) =>
            {
              if (r5.Error != null)
                throw r5.Error;

              Question = (PhraseEdit)r5.NewObject;
              _InitialQuestionText = Question.Text;
              _ModifiedQuestionText = "";
              SaveQuestionButtonIsEnabled = true;
              QuestionIsReadOnly = true;
              UpdateEditQuestionButtonVisibilities();
            });
            #endregion
          }
        });

      #endregion

      #endregion
    }

    private bool _SaveQuestionButtonIsEnabled = true;
    public bool SaveQuestionButtonIsEnabled
    {
      get { return _SaveQuestionButtonIsEnabled; }
      set
      {
        if (value != _SaveQuestionButtonIsEnabled)
        {
          _SaveQuestionButtonIsEnabled = value;
          NotifyOfPropertyChange(() => SaveQuestionButtonIsEnabled);
          NotifyOfPropertyChange(() => CanSaveQuestion);
        }
      }
    }

    private void UpdateEditQuestionButtonVisibilities()
    {
      if (QuestionIsReadOnly)
      {
        EditQuestionButtonVisibility = Visibility.Visible;
        SaveQuestionButtonVisibility = Visibility.Collapsed;
      }
      else
      {
        EditQuestionButtonVisibility = Visibility.Collapsed;
        SaveQuestionButtonVisibility = Visibility.Visible;
      }
    }

    private string _InitialQuestionText;
    private string _ModifiedQuestionText;

    #endregion

    #region Answer

    private Visibility _EditAnswerButtonVisibility = Visibility.Collapsed;
    public Visibility EditAnswerButtonVisibility
    {
      get { return _EditAnswerButtonVisibility; }
      set
      {
        if (value != _EditAnswerButtonVisibility)
        {
          _EditAnswerButtonVisibility = value;
          NotifyOfPropertyChange(() => EditAnswerButtonVisibility);
        }
      }
    }

    private Visibility _SaveAnswerButtonVisibility = Visibility.Collapsed;
    public Visibility SaveAnswerButtonVisibility
    {
      get { return _SaveAnswerButtonVisibility; }
      set
      {
        if (value != _SaveAnswerButtonVisibility)
        {
          _SaveAnswerButtonVisibility = value;
          NotifyOfPropertyChange(() => SaveAnswerButtonVisibility);
        }
      }
    }

    public string ButtonLabelEditAnswer { get { return StudyResources.ButtonLabelEdit; } }
    public string ButtonLabelSaveAnswer { get { return StudyResources.ButtonLabelSave; } }

    private bool _AnswerIsReadOnly = true;
    public bool AnswerIsReadOnly
    {
      get { return _AnswerIsReadOnly; }
      set
      {
        if (value != _AnswerIsReadOnly)
        {
          _AnswerIsReadOnly = value;
          NotifyOfPropertyChange(() => AnswerIsReadOnly);
        }
      }
    }

    public bool CanEditAnswer
    {
      get
      {
        //if the question is read only, then we are NOT currently editing the question.
        //therefore, we can press the edit question button which is what CanEditAnswer is asking.
        return AnswerIsReadOnly;
      }
    }
    public void EditAnswer()
    {
      AnswerIsReadOnly = false;
      //hack: UpdateEditButtonVisibilities method instead of visibility converter
      UpdateEditAnswerButtonVisibilities();
    }

    public bool CanSaveAnswer
    {
      get
      {
        //return Answer != null && Answer.IsSavable && SaveAnswerButtonIsEnabled;
        return Answer != null && Answer.IsValid && SaveAnswerButtonIsEnabled;
      }
    }
    public void SaveAnswer()
    {
      AnswerIsReadOnly = true;
      SaveAnswerButtonIsEnabled = false;

      #region Get the DB version of answer, then modify it and save

      //STORE OUR MODIFIED TEXT
      _ModifiedAnswerText = Answer.Text;

      //PREPARE ANSWER TO BE SEARCHED FOR IN DB
      Answer.Text = _InitialAnswerText;

      #region SEARCH IN DB FOR ANSWER (WITH INITIAL TEXT)
      Business.PhrasesByTextAndLanguageRetriever.CreateNew(Answer, (s, r) =>
      {
        if (r.Error != null)
          throw r.Error;

        var dbAnswer = r.Object.RetrievedSinglePhrase;
        if (dbAnswer == null)
        {
          //ANSWER NOT IN DB, BUT ANSWER IS A CHILD
          if (Answer.IsChild)
          {
            //CANNOT SAVE THE ANSWER DIRECTLY BECAUSE IT IS A CHILD

            #region CREATE NEW PHRASE EDIT, LOAD FROM ANSWER DTO, SAVE ANSWER
            PhraseEdit.NewPhraseEdit((s3, r3) =>
            {
              if (r3.Error != null)
                throw r3.Error;

              var newAnswerObj = r3.Object;

              Answer.Text = _ModifiedAnswerText;
              var dto = Answer.CreateDto();
              dto.Id = Guid.NewGuid();

              newAnswerObj.LoadFromDtoBypassPropertyChecks(dto);
              Answer = newAnswerObj;

              #region Save Answer
              Answer.BeginSave((s4, r4) =>
              {
                if (r4.Error != null)
                  throw r4.Error;

                Answer = (PhraseEdit)r4.NewObject;
                _InitialAnswerText = Answer.Text;
                _ModifiedAnswerText = "";
                SaveAnswerButtonIsEnabled = true;
                AnswerIsReadOnly = true;
                UpdateEditAnswerButtonVisibilities();
              });
              #endregion
            });
            #endregion
          }
          //ANSWER NOT IN DB, BUT ANSWER IS NOT A CHILD
          else
          {
            #region SAVE THE ANSWER DIRECTLY, B/C IT IS NOT A CHILD
            Answer.BeginSave((s2, r2) =>
            {
              if (r2.Error != null)
                throw r2.Error;

              Answer = (PhraseEdit)r2.NewObject;
              _InitialAnswerText = Answer.Text;
              _ModifiedAnswerText = "";
              SaveAnswerButtonIsEnabled = true;
              AnswerIsReadOnly = true;
              UpdateEditAnswerButtonVisibilities();
            });
            #endregion
          }
        }
        //ANSWER WAS FOUND IN DB
        else
        {
          #region REASSIGN THE ANSWER WITH THE DBANSWER, SAVE WITH THE MODIFIED TEXT
          Answer = dbAnswer;
          Answer.Text = _ModifiedAnswerText;
          Answer.BeginSave((s5, r5) =>
          {
            if (r5.Error != null)
              throw r5.Error;

            Answer = (PhraseEdit)r5.NewObject;
            _InitialAnswerText = Answer.Text;
            _ModifiedAnswerText = "";
            SaveAnswerButtonIsEnabled = true;
            AnswerIsReadOnly = true;
            UpdateEditAnswerButtonVisibilities();
          });
          #endregion
        }
      });

      #endregion

      #endregion
    }

    private bool _SaveAnswerButtonIsEnabled = true;
    public bool SaveAnswerButtonIsEnabled
    {
      get { return _SaveAnswerButtonIsEnabled; }
      set
      {
        if (value != _SaveAnswerButtonIsEnabled)
        {
          _SaveAnswerButtonIsEnabled = value;
          NotifyOfPropertyChange(() => SaveAnswerButtonIsEnabled);
          NotifyOfPropertyChange(() => CanSaveAnswer);
        }
      }
    }

    private void UpdateEditAnswerButtonVisibilities()
    {
      //IF THE ANSWER ISN'T SHOWN YET, WE DON'T WANT TO SEE EDIT/SAVE BUTTONS EITHER
      if (AnswerVisibility == Visibility.Collapsed)
      {
        EditAnswerButtonVisibility = Visibility.Collapsed;
        SaveAnswerButtonVisibility = Visibility.Collapsed;
      }
      //IF THE ANSWER BUTTON IS SHOWN AND ANSWER IS READONLY, THEN WE CAN EDIT BUT NOT SAVE
      else if (AnswerIsReadOnly)
      {
        EditAnswerButtonVisibility = Visibility.Visible;
        SaveAnswerButtonVisibility = Visibility.Collapsed;
      }
      //OTHERWISE, WE CAN SAVE BUT NOT EDIT
      else
      {
        EditAnswerButtonVisibility = Visibility.Collapsed;
        SaveAnswerButtonVisibility = Visibility.Visible;
      }
    }

    private string _InitialAnswerText;
    private string _ModifiedAnswerText;

    #endregion

    #endregion
  }
}
