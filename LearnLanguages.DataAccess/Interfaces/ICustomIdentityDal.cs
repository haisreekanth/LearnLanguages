using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public interface ICustomIdentityDal
  {
    Result<bool?> VerifyUser(string username, string password);
    Result<UserDto> GetUser(string username);
    Result<ICollection<RoleDto>> GetRoles(string username);

    Result<UserDto> AddUser(string newUserName, string password);
    Result<bool?> DeleteUser(string username);
    Result<ICollection<UserDto>> GetAllUsers();
  }
}
