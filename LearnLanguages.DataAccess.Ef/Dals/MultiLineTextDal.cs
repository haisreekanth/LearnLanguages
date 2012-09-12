using System;
using System.Collections.Generic;
using System.Linq;
using Csla.Data;

using System.Data.Objects;
using System.Data.Objects.DataClasses;
using LearnLanguages.Business.Security;

namespace LearnLanguages.DataAccess.Ef
{
  public class MultiLineTextDal : MultiLineTextDalBase
  {
    //public Result<MultiLineTextDto> New(object criteria)
    //{
    //  //throw new NotImplementedException("Ef.MultiLineTextDal.New(object)");
    //  Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
    //  try
    //  {
        
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.CreateFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<MultiLineTextDto> Fetch(Guid id)
    //{
    //  Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from multiLineTextData in ctx.ObjectContext.MultiLineTextDatas
    //                    where multiLineTextData.Id == id
    //                    select multiLineTextData;

    //      if (results.Count() == 1)
    //        retResult = Result<MultiLineTextDto>.Success(EfHelper.ToDto(results.First()));
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<MultiLineTextDto>.FailureWithInfo(null,
    //            new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFound));
    //        else
    //          retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.FetchFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<MultiLineTextDto> Update(MultiLineTextDto dto)
    //{
    //  Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from multiLineTextData in ctx.ObjectContext.MultiLineTextDatas
    //                    where multiLineTextData.Id == dto.Id
    //                    select multiLineTextData;

    //      if (results.Count() == 1)
    //      {
    //        var multiLineTextDataToUpdate = results.First();
    //        ctx.ObjectContext.MultiLineTextDatas.DeleteObject(multiLineTextDataToUpdate);
    //        var newMultiLineTextData = EfHelper.ToData(dto);
    //        ctx.ObjectContext.MultiLineTextDatas.AddObject(newMultiLineTextData);
    //        ctx.ObjectContext.SaveChanges();
    //        dto.Id = newMultiLineTextData.Id;
    //        retResult = Result<MultiLineTextDto>.Success(dto);
    //      }
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<MultiLineTextDto>.FailureWithInfo(null,
    //            new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFound));
    //        else
    //          retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<MultiLineTextDto> Insert(MultiLineTextDto dto)
    //{
    //  Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from multiLineTextData in ctx.ObjectContext.MultiLineTextDatas
    //                    where multiLineTextData.Id == dto.Id
    //                    select multiLineTextData;

    //      //SHOULD FIND ZERO LANGUAGEDTOS (NO DUPLICATE IDS, NO DUPLICATE DTOS)
    //      if (results.Count() == 0)
    //      {
    //        var data = EfHelper.ToData(dto);
    //        ctx.ObjectContext.MultiLineTextDatas.AddObject(data);
    //        ctx.ObjectContext.SaveChanges();
    //        dto.Id = data.Id;
    //        retResult = Result<MultiLineTextDto>.Success(dto);
    //      }
    //      else
    //      {
    //        if (results.Count() == 1) //ID ALREADY EXISTS
    //          retResult = Result<MultiLineTextDto>.FailureWithInfo(dto,
    //            new Exceptions.InsertFailedException(DalResources.ErrorMsgIdNotFound));
    //        else                      //MULTIPLE IDS ALREADY EXIST?? SHOULD NOT BE POSSIBLE
    //          retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.InsertFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.InsertFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<MultiLineTextDto> Delete(Guid id)
    //{
    //  Result<MultiLineTextDto> retResult = Result<MultiLineTextDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from multiLineTextData in ctx.ObjectContext.MultiLineTextDatas
    //                    where multiLineTextData.Id == id
    //                    select multiLineTextData;

    //      if (results.Count() == 1)
    //      {
    //        var multiLineTextDataToRemove = results.First();
    //        ctx.ObjectContext.MultiLineTextDatas.DeleteObject((multiLineTextDataToRemove));
    //        ctx.ObjectContext.SaveChanges();
    //        retResult = Result<MultiLineTextDto>.Success(EfHelper.ToDto(multiLineTextDataToRemove));
    //      }
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<MultiLineTextDto>.FailureWithInfo(null,
    //            new Exceptions.DeleteFailedException(DalResources.ErrorMsgIdNotFound));
    //        else
    //          retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<MultiLineTextDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<ICollection<MultiLineTextDto>> GetAll()
    //{
    //  var retAllMultiLineTextDtos = new List<MultiLineTextDto>();
    //  using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //  {
    //    var allMultiLineTextDatas = from multiLineTextData in ctx.ObjectContext.MultiLineTextDatas
    //                         select multiLineTextData;
    //    foreach (var multiLineTextData in allMultiLineTextDatas)
    //    {
    //      var multiLineTextDto = EfHelper.ToDto(multiLineTextData);
    //      retAllMultiLineTextDtos.Add(multiLineTextDto);
    //    }
    //    //var allDtos = new List<MultiLineTextDto>(ctx.ObjectContext.MultiLineTextDatas);
    //    return retAllMultiLineTextDtos;
    //  }
    //}
    protected override MultiLineTextDto NewImpl(object criteria)
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

      MultiLineTextDto newMultiLineTextDto = new MultiLineTextDto()
      {
        Id = Guid.NewGuid(),
        Title = DalResources.DefaultNewMultiLineTextTitle,
        AdditionalMetadata = DalResources.DefaultNewMultiLineTextAdditionalMetadata,
        UserId = currentUserId,
        Username = currentUsername
      };

      return newMultiLineTextDto;
    }
    protected override MultiLineTextDto FetchImpl(Guid id)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from multiLineTextData in ctx.ObjectContext.MultiLineTextDatas
                      where multiLineTextData.Id == id &&
                            multiLineTextData.UserDataId == currentUserId
                      select multiLineTextData;

        if (results.Count() == 1)
        {
          var fetchedMultiLineTextData = results.First();

          MultiLineTextDto multiLineTextDto = EfHelper.ToDto(fetchedMultiLineTextData);
          return multiLineTextDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple multiLineTexts with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override ICollection<MultiLineTextDto> FetchImpl(ICollection<Guid> ids)
    {
      var multiLineTextDtos = new List<MultiLineTextDto>();
      foreach (var id in ids)
      {
        multiLineTextDtos.Add(FetchImpl(id));
      }
      return multiLineTextDtos;
    }
    protected override MultiLineTextDto UpdateImpl(MultiLineTextDto dto)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from multiLineTextData in ctx.ObjectContext.MultiLineTextDatas
                      where multiLineTextData.Id == dto.Id &&
                            multiLineTextData.UserDataId == currentUserId
                      select multiLineTextData;

        if (results.Count() == 1)
        {
          var multiLineTextData = results.First();
          EfHelper.LoadDataFromDto(ref multiLineTextData, dto, ctx.ObjectContext);
          
          ctx.ObjectContext.SaveChanges(); 

          var updatedDto = EfHelper.ToDto(multiLineTextData);
          return updatedDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(dto.Id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple multiLineTexts with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override MultiLineTextDto InsertImpl(MultiLineTextDto dto)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        MultiLineTextData newMultiLineTextData = EfHelper.AddToContext(dto, ctx.ObjectContext);
        ctx.ObjectContext.SaveChanges();
        dto.Id = newMultiLineTextData.Id;
        return dto;
      }
    }
    protected override MultiLineTextDto DeleteImpl(Guid id)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from multiLineTextData in ctx.ObjectContext.MultiLineTextDatas
                      where multiLineTextData.Id == id &&
                            multiLineTextData.UserDataId == currentUserId
                      select multiLineTextData;

        if (results.Count() == 1)
        {
          //GET
          var multiLineTextDataToDelete = results.First();
          var retDto = EfHelper.ToDto(multiLineTextDataToDelete);
          
          //DELETE
          ctx.ObjectContext.MultiLineTextDatas.DeleteObject(multiLineTextDataToDelete);

          //SAVE CHANGES
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
            //which means that we have multiple multiLineTexts with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override ICollection<MultiLineTextDto> GetAllImpl()
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var allMultiLineTextDtos = new List<MultiLineTextDto>();
        CustomIdentity identity = (CustomIdentity)Csla.ApplicationContext.User.Identity;

        var datas = (from data in ctx.ObjectContext.MultiLineTextDatas
                     where data.UserDataId == identity.UserId
                     select data);//.ToList();

        foreach (var usersMultiLineTextData in datas)
        {
          allMultiLineTextDtos.Add(EfHelper.ToDto(usersMultiLineTextData));
        }

        return allMultiLineTextDtos;
      }
    }

    //private Guid GetDefaultLanguageId()
    //{
    //  Guid retDefaultLanguageId;

    //  using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //  {
    //    try
    //    {

    //      retDefaultLanguageId = (from defaultLanguage in ctx.ObjectContext.LanguageDatas
    //                              where defaultLanguage.Text == EfResources.DefaultLanguageText
    //                              select defaultLanguage).First().Id;
    //    }
    //    catch (Exception ex)
    //    {
    //      throw new Exceptions.GeneralDataAccessException(ex);
    //    }
    //  }

    //  return retDefaultLanguageId;
    //}

    protected override ICollection<MultiLineTextDto> FetchByIdImpl(Guid lineId)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from mltData in ctx.ObjectContext.MultiLineTextDatas
                      where mltData.UserDataId == currentUserId &&
                            (from lineData in mltData.LineDatas
                             where lineData.Id == lineId
                             select lineData).Count() > 0
                      select mltData;

        List<MultiLineTextDto> dtos = new List<MultiLineTextDto>();
        foreach (var data in results)
        {
          var dto = EfHelper.ToDto(data);
          dtos.Add(dto);
        }

        return dtos;
        //if (results.Count() == 1)
        //{
        //  var fetchedMultiLineTextData = results.First();

        //  MultiLineTextDto multiLineTextDto = EfHelper.ToDto(fetchedMultiLineTextData);
        //  return multiLineTextDto;
        //}
        //else
        //{
        //  if (results.Count() == 0)
        //    throw new Exceptions.IdNotFoundException(id);
        //  else
        //  {
        //    //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
        //    //which means that we have multiple multiLineTexts with the same id.  this is very bad.
        //    var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
        //                                 DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
        //    throw new Exceptions.VeryBadException(errorMsg);
        //  }
        //}
      }
    }
  }
}
