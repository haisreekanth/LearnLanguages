using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;
using Microsoft.Silverlight.Testing;
using LearnLanguages.DataAccess;
using LearnLanguages.DataAccess.Exceptions;
using System.Linq;

namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("phrase")]
  public class PhraseTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    private LanguageEdit _ServerEnglishLang;
    private LanguageEdit _ServerSpanishLang;
    
    [ClassInitialize]
    [Asynchronous]
    public void InitializePhraseTests()
    {

      //WE NEED TO UPDATE THE CLIENT SeedData.Ton IDS.  
      var isLoaded = false;
      var phrasesCorrected = false;
      Exception error = new Exception();
      Exception errorPhraseList = new Exception();
      LanguageList allLanguages = null;
      PhraseList allPhrases = null;

      LanguageList.GetAll((s, r) =>
      {
        #region Initialize Language Data
        error = r.Error;
        if (error != null)
          throw error;

        allLanguages = r.Object;
        _ServerEnglishLang = (from language in allLanguages
                              where language.Text == SeedData.Ton.EnglishText
                              select language).First();

        SeedData.Ton.EnglishLanguageDto.Id = _ServerEnglishLang.Id;

        _ServerSpanishLang = (from language in allLanguages
                              where language.Text == SeedData.Ton.SpanishText
                              select language).First();

        SeedData.Ton.SpanishLanguageDto.Id = _ServerSpanishLang.Id;

        #endregion

        isLoaded = true;

        PhraseList.GetAll((s2, r2) =>
          {
            errorPhraseList = r2.Error;
            if (errorPhraseList != null)
              throw errorPhraseList;

            allPhrases = r2.Object;

            var serverHelloPhraseQuery = (from phrase in allPhrases
                                     where phrase.Text == SeedData.Ton.HelloText && 
                                           phrase.Language.Text == SeedData.Ton.EnglishText
                                     select phrase);
            PhraseEdit serverHelloPhrase = null;
            if (serverHelloPhraseQuery.Count() == 0) //we don't have the hello phrase in the db, so put it there
            {
              var phrase = allPhrases[0];
              phrase.BeginEdit();
              phrase.Text = SeedData.Ton.HelloText;
              phrase.Language = _ServerEnglishLang;
              phrase.ApplyEdit();
              serverHelloPhrase = phrase;
            }
            else
              serverHelloPhrase = serverHelloPhraseQuery.First();


            var serverHolaPhraseQuery = (from phrase in allPhrases
                                          where phrase.Text == SeedData.Ton.HolaText &&
                                                phrase.Language.Text == SeedData.Ton.EnglishText
                                          select phrase);
            PhraseEdit serverHolaPhrase = null;
            if (serverHolaPhraseQuery.Count() == 0) //we don't have the Hola phrase in the db, so put it there
            {
              var phrase = allPhrases[1];
              phrase.BeginEdit();
              phrase.Text = SeedData.Ton.HolaText;
              phrase.Language = _ServerSpanishLang;
              phrase.ApplyEdit();
              serverHolaPhrase = phrase;
            }
            else
              serverHolaPhrase = serverHolaPhraseQuery.First();

            var serverLongPhraseQuery = (from phrase in allPhrases
                                         where phrase.Text == SeedData.Ton.LongText &&
                                               phrase.Language.Text == SeedData.Ton.EnglishText
                                         select phrase);
            PhraseEdit serverLongPhrase = null;
            if (serverLongPhraseQuery.Count() == 0) //we don't have the Long phrase in the db, so put it there
            {
              var phrase = allPhrases[2];
              phrase.BeginEdit();
              phrase.Text = SeedData.Ton.LongText;
              phrase.Language = _ServerEnglishLang;
              phrase.ApplyEdit();
              serverLongPhrase = phrase;
            }
            else
              serverLongPhrase = serverLongPhraseQuery.First();


            var serverDogPhraseQuery = (from phrase in allPhrases
                                         where phrase.Text == SeedData.Ton.DogText &&
                                               phrase.Language.Text == SeedData.Ton.EnglishText
                                         select phrase);
            PhraseEdit serverDogPhrase = null;
            if (serverDogPhraseQuery.Count() == 0) //we don't have the Dog phrase in the db, so put it there
            {
              var phrase = allPhrases[3];
              phrase.BeginEdit();
              phrase.Text = SeedData.Ton.DogText;
              phrase.Language = _ServerSpanishLang;
              phrase.ApplyEdit();
              serverDogPhrase = phrase;
            }
            else
              serverDogPhrase = serverDogPhraseQuery.First();

            var validUserId = serverHelloPhrase.UserId;
            SeedData.Ton.GetTestValidUserDto().Id = validUserId;

            SeedData.Ton.HelloPhraseDto.Id = serverHelloPhrase.Id;
            SeedData.Ton.HolaPhraseDto.Id = serverHolaPhrase.Id;
            SeedData.Ton.LongPhraseDto.Id = serverLongPhrase.Id;
            SeedData.Ton.DogPhraseDto.Id = serverDogPhrase.Id;

            SeedData.Ton.HelloPhraseDto.UserId = serverHelloPhrase.UserId;
            SeedData.Ton.HolaPhraseDto.UserId = serverHolaPhrase.UserId;
            SeedData.Ton.LongPhraseDto.UserId = serverLongPhrase.UserId;
            SeedData.Ton.DogPhraseDto.UserId = serverDogPhrase.UserId;

            phrasesCorrected = true;
          });
      });

      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => phrasesCorrected);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNotNull(allLanguages); },
                      () => { Assert.AreNotEqual(Guid.Empty, SeedData.Ton.EnglishId); },
                      () => { Assert.AreNotEqual(Guid.Empty, SeedData.Ton.SpanishId); },
                      () => { Assert.IsTrue(allLanguages.Count > 0); });
      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void CREATE_NEW()
    {
      var isCreated = false;
      PhraseEdit newPhraseEdit = null;
      Exception newError = new Exception();
      
      PhraseEdit.NewPhraseEdit( (s,r) =>
        {
          newError = r.Error;
          if (newError != null)
            throw newError;

          newPhraseEdit = r.Object;
          isCreated = true;
        });
      EnqueueConditional(() => isCreated);
      EnqueueCallback(
                      () => { Assert.IsNotNull(newPhraseEdit); },
                      () => { Assert.IsNull(newError); },
                      () => { Assert.AreEqual(SeedData.Ton.EnglishId, newPhraseEdit.LanguageId); },
                      () => { Assert.IsNotNull(newPhraseEdit.Language); }
                      );
      EnqueueTestComplete();
    }

    //[TestMethod]
    //[Asynchronous]
    //public void CREATE_NEW_WITH_ID()
    //{
    //  Guid id = new Guid("BDEF87AC-21FA-4BAE-A155-91CDDA52C9CD");
    
    //  var isCreated = false;
    //  PhraseEdit PhraseEdit = null;
    //  PhraseEdit.NewPhraseEdit(id, (s,r) =>
    //    {
    //      if (r.Error != null)
    //        throw r.Error;

    //      PhraseEdit = r.Object;
    //      isCreated = true;
    //    });
    //  EnqueueConditional(() => isCreated);
    //  EnqueueCallback(() => { Assert.IsNotNull(PhraseEdit); },
    //                  () => { Assert.IsNull(null); },
    //                  () => { Assert.AreEqual(id, PhraseEdit.Id); });
    //  EnqueueTestComplete();
    //}

    [TestMethod]
    [Asynchronous]
    [Tag("pget")]
    public void GET()
    {
      Guid testId = Guid.Empty;
      var allLoaded = false;
      var isLoaded = false;
      Exception getAllError = new Exception();
      Exception error = new Exception();
      PhraseEdit PhraseEdit = null;

      PhraseList.GetAll((s1, r1) =>
        {
          getAllError = r1.Error;
          if (getAllError != null)
            throw r1.Error;
          testId = r1.Object.First().Id;
          allLoaded = true;
          PhraseEdit.GetPhraseEdit(testId, (s, r) =>
          {
            error = r.Error;
            PhraseEdit = r.Object;
            isLoaded = true;
          });
        });


      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => allLoaded);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNull(getAllError); },
                      () => { Assert.IsNotNull(PhraseEdit); },
                      () => { Assert.AreEqual(testId, PhraseEdit.Id); });
      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void NEW_EDIT_BEGINSAVE_GET()
    {
      //INITIALIZE ERRORS TO EXCEPTION, BECAUSE WE TEST IF THEY ARE NULL LATER
      Exception newError = new Exception();
      Exception savedError = new Exception();
      Exception gottenError = new Exception();

      PhraseEdit newPhraseEdit = null;
      PhraseEdit savedPhraseEdit = null;
      PhraseEdit gottenPhraseEdit = null;    
  
      var isNewed = false;
      var isEdited = false;
      var isSaved = false;
      var isGotten = false;
      
      //NEW
      PhraseEdit.NewPhraseEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        newPhraseEdit = r.Object;
        isNewed = true;

        //EDIT
        newPhraseEdit.Text = "TestPhrase NEW_EDIT_BEGINSAVE_GET";
        newPhraseEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
        newPhraseEdit.Username = SeedData.Ton.TestValidUsername;
        isEdited = true;

        //SAVE
        newPhraseEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;
          savedPhraseEdit = r2.NewObject as PhraseEdit;
          isSaved = true;
          //GET (CONFIRM SAVE)
          PhraseEdit.GetPhraseEdit(savedPhraseEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;
            gottenPhraseEdit = r3.Object;
            isGotten = true;
          });
        });



      });

      EnqueueConditional(() => isNewed);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isSaved);
      EnqueueConditional(() => isGotten);
      EnqueueCallback(
                      () => { Assert.IsNull(newError); },
                      () => { Assert.IsNull(savedError); },
                      () => { Assert.IsNull(gottenError); },
                      () => { Assert.IsNotNull(newPhraseEdit); },
                      () => { Assert.IsNotNull(savedPhraseEdit); },
                      () => { Assert.IsNotNull(gottenPhraseEdit); },
                      () => { Assert.AreEqual(savedPhraseEdit.Id, gottenPhraseEdit.Id); },
                      () => { Assert.AreEqual(savedPhraseEdit.Text, gottenPhraseEdit.Text); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    [ExpectedException(typeof(ExpectedException))]
    public void NEW_EDIT_BEGINSAVE_GET_DELETE_GET()
    {
      //INITIALIZE ERRORS TO EXCEPTION, BECAUSE EXPECT THEM TO BE NULL LATER
      Exception newError = new Exception();
      Exception savedError = new Exception();
      Exception gottenError = new Exception();
      Exception deletedError = new Exception();

      //INITIALIZE CONFIRM TO NULL, BECAUSE WE EXPECT THIS TO BE AN ERROR LATER
      Exception deleteConfirmedError = null;

      PhraseEdit newPhraseEdit = null;
      PhraseEdit savedPhraseEdit = null;
      PhraseEdit gottenPhraseEdit = null;
      PhraseEdit deletedPhraseEdit = null;

      //INITIALIZE TO EMPTY Phrase EDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
      PhraseEdit deleteConfirmedPhraseEdit = new PhraseEdit();

      var isNewed = false;
      var isSaved = false;
      var isGotten = false;
      var isDeleted = false;
      var isDeleteConfirmed = false;

      //NEW
      PhraseEdit.NewPhraseEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        newPhraseEdit = r.Object;
        isNewed = true;

        //EDIT
        newPhraseEdit.Text = "TestPhrase NEW_EDIT_BEGINSAVE_GET_DELETE_GET";
        newPhraseEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
        newPhraseEdit.Username = SeedData.Ton.TestValidUsername;

        //SAVE
        newPhraseEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;
          savedPhraseEdit = r2.NewObject as PhraseEdit;
          isSaved = true;
          //GET (CONFIRM SAVE)
          PhraseEdit.GetPhraseEdit(savedPhraseEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;
            gottenPhraseEdit = r3.Object;
            isGotten = true;

            //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
            gottenPhraseEdit.Delete();
            gottenPhraseEdit.BeginSave((s4, r4) =>
            {
              deletedError = r4.Error;
              if (deletedError != null)
                throw deletedError;

              deletedPhraseEdit = r4.NewObject as PhraseEdit;
              //TODO: Figure out why PhraseEditTests final callback gets thrown twice.  When server throws an exception (expected because we are trying to fetch a deleted phrase that shouldn't exist), the callback is executed twice.
              PhraseEdit.GetPhraseEdit(deletedPhraseEdit.Id, (s5, r5) =>
              {
                var debugExecutionLocation = Csla.ApplicationContext.ExecutionLocation;
                var debugLogicalExecutionLocation = Csla.ApplicationContext.LogicalExecutionLocation;
                deleteConfirmedError = r5.Error;
                if (deleteConfirmedError != null)
                {
                  isDeleteConfirmed = true;
                  throw new ExpectedException(deleteConfirmedError);
                }
                deleteConfirmedPhraseEdit = r5.Object;
              });
            });
          });
        });
      });

      EnqueueConditional(() => isNewed);
      EnqueueConditional(() => isSaved);
      EnqueueConditional(() => isGotten);
      EnqueueConditional(() => isDeleted);
      EnqueueConditional(() => isDeleteConfirmed);
      EnqueueCallback(
                      () => { Assert.IsNull(newError); },
                      () => { Assert.IsNull(savedError); },
                      () => { Assert.IsNull(gottenError); },
                      () => { Assert.IsNull(deletedError); },
                      //WE EXPECT AN ERROR, AS WE TRIED A GET ON AN ID THAT SHOULD HAVE BEEN DELETED
                      () => { Assert.IsNotNull(deleteConfirmedError); },

                      () => { Assert.IsNotNull(newPhraseEdit); },
                      () => { Assert.IsNotNull(savedPhraseEdit); },
                      () => { Assert.IsNotNull(gottenPhraseEdit); },
                      () => { Assert.IsNotNull(deletedPhraseEdit); },
                      () => { Assert.IsNull(deleteConfirmedPhraseEdit); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void MAKE_PHRASE_WITH_REALLY_LONG_VARIED_TEXT()
    {
      #region var reallyLongText
      var reallyLongText = @"asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`";
      #endregion
      
      //INITIALIZE ERRORS TO EXCEPTION, BECAUSE WE TEST IF THEY ARE NULL LATER
      Exception newError = new Exception();
      Exception savedError = new Exception();
      Exception gottenError = new Exception();

      PhraseEdit newPhraseEdit = null;
      PhraseEdit savedPhraseEdit = null;
      PhraseEdit gottenPhraseEdit = null;    
  
      var isNewed = false;
      var isEdited = false;
      var isSaved = false;
      var isGotten = false;
      
      //NEW
      PhraseEdit.NewPhraseEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        newPhraseEdit = r.Object;
        isNewed = true;

        //EDIT
        newPhraseEdit.Text = reallyLongText;
        newPhraseEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
        newPhraseEdit.Username = SeedData.Ton.TestValidUsername;
        Assert.AreEqual(SeedData.Ton.EnglishId, newPhraseEdit.LanguageId);
        isEdited = true;

        //SAVE
        newPhraseEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;
          savedPhraseEdit = r2.NewObject as PhraseEdit;
          isSaved = true;
          //GET (CONFIRM SAVE)
          PhraseEdit.GetPhraseEdit(savedPhraseEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;
            gottenPhraseEdit = r3.Object;
            isGotten = true;
          });
        });



      });

      EnqueueConditional(() => isNewed);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isSaved);
      EnqueueConditional(() => isGotten);
      EnqueueCallback(
                      () => { Assert.IsNull(newError); },
                      () => { Assert.IsNull(savedError); },
                      () => { Assert.IsNull(gottenError); },
                      () => { Assert.IsNotNull(newPhraseEdit); },
                      () => { Assert.IsNotNull(savedPhraseEdit); },
                      () => { Assert.IsNotNull(gottenPhraseEdit); },
                      () => { Assert.AreEqual(savedPhraseEdit.Id, gottenPhraseEdit.Id); },
                      () => { Assert.AreEqual(savedPhraseEdit.Text, gottenPhraseEdit.Text); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void MAKE_PHRASE_WITH_REALLY_REALLY_LONG_NUMERICAL_TEXT()
    {
      var maxLength = 300000;//worked
      maxLength = 100000;//worked
      maxLength = 30000; //just for a big number, to lessen time to do test.
      var i = 0;
      string reallyReallyLongText = "";
      while (reallyReallyLongText.Length < maxLength)
      {
        reallyReallyLongText += i.ToString();
        i++;
        if (i == maxLength) //shouldn't happen, but no big deal if it does. It's just a reallyraeallyreallyreally long text.
          break;
      }
      if (reallyReallyLongText.Length > maxLength)
        reallyReallyLongText = reallyReallyLongText.Substring(0, maxLength);

      //INITIALIZE ERRORS TO EXCEPTION, BECAUSE WE TEST IF THEY ARE NULL LATER
      Exception newError = new Exception();
      Exception savedError = new Exception();
      Exception gottenError = new Exception();

      PhraseEdit newPhraseEdit = null;
      PhraseEdit savedPhraseEdit = null;
      PhraseEdit gottenPhraseEdit = null;

      var isNewed = false;
      var isEdited = false;
      var isSaved = false;
      var isGotten = false;

      //NEW
      PhraseEdit.NewPhraseEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        newPhraseEdit = r.Object;
        isNewed = true;

        //EDIT
        newPhraseEdit.Text = reallyReallyLongText;
        newPhraseEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
        newPhraseEdit.Username = SeedData.Ton.TestValidUsername;

        isEdited = true;

        //SAVE
        newPhraseEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;
          savedPhraseEdit = r2.NewObject as PhraseEdit;
          isSaved = true;
          //GET (CONFIRM SAVE)
          PhraseEdit.GetPhraseEdit(savedPhraseEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;
            gottenPhraseEdit = r3.Object;
            isGotten = true;
          });
        });



      });

      EnqueueConditional(() => isNewed);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isSaved);
      EnqueueConditional(() => isGotten);
      EnqueueCallback(
                      () => { Assert.IsNull(newError); },
                      () => { Assert.IsNull(savedError); },
                      () => { Assert.IsNull(gottenError); },
                      () => { Assert.IsNotNull(newPhraseEdit); },
                      () => { Assert.IsNotNull(savedPhraseEdit); },
                      () => { Assert.IsNotNull(gottenPhraseEdit); },
                      () => { Assert.AreEqual(savedPhraseEdit.Id, gottenPhraseEdit.Id); },
                      () => { Assert.AreEqual(reallyReallyLongText.Length, gottenPhraseEdit.Text.Length); },
                      () => { Assert.AreEqual(savedPhraseEdit.Text, gottenPhraseEdit.Text); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void GET_ALL_PHRASES_THAT_CONTAIN_THE_LETTER_H_IN_ALL_LANGUAGES()
    {
      Guid testId = Guid.Empty;
      var allLoaded = false;
      var isLoaded = false;
      PhraseEdit PhraseEdit = null;
      
      //THE SEARCH TEXT WE ARE LOOKING FOR
      string textToLookFor = "h";
      
      //ASSUME TRUE
      bool allRetrievedPhrasesContainTheCorrectText = true;

      PhraseList.GetAllContainingText(textToLookFor, (s1, r1) =>
      {
        if (r1.Error != null)
          throw r1.Error;
        testId = r1.Object.First().Id;
        allLoaded = true;
        
        //CHECK TO SEE IF ALL THE PHRASES CONTAIN THE TEXT TO LOOK FOR, LIKE THEY ARE SUPPOSED TO
        for (int i = 0; i < r1.Object.Count; i++)
        {
          var phrase = r1.Object[i];

          if (!phrase.Text.Contains(textToLookFor))
            //THIS SHOULDN'T HAPPEN
            allRetrievedPhrasesContainTheCorrectText = false;
        }

        //JUST TO CHECK TO SEE THAT OUR RESULTS WERE AT LEAST ONE, AND THAT IT'S A VALID PHRASE
        PhraseEdit.GetPhraseEdit(testId, (s, r) =>
        {
          if (r.Error != null)
            throw r.Error;
          PhraseEdit = r.Object;
          isLoaded = true;
        });
      });

      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => allLoaded);
      EnqueueCallback(() => { Assert.IsTrue(allRetrievedPhrasesContainTheCorrectText); },
                      () => { Assert.IsNotNull(PhraseEdit); },
                      () => { Assert.AreEqual(testId, PhraseEdit.Id); });
      EnqueueTestComplete();
    }

  }
}