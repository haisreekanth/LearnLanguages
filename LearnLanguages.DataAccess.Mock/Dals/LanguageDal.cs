using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.Mock
{
  public class LanguageDal : LanguageDalBase
  {
    //public Result<LanguageDto> New(object criteria)
    //{
    //  Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
    //  try
    //  {
    //    var dto = new LanguageDto() { Id = Guid.NewGuid() };
    //    retResult = Result<LanguageDto>.Success(dto);
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
    //    var results = from item in SeedData.Ton.Languages
    //                  where item.Id == id
    //                  select item;

    //    if (results.Count() == 1)
    //      retResult = Result<LanguageDto>.Success(results.First());
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<LanguageDto>.FailureWithInfo(null,
    //          new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LanguageDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<LanguageDto> Update(LanguageDto dto)
    //{
    //  Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.Languages
    //                  where item.Id == dto.Id
    //                  select item;

    //    if (results.Count() == 1)
    //    {
    //      var languageToUpdate = results.First();
    //      SeedData.Ton.Languages.Remove(languageToUpdate);
    //      dto.Id = Guid.NewGuid();
    //      SeedData.Ton.Languages.Add(dto);
    //      //UPDATE PHRASES WHO REFERENCE THIS LANGUAGE
    //      UpdateReferences(languageToUpdate, dto);

    //      retResult = Result<LanguageDto>.Success(dto);
    //    }
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<LanguageDto>.FailureWithInfo(null,
    //          new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LanguageDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<LanguageDto> Insert(LanguageDto dto)
    //{
    //  Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.Languages
    //                  where item.Id == dto.Id
    //                  select item;

    //    if (results.Count() == 0)
    //    {
    //      dto.Id = Guid.NewGuid();
    //      SeedData.Ton.Languages.Add(dto);
    //      retResult = Result<LanguageDto>.Success(dto);
    //    }
    //    else
    //    {
    //      if (results.Count() == 1) //ID ALREADY EXISTS
    //        retResult = Result<LanguageDto>.FailureWithInfo(dto,
    //          new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else                      //MULTIPLE IDS ALREADY EXIST??
    //        retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LanguageDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<LanguageDto> Delete(Guid id)
    //{
    //  Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.Languages
    //                  where item.Id == id
    //                  select item;

    //    if (results.Count() == 1)
    //    {
    //      var languageToRemove = results.First();
    //      SeedData.Ton.Languages.Remove(languageToRemove);
    //      retResult = Result<LanguageDto>.Success(languageToRemove);
    //    }
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<LanguageDto>.FailureWithInfo(null,
    //          new Exceptions.DeleteFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<LanguageDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public LearnLanguages.Result<ICollection<LanguageDto>> GetAll()
    //{
    //  Result<ICollection<LanguageDto>> retResult = Result<ICollection<LanguageDto>>.Undefined(null);
    //  try
    //  {
    //    var allDtos = new List<LanguageDto>(SeedData.Ton.Languages);
    //    retResult = Result<ICollection<LanguageDto>>.Success(allDtos);
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<ICollection<LanguageDto>>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}

    private void UpdateReferences(LanguageDto oldLanguageDto, LanguageDto newLanguageDto)
    {
      //UPDATE PHRASES
      var referencedPhrases = from p in SeedData.Ton.Phrases
                              where p.LanguageId == oldLanguageDto.Id
                              select p;

      foreach (var phrase in referencedPhrases)
      {
        phrase.LanguageId = newLanguageDto.Id;
      }
    }

    protected override LanguageDto NewImpl(object criteria)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();
      var currentUsername = Business.BusinessHelper.GetCurrentUsername();
      var dto = new LanguageDto() 
      {
        Id = Guid.NewGuid(), 
        UserId = currentUserId,
        Username = currentUsername
      };
      return dto;
    }
    protected override LanguageDto FetchImpl(Guid id)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      LanguageDto retLanguageDto = null;
      var results = from item in SeedData.Ton.Languages
                    where item.Id == id &&
                          item.UserId == currentUserId
                    select item;

      if (results.Count() == 1)
        retLanguageDto = results.First();
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(id);
        else
          throw new Exceptions.VeryBadException(
            string.Format(DalResources.ErrorMsgVeryBadException, 
                          DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero));
      }

      return retLanguageDto;
    }
    protected override LanguageDto FetchImpl(string languageText)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      LanguageDto retLanguageDto = null;
      var results = from item in SeedData.Ton.Languages
                    where item.Text == languageText &&
                          item.UserId == currentUserId
                    select item;

      if (results.Count() == 1)
        retLanguageDto = results.First();
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.LanguageTextNotFoundException(languageText);
        else
          throw new Exceptions.VeryBadException(
            string.Format(DalResources.ErrorMsgVeryBadException,
                          DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero));
      }

      return retLanguageDto;
    }
    protected override ICollection<LanguageDto> FetchImpl(ICollection<Guid> ids)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      List<LanguageDto> retLanguageDtos = null;

      var results = (from item in SeedData.Ton.Languages
                     where ids.Contains(item.Id) &&
                           item.UserId == currentUserId
                     select item).ToList();

      if (results.Count() == ids.Count)
        retLanguageDtos = results;
      else
      {
        if (results.Count() >= 0 && results.Count() < ids.Count)
        {
          var idsNotFound = new List<Guid>(ids);
          //AT LEAST ONE OF THE IDS WAS NOT FOUND.  REMOVING IDS THAT WERE FOUND
          //LEAVES US WITH THE ONES NOT FOUND.
          {
            var languagesThatWereFound = (from languageDto in results
                                          where ids.Contains(languageDto.Id)
                                          select languageDto);
            foreach (var foundLanguage in languagesThatWereFound)
            {
              idsNotFound.Remove(foundLanguage.Id);
            }

            if (idsNotFound.Count == 1)
              throw new Exceptions.IdNotFoundException(idsNotFound[0]);
            else if (idsNotFound.Count > 1)
              throw new Exceptions.MultipleIdsNotFoundException();
            else
              throw new Exceptions.VeryBadException();
          }
        }
        else
          throw new Exceptions.VeryBadException(
            string.Format(DalResources.ErrorMsgVeryBadException,
                          DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero));
      }

      return retLanguageDtos;
    }
    protected override LanguageDto UpdateImpl(LanguageDto dto)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      LanguageDto retLanguageDto = null;
      var results = from item in SeedData.Ton.Languages
                    where item.Id == dto.Id &&
                          item.UserId == currentUserId
                    select item;

      if (results.Count() == 1)
      {
        var languageToUpdate = results.First();
        SeedData.Ton.Languages.Remove(languageToUpdate);
        dto.Id = Guid.NewGuid();
        SeedData.Ton.Languages.Add(dto);
        //UPDATE PHRASES WHO REFERENCE THIS LANGUAGE
        UpdateReferences(languageToUpdate, dto);

        retLanguageDto = dto;
      }
      else
      {
        if (results.Count() == 0)
        {
          //ITEM NOT FOUND TO UPDATE, SO INSERT IT
          return InsertImpl(dto);
          //throw new Exceptions.IdNotFoundException(dto.Id);
        }
        else
          throw new Exceptions.VeryBadException(
            string.Format(DalResources.ErrorMsgVeryBadException,
                          DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero));

      }

      return retLanguageDto;
    }
    protected override LanguageDto InsertImpl(LanguageDto dto)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      LanguageDto retResult = null;
      var results = from item in SeedData.Ton.Languages
                    where item.Id == dto.Id &&
                          item.UserId == currentUserId
                    select item;

      if (results.Count() == 0)
      {
        dto.Id = Guid.NewGuid();
        SeedData.Ton.Languages.Add(dto);
        retResult = dto;
      }
      else
      {
        if (results.Count() == 1)
          throw new Exceptions.IdAlreadyExistsException(dto.Id);
        else
          throw new Exceptions.VeryBadException(
            string.Format(DalResources.ErrorMsgVeryBadException,
                          DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero));

      }
      
      return retResult;
    }
    protected override LanguageDto DeleteImpl(Guid id)
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      LanguageDto retResult = null;
      var results = from item in SeedData.Ton.Languages
                    where item.Id == id &&
                          item.UserId == currentUserId
                    select item;

      if (results.Count() == 1)
      {
        var languageToRemove = results.First();
        SeedData.Ton.Languages.Remove(languageToRemove);
        retResult = languageToRemove;
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
      
      return retResult;
    }
    protected override ICollection<LanguageDto> GetAllImpl()
    {
      var currentUserId = Business.BusinessHelper.GetCurrentUserId();

      var results = from language in SeedData.Ton.Languages
                    where language.UserId == currentUserId
                    select language;
      var allDtos = new List<LanguageDto>(results);
      return allDtos;
    }
  }
}
