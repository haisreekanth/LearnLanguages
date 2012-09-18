using System;
using System.Collections.Generic;
using System.Linq;
using Csla.Data;

using System.Data.Objects;
using System.Data.Objects.DataClasses;
using LearnLanguages.Business.Security;

namespace LearnLanguages.DataAccess.Ef
{
  public class LanguageDal : LanguageDalBase
  {
    //public Result<LanguageDto> New(object criteria)
    //{
    //  Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
    //  try
    //  {
    //    var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
    //    LanguageDto newLanguageDto = new LanguageDto()
    //    {
    //      Id = Guid.NewGuid(),
    //      Text = DalResources.DefaultNewLanguageText,
    //      UserId = identity.UserId,
    //      Username = identity.Name
    //    };
    //    retResult = Result<LanguageDto>.Success(newLanguageDto);
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LanguageDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<LanguageDto> Fetch(Guid id)
    //{
    //  Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from languageData in ctx.ObjectContext.LanguageDatas
    //                    where languageData.Id == id
    //                    select languageData;

    //      if (results.Count() == 1)
    //        retResult = Result<LanguageDto>.Success(EfHelper.ToDto(results.First()));
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<LanguageDto>.FailureWithInfo(null,
    //            new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFoundException));
    //        else
    //          retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.FetchFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<LanguageDto> Update(LanguageDto dto)
    //{
    //  Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      //CHECK TO SEE IF *NEW* LANGUAGE TEXT ALREADY EXISTS.  NO DUPLICATE LANGUAGE TEXTS.
    //      //IF THE TEXT IS THE SAME AS THE CURRENT DTO, THEN WHAT ARE WE UPDATING?  THIS IS WHY I'M NOT CHECKING 
    //      //IF THE CURRENT TEXT == OLD TEXT
    //      var newLanguageTextAlreadyExists = (from languageData in ctx.ObjectContext.LanguageDatas
    //                                          where languageData.Text == dto.Text
    //                                          select languageData).Count() > 0;

    //      if (newLanguageTextAlreadyExists)
    //        throw new Exceptions.LanguageTextAlreadyExistsException(dto.Text);

    //      //FIND CURRENT LANGUAGEDATA TO UPDATE
    //      var results = from languageData in ctx.ObjectContext.LanguageDatas
    //                    where languageData.Id == dto.Id
    //                    select languageData;

    //      if (results.Count() == 1)
    //      {
    //        var languageDataToUpdate = results.First();
    //        ctx.ObjectContext.LanguageDatas.DeleteObject(languageDataToUpdate);
    //        dto.Id = Guid.NewGuid();
    //        ctx.ObjectContext.LanguageDatas.AddObject(EfHelper.ToData(dto, ctx.ObjectContext));
    //        ctx.ObjectContext.SaveChanges();
    //        retResult = Result<LanguageDto>.Success(dto);
    //      }
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<LanguageDto>.FailureWithInfo(null,
    //            new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFoundException));
    //        else
    //          retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<LanguageDto> Insert(LanguageDto dto)
    //{
    //  Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      //CHECK TO SEE IF LANGUAGE TEXT ALREADY EXISTS.  NO DUPLICATE LANGUAGE TEXTS.
    //      var languageTextAlreadyExists = (from languageData in ctx.ObjectContext.LanguageDatas
    //                                       where languageData.Text == dto.Text
    //                                       select languageData).Count() > 0;

    //      if (languageTextAlreadyExists)
    //        throw new Exceptions.LanguageTextAlreadyExistsException(dto.Text);

    //      var resultsSameIdFound = from languageData in ctx.ObjectContext.LanguageDatas
    //                               where languageData.Id == dto.Id
    //                               select languageData;
            
    //      //SHOULD FIND ZERO LANGUAGEDTOS (NO DUPLICATE IDS, NO DUPLICATE DTOS)
    //      if (resultsSameIdFound.Count() == 0)
    //      {
    //        var data = EfHelper.ToData(dto);
    //        ctx.ObjectContext.LanguageDatas.AddObject(data);
    //        ctx.ObjectContext.SaveChanges();
    //        dto.Id = data.Id;
    //        retResult = Result<LanguageDto>.Success(dto);
    //      }
    //      else
    //      {
    //        if (resultsSameIdFound.Count() == 1) //ID ALREADY EXISTS
    //          retResult = Result<LanguageDto>.FailureWithInfo(dto,
    //            new Exceptions.InsertFailedException(DalResources.ErrorMsgIdNotFoundException));
    //        else                      //MULTIPLE IDS ALREADY EXIST?? SHOULD NOT BE POSSIBLE
    //          retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.InsertFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.InsertFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<LanguageDto> Delete(Guid id)
    //{
    //  Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from languageData in ctx.ObjectContext.LanguageDatas
    //                    where languageData.Id == id
    //                    select languageData;

    //      if (results.Count() == 1)
    //      {
    //        var languageDataToRemove = results.First();

    //        #region Cascade Delete to Phrases and Translations
    //        ////DELETE ALL PHRASES ASSOCIATED WITH LANGUAGE
    //        //THIS CASCADE IS TRICKY, BECAUSE WE NEED TO DELETE A TRANSLATION ONLY IF THAT TRANSLATION WILL CONTAIN
    //        //LESS THAN TWO PHRASES *AFTER* THIS DELETE.

    //        var phrasesToDelete = languageDataToRemove.PhraseDatas.ToList();
    //        foreach (var phraseDataToDelete in phrasesToDelete)
    //        {
    //          //WHEN WE DELETE A PHRASE, IF IT IS PART OF A TRANSLATION, AND THAT TRANSLATION
    //          //AFTER DELETE WOULD HAVE LESS THAN TWO PHRASES, WE NEED TO CASCADE THE DELETE TO THE TRANSLATION
    //          var translationsToDelete = new List<TranslationData>();
    //          foreach (var translation in phraseDataToDelete.TranslationDatas)
    //          {
    //            //IF WE ONLY HAVE TWO OR LESS PHRASES *NOW* (WE HAVE NOT DELETED PHRASE YET), THEN AFTER DELETE 
    //            //WE *WILL* HAVE ONLY ONE OR ZERO, SO MARK THIS TRANSLATION FOR DELETION.
    //            if (translation.PhraseDatas.Count <= 2)
    //              translationsToDelete.Add(translation);
    //          }

    //          //EXECUTE DELETE OF THOSE TRANSLATIONS
    //          foreach (var translation in translationsToDelete)
    //          {
    //            ctx.ObjectContext.TranslationDatas.DeleteObject(translation);
    //          }

    //          //DELETE THE PHRASE ITSELF
    //          ctx.ObjectContext.PhraseDatas.DeleteObject(phraseDataToDelete);
    //        }
    //        #endregion

    //        //DELETE LANGUAGE ITSELF
    //        ctx.ObjectContext.LanguageDatas.DeleteObject((languageDataToRemove));
            
    //        //SAVE CHANGES
    //        ctx.ObjectContext.SaveChanges();

    //        retResult = Result<LanguageDto>.Success(EfHelper.ToDto(languageDataToRemove));
    //        //todo: when deleting a language, it would be good to have successwithinfo about how many phrases/translations were deleted.
    //      }
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<LanguageDto>.FailureWithInfo(null,
    //            new Exceptions.DeleteFailedException(DalResources.ErrorMsgIdNotFoundException));
    //        else
    //          retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException(ex));
    //  }
    //  return retResult;
    //}

    //public Result<ICollection<LanguageDto>> GetAll()
    //{
    //  Result<ICollection<LanguageDto>> retResult = Result<ICollection<LanguageDto>>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var allDtos = (

    //                     from languageData in ctx.ObjectContext.LanguageDatas
    //                     select new LanguageDto()
    //                     {
    //                       Id = languageData.Id,
    //                       Text = languageData.Text
    //                     }

    //                    ).ToList();
    //      //var allDtos = new List<LanguageDto>(ctx.ObjectContext.LanguageDatas);
    //      retResult = Result<ICollection<LanguageDto>>.Success(allDtos);
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<ICollection<LanguageDto>>.FailureWithInfo(null, new Exceptions.GetAllFailedException(ex));
    //  }
    //  return retResult;
    //}

    protected override LanguageDto NewImpl(object criteria)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();
      var currentUsername = Business.BusinessHelper.GetCurrentUsername();
      
      LanguageDto newLanguageDto = new LanguageDto()
      {
        Id = Guid.NewGuid(),
        Text = DalResources.DefaultNewLanguageText,
        UserId = currentUserId,
        Username = currentUsername
      };
     
      return newLanguageDto;
    }
    protected override LanguageDto FetchImpl(Guid id)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from languageData in ctx.ObjectContext.LanguageDatas
                      where languageData.Id == id &&
                            languageData.UserDataId == currentUserId
                      select languageData;

        if (results.Count() == 1)
          return EfHelper.ToDto(results.First());
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(id);
          else
            throw new Exceptions.VeryBadException(
              string.Format(DalResources.ErrorMsgVeryBadException,
                            DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero));
        }
      }
    }
    protected override LanguageDto FetchImpl(string languageText)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from languageData in ctx.ObjectContext.LanguageDatas
                      where languageData.Text == languageText &&
                            languageData.UserDataId == currentUserId
                      select languageData;

