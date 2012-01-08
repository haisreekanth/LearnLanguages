﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.Mock
{
  public class PhraseDal : IPhraseDal
  {
    public Result<PhraseDto> New(object criteria)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        var dto = new PhraseDto() { Id = Guid.NewGuid() };
        retResult = Result<PhraseDto>.Success(dto);
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
    public Result<PhraseDto> Fetch(Guid id)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        var results = from item in MockDb.Phrases
                      where item.Id == id
                      select item;

        if (results.Count() == 1)
          retResult = Result<PhraseDto>.Success(results.First());
        else
        {
          if (results.Count() == 0)
            retResult = Result<PhraseDto>.FailureWithInfo(null,
              new Exceptions.FetchFailedException(DalResources.Dal_IdNotFound));
          else
            retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
        }
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
    public Result<PhraseDto> Update(PhraseDto dto)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        var results = from item in MockDb.Phrases
                      where item.Id == dto.Id
                      select item;

        if (results.Count() == 1)
        {
          var PhraseToUpdate = results.First();
          MockDb.Phrases.Remove(PhraseToUpdate);
          dto.Id = Guid.NewGuid();
          MockDb.Phrases.Add(dto);
          retResult = Result<PhraseDto>.Success(dto);
        }
        else
        {
          if (results.Count() == 0)
            retResult = Result<PhraseDto>.FailureWithInfo(null,
              new Exceptions.UpdateFailedException(DalResources.Dal_IdNotFound));
          else
            retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
        }
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
    public Result<PhraseDto> Insert(PhraseDto dto)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        var results = from item in MockDb.Phrases
                      where item.Id == dto.Id
                      select item;

        if (results.Count() == 0)
        {
          dto.Id = Guid.NewGuid();
          MockDb.Phrases.Add(dto);
          retResult = Result<PhraseDto>.Success(dto);
        }
        else
        {
          if (results.Count() == 1) //ID ALREADY EXISTS
            retResult = Result<PhraseDto>.FailureWithInfo(dto,
              new Exceptions.UpdateFailedException(DalResources.Dal_IdNotFound));
          else                      //MULTIPLE IDS ALREADY EXIST??
            retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
        }
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
    public Result<PhraseDto> Delete(Guid id)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        var results = from item in MockDb.Phrases
                      where item.Id == id
                      select item;

        if (results.Count() == 1)
        {
          var PhraseToRemove = results.First();
          MockDb.Phrases.Remove(PhraseToRemove);
          retResult = Result<PhraseDto>.Success(PhraseToRemove);
        }
        else
        {
          if (results.Count() == 0)
            retResult = Result<PhraseDto>.FailureWithInfo(null,
              new Exceptions.DeleteFailedException(DalResources.Dal_IdNotFound));
          else
            retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
        }
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
    public LearnLanguages.Result<ICollection<PhraseDto>> GetAll()
    {
      Result<ICollection<PhraseDto>> retResult = Result<ICollection<PhraseDto>>.Undefined(null);
      try
      {
        var allDtos = new List<PhraseDto>(MockDb.Phrases);
        retResult = Result<ICollection<PhraseDto>>.Success(allDtos);
      }
      catch (Exception ex)
      {
        retResult = Result<ICollection<PhraseDto>>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
  }
}
