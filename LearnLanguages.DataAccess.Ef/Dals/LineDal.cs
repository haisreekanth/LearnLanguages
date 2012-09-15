using System;
using System.Collections.Generic;
using System.Linq;
using Csla.Data;

using System.Data.Objects;
using System.Data.Objects.DataClasses;
using LearnLanguages.Business.Security;

namespace LearnLanguages.DataAccess.Ef
{
  public class LineDal : LineDalBase
  {
    //public Result<LineDto> New(object criteria)
    //{
    //  //throw new NotImplementedException("Ef.LineDal.New(object)");
    //  Result<LineDto> retResult = Result<LineDto>.Undefined(null);
    //  try
    //  {
        
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.CreateFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<LineDto> Fetch(Guid id)
    //{
    //  Result<LineDto> retResult = Result<LineDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from lineData in ctx.ObjectContext.LineDatas
    //                    where lineData.Id == id
    //                    select lineData;

    //      if (results.Count() == 1)
    //        retResult = Result<LineDto>.Success(EfHelper.ToDto(results.First()));
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<LineDto>.FailureWithInfo(null,
    //            new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFound));
    //        else
    //          retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.FetchFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<LineDto> Update(LineDto dto)
    //{
    //  Result<LineDto> retResult = Result<LineDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from lineData in ctx.ObjectContext.LineDatas
    //                    where lineData.Id == dto.Id
    //                    select lineData;

    //      if (results.Count() == 1)
    //      {
    //        var lineDataToUpdate = results.First();
    //        ctx.ObjectContext.LineDatas.DeleteObject(lineDataToUpdate);
    //        var newLineData = EfHelper.ToData(dto);
    //        ctx.ObjectContext.LineDatas.AddObject(newLineData);
    //        ctx.ObjectContext.SaveChanges();
    //        dto.Id = newLineData.Id;
    //        retResult = Result<LineDto>.Success(dto);
    //      }
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<LineDto>.FailureWithInfo(null,
    //            new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFound));
    //        else
    //          retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<LineDto> Insert(LineDto dto)
    //{
    //  Result<LineDto> retResult = Result<LineDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from lineData in ctx.ObjectContext.LineDatas
    //                    where lineData.Id == dto.Id
    //                    select lineData;

    //      //SHOULD FIND ZERO LANGUAGEDTOS (NO DUPLICATE IDS, NO DUPLICATE DTOS)
    //      if (results.Count() == 0)
    //      {
    //        var data = EfHelper.ToData(dto);
    //        ctx.ObjectContext.LineDatas.AddObject(data);
    //        ctx.ObjectContext.SaveChanges();
    //        dto.Id = data.Id;
    //        retResult = Result<LineDto>.Success(dto);
    //      }
    //      else
    //      {
    //        if (results.Count() == 1) //ID ALREADY EXISTS
    //          retResult = Result<LineDto>.FailureWithInfo(dto,
    //            new Exceptions.InsertFailedException(DalResources.ErrorMsgIdNotFound));
    //        else                      //MULTIPLE IDS ALREADY EXIST?? SHOULD NOT BE POSSIBLE
    //          retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.InsertFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.InsertFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<LineDto> Delete(Guid id)
    //{
    //  Result<LineDto> retResult = Result<LineDto>.Undefined(null);
    //  try
    //  {
    //    using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //    {
    //      var results = from lineData in ctx.ObjectContext.LineDatas
    //                    where lineData.Id == id
    //                    select lineData;

    //      if (results.Count() == 1)
    //      {
    //        var lineDataToRemove = results.First();
    //        ctx.ObjectContext.LineDatas.DeleteObject((lineDataToRemove));
    //        ctx.ObjectContext.SaveChanges();
    //        retResult = Result<LineDto>.Success(EfHelper.ToDto(lineDataToRemove));
    //      }
    //      else
    //      {
    //        if (results.Count() == 0)
    //          retResult = Result<LineDto>.FailureWithInfo(null,
    //            new Exceptions.DeleteFailedException(DalResources.ErrorMsgIdNotFound));
    //        else
    //          retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LineDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException(ex));
    //  }
    //  return retResult;
    //}
    //public Result<ICollection<LineDto>> GetAll()
    //{
    //  var retAllLineDtos = new List<LineDto>();
    //  using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //  {
    //    var allLineDatas = from lineData in ctx.ObjectContext.LineDatas
    //                         select lineData;
    //    foreach (var lineData in allLineDatas)
    //    {
    //      var lineDto = EfHelper.ToDto(lineData);
    //      retAllLineDtos.Add(lineDto);
    //    }
    //    //var allDtos = new List<LineDto>(ctx.ObjectContext.LineDatas);
    //    return retAllLineDtos;
    //  }
    //}
    protected override LineDto NewImpl(object criteria)
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

      LineDto newLineDto = new LineDto()
      {
        Id = Guid.NewGuid(),
        PhraseId = Guid.Empty,
        UserId = currentUserId,
        Username = currentUsername
      };

      return newLineDto;
    }
    protected override LineDto FetchImpl(Guid id)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from lineData in ctx.ObjectContext.LineDatas
                      where lineData.Id == id &&
                            lineData.UserDataId == currentUserId
                      select lineData;

        if (results.Count() == 1)
        {
          var fetchedLineData = results.First();

          var lineDto = EfHelper.ToDto(fetchedLineData);
          return lineDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple lines with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override ICollection<LineDto> FetchImpl(ICollection<Guid> ids)
    {
      var lineDtos = new List<LineDto>();
      foreach (var id in ids)
      {
        lineDtos.Add(FetchImpl(id));
      }
      return lineDtos;
    }
    protected override LineDto UpdateImpl(LineDto dto)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from lineData in ctx.ObjectContext.LineDatas
                      where lineData.Id == dto.Id &&
                            lineData.UserDataId == currentUserId
                      select lineData;

        if (results.Count() == 1)
        {
          var lineData = results.First();
          EfHelper.LoadDataFromDto(ref lineData, dto, ctx.ObjectContext);
          
          ctx.ObjectContext.SaveChanges(); 

          var updatedDto = EfHelper.ToDto(lineData);
          return updatedDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(dto.Id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple lines with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override LineDto InsertImpl(LineDto dto)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var newLineData = EfHelper.AddToContext(dto, ctx.ObjectContext);
        ctx.ObjectContext.SaveChanges();
        dto.Id = newLineData.Id;
        return dto;
      }
    }
    protected override LineDto DeleteImpl(Guid id)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from lineData in ctx.ObjectContext.LineDatas
                      where lineData.Id == id &&
                            lineData.UserDataId == currentUserId
                      select lineData;

        if (results.Count() == 1)
        {
          var lineDataToDelete = results.First();
          var retDto = EfHelper.ToDto(lineDataToDelete);

          //DELETE THE LINE ITSELF
          ctx.ObjectContext.LineDatas.DeleteObject(lineDataToDelete);

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
            //which means that we have multiple lines with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override ICollection<LineDto> GetAllImpl()
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var allLineDtos = new List<LineDto>();
        UserIdentity identity = (UserIdentity)Csla.ApplicationContext.User.Identity;

        var lineDatas = (from lineData in ctx.ObjectContext.LineDatas
                         where lineData.UserDataId == identity.UserId
                         select lineData).ToList();

        foreach (var usersLineData in lineDatas)
        {
          var dto = EfHelper.ToDto(usersLineData);
          allLineDtos.Add(dto);
        }

        return allLineDtos;
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

  }
}
