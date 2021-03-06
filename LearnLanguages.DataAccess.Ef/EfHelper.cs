﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using LearnLanguages.Business.Security;
using Csla.Core;

namespace LearnLanguages.DataAccess.Ef
{
  public static class EfHelper
  {
    public static string GetConnectionString()
    {
      return ConfigurationManager.ConnectionStrings[EfResources.LearnLanguagesConnectionStringKey].ConnectionString;
    }

    #region ToData
    public static RoleData ToData(RoleDto dto)
    {
      return new RoleData()
      {
        Id = dto.Id,
        Text = dto.Text,
      };
    }

    public static LanguageData ToData(LanguageDto dto, LearnLanguagesContext context)
    {
      //CREATE DATA OBJECT
      var languageData = context.LanguageDatas.CreateObject();

      //ASSIGN SIMPLE PROPERTIES
      languageData.Id = dto.Id;
      languageData.Text = dto.Text;
      languageData.UserDataId = dto.UserId;

      //POPULATE USERDATA
      var results = (from u in context.UserDatas
                     where u.Id == dto.UserId
                     select u);
      if (results.Count() == 1)
      {
        var userData = results.First();

        //MAKE SURE USERNAMES MATCH
        if (userData.Username != dto.Username)
          throw new ArgumentException("languageDto dto");
        languageData.UserData = results.First();
      }
      else if (results.Count() == 0)
        throw new Exceptions.UsernameAndUserIdDoNotMatchException(dto.Username, dto.UserId);
      else
        throw new Exceptions.VeryBadException(
          string.Format(DalResources.ErrorMsgVeryBadException,
          DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero));

      //RETURN
      return languageData;
    }

    #endregion

    #region ToDto
    public static UserDto ToDto(UserData data)
    {
      //SCALARS
      var retUserDto = new UserDto()
      {
        Id = data.Id,
        Username = data.Username,
        Salt = data.Salt,
        SaltedHashedPasswordValue = data.SaltedHashedPasswordValue
      };

      //PHRASES
      retUserDto.PhraseIds = new List<Guid>();
      foreach (var phraseData in data.PhraseDatas)
      {
        retUserDto.PhraseIds.Add(phraseData.Id);
      }

      //ROLES
      retUserDto.RoleIds = new List<Guid>();
      foreach (var roleData in data.RoleDatas)
      {
        retUserDto.RoleIds.Add(roleData.Id);
      }

      return retUserDto;
    }
    public static PhraseDto ToDto(PhraseData data)
    {
      return new PhraseDto()
      {
        Id = data.Id,
        Text = data.Text,
        LanguageId = data.LanguageDataId,
        UserId = data.UserDataId,
        Username = data.UserDataReference.Value.Username
      };
    }
    public static LanguageDto ToDto(LanguageData data)
    {
      var dto = new LanguageDto()
      {
        Id = data.Id,
        Text = data.Text,
        UserId = data.UserDataId,
        Username = data.UserData.Username
      };

      return dto;
    }
    public static TranslationDto ToDto(TranslationData data)
    {
      var dto = new TranslationDto()
      {
        Id = data.Id,
        PhraseIds = (from phrase in data.PhraseDatas
                     select phrase.Id).ToList(),
        UserId = data.UserDataId,
        Username = data.UserData.Username
      };

      return dto;
    }
    public static LineDto ToDto(LineData data)
    {
      var dto = new LineDto()
      {
        Id = data.Id,
        LineNumber = data.LineNumber,
        PhraseId = data.PhraseDataId,
        UserId = data.UserDataId,
        Username = data.UserData.Username
      };

      return dto;
    }
    public static RoleDto ToDto(RoleData data)
    {
      return new RoleDto()
      {
        Id = data.Id,
        Text = data.Text
      };
    }
    public static StudyDataDto ToDto(StudyDataData data)
    {
      var dto = new StudyDataDto()
      {
        Id = data.Id,
        NativeLanguageText = data.NativeLanguageText,
        Username = data.Username
      };

      return dto;
    }
    public static PhraseBeliefDto ToDto(PhraseBeliefData data)
    {
      var dto = new PhraseBeliefDto()
      {
        Id = data.Id,
        BelieverId = data.BelieverId,
        PhraseId = data.PhraseDataId,
        ReviewMethodId = data.ReviewMethodId,
        Strength = data.Strength,
        Text = data.Text,
        TimeStamp = data.TimeStamp,
        UserId = data.UserDataId,
        Username = data.UserData.Username,
      };

      return dto;
    }
    public static MultiLineTextDto ToDto(MultiLineTextData data)
    {
      var dto = new MultiLineTextDto()
      {
        Id = data.Id,
        AdditionalMetadata = data.AdditionalMetadata,
        LineIds = (from line in data.LineDatas
                   select line.Id).ToList(),
        Title = data.Title,
        UserId = data.UserDataId,
        Username = data.UserData.Username,
      };

      return dto;
    }

