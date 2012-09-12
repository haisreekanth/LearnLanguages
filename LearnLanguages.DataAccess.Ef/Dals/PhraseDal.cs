using System;
using System.Collections.Generic;
using System.Linq;
using Csla.Data;

using System.Data.Objects;
using System.Data.Objects.DataClasses;
using LearnLanguages.Business.Security;

namespace LearnLanguages.DataAccess.Ef
{
  public class PhraseDal : PhraseDalBase
  {
    //public Result<PhraseDto> New(object criteria)
    //{
    //  //throw new NotImplementedException("Ef.PhraseDal.New(object)");
    //  Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
    //  try
    //  {
        
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.CreateFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<PhraseDto> Fetch(Guid id)
    //{
    //  Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from phraseData in ctx.ObjectContext.PhraseDatas
    //                    where phraseData.Id == id
    //                    select phraseData;

    //      if (results.Count() == 1)
    //        retResult = Result<PhraseDto>.Success(EfHelper.ToDto(results.First()));
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<PhraseDto>.FailureWithInfo(null,
    //            new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFound));
    //        else
    //          retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.FetchFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<PhraseDto> Update(PhraseDto dto)
    //{
    //  Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from phraseData in ctx.ObjectContext.PhraseDatas
    //                    where phraseData.Id == dto.Id
    //                    select phraseData;

    //      if (results.Count() == 1)
    //      {
    //        var phraseDataToUpdate = results.First();
    //        ctx.ObjectContext.PhraseDatas.DeleteObject(phraseDataToUpdate);
    //        var newPhraseData = EfHelper.ToData(dto);
    //        ctx.ObjectContext.PhraseDatas.AddObject(newPhraseData);
    //        ctx.ObjectContext.SaveChanges();
    //        dto.Id = newPhraseData.Id;
    //        retResult = Result<PhraseDto>.Success(dto);
    //      }
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<PhraseDto>.FailureWithInfo(null,
    //            new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFound));
    //        else
    //          retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<PhraseDto> Insert(PhraseDto dto)
    //{
    //  Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from phraseData in ctx.ObjectContext.PhraseDatas
    //                    where phraseData.Id == dto.Id
    //                    select phraseData;

    //      //SHOULD FIND ZERO LANGUAGEDTOS (NO DUPLICATE IDS, NO DUPLICATE DTOS)
    //      if (results.Count() == 0)
    //      {
    //        var data = EfHelper.ToData(dto);
    //        ctx.ObjectContext.PhraseDatas.AddObject(data);
    //        ctx.ObjectContext.SaveChanges();
    //        dto.Id = data.Id;
    //        retResult = Result<PhraseDto>.Success(dto);
    //      }
    //      else
    //      {
    //        if (results.Count() == 1) //ID ALREADY EXISTS
    //          retResult = Result<PhraseDto>.FailureWithInfo(dto,
    //            new Exceptions.InsertFailedException(DalResources.ErrorMsgIdNotFound));
    //        else                      //MULTIPLE IDS ALREADY EXIST?? SHOULD NOT BE POSSIBLE
    //          retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.InsertFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.InsertFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<PhraseDto> Delete(Guid id)
    //{
    //  Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from phraseData in ctx.ObjectContext.PhraseDatas
    //                    where phraseData.Id == id
    //                    select phraseData;

    //      if (results.Count() == 1)
    //      {
    //        var phraseDataToRemove = results.First();
    //        ctx.ObjectContext.PhraseDatas.DeleteObject((phraseDataToRemove));
    //        ctx.ObjectContext.SaveChanges();
    //        retResult = Result<PhraseDto>.Success(EfHelper.ToDto(phraseDataToRemove));
    //      }
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<PhraseDto>.FailureWithInfo(null,
    //            new Exceptions.DeleteFailedException(DalResources.ErrorMsgIdNotFound));
    //        else
    //          retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<ICollection<PhraseDto>> GetAll()
    //{
    //  var retAllPhraseDtos = new List<PhraseDto>();
    //  using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //  {
    //    var allPhraseDatas = from phraseData in ctx.ObjectContext.PhraseDatas
    //                         select phraseData;
    //    foreach (var phraseData in allPhraseDatas)
    //    {
    //      var phraseDto = EfHelper.ToDto(phraseData);
    //      retAllPhraseDtos.Add(phraseDto);
    //    }
    //    //var allDtos = new List<PhraseDto>(ctx.ObjectContext.PhraseDatas);
    //    return retAllPhraseDtos;
    //  }
    //}
    protected override PhraseDto NewImpl(object criteria)
    {
      var identity = (CustomIdentity)Csla.ApplicationContext.User.Identity;
      string currentUsername = identity.Name;
      Guid currentUserId = Guid.Empty;
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        currentUserId = (from user in ctx.ObjectContext.UserDatas
                         where user.Username == currentUsername
                         select user.Id).First();
      }
      //if ((criteria != null) && !(criteria is UserDto))
      //  throw new Exceptions.BadCriteriaException(DalResources.ErrorMsgBadCriteriaExceptionDetail_ExpectedTypeIsUserDto);

      PhraseDto newPhraseDto = new PhraseDto()
      {
        Id = Guid.NewGuid(),
        Text = DalResources.DefaultNewPhraseText,
        LanguageId = GetDefaultLanguageId(),
        UserId = currentUserId,
        Username = currentUsername
      };

      return newPhraseDto;
    }
    protected override PhraseDto FetchImpl(Guid id)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from phraseData in ctx.ObjectContext.PhraseDatas
                      where phraseData.Id == id &&
                            phraseData.UserDataId == currentUserId
                      select phraseData;

        if (results.Count() == 1)
        {
          var fetchedPhraseData = results.First();

          var phraseDto = EfHelper.ToDto(fetchedPhraseData);
          return phraseDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple phrases with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override ICollection<PhraseDto> FetchImpl(string text)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();
      var retPhraseDtos = new List<PhraseDto>();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        //FIND ALL PHRASE DATAS THAT CONTAIN THE TEXT (ANY LANGUAGE)
        var results = from phraseData in ctx.ObjectContext.PhraseDatas
                      where phraseData.Text.Contains(text) &&
                            phraseData.UserDataId == currentUserId
                      select phraseData;

        //CONVERT THE DATA TO A DTO, AND ADD TO RETURN LIST
        foreach (var phraseData in results)
        {
          var phraseDto = EfHelper.ToDto(phraseData);
          retPhraseDtos.Add(phraseDto);
        }
      }

      return retPhraseDtos;
    }
    protected override ICollection<PhraseDto> FetchImpl(ICollection<Guid> ids)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();
      var retPhraseDtos = new List<PhraseDto>();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        //FIND ALL PHRASE DATAS THAT CONTAIN THE TEXT (ANY LANGUAGE)
        var results = from phraseData in ctx.ObjectContext.PhraseDatas
                      where ids.Contains(phraseData.Id) &&
                            phraseData.UserDataId == currentUserId
                      select phraseData;

        //CONVERT THE DATA TO A DTO, AND ADD TO RETURN LIST
        foreach (var phraseData in results)
        {
          var phraseDto = EfHelper.ToDto(phraseData);
          retPhraseDtos.Add(phraseDto);
        }
      }

