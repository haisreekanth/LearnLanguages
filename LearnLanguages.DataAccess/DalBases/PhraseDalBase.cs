﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  /// <summary>
  /// This class wraps every IPhraseDal with a try..catch wrapper
  /// block that does the wrapping for each call.  The descending classes only need
  /// to implement the 
  /// </summary>
  public abstract class PhraseDalBase : IPhraseDal
  {
    public Result<PhraseDto> New(object criteria)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        var dto = NewImpl(criteria);
        retResult = Result<PhraseDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.CreateFailedException(ex);
        retResult = Result<PhraseDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<PhraseDto> Fetch(Guid id)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        var dto = FetchImpl(id);
        retResult = Result<PhraseDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<PhraseDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<PhraseDto> Update(PhraseDto dtoToUpdate)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        var updatedDto = UpdateImpl(dtoToUpdate);
        retResult = Result<PhraseDto>.Success(updatedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.UpdateFailedException(ex);
        retResult = Result<PhraseDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<PhraseDto> Insert(PhraseDto dtoToInsert)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        var insertedDto = InsertImpl(dtoToInsert);
        retResult = Result<PhraseDto>.Success(insertedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.InsertFailedException(ex);
        retResult = Result<PhraseDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<PhraseDto> Delete(Guid id)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        var dto = DeleteImpl(id);
        retResult = Result<PhraseDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.DeleteFailedException(ex);
        retResult = Result<PhraseDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<ICollection<PhraseDto>> GetAll()
    {
      Result<ICollection<PhraseDto>> retResult = Result<ICollection<PhraseDto>>.Undefined(null);
      try
      {
        var allDtos = GetAllImpl();
        retResult = Result<ICollection<PhraseDto>>.Success(allDtos);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.GetAllFailedException(ex);
        retResult = Result<ICollection<PhraseDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }

    protected abstract PhraseDto NewImpl(object criteria);
    protected abstract PhraseDto FetchImpl(Guid id);
    protected abstract PhraseDto UpdateImpl(PhraseDto dto);
    protected abstract PhraseDto InsertImpl(PhraseDto dto);
    protected abstract PhraseDto DeleteImpl(Guid id);
    protected abstract ICollection<PhraseDto> GetAllImpl();
  }
}
