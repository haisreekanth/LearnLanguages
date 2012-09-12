using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public interface IPhraseDal
  {
    Result<PhraseDto> New(object criteria);
    Result<PhraseDto> Fetch(Guid id);
    /// <summary>
    /// Fetch all phrases that include the given text, in all languages, for the current user.
    /// </summary>
    /// <param name="text">text to search for</param>
    /// <returns>collection of PhraseDto that include the given text</returns>
    Result<ICollection<PhraseDto>> Fetch(string text);
    Result<ICollection<PhraseDto>> Fetch(ICollection<Guid> ids);
    Result<PhraseDto> Update(PhraseDto dto);
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    Result<PhraseDto> Insert(PhraseDto dto);
    /// <summary>
    /// Apparently cannot overload when using WCF services.
    /// </summary>
    Result<PhraseDto> Delete(Guid id);
    Result<ICollection<PhraseDto>> GetAll();
  }
}
