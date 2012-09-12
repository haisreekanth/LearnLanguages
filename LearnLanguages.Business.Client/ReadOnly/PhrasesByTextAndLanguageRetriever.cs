using System;
using System.Linq;
using System.Net;
using Csla;
using Csla.Serialization;
using Csla.Core;
using LearnLanguages.DataAccess;
using System.Collections.Generic;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class retrieves phrases by searching using their text and language.text.
  /// </summary>
  [Serializable]
  public class PhrasesByTextAndLanguageRetriever : Common.CslaBases.ReadOnlyBase<PhrasesByTextAndLanguageRetriever>
  {
    #region Factory Methods

    /// <summary>
    /// Retrieves PhraseEdits from the DB that match the PhraseEdits given in the criteria.
    /// It stores these retrieved phrases in a dictionary, with the given phrase.Id in the criteria
    /// as the keys and the retrieved phrases as the values.
    /// 
    /// For any phrase in criteria given that is not found in DB, the retrieved phrase will be null.
    /// 
    /// THIS IS NOT OPTIMIZED, AND IT DOES MOST OF THE WORK ON THE SERVER SIDE.  I MIGHT SHOULD TURN THIS INTO A COMMAND OBJECT
    /// AND HAVE THIS DONE ON THE CLIENT SIDE, ESPECIALLY SINCE CURRENTLY IT IS JUST USING PHRASELIST.GETALL() AND THEN 
    /// WORKING OFF OF THAT ANYWAY.
    /// </summary>
    /// <param name="criteria">phrases to be retrieved from DB</param>
    /// <param name="callback">callback executed once retrieval is complete</param>
    public static void CreateNew(Criteria.ListOfPhrasesCriteria criteria, 
      EventHandler<DataPortalResult<PhrasesByTextAndLanguageRetriever>> callback)
    {
      DataPortal.BeginCreate<PhrasesByTextAndLanguageRetriever>(criteria, callback);
    }

    /// <summary>
    /// Retrieves a single PhraseEdit from the DB that match the PhraseEdit given in 
    /// the criteria.  It stores this retrieved phrases in a dictionary, with the 
    /// given phrase.Id in the criteria as the keys and the retrieved phrases as the 
    /// values.
    /// 
    /// For any phrase in criteria given that is not found in DB, the retrieved phrase 
    /// will be null.
    /// 
    /// THIS IS NOT OPTIMIZED, AND IT DOES MOST OF THE WORK ON THE SERVER SIDE.  I MIGHT 
    /// SHOULD TURN THIS INTO A COMMAND OBJECT
    /// AND HAVE THIS DONE ON THE CLIENT SIDE, ESPECIALLY SINCE CURRENTLY IT IS JUST 
    /// USING PHRASELIST.GETALL() AND THEN 
    /// WORKING OFF OF THAT ANYWAY.
    /// </summary>
    /// <param name="criteria">single phrase that you want to retrieve</param>
    /// <param name="callback">callback executed once retrieval is complete</param>
    public static void CreateNew(PhraseEdit criteria,
      EventHandler<DataPortalResult<PhrasesByTextAndLanguageRetriever>> callback)
    {
      DataPortal.BeginCreate<PhrasesByTextAndLanguageRetriever>(criteria, callback);
    }

#if SILVERLIGHT
    /// <summary>
    /// Retrieves a single PhraseEdit FROM THE GIVEN LIST that match 
    /// the PhraseEdit.Text and Language.Text in the criteria.
    /// It stores this in the RetrievedSinglePhrase Property if found.
    /// 
    /// THIS DOES NOT TOUCH THE SERVER.  I JUST PUT IT HERE TO HAVE THE 
    /// RETRIEVAL PARALLEL THE ONES THAT DO INDEED TOUCH THE SERVER.
    /// </summary>
    /// <param name="criteria">single phrase that you want to retrieve</param>
    public static void CreateNew(Criteria.FindPhraseInPhraseListCriteria criteria,
      EventHandler<DataPortalResult<PhrasesByTextAndLanguageRetriever>> callback)
    {
      DataPortal.BeginCreate<PhrasesByTextAndLanguageRetriever>(criteria, callback,
        DataPortal.ProxyModes.LocalOnly);      
    }
#endif 

#if !SILVERLIGHT
    /// <summary>
    /// Retrieves PhraseEdits from the DB that match the PhraseEdits given in the criteria.
    /// It stores these retrieved phrases in a dictionary, with the given phrase.Id in the criteria
    /// as the keys and the retrieved phrases as the values.
    /// 
    /// For any phrase in criteria given that is not found in DB, the retrieved phrase will be null.
    /// 
    /// THIS IS NOT OPTIMIZED, AND IT DOES MOST OF THE WORK ON THE SERVER SIDE.  I MIGHT SHOULD TURN THIS INTO A COMMAND OBJECT
    /// AND HAVE THIS DONE ON THE CLIENT SIDE, ESPECIALLY SINCE CURRENTLY IT IS JUST USING PHRASELIST.GETALL() AND THEN 
    /// WORKING OFF OF THAT ANYWAY.
    /// </summary>
    /// <param name="criteria">phrases to be retrieved from DB</param>
    public static PhrasesByTextAndLanguageRetriever CreateNew(Criteria.ListOfPhrasesCriteria criteria)
    {
      return DataPortal.Create<PhrasesByTextAndLanguageRetriever>(criteria);
    }

    /// <summary>
    /// Retrieves a single PhraseEdit from the DB that match the PhraseEdit given in the criteria.
    /// It stores this retrieved phrases in a dictionary, with the given phrase.Id in the criteria
    /// as the keys and the retrieved phrases as the values.
    /// 
    /// For any phrase in criteria given that is not found in DB, the retrieved phrase will be null.
    /// 
    /// THIS IS NOT OPTIMIZED, AND IT DOES MOST OF THE WORK ON THE SERVER SIDE.  I MIGHT SHOULD TURN THIS INTO A COMMAND OBJECT
    /// AND HAVE THIS DONE ON THE CLIENT SIDE, ESPECIALLY SINCE CURRENTLY IT IS JUST USING PHRASELIST.GETALL() AND THEN 
    /// WORKING OFF OF THAT ANYWAY.
    /// </summary>
    /// <param name="criteria">single phrase that you want to retrieve</param>
    /// <param name="callback">callback executed once retrieval is complete</param>
    public static PhrasesByTextAndLanguageRetriever CreateNew(PhraseEdit criteria)
    {
      return DataPortal.Create<PhrasesByTextAndLanguageRetriever>(criteria);
    }

    /// <summary>
    /// Retrieves a single PhraseEdit FROM THE GIVEN LIST that match 
    /// the PhraseEdit.Text and Language.Text in the criteria.
    /// It stores this in the RetrievedSinglePhrase Property if found.
    /// 
    /// THIS DOES NOT TOUCH THE SERVER.  I JUST PUT IT HERE TO HAVE THE 
    /// RETRIEVAL PARALLEL THE ONES THAT DO INDEED TOUCH THE SERVER.
    /// </summary>
    /// <param name="criteria">single phrase that you want to retrieve</param>
    [RunLocal]
    public static void CreateNew(Criteria.FindPhraseInPhraseListCriteria criteria)
    {
      DataPortal.Create<PhrasesByTextAndLanguageRetriever>(criteria);
    }

#endif

    #endregion

    #region Properties

    public static readonly PropertyInfo<Guid> RetrieverIdProperty = RegisterProperty<Guid>(c => c.RetrieverId);
    public Guid RetrieverId
    {
      get { return GetProperty(RetrieverIdProperty); }
      private set { LoadProperty(RetrieverIdProperty, value); }
    }

    public static readonly PropertyInfo<MobileDictionary<Guid, PhraseEdit>> RetrievedPhrasesProperty =
      RegisterProperty<MobileDictionary<Guid, PhraseEdit>>(c => c.RetrievedPhrases);
    public MobileDictionary<Guid, PhraseEdit> RetrievedPhrases
    {
      get { return ReadProperty(RetrievedPhrasesProperty); }
      private set { LoadProperty(RetrievedPhrasesProperty, value); }
    }

    public static readonly PropertyInfo<PhraseEdit> RetrievedSinglePhraseProperty =
      RegisterProperty<PhraseEdit>(c => c.RetrievedSinglePhrase);
    /// <summary>
    /// If this retriever was used to get a single phrase, then this property will be used.
    /// If it is successful, then the phrase will be here.  If it doesn't find it, then this
    /// will be null.
    /// 
    /// HOWEVER, if a list of phrases is used, this will always be null!
    /// </summary>
    public PhraseEdit RetrievedSinglePhrase
    {
      get { return ReadProperty(RetrievedSinglePhraseProperty); }
      private set { LoadProperty(RetrievedSinglePhraseProperty, value); }
    }


    #endregion

    #region DP_XYZ

    
#if !SILVERLIGHT

    public void DataPortal_Create(Criteria.ListOfPhrasesCriteria criteria)
    {
      //WE ARE GOING TO GET ALL THE PHRASES
      //WE WILL THEN ITERATE THROUGH OUR CRITERIA PHRASES, POPULATING OUR
      //RETRIEVED PHRASES 

      //INITIALIZE
      RetrieverId = Guid.NewGuid();
      if (RetrievedPhrases == null)
        RetrievedPhrases = new MobileDictionary<Guid, PhraseEdit>();

      ////GET ALL PHRASES (FOR THIS USER ONLY)
      //PhraseList allPhrases = PhraseList.GetAll();

      //KEY = TEXT, VALUE = PHRASELIST CONTAINING ALL PHRASES THAT CONTAIN THE KEY TEXT
      var phraseLists = new Dictionary<string, PhraseList>();
      for (int i = 0; i < criteria.Phrases.Count; i++)
      {
        var criteriaPhrase = criteria.Phrases[i];

        //IF WE'VE ALREADY DONE THIS PHRASE, THEN GO ON TO THE NEXT ONE, SO WE DON'T DUPLICATE WORK
        if (RetrievedPhrases.ContainsKey(criteriaPhrase.Id))
          continue;

        //INITIALIZE RETRIEVED PHRASE
        PhraseEdit retrievedPhrase = null;

        //GET ALL PHRASES THAT CONTAIN THIS PHRASE, IN ANY LANGUAGE
        var allPhrasesContainingPhrase = PhraseList.GetAllContainingText(criteriaPhrase.Text);

        //IF WE FOUND A PHRASE/MULTIPLE PHRASES, THEN WE WANT THE ONE THAT MATCHES OUR
        //CRITERIA PHRASE IN TEXT AND LANGUAGE
        if (allPhrasesContainingPhrase.Count > 0)
        {
          retrievedPhrase = (from phrase in allPhrasesContainingPhrase
                             where phrase.Text == criteriaPhrase.Text &&
                                   phrase.Language.Text == criteriaPhrase.Language.Text
                             select phrase).FirstOrDefault();
        }

        if (retrievedPhrase != null && retrievedPhrase.IsChild)
        {
          //WE ONLY WANT NON-CHILD VERSIONS OF PHRASE
          var nonChildVersion = PhraseEdit.GetPhraseEdit(retrievedPhrase.Id);
          RetrievedPhrases.Add(criteriaPhrase.Id, nonChildVersion);
        }
        else if (retrievedPhrase != null && !retrievedPhrase.IsChild)
        {
          //PHRASE IS ALREADY NOT A CHILD, SO ADD IT
          RetrievedPhrases.Add(criteriaPhrase.Id, retrievedPhrase);
        }
        else
        {
          //NO RETRIEVED PHRASE, SO ADD NULL FOR THIS CRITERIAPHRASE.ID
          RetrievedPhrases.Add(criteriaPhrase.Id, null);
        }
      }
    }

    public void DataPortal_Create(PhraseEdit criteria)
    {
      //INITIALIZE
      RetrieverId = Guid.NewGuid();
      RetrievedPhrases = null;
      RetrievedSinglePhrase = null;

      //GET ALL PHRASES (FOR THIS USER ONLY)
      PhraseList allPhrases = PhraseList.GetAll();

      var retrievedPhrase = FindPhraseInPhraseList(criteria.Text, criteria.Language.Text, allPhrases);

      //if we directly add this retrievedPhrase, then it will be a child
      //we need to get the non-child version of this
      //RetrievedPhrases.Add(criteriaPhrase.Id, retrievedPhrase);
      if (retrievedPhrase != null && retrievedPhrase.IsChild)
      {
        var nonChildVersion = PhraseEdit.GetPhraseEdit(retrievedPhrase.Id);
        RetrievedSinglePhrase = nonChildVersion;
      }
    }

    public void DataPortal_Create(Criteria.FindPhraseInPhraseListCriteria criteria)
    {
      //INITIALIZE
      RetrieverId = Guid.NewGuid();
      RetrievedPhrases = null;
      RetrievedSinglePhrase = null;

      var retrievedPhrase = FindPhraseInPhraseList(criteria.PhraseText, 
                                                   criteria.LanguageText, 
                                                   criteria.Phrases);

      //if we directly add this retrievedPhrase, then it will be a child
      //we need to get the non-child version of this
      //RetrievedPhrases.Add(criteriaPhrase.Id, retrievedPhrase);
      if (criteria.GetPhraseFromDB && retrievedPhrase != null && retrievedPhrase.IsChild)
      {
        var nonChildVersion = PhraseEdit.GetPhraseEdit(retrievedPhrase.Id);
        RetrievedSinglePhrase = nonChildVersion;
      }
      else
        RetrievedSinglePhrase = retrievedPhrase;

    }

    


    /// <summary>
    /// Returns the FirstOrDefault PhraseEdit in phrases matching exactly the phraseText and languageText given.
    /// </summary>
    /// <param name="phraseText">PhraseEdit.Text to match</param>
    /// <param name="languageText">PhraseEdit.Language.Text to match</param>
    /// <param name="phrases">list of phrases in which to search</param>
    /// <returns>First PhraseEdit matching phraseText and languageText if found, or null if none found</returns>
    private PhraseEdit FindPhraseInPhraseList(string phraseText, string languageText, PhraseList phrases)
    {
      return (from phrase in phrases
              where phrase.Text == phraseText &&
                    phrase.Language.Text == languageText
              select phrase).FirstOrDefault();
    }
  
#endif

    #endregion
  }
}
