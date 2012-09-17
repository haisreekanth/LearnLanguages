using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public interface IUserDal
  {
    Result<bool?> VerifyUser(string username, string password);
    Result<ICollection<RoleDto>> GetRoles(string username);

    Result<UserDto> New(object criteria);
    Result<UserDto> AddUser(Csla.Security.UsernameCriteria criteria);
    //Result<UserDto> AddUser(string newUserName, string password);
    Result<UserDto> Fetch(string username);
    Result<UserDto> Fetch(Guid id);
    Result<UserDto> Insert(UserDto dto);
    Result<UserDto> Update(UserDto dto);
    Result<UserDto> Delete(Guid id);
    Result<bool?> Delete(string username);
    Result<ICollection<UserDto>> GetAll();
  }
}
