using System;
using System.Collections.Generic;
using System.Linq;
using Csla.Data;

using System.Data.Objects;
using System.Data.Objects.DataClasses;
using LearnLanguages.Business.Security;

namespace LearnLanguages.DataAccess.Ef
{
  public class PhraseBeliefDal : PhraseBeliefDalBase
  {
    //public Result<PhraseBeliefDto> New(object criteria)
    //{
    //  //throw new NotImplementedException("Ef.PhraseBeliefDal.New(object)");
    //  Result<PhraseBeliefDto> retResult = Result<PhraseBeliefDto>.Undefined(null);
    //  try
    //  {
        
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, new Exceptions.CreateFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<PhraseBeliefDto> Fetch(Guid id)
    //{
    //  Result<PhraseBeliefDto> retResult = Result<PhraseBeliefDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from phraseBeliefData in ctx.ObjectContext.PhraseBeliefDatas
    //                    where phraseBeliefData.Id == id
    //                    select phraseBeliefData;

    //      if (results.Count() == 1)
    //        retResult = Result<PhraseBeliefDto>.Success(EfHelper.ToDto(results.First()));
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<PhraseBeliefDto>.FailureWithInfo(null,
    //            new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFound));
    //        else
    //          retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, new Exceptions.FetchFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<PhraseBeliefDto> Update(PhraseBeliefDto dto)
    //{
    //  Result<PhraseBeliefDto> retResult = Result<PhraseBeliefDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from phraseBeliefData in ctx.ObjectContext.PhraseBeliefDatas
    //                    where phraseBeliefData.Id == dto.Id
    //                    select phraseBeliefData;

    //      if (results.Count() == 1)
    //      {
    //        var phraseBeliefDataToUpdate = results.First();
    //        ctx.ObjectContext.PhraseBeliefDatas.DeleteObject(phraseBeliefDataToUpdate);
    //        var newPhraseBeliefData = EfHelper.ToData(dto);
    //        ctx.ObjectContext.PhraseBeliefDatas.AddObject(newPhraseBeliefData);
    //        ctx.ObjectContext.SaveChanges();
    //        dto.Id = newPhraseBeliefData.Id;
    //        retResult = Result<PhraseBeliefDto>.Success(dto);
    //      }
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<PhraseBeliefDto>.FailureWithInfo(null,
    //            new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFound));
    //        else
    //          retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<PhraseBeliefDto> Insert(PhraseBeliefDto dto)
    //{
    //  Result<PhraseBeliefDto> retResult = Result<PhraseBeliefDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from phraseBeliefData in ctx.ObjectContext.PhraseBeliefDatas
    //                    where phraseBeliefData.Id == dto.Id
    //                    select phraseBeliefData;

    //      //SHOULD FIND ZERO LANGUAGEDTOS (NO DUPLICATE IDS, NO DUPLICATE DTOS)
    //      if (results.Count() == 0)
    //      {
    //        var data = EfHelper.ToData(dto);
    //        ctx.ObjectContext.PhraseBeliefDatas.AddObject(data);
    //        ctx.ObjectContext.SaveChanges();
    //        dto.Id = data.Id;
    //        retResult = Result<PhraseBeliefDto>.Success(dto);
    //      }
    //      else
    //      {
    //        if (results.Count() == 1) //ID ALREADY EXISTS
    //          retResult = Result<PhraseBeliefDto>.FailureWithInfo(dto,
    //            new Exceptions.InsertFailedException(DalResources.ErrorMsgIdNotFound));
    //        else                      //MULTIPLE IDS ALREADY EXIST?? SHOULD NOT BE POSSIBLE
    //          retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, new Exceptions.InsertFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, new Exceptions.InsertFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<PhraseBeliefDto> Delete(Guid id)
    //{
    //  Result<PhraseBeliefDto> retResult = Result<PhraseBeliefDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from phraseBeliefData in ctx.ObjectContext.PhraseBeliefDatas
    //                    where phraseBeliefData.Id == id
    //                    select phraseBeliefData;

    //      if (results.Count() == 1)
    //      {
    //        var phraseBeliefDataToRemove = results.First();
    //        ctx.ObjectContext.PhraseBeliefDatas.DeleteObject((phraseBeliefDataToRemove));
    //        ctx.ObjectContext.SaveChanges();
    //        retResult = Result<PhraseBeliefDto>.Success(EfHelper.ToDto(phraseBeliefDataToRemove));
    //      }
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<PhraseBeliefDto>.FailureWithInfo(null,
    //            new Exceptions.DeleteFailedException(DalResources.ErrorMsgIdNotFound));
    //        else
    //          retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseBeliefDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<ICollection<PhraseBeliefDto>> GetAll()
    //{
    //  var retAllPhraseBeliefDtos = new List<PhraseBeliefDto>();
    //  using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //  {
    //    var allPhraseBeliefDatas = from phraseBeliefData in ctx.ObjectContext.PhraseBeliefDatas
    //                         select phraseBeliefData;
    //    foreach (var phraseBeliefData in allPhraseBeliefDatas)
    //    {
    //      var phraseBeliefDto = EfHelper.ToDto(phraseBeliefData);
    //      retAllPhraseBeliefDtos.Add(phraseBeliefDto);
    //    }
    //    //var allDtos = new List<PhraseBeliefDto>(ctx.ObjectContext.PhraseBeliefDatas);
    //    return retAllPhraseBeliefDtos;
    //  }
    //}
    protected override PhraseBeliefDto NewImpl(object criteria)
    {
      var identity = (UserIdentity)Csla.ApplicationContext.User.Identity;
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

      PhraseBeliefDto newPhraseBeliefDto = new PhraseBeliefDto()
      {
        Id = Guid.NewGuid(),
        Text = DalResources.DefaultNewPhraseBeliefText,
        BelieverId = Guid.Empty,
        ReviewMethodId = Guid.Empty,
        Strength = double.Parse(DalResources.DefaultNewPhraseBeliefStrength),
        PhraseId = Guid.Empty,
        TimeStamp = DateTime.Now,
        UserId = currentUserId,
        Username = currentUsername
      };

      return newPhraseBeliefDto;
    }
    protected override PhraseBeliefDto FetchImpl(Guid id)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from phraseBeliefData in ctx.ObjectContext.PhraseBeliefDatas
                      where phraseBeliefData.Id == id &&
                            phraseBeliefData.UserDataId == currentUserId
                      select phraseBeliefData;

        if (results.Count() == 1)
        {
          var fetchedPhraseBeliefData = results.First();

          PhraseBeliefDto phraseBeliefDto = EfHelper.ToDto(fetchedPhraseBeliefData);
          return phraseBeliefDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple phraseBeliefs with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override ICollection<PhraseBeliefDto> FetchAllRelatedToPhraseImpl(Guid phraseId)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var allPhraseBeliefDtos = new List<PhraseBeliefDto>();
        UserIdentity identity = (UserIdentity)Csla.ApplicationContext.User.Identity;

        var phraseBeliefDatas = (from phraseBeliefData in ctx.ObjectContext.PhraseBeliefDatas
                                 where phraseBeliefData.UserDataId == identity.UserId &&
                                       phraseBeliefData.PhraseDataId == phraseId
                                 select phraseBeliefData);//.ToList();

        foreach (var usersPhraseBeliefData in phraseBeliefDatas)
        {
          allPhraseBeliefDtos.Add(EfHelper.ToDto(usersPhraseBeliefData));
        }

        return allPhraseBeliefDtos;
      }
    }
    //protected override ICollection<PhraseBeliefDto> FetchImpl(ICollection<Guid> ids)
    //{
    //  var phraseBeliefDtos = new List<PhraseBeliefDto>();
    //  foreach (var id in ids)
    //  {
    //    phraseBeliefDtos.Add(FetchImpl(id));
    //  }
    //  return phraseBeliefDtos;
    //}
    protected override PhraseBeliefDto UpdateImpl(PhraseBeliefDto dto)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from phraseBeliefData in ctx.ObjectContext.PhraseBeliefDatas
                      where phraseBeliefData.Id == dto.Id &&
                            phraseBeliefData.UserDataId == currentUserId
                      select phraseBeliefData;

        if (results.Count() == 1)
        {
          var phraseBeliefData = results.First();
          EfHelper.LoadDataFromDto(ref phraseBeliefData, dto, ctx.ObjectContext);
          
          ctx.ObjectContext.SaveChanges(); 

          var updatedDto = EfHelper.ToDto(phraseBeliefData);
          return updatedDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(dto.Id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple phraseBeliefs with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override PhraseBeliefDto InsertImpl(PhraseBeliefDto dto)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        PhraseBeliefData newPhraseBeliefData = EfHelper.AddToContext(dto, ctx.ObjectContext);
        ctx.ObjectContext.SaveChanges();
        dto.Id = newPhraseBeliefData.Id;
        return dto;
      }
    }
    protected override PhraseBeliefDto DeleteImpl(Guid id)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from phraseBeliefData in ctx.ObjectContext.PhraseBeliefDatas
                      where phraseBeliefData.Id == id &&
                            phraseBeliefData.UserDataId == currentUserId
                      select phraseBeliefData;

        if (results.Count() == 1)
        {
          //IDENTIFY
          var phraseBeliefDataToDelete = results.First();
          var retDto = EfHelper.ToDto(phraseBeliefDataToDelete);
          
          //DELETE
          ctx.ObjectContext.PhraseBeliefDatas.DeleteObject(phraseBeliefDataToDelete);

          //SAVE
          ctx.ObjectContext.SaveChanges();

          //RETURN
          return retDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple phraseBeliefs with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override ICollection<PhraseBeliefDto> GetAllImpl()
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var allPhraseBeliefDtos = new List<PhraseBeliefDto>();
        UserIdentity identity = (UserIdentity)Csla.ApplicationContext.User.Identity;

        var phraseBeliefDatas = (from phraseBeliefData in ctx.ObjectContext.PhraseBeliefDatas
                           where phraseBeliefData.UserDataId == identity.UserId
                           select phraseBeliefData);//.ToList();

        foreach (var usersPhraseBeliefData in phraseBeliefDatas)
        {
          allPhraseBeliefDtos.Add(EfHelper.ToDto(usersPhraseBeliefData));
        }

        return allPhraseBeliefDtos;
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
