using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LearnLanguages.DataAccess.Mock
{
  public class PhraseDal : PhraseDalBase
  {
    //public Result<PhraseDto> New(object criteria)
    //{
    //  Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
    //  try
    //  {
    //    var dto = new PhraseDto() 
    //    { 
    //      Id = Guid.NewGuid(),
    //      LanguageId = SeedData.Ton.DefaultLanguageId
    //    };
    //    retResult = Result<PhraseDto>.Success(dto);
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<PhraseDto> Fetch(Guid id)
    //{
    //  Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.Phrases
    //                  where item.Id == id
    //                  select item;

    //    if (results.Count() == 1)
    //      retResult = Result<PhraseDto>.Success(results.First());
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<PhraseDto>.FailureWithInfo(null,
    //          new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<PhraseDto> Update(PhraseDto dto)
    //{
    //  Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.Phrases
    //                  where item.Id == dto.Id
    //                  select item;

    //    if (results.Count() == 1)
    //    {
    //      var PhraseToUpdate = results.First();
    //      SeedData.Ton.Phrases.Remove(PhraseToUpdate);
    //      dto.Id = Guid.NewGuid();
    //      SeedData.Ton.Phrases.Add(dto);
    //      retResult = Result<PhraseDto>.Success(dto);
    //    }
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<PhraseDto>.FailureWithInfo(null,
    //          new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<PhraseDto> Insert(PhraseDto dto)
    //{
    //  Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.Phrases
    //                  where item.Id == dto.Id
    //                  select item;

    //    if (results.Count() == 0)
    //    {
    //      dto.Id = Guid.NewGuid();
    //      //MIMIC LANGUAGEID REQUIRED CONSTRAINT IN DB
    //      if (dto.LanguageId == Guid.Empty || !SeedData.Ton.ContainsLanguageId(dto.LanguageId))
    //      {
    //        //I'VE RESTRUCTURED HOW TO DO EXCEPTIONHANDLING, SO THIS IS NOT QUITE HOW IT SHOULD BE DONE.
    //        //THIS SHOULD BE AN INSERTIMPL METHOD, AND IT SHOULD THROW ITS OWN EXCEPTION THAT IS WRAPPED IN THE 
    //        //PHRASEDALBASE CLASS IN AN INSERTFAILEDEXCEPTION.
    //        throw new Exceptions.InsertFailedException(string.Format(DalResources.ErrorMsgIdNotFoundException, dto.LanguageId));
    //      }
    //      SeedData.Ton.Phrases.Add(dto);
    //      retResult = Result<PhraseDto>.Success(dto);
    //    }
    //    else
    //    {
    //      if (results.Count() == 1) //ID ALREADY EXISTS
    //        retResult = Result<PhraseDto>.FailureWithInfo(dto,
    //          new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else                      //MULTIPLE IDS ALREADY EXIST??
    //        retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<PhraseDto> Delete(Guid id)
    //{
    //  Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Ton.Phrases
    //                  where item.Id == id
    //                  select item;

    //    if (results.Count() == 1)
    //    {
    //      var PhraseToRemove = results.First();
    //      SeedData.Ton.Phrases.Remove(PhraseToRemove);
    //      retResult = Result<PhraseDto>.Success(PhraseToRemove);
    //    }
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<PhraseDto>.FailureWithInfo(null,
    //          new Exceptions.DeleteFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public LearnLanguages.Result<ICollection<PhraseDto>> GetAll()
    //{
    //  Result<ICollection<PhraseDto>> retResult = Result<ICollection<PhraseDto>>.Undefined(null);
    //  try
    //  {
    //    var allDtos = new List<PhraseDto>(SeedData.Ton.Phrases);
    //    retResult = Result<ICollection<PhraseDto>>.Success(allDtos);
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<ICollection<PhraseDto>>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}

    protected override PhraseDto NewImpl(object criteria)
    {
      //to get to this point, we must have already been authenticated
      Debug.Assert(Csla.ApplicationContext.User.Identity.IsAuthenticated);
      //if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
      //  throw new Common.Exceptions.UserNotAuthenticatedException();

      var languageId = SeedData.Ton.DefaultLanguageId;

      if (criteria is string)
      {
        //IF WE HAVE A STRING PARAM, THEN IT IS LANGUAGETEXT, SO GET THE LANGUAGE ID FROM THAT
        var languageText = (string)criteria;
        var languageResults = (from language in SeedData.Ton.Languages
                               where language.Text == languageText
                               select language);
        if (languageResults.Count() == 1)
        {
          var languageDto = languageResults.First();
          languageId = languageDto.Id;
        }
        else if (languageResults.Count() == 0)
          throw new Exceptions.LanguageTextNotFoundException(languageText);
        else
          throw new Exceptions.VeryBadException();
      }

      var username = Csla.ApplicationContext.User.Identity.Name;
      var userId = (from u in SeedData.Ton.Users
                    where u.Username == username
                    select u.Id).FirstOrDefault();
      if (userId == Guid.Empty)
        throw new Exceptions.UserNotAuthorizedException();


      var dto = new PhraseDto()
      {
        Id = Guid.NewGuid(),
        LanguageId = languageId,
        UserId = userId, 
        Username = username
      };

      return dto;
    }
    protected override PhraseDto FetchImpl(Guid id)
    {
      var currentUserId = MockHelper.GetCurrentUserId();
      
      var results = from item in SeedData.Ton.Phrases
                    where item.Id == id && 
                          item.UserId == currentUserId
                    select item;

      if (results.Count() == 1)
        return results.First();
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(id);
        else
          throw new Exceptions.VeryBadException();
      }
    }
    protected override ICollection<PhraseDto> FetchImpl(string text)
    {
      if (string.IsNullOrEmpty(text))
        throw new ArgumentNullException("text", "PhraseDal.FetchImpl(text): text argument cannot be null or empty string");

      var retPhrases = (from phrase in SeedData.Ton.Phrases
                        where phrase.Text.Contains(text)
                        select phrase).ToList();

      return retPhrases;
    }
    protected override ICollection<PhraseDto> FetchImpl(ICollection<Guid> ids)
    {
      if (ids == null)
        throw new ArgumentNullException("ids");
      else if (ids.Count == 0)
        throw new ArgumentOutOfRangeException("ids", "ids cannot be empty.");

      var retPhrases = (from phrase in SeedData.Ton.Phrases
                        where ids.Contains(phrase.Id)
                        select phrase).ToList();

      return retPhrases;

      
      
      //var retPhrases = new List<PhraseDto>();

      //foreach (var id in ids)
      //{
      //  var dto = FetchImpl(id);
      //  retPhrases.Add(dto);
      //}

      //return retPhrases;
    }
    protected override PhraseDto UpdateImpl(PhraseDto dto)
    {
      var currentUserId = MockHelper.GetCurrentUserId();

      var results = from item in SeedData.Ton.Phrases
                    where item.Id == dto.Id &&
                          item.UserId == currentUserId
                    select item;

      if (results.Count() == 1)
      {
        CheckContraints(dto);

        var phraseToUpdate = results.First();
        SeedData.Ton.Phrases.Remove(phraseToUpdate);
        dto.Id = Guid.NewGuid();
        SeedData.Ton.Phrases.Add(dto);
        UpdateReferences(phraseToUpdate, dto);
        return dto;
      }
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(dto.Id);
        else
          throw new Exceptions.VeryBadException();
      }
    }
    protected override PhraseDto InsertImpl(PhraseDto dto)
    {
      var currentUserId = MockHelper.GetCurrentUserId();

      var results = from item in SeedData.Ton.Phrases
                    where item.Id == dto.Id
                    select item;

      if (results.Count() == 0)
      {
        CheckContraints(dto);

        dto.Id = Guid.NewGuid();
        SeedData.Ton.Phrases.Add(dto);

        //ADD PHRASE.ID TO USER
        var resultsUser = from u in SeedData.Ton.Users
                          where u.Id == dto.UserId
                          select u;

        var user = resultsUser.First();
        user.PhraseIds.Add(dto.Id);

        return dto;
      }
      else
      {
        if (results.Count() == 1) //ID ALREADY EXISTS
          throw new Exceptions.IdAlreadyExistsException(dto.Id);
        else                      //MULTIPLE IDS ALREADY EXIST??
          throw new Exceptions.VeryBadException();
      }
    }
    protected override PhraseDto DeleteImpl(Guid id)
    {
      var currentUserId = MockHelper.GetCurrentUserId();

      var results = from item in SeedData.Ton.Phrases
                    where item.Id == id &&
                          item.UserId == currentUserId
                    select item;

      if (results.Count() == 1)
      {
        var PhraseToRemove = results.First();
        SeedData.Ton.Phrases.Remove(PhraseToRemove);
        return PhraseToRemove;
      }
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(id);
        else
          throw new Exceptions.VeryBadException();
      }
    }
    protected override ICollection<PhraseDto> GetAllImpl()
    {
      var currentUserId = MockHelper.GetCurrentUserId();

      var results = from dto in SeedData.Ton.Phrases
                    where dto.UserId == currentUserId
                    select dto;

      var allDtos = new List<PhraseDto>(results);
      return allDtos;
    }

    private void CheckContraints(PhraseDto dto)
    {
      //REFERENTIAL INTEGRITY
      if (dto.LanguageId == Guid.Empty || !SeedData.Ton.ContainsLanguageId(dto.LanguageId))
        throw new Exceptions.IdNotFoundException(dto.LanguageId);
      if (dto.UserId == Guid.Empty || !SeedData.Ton.ContainsUserId(dto.UserId))
        throw new Exceptions.IdNotFoundException(dto.UserId);
      if (string.IsNullOrEmpty(dto.Username) ||
         !(SeedData.Ton.GetUsername(dto.UserId) == dto.Username))
        throw new ArgumentException("dto.Username");
    }
    private void UpdateReferences(PhraseDto oldPhrase, PhraseDto newPhrase)
    {
      //UPDATE USERS WHO REFERENCE THIS PHRASE
      var referencedUsers = from u in SeedData.Ton.Users
                            where u.PhraseIds.Contains(oldPhrase.Id)
                            select u;

      foreach (var user in referencedUsers)
      {
        user.PhraseIds.Remove(oldPhrase.Id);
        user.PhraseIds.Add(newPhrase.Id);
      }

      //UPDATE TRANSLATIONS WHO REFERENCE THIS PHRASE
      var referencedTranslations = from t in SeedData.Ton.Translations
                                   where t.PhraseIds.Contains(oldPhrase.Id)
                                   select t;

      foreach (var translation in referencedTranslations)
      {
        translation.PhraseIds.Remove(oldPhrase.Id);
        translation.PhraseIds.Add(newPhrase.Id);
      }
    }

  }
}
