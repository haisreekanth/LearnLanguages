﻿using System;
using System.Linq;
using System.Collections.Generic;
using LearnLanguages.Business;
using LearnLanguages.Common;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Offer;

namespace LearnLanguages.Study
{
  /// <summary>
  /// The studier that actually "does" something.
  /// 
  /// This implementation: "Dumb" studier.  This doesn't know anything about whether 
  /// a phrase's history, it "simply" takes the phrase, produces an offer to show a Q & A about it.
  /// Listens for the ViewModel to publish a Q & A response
  /// </summary>
  public class DefaultPhraseMeaningStudier : StudierBase<PhraseEdit>
  {
    #region Ctors and Init
    public DefaultPhraseMeaningStudier()
    {

    }
    #endregion

    #region Methods

    public override void InitializeForNewStudySession(PhraseEdit target)
    {
      _Target = target;
    }

    public override void GetNextStudyItemViewModel(Common.Delegates.AsyncCallback<StudyItemViewModelArgs> callback)
    {
      //USING THE TARGET PHRASE, 
      //IF IT IS IN THE NATIVE LANGUAGE, THEN IT JUST POPS UP A NATIVE LANGUAGE STUDY QUESTION.
      //IF IT IS IN A DIFFERENT LANGUAGE, THEN IT POPS UP EITHER DIRECTION Q & A, 50% CHANCE.
      StudyDataRetriever.CreateNew((s, r) =>
      {
        if (r.Error != null)
          throw r.Error;

        var retriever = r.Object;
        var nativeLanguageText = retriever.StudyData.NativeLanguageText;
        if (string.IsNullOrEmpty(nativeLanguageText))
          throw new StudyException("No native language set.");

        if (_Target == null)
          throw new StudyException("No PhraseEdit to study, _StudyJobInfo.Target == null.");

        var phraseEdit = _Target;
        var phraseText = phraseEdit.Text;
        if (string.IsNullOrEmpty(phraseText))
          throw new StudyException("Attempted to study empty phrase text, (PhraseEdit)_Target.Text is null or empty.");

        var languageText = phraseEdit.Language.Text;

        //WE HAVE A PHRASEEDIT WITH A LANGUAGE AND WE HAVE OUR NATIVE LANGUAGE.
        //IF THE TWO LANGUAGES ARE DIFFERENT, THEN WE CREATE A TRANSLATION Q & A.
        //IF THE TWO LANGUAGES ARE THE SAME, THEN WE CREATE A STUDY NATIVE LANGUAGE PHRASE Q & A.

        bool languagesAreDifferent = (languageText != nativeLanguageText);
        if (languagesAreDifferent)
        {
          //DO A TRANSLATION Q & A
          var qaViewModel = new ViewModels.StudyQuestionAnswerViewModel();
          qaViewModel.Initialize(phraseEdit, phraseEdit, 1000); //debug...need to populate answer.
          StudyItemViewModelArgs returnArgs = new StudyItemViewModelArgs(qaViewModel);

          //INITIATE THE CALLBACK TO LET IT KNOW WE HAVE OUR VIEWMODEL!  WHEW THAT'S A LOT OF ASYNC.
          callback(this, new ResultArgs<StudyItemViewModelArgs>(returnArgs));
        }
        else
        {
          //DO A NATIVE LANGUAGE Q & A
        }
      });
    }

    #endregion

  }
}