using System;
using System.Collections.Generic;
using System.Linq;
using Csla.Data;

using System.Data.Objects;
using System.Data.Objects.DataClasses;
using LearnLanguages.Business.Security;

namespace LearnLanguages.DataAccess.Ef
{
  public class StudyDataDal : StudyDataDalBase
  {
    protected override StudyDataDto NewImpl(object criteria)
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

      StudyDataDto newStudyDataDto = new StudyDataDto()
      {
        Id = Guid.NewGuid(),
        NativeLanguageText = DalResources.DefaultNativeLanguageText,
        Username = currentUsername
      };

      return newStudyDataDto;
    }
    //protected override StudyDataDto FetchImpl(Guid id)
    //{
    //  var currentUsername = ((UserIdentity)(Csla.ApplicationContext.User.Identity)).Name;

    //  using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
    //  {
    //    var results = from studyDataData in ctx.ObjectContext.StudyDataDatas
    //                  where studyDataData.Id == id &&
    //                        studyDataData.Username == currentUsername
    //                  select studyDataData;

    //    if (results.Count() == 1)
    //    {
    //      var fetchedStudyDataData = results.First();

    //      StudyDataDto studyDataDto = EfHelper.ToDto(fetchedStudyDataData);
    //      return studyDataDto;
    //    }
    //    else
    //    {
    //      if (results.Count() == 0)
    //        throw new Exceptions.IdNotFoundException(id);
    //      else
    //      {
    //        //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
    //        //which means that we have multiple studyDatas with the same id.  this is very bad.
    //        var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
    //                                     DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
    //        throw new Exceptions.VeryBadException(errorMsg);
    //      }
    //    }
    //  }
    //}
    protected override StudyDataDto FetchForCurrentUserImpl()
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var currentUsername = Csla.ApplicationContext.User.Identity.Name;

        var results = from studyDataData in ctx.ObjectContext.StudyDataDatas
                      where studyDataData.Username == currentUsername
                      select studyDataData;

        StudyDataDto dto = null;

        if (results.Count() == 1)
        {
          var studyDataData = results.First();
          dto = EfHelper.ToDto(studyDataData);
          return dto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.StudyDataNotFoundForUserException(currentUsername);
          else
            throw new Exceptions.VeryBadException();
        }
      }
    }
    protected override bool StudyDataExistsForCurrentUserImpl()
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var currentUsername = Csla.ApplicationContext.User.Identity.Name;

        var results = from studyDataData in ctx.ObjectContext.StudyDataDatas
                      where studyDataData.Username == currentUsername
                      select studyDataData;

        if (results.Count() == 1)
        {
          return true;
        }
        else
        {
          if (results.Count() == 0)
            return false;
          else
            throw new Exceptions.VeryBadException();
        }
      }
    }
    //protected override ICollection<StudyDataDto> FetchImpl(ICollection<Guid> ids)
    //{
    //  var studyDataDtos = new List<StudyDataDto>();
    //  foreach (var id in ids)
    //  {
    //    studyDataDtos.Add(FetchImpl(id));
    //  }
    //  return studyDataDtos;
    //}
    protected override StudyDataDto UpdateImpl(StudyDataDto dto)
    {
      var currentUsername = Business.BusinessHelper.GetCurrentUsername();

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from studyDataData in ctx.ObjectContext.StudyDataDatas
                      where studyDataData.Id == dto.Id &&
                            studyDataData.Username == currentUsername
                      select studyDataData;

        if (results.Count() == 1)
        {
          var studyDataData = results.First();
          EfHelper.LoadDataFromDto(ref studyDataData, dto, ctx.ObjectContext);

          ctx.ObjectContext.SaveChanges();

          var updatedDto = EfHelper.ToDto(studyDataData);
          return updatedDto;
        }
        else if (results.Count() == 0)
        {
          //NO STUDY DATA FOR CURRENT USER TO UPDATE. 
          //SO, INSERT STUDY DATA INSTEAD
          return InsertImpl(dto);
        }
        else
        {
          //RESULTS.COUNT IS NOT ONE OR ZERO.  EITHER IT'S NEGATIVE, WHICH WOULD BE FRAMEWORK ABSURD, OR ITS MORE THAN ONE,
          //WHICH MEANS THAT WE HAVE MULTIPLE STUDYDATAS WITH THE SAME ID.  THIS IS VERY BAD.
          var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                       DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
          throw new Exceptions.VeryBadException(errorMsg);
        }
      }
    }
    protected override StudyDataDto InsertImpl(StudyDataDto dto)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var newStudyDataData = EfHelper.AddToContext(dto, ctx.ObjectContext);
        ctx.ObjectContext.SaveChanges();
        dto.Id = newStudyDataData.Id;
        return dto;
      }
    }
    protected override StudyDataDto DeleteImpl(Guid id)
    {
      var currentUsername = ((UserIdentity)(Csla.ApplicationContext.User.Identity)).Name;

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from studyDataData in ctx.ObjectContext.StudyDataDatas
                      where studyDataData.Id == id &&
                            studyDataData.Username == currentUsername
                      select studyDataData;

        if (results.Count() == 1)
        {
          var studyDataDataToDelete = results.First();

          //GET DTO BEFORE DELETE
          var retDto = EfHelper.ToDto(studyDataDataToDelete);

          //DELETE THE OBJECT FROM THE CONTEXT
          ctx.ObjectContext.StudyDataDatas.DeleteObject(studyDataDataToDelete);

          //SAVE CHANGES
          ctx.ObjectContext.SaveChanges();

          //RETURN THE DTO
          return retDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple studyDatas with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }
    protected override ICollection<StudyDataDto> GetAllImpl()
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var allStudyDataDtos = new List<StudyDataDto>();
        UserIdentity identity = (UserIdentity)Csla.ApplicationContext.User.Identity;

        var studyDataDatas = (from studyDataData in ctx.ObjectContext.StudyDataDatas
                           where studyDataData.Username == identity.Name
                           select studyDataData).ToList();

        foreach (var usersStudyDataData in studyDataDatas)
        {
          allStudyDataDtos.Add(EfHelper.ToDto(usersStudyDataData));
        }

        return allStudyDataDtos;
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