    #endregion

    //public static UserData ToData(UserDto dto, bool includeForeignEntities = true)
    //{
    //  var retUserData = new UserData()
    //  {
    //    Id = dto.Id,
    //    Username = dto.Username,
    //    Salt = dto.Salt,
    //    SaltedHashedPasswordValue = dto.SaltedHashedPasswordValue
    //  };

    //  if (includeForeignEntities)
    //  {
    //    using (var dalManager = DalFactory.GetDalManager())
    //    {
    //      //USER PHRASES
    //      var phraseDal = dalManager.GetProvider<IPhraseDal>();
    //      foreach (var phraseId in dto.PhraseIds)
    //      {
    //        var result = phraseDal.Fetch(phraseId);
    //        if (!result.IsSuccess || result.IsError)
    //          throw new Exceptions.FetchFailedException(result.Msg);

    //        //var phraseData = ToData(result.Obj);
    //        //retUserData.PhraseDatas.Add(phraseData);
    //        var phraseData = EfHelper.AddToContext(result.Obj, retUserData.
    //      }

    //      //USER ROLES
    //      var customIdentityDal = dalManager.GetProvider<ICustomIdentityDal>();
    //      foreach (var roleId in dto.RoleIds)
    //      {
    //        var result = customIdentityDal.GetRoles(dto.Username);
    //        if (!result.IsSuccess || result.IsError)
    //          throw new Exceptions.FetchFailedException(result.Msg);

    //        var roleDtos = result.Obj;
    //        foreach (var roleDto in roleDtos)
    //        {
    //          RoleData roleData = new RoleData()
    //          {
    //            Id = roleDto.Id,
    //            Text = roleDto.Text
    //          };
    //          retUserData.RoleDatas.Add(roleData);
    //        }
    //      }
    //    }
    //  }

    //  return retUserData;
    //}

    #region AddToContext

