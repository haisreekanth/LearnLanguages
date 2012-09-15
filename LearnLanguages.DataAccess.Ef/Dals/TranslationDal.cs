using System;
using System.Collections.Generic;
using System.Linq;
using Csla.Data;

using System.Data.Objects;
using System.Data.Objects.DataClasses;
using LearnLanguages.Business.Security;

namespace LearnLanguages.DataAccess.Ef
{
  public class TranslationDal : TranslationDalBase
  {
    protected override TranslationDto NewImpl(object criteria)
    {
      var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
      string currentUsername = identity.Name;
      Guid currentUserId = identity.UserId;

      TranslationDto newTranslationDto = new TranslationDto()
      {
        Id = Guid.NewGuid(),
        PhraseIds = null,
        ContextPhraseId = Guid.Empty,
        UserId = currentUserId,
        Username = currentUsername,
      };

      return newTranslationDto;
    }
    protected override TranslationDto FetchImpl(Guid id)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var currentUserId = Business.BusinessHelper.GetCurrentUserId();
        var results = (from translationData in ctx.ObjectContext.TranslationDatas
                       where translationData.Id == id &&
                             translationData.UserDataId == currentUserId
                       select translationData);

        if (results.Count() == 1)
        {
          var fetchedTranslationData = results.First();

          var translationDto = EfHelper.ToDto(fetchedTranslationData);
          return translationDto;
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
    protected override ICollection<TranslationDto> FetchImpl(ICollection<Guid> ids)
    {
      var translationDtos = new List<TranslationDto>();
      foreach (var id in ids)
      {
        translationDtos.Add(FetchImpl(id));
      }
      return translationDtos;
    }
    protected override ICollection<TranslationDto> FetchByIdImpl(Guid phraseId)
    {
      var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var retTranslationDtos = new List<TranslationDto>();

        //FIND ALL TRANSLATIONS THAT CONTAIN PHRASEDATAS WITH THE CORRESPONDING PHRASEID
        var translationDatas = (from translationData in ctx.ObjectContext.TranslationDatas

                                where (from phraseData in translationData.PhraseDatas
                                       where phraseData.Id == phraseId
                                       select phraseData).Count() > 0

                                select translationData).ToList();

        //ADD DTOS OF THOSE TRANSLATION DATAS TO RETURN LIST
        foreach (var translationData in translationDatas)
        {
          var dto = EfHelper.ToDto(translationData);
          retTranslationDtos.Add(dto);
        }

        //RETURN THE TRANSLATIONS
        return retTranslationDtos;
      }
    }
    protected override TranslationDto UpdateImpl(TranslationDto dto)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = (from translationData in ctx.ObjectContext.TranslationDatas
                       where translationData.Id == dto.Id &&
                             translationData.UserDataId == dto.UserId
                       select translationData);

        if (results.Count() == 1)
        {
          //UPDATE THE TRANSLATIONDATA IN THE CONTEXT
          var translationData = results.First();
          EfHelper.LoadDataFromDto(ref translationData, dto, ctx.ObjectContext);

          //SAVE TO MAKE SURE AFFECTED USERS ARE UPDATED TO REMOVE THIS PHRASE
          ctx.ObjectContext.SaveChanges();

          var updatedDto = EfHelper.ToDto(translationData);
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
    protected override TranslationDto InsertImpl(TranslationDto dto)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var newTranslationData = EfHelper.AddToContext(dto, ctx.ObjectContext);
        ctx.ObjectContext.SaveChanges();
        dto.Id = newTranslationData.Id;
        return dto;
      }
    }
    protected override TranslationDto DeleteImpl(Guid id)
    {
      var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
      string currentUsername = identity.Name;
      Guid currentUserId = identity.UserId;

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from translationData in ctx.ObjectContext.TranslationDatas
                      where translationData.Id == id &&
                            translationData.UserDataId == currentUserId
                      select translationData;

        if (results.Count() == 1)
        {
          var translationDataToDelete = results.First();
          var retDto = EfHelper.ToDto(translationDataToDelete);
          ctx.ObjectContext.TranslationDatas.DeleteObject(translationDataToDelete);
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

    protected override ICollection<TranslationDto> GetAllImpl()
    {
      var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var allTranslationDtos = new List<TranslationDto>();

        var translationDatas = (from translationData in ctx.ObjectContext.TranslationDatas
                                where translationData.UserDataId == identity.UserId
                                select translationData).ToList();

        foreach (var translationData in translationDatas)
        {
          allTranslationDtos.Add(EfHelper.ToDto(translationData));
        }

        return allTranslationDtos;
      }
    }

    
  }
}