      return retPhraseDtos;
    }
    //{
    //  var phraseDtos = new List<PhraseDto>();
    //  foreach (var id in ids)
    //  {
    //    phraseDtos.Add(FetchImpl(id));
    //  }
    //  return phraseDtos;
    //}
    protected override PhraseDto UpdateImpl(PhraseDto dto)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from phraseData in ctx.ObjectContext.PhraseDatas
                      where phraseData.Id == dto.Id &&
                            phraseData.UserDataId == currentUserId
                      select phraseData;

        if (results.Count() == 1)
        {
          var phraseData = results.First();
          EfHelper.LoadDataFromDto(ref phraseData, dto, ctx.ObjectContext);
          
          ctx.ObjectContext.SaveChanges(); 

          var updatedDto = EfHelper.ToDto(phraseData);
          return updatedDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(dto.Id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple phrases with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override PhraseDto InsertImpl(PhraseDto dto)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();
      PhraseData phraseData = null;
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        //BEFORE INSERTING, WE NEED TO MAKE SURE THAT THE PHRASE.TEXT 
        //AND PHRASE.LANGUAGE.TEXT ARE NOT ALREADY IN THE DB.  IF SO, WE NEED 
        //TO CALL AN UPDATE INSTEAD OF INSERT

        var results = (from data in ctx.ObjectContext.PhraseDatas
                       where data.Text == dto.Text &&
                             data.LanguageData.Id == dto.LanguageId &&
                             data.UserDataId == currentUserId
                       select data).FirstOrDefault();
        if (results != null)
        {
          //ALREADY EXISTS IN DB.  NEED TO PERFORM AN UPDATE
          dto.Id = results.Id;

          //hack: would be faster to effectively duplicate UpdateImpl code here instead of calling UpdateImpl(). (refactor and reshape code).  Would be cleaner as well, I think.
          return UpdateImpl(dto);
        }
        else
        {
          //NO PHRASE EXISTS IN THE DB, SO PERFORM THE INSERT, SAVE CHANGES AND RETURN
          phraseData = EfHelper.AddToContext(dto, ctx.ObjectContext);
          ctx.ObjectContext.SaveChanges();
          dto.Id = phraseData.Id;
          return dto;
        }
      }
    }
    protected override PhraseDto DeleteImpl(Guid id)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from phraseData in ctx.ObjectContext.PhraseDatas
                      where phraseData.Id == id &&
                            phraseData.UserDataId == currentUserId
                      select phraseData;

        if (results.Count() == 1)
        {
          var phraseDataToDelete = results.First();
          var retDto = EfHelper.ToDto(phraseDataToDelete);
          
          //WHEN WE DELETE PHRASES, IF IT IS PART OF A TRANSLATION, AND THAT TRANSLATION
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

          //SAVE CHANGES
          ctx.ObjectContext.SaveChanges();

          return retDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple phrases with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override ICollection<PhraseDto> GetAllImpl()
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var allPhraseDtos = new List<PhraseDto>();
        CustomIdentity identity = (CustomIdentity)Csla.ApplicationContext.User.Identity;

        var phraseDatas = (from phraseData in ctx.ObjectContext.PhraseDatas
                           where phraseData.UserDataId == identity.UserId
                           select phraseData).ToList();

        foreach (var usersPhraseData in phraseDatas)
        {
          allPhraseDtos.Add(EfHelper.ToDto(usersPhraseData));
        }

        return allPhraseDtos;
      }
    }

    private Guid GetDefaultLanguageId()
    {
      Guid retDefaultLanguageId;

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        try
        {

          retDefaultLanguageId = (from defaultLanguage in ctx.ObjectContext.LanguageDatas
                                  where defaultLanguage.Text == EfResources.DefaultLanguageText
                                  select defaultLanguage).First().Id;
        }
        catch (Exception ex)
        {
          throw new Exceptions.GeneralDataAccessException(ex);
        }
      }

      return retDefaultLanguageId;
    }


    
  }
}