    /// <summary>
    /// Adds the phraseDto to the context, loading UserData and LanguageData into the newly
    /// created PhraseData.  Does NOT save changes to the context.
    /// </summary>
    public static PhraseData AddToContext(PhraseDto dto, LearnLanguagesContext context)
    {
      //only creates, does not add to phrasedatas
      //var beforeCount = context.PhraseDatas.Count();
      var newPhraseData = context.PhraseDatas.CreateObject();
      //var afterCount = context.PhraseDatas.Count();

      //assign properties
      newPhraseData.Text = dto.Text;
      newPhraseData.LanguageDataId = dto.LanguageId;
      newPhraseData.UserDataId = dto.UserId;

      context.PhraseDatas.AddObject(newPhraseData);

      return newPhraseData;
    }
    /// <summary>
    /// Adds the lineDto to the context, loading UserData and PhraseData into the newly
    /// created LineData.  Does NOT save changes to the context.
    /// </summary>
    public static LineData AddToContext(LineDto dto, LearnLanguagesContext context)
    {
      //only creates, does not add to linedatas
      //var beforeCount = context.LineDatas.Count();
      var newLineData = context.LineDatas.CreateObject();
      //var afterCount = context.LineDatas.Count();

      //assign properties
      newLineData.LineNumber = dto.LineNumber;
      newLineData.PhraseDataId = dto.PhraseId;
      newLineData.UserDataId = dto.UserId;


      context.LineDatas.AddObject(newLineData);

      return newLineData;
    }
    /// <summary>
    /// Adds the TranslationDto to the context, loading UserData and PhraseDatas into the newly
    /// created PhraseData.  Does NOT save changes to the context.
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="learnLanguagesContext"></param>
    /// <returns></returns>
    public static TranslationData AddToContext(TranslationDto dto, LearnLanguagesContext context)
    {
      //CREATE NEW TRANSLATIONDATA
      var newTranslationData = context.TranslationDatas.CreateObject();

      //ASSIGN USER INFO
      newTranslationData.UserDataId = dto.UserId;
      //var userResults = (from user in context.UserDatas
      //                   where user.Id == dto.UserId
      //                   select user);
      //if (userResults.Count() == 1)
      //  newTranslationData.UserData = userResults.First();
      //else if (userResults.Count() == 0)
      //  throw new Exceptions.IdNotFoundException(dto.UserId);
      //else
      //{
      //  var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
      //                               DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
      //  throw new Exceptions.VeryBadException(errorMsg);
      //}
      

      //GET AND ADD PHRASEDATAS TO TRANSLATIONDATA
      if (dto.PhraseIds != null)
      {
        foreach (var id in dto.PhraseIds)
        {
          var results = (from phrase in context.PhraseDatas
                         where phrase.Id == id
                         select phrase);

          if (results.Count() == 1)
          {
            var phraseData = results.First();
            newTranslationData.PhraseDatas.Add(phraseData);
          }
          else if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(id);
          else
          {
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
      
      //ADD TRANSLATIONDATA TO CONTEXT
      context.TranslationDatas.AddObject(newTranslationData);

      return newTranslationData;
    }
    /// <summary>
    /// Adds the StudyDataDto to the context, loading UserData and PhraseDatas into the newly
    /// created PhraseData.  Does NOT save changes to the context.
    /// </summary>
    public static StudyDataData AddToContext(StudyDataDto dto, LearnLanguagesContext context)
    {
      //CREATE THE NEW OBJECT
      var newStudyDataData = context.StudyDataDatas.CreateObject();

      //ASSIGN PROPERTIES
      newStudyDataData.NativeLanguageText = dto.NativeLanguageText;
      newStudyDataData.Username = dto.Username;

      //ADD OBJECT TO CONTEXT
      context.StudyDataDatas.AddObject(newStudyDataData);

      //RETURN OBJECT
      return newStudyDataData;
    }
    /// <summary>
    /// Adds the PhraseBeliefDto to the context, loading UserData and PhraseDatas into the newly
    /// created PhraseBeliefData.  Does NOT save changes to the context.
    /// </summary>
    public static PhraseBeliefData AddToContext(PhraseBeliefDto dto, LearnLanguagesContext context)
    {
      //CREATE THE NEW OBJECT
      var data = context.PhraseBeliefDatas.CreateObject();

      //ASSIGN PROPERTIES
      data.PhraseDataId = dto.PhraseId;
      data.ReviewMethodId = dto.ReviewMethodId;
      data.Strength = dto.Strength;
      data.Text = dto.Text;
      data.TimeStamp = dto.TimeStamp;
      data.UserDataId = dto.UserId;

      //ADD OBJECT TO CONTEXT
      context.PhraseBeliefDatas.AddObject(data);

      //RETURN OBJECT
      return data;
    }
    /// <summary>
    /// Adds the dto to the context, loading UserData and LineDatas into the newly
    /// created PhraseBeliefData.  Does NOT save changes to the context.
    /// </summary>
    public static MultiLineTextData AddToContext(MultiLineTextDto dto, LearnLanguagesContext context)
    {
      //CREATE NEW TRANSLATIONDATA
      var data = context.MultiLineTextDatas.CreateObject();

      //SCALARS
      data.Title = dto.Title;
      data.AdditionalMetadata = dto.AdditionalMetadata;

      //ASSIGN USER INFO
      data.UserDataId = dto.UserId;

      //ADD LINE DATAS FROM DTO.LINEIDS
      if (dto.LineIds != null)
      {
        foreach (var id in dto.LineIds)
        {
          var results = (from line in context.LineDatas
                         where line.Id == id
                         select line);

          if (results.Count() == 1)
          {
            var lineData = results.First();
            data.LineDatas.Add(lineData);
          }
          else if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(id);
          else
          {
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }

      //ADD DATA TO CONTEXT
      context.MultiLineTextDatas.AddObject(data);

      return data;
    }

    #endregion

    #region LoadDataFromDto

    /// <summary>
    /// Loads the information from the dto into the data object. Except...
    /// Does NOT load dto.Id.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="dto"></param>
    public static void LoadDataFromDto(ref TranslationData data, 
                                       TranslationDto dto, 
                                       LearnLanguagesContext context)
    {
      //COPY USER INFO
      data.UserDataId = dto.UserId;
      data.UserData = EfHelper.GetUserData(dto.UserId, context);

      var currentPhraseIds = (from phrase in data.PhraseDatas
                              select phrase.Id);

      //COPY PHRASEID INFO
      //ADD NEW PHRASEDATAS IN THE DTO
      foreach (var id in dto.PhraseIds)
      {
        if (!currentPhraseIds.Contains(id))
        {
          PhraseData phraseData = EfHelper.GetPhraseData(id, context);
          data.PhraseDatas.Add(phraseData);
        }
      }

      //REMOVE PHRASEDATAS THAT ARE NOT IN DTO ANYMORE
      foreach (var phraseId in currentPhraseIds)
      {
        if (!dto.PhraseIds.Contains(phraseId))
        {
          var dataToRemove = (from phraseData in data.PhraseDatas
                              where phraseData.Id == phraseId
                              select phraseData).First();
          data.PhraseDatas.Remove(dataToRemove);
        }
      }
    }

    /// <summary>
    /// Loads the information from the dto into the data object. Except...
    /// Does NOT load dto.Id.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="dto"></param>
    public static void LoadDataFromDto(ref PhraseData data,
                                       PhraseDto dto,
                                       LearnLanguagesContext context)
    {
      //USER INFO
      data.UserDataId = dto.UserId;
      data.UserData = EfHelper.GetUserData(dto.UserId, context);

      //LANGUAGE INFO
      data.LanguageDataId = dto.LanguageId;
      data.LanguageData = EfHelper.GetLanguageData(dto.LanguageId, context);

      //TEXT
      data.Text = dto.Text;

      //TRANSLATIONDATAS
      data.TranslationDatas.Load();
    }

    /// <summary>
    /// Loads the information from the dto into the data object. Except...
    /// Does NOT load dto.Id.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="dto"></param>
    public static void LoadDataFromDto(ref LanguageData data,
                                       LanguageDto dto,
                                       LearnLanguagesContext context)
    {
      //USER INFO
      data.UserDataId = dto.UserId;
      data.UserData = EfHelper.GetUserData(dto.UserId, context);

      //MAKE SURE USERDATA USERNAME MATCHES DTO.USERNAME
      if (data.UserData.Username != dto.Username)
        throw new Exceptions.UsernameAndUserIdDoNotMatchException(dto.Username, dto.UserId);

      //TEXT
      data.Text = dto.Text;
    }

    public static void LoadDataFromDto(ref LineData data,
                                       LineDto dto, 
                                       LearnLanguagesContext context)
    {
      //USER INFO
      data.UserDataId = dto.UserId;
      data.UserData = EfHelper.GetUserData(dto.UserId, context);

      //PHRASE INFO
      data.PhraseDataId = dto.PhraseId;
      data.PhraseData = EfHelper.GetPhraseData(dto.PhraseId, context);

      //TEXT
      data.LineNumber = dto.LineNumber;
    }

    public static void LoadDataFromDto(ref StudyDataData data,
                                       StudyDataDto dto,
                                       LearnLanguagesContext context)
    {
      data.NativeLanguageText = dto.NativeLanguageText;
      data.Username = dto.Username;
    }

    public static void LoadDataFromDto(ref PhraseBeliefData data, 
                                       PhraseBeliefDto dto, 
                                       LearnLanguagesContext context)
    {
      //USER INFO
      data.UserDataId = dto.UserId;
      data.UserData = EfHelper.GetUserData(dto.UserId, context);
      
      //PHRASE
      data.PhraseDataId = dto.PhraseId;
      data.PhraseData = EfHelper.GetPhraseData(dto.PhraseId, context);

      //SCALAR
      data.ReviewMethodId = dto.ReviewMethodId;
      data.Strength = dto.Strength;
      data.Text = dto.Text;
      data.TimeStamp = dto.TimeStamp;
    }

    public static void LoadDataFromDto(ref MultiLineTextData data,
                                       MultiLineTextDto dto, 
                                       LearnLanguagesContext context)
    {
      //USER INFO
      data.UserDataId = dto.UserId;
      data.UserData = EfHelper.GetUserData(dto.UserId, context);

      //LINE IDS
      foreach (var id in dto.LineIds)
      {
        LineData lineData = EfHelper.GetLineData(id, context);
        data.LineDatas.Add(lineData);
      }

      //SCALAR
      data.Title = dto.Title;
      data.AdditionalMetadata = dto.AdditionalMetadata;
    }

    

    #endregion

    #region Get [User/Language/Phrase/Line] Data

    private static LanguageData GetLanguageData(Guid languageId, LearnLanguagesContext context)
    {
      var currentUserId = ((CustomIdentity)Csla.ApplicationContext.User.Identity).UserId;

      var results = from languageData in context.LanguageDatas
                    where languageData.Id == languageId 
                    select languageData;

      if (results.Count() == 1)
        return results.First();
      else if (results.Count() == 0)
        throw new Exceptions.IdNotFoundException(languageId);
      else
      {
        var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                     DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
        throw new Exceptions.VeryBadException(errorMsg);
      }
    }
    private static UserData GetUserData(Guid userId, LearnLanguagesContext context)
    {
      var results = (from user in context.UserDatas
                     where user.Id == userId
                     select user);

      if (results.Count() == 1)
        return results.First();
      else if (results.Count() == 0)
        throw new Exceptions.IdNotFoundException(userId);
      else
      {
        var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                     DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
        throw new Exceptions.VeryBadException(errorMsg);
      }
    }
    private static PhraseData GetPhraseData(Guid phraseId, LearnLanguagesContext context)
    {
      var currentUserId = ((CustomIdentity)Csla.ApplicationContext.User.Identity).UserId;

      var results = from phraseData in context.PhraseDatas
                    where phraseData.Id == phraseId &&
                          phraseData.UserDataId == currentUserId
                    select phraseData;

      if (results.Count() == 1)
        return results.First();
      else if (results.Count() == 0)
        throw new Exceptions.IdNotFoundException(phraseId);
      else
      {
        var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                     DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
        throw new Exceptions.VeryBadException(errorMsg);
      }
    }
    private static LineData GetLineData(Guid id, LearnLanguagesContext context)
    {
      var currentUserId = ((CustomIdentity)Csla.ApplicationContext.User.Identity).UserId;

      var results = from lineData in context.LineDatas
                    where lineData.Id == id &&
                          lineData.UserDataId == currentUserId
                    select lineData;

      if (results.Count() == 1)
        return results.First();
      else if (results.Count() == 0)
        throw new Exceptions.IdNotFoundException(id);
      else
      {
        var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                     DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
        throw new Exceptions.VeryBadException(errorMsg);
      }
    }

    #endregion
  }
}
