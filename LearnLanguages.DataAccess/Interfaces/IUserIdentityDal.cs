using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public interface IUserIdentityDal
  {
    Result<bool?> VerifyUser(string username, string password);
    Result<ICollection<RoleDto>> GetRoles(string username);
    Result<UserDto> GetUser(string username);

    Result<UserDto> AddUser(string newUserName, string password);

    Result<UserDto> New(object criteria);
    Result<UserDto> Fetch(Guid id);
    Result<UserDto> Insert(UserDto dto);
    Result<UserDto> Update(UserDto dto);
    Result<UserDto> Delete(Guid id);
    Result<bool?> DeleteUser(string username);
    Result<ICollection<UserDto>> GetAllUsers();
  }
}