        if (results.Count() == 1)
          return EfHelper.ToDto(results.First());
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.LanguageTextNotFoundException(languageText);
          else
            throw new Exceptions.VeryBadException(
              string.Format(DalResources.ErrorMsgVeryBadException,
                            DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero));
        }
      }
    }
    protected override ICollection<LanguageDto> FetchImpl(ICollection<Guid> ids)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      var retLanguages = new List<LanguageDto>();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        foreach (var id in ids)
        {
          //old lazy way, lazy as in I'm lazy for doing it this way
          //var dto = FetchImpl(id);
          //retLanguages.Add(dto);

          var data = EfHelper.GetLanguageData(id, ctx.ObjectContext);
          var dto = EfHelper.ToDto(data);
          retLanguages.Add(dto);
        }
      }

      return retLanguages;
    }
    protected override LanguageDto UpdateImpl(LanguageDto dto)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

        using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
        {
          //CHECK TO SEE IF *NEW* LANGUAGE TEXT ALREADY EXISTS IN SOME OTHER LANGUAGE EDIT.  IF WE 
          //ARE REPLACING THE CURRENT TEXT WITH THE SAME TEXT, THAT IS FINE AS IT WILL JUST BE OVERWRITTEN.
          var newLanguageTextAlreadyExists = (from languageData in ctx.ObjectContext.LanguageDatas
                                              where languageData.Text == dto.Text &&
                                                    languageData.Id != dto.Id &&
                                                    languageData.UserDataId == currentUserId
                                              select languageData).Count() > 0;

          if (newLanguageTextAlreadyExists)
            throw new Exceptions.LanguageTextAlreadyExistsException(dto.Text);

          //FIND CURRENT LANGUAGEDATA TO UPDATE
          var results = from languageData in ctx.ObjectContext.LanguageDatas
                        where languageData.Id == dto.Id &&
                              languageData.UserDataId == currentUserId
                        select languageData;

          if (results.Count() == 1)
          {
            var languageDataToUpdate = results.First();
            EfHelper.LoadDataFromDto(ref languageDataToUpdate, dto, ctx.ObjectContext);
            ctx.ObjectContext.SaveChanges();
            dto.Id = languageDataToUpdate.Id;
            return dto;
          }
          else
          {
            if (results.Count() == 0)
              throw new Exceptions.IdNotFoundException(dto.Id);
            else
              throw new Exceptions.VeryBadException(
                string.Format(DalResources.ErrorMsgVeryBadException,
                              DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero));
          }
        }
      }
    protected override LanguageDto InsertImpl(LanguageDto dto)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        //CHECK TO SEE IF LANGUAGE TEXT ALREADY EXISTS.  NO DUPLICATE LANGUAGE TEXTS.
        var languageTextAlreadyExists = (from languageData in ctx.ObjectContext.LanguageDatas
                                         where languageData.Text == dto.Text &&
                                               languageData.UserDataId == currentUserId
                                         select languageData).Count() > 0;

        if (languageTextAlreadyExists)
          throw new Exceptions.LanguageTextAlreadyExistsException(dto.Text);

        var results = from languageData in ctx.ObjectContext.LanguageDatas
                      where languageData.Id == dto.Id && 
                            languageData.UserDataId == currentUserId
                      select languageData;

        //SHOULD FIND ZERO LANGUAGEDTOS (NO DUPLICATE IDS, NO DUPLICATE DTOS)
        if (results.Count() == 0)
        {
          var data = EfHelper.ToData(dto, ctx.ObjectContext);
          ctx.ObjectContext.LanguageDatas.AddObject(data);
          ctx.ObjectContext.SaveChanges();
          dto.Id = data.Id;

          return dto;
        }
        else
        {
          if (results.Count() == 1) 
            //ID ALREADY EXISTS
            throw new Exceptions.IdAlreadyExistsException(dto.Id);
          else                      
            //MULTIPLE IDS ALREADY EXIST?? SHOULD NOT BE POSSIBLE
            throw new Exceptions.VeryBadException(
              string.Format(DalResources.ErrorMsgVeryBadException,
                            DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero));
        }
      }
    }
    protected override LanguageDto DeleteImpl(Guid id)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from languageData in ctx.ObjectContext.LanguageDatas
                      where languageData.Id == id &&
                            languageData.UserDataId == currentUserId
                      select languageData;

        if (results.Count() == 1)
        {
          var languageDataToRemove = results.First();
          //Todo: FIX DELETE LANGUAGE CASCADE TO INCLUDE DELETE MLT, PHRASEBELIEFS, LINES
          #region Cascade Delete to Phrases, and Translations
          ////DELETE ALL PHRASES ASSOCIATED WITH LANGUAGE
          //THIS CASCADE IS TRICKY, BECAUSE WE NEED TO DELETE A TRANSLATION ONLY IF THAT TRANSLATION WILL CONTAIN
          //LESS THAN TWO PHRASES *AFTER* THIS DELETE.

          var phrasesToDelete = languageDataToRemove.PhraseDatas.ToList();
          foreach (var phraseDataToDelete in phrasesToDelete)
          {
            //WHEN WE DELETE A PHRASE, IF IT IS PART OF A TRANSLATION, AND THAT TRANSLATION
            //AFTER DELETE WOULD HAVE LESS THAN TWO PHRASES, WE NEED TO CASCADE THE DELETE TO THE TRANSLATION
            var translationsToDelete = new List<TranslationData>();
            foreach (var translation in phraseDataToDelete.TranslationDatas)
            {
              //IF WE ONLY HAVE TWO OR LESS PHRASES *NOW* (WE HAVE NOT DELETED PHRASE YET), THEN AFTER DELETE 
              //WE *WILL* HAVE ONLY ONE OR ZERO, SO MARK THIS TRANSLATION FOR DELETION.
              if (translation.PhraseDatas.Count <= 2)
                translationsToDelete.Add(translation);
            }

            //EXECUTE DELETE OF THOSE TRANSLATIONS
            foreach (var translation in translationsToDelete)
            {
              ctx.ObjectContext.TranslationDatas.DeleteObject(translation);
            }

            //DELETE THE PHRASE ITSELF
            ctx.ObjectContext.PhraseDatas.DeleteObject(phraseDataToDelete);
          }
          #endregion

          //MUST CREATE THIS RETURN DTO BEFORE DELETE, BECAUSE IT REQUIRES LANGUAGEDATA TO BE IN DB
          var dto = EfHelper.ToDto(languageDataToRemove);

          //DELETE LANGUAGE ITSELF
          ctx.ObjectContext.LanguageDatas.DeleteObject((languageDataToRemove));
          
          //SAVE CHANGES
          ctx.ObjectContext.SaveChanges();

          return dto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(id);
          else
            throw new Exceptions.VeryBadException(
              string.Format(DalResources.ErrorMsgVeryBadException,
                            DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero));
        }
      }
    }

    protected override ICollection<LanguageDto> GetAllImpl()
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      var allDtos = new List<LanguageDto>();
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var allLanguageDatas = from languageData in ctx.ObjectContext.LanguageDatas
                               where languageData.UserDataId == currentUserId 
                               select languageData;

        foreach (var languageData in allLanguageDatas)
        {
          var dto = EfHelper.ToDto(languageData);
          allDtos.Add(dto);
        }
      }

      return allDtos;
    }
  }
}
