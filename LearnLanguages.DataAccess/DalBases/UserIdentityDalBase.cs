using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  /// <summary>
  /// This class wraps every IUserIdentityDal with a try..catch wrapper
  /// block that does the wrapping for each call.  The descending classes only need
  /// to implement the 
  /// </summary>
  public abstract class UserIdentityDalBase : IUserIdentityDal
  {
    public Result<bool?> VerifyUser(string username, string password)
    {
      Result<bool?> retResult = Result<bool?>.Undefined(null);
      try
      {
        var verified = VerifyUserImpl(username, password);
        retResult = Result<bool?>.Success(verified);
      }
      catch (Exception ex)
      {
        var wrapperEx = new Exceptions.VerifyUserFailedException(ex, username);
        retResult = Result<bool?>.FailureWithInfo(null, wrapperEx);
      }

      return retResult;
    }
    public Result<UserDto> GetUser(string username)
    {
      Result<UserDto> retResult = Result<UserDto>.Undefined(null);
      try
      {
        var userDto = GetUserImpl(username);
        if (userDto == null)
          throw new Exceptions.UsernameNotFoundException(username);
        retResult = Result<UserDto>.Success(userDto);
      }
      catch (Exception ex)
      {
        var wrapperEx = new Exceptions.GetUserFailedException(ex, username);
        retResult = Result<UserDto>.FailureWithInfo(null, wrapperEx);
      }

      return retResult;
    }
    public Result<ICollection<RoleDto>> GetRoles(string username)
    {
      Result<ICollection<RoleDto>> retResult = Result<ICollection<RoleDto>>.Undefined(null);
      try
      {
        var roles = GetRolesImpl(username);
        retResult = Result<ICollection<RoleDto>>.Success(roles);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.GetRolesFailedException(ex, username);
        retResult = Result<ICollection<RoleDto>>.FailureWithInfo(null, wrappedEx);
      }

      return retResult;
    }
    public Result<UserDto> AddUser(string username, string password)
    {
      Result<UserDto> retResult = Result<UserDto>.Undefined(null);
      try
      {
        DalHelper.CheckAuthorizationToAddUser();
        var userDto = AddUserImpl(username, password);
        if (userDto == null)
          throw new Exception();
        retResult = Result<UserDto>.Success(userDto);
      }
      catch (Exception ex)
      {
        var wrapperEx = new Exceptions.GetUserFailedException(ex, username);
        retResult = Result<UserDto>.FailureWithInfo(null, wrapperEx);
      }

      return retResult;
    }
    public Result<bool?> DeleteUser(string username)
    {
      Result<bool?> retResult = Result<bool?>.Undefined(null);
      try
      {
        DalHelper.CheckAuthorizationToDeleteUser();
        var deleteResult = DeleteUserImpl(username);
        if (deleteResult == null)
          throw new Exception();
        retResult = Result<bool?>.Success(deleteResult);
      }
      catch (Exception ex)
      {
        var wrapperEx = new Exceptions.GetUserFailedException(ex, username);
        retResult = Result<bool?>.FailureWithInfo(null, wrapperEx);
      }

      return retResult;
    }
    public Result<ICollection<UserDto>> GetAllUsers()
    {
      Result<ICollection<UserDto>> retResult = Result<ICollection<UserDto>>.Undefined(null);
      try
      {
        DalHelper.CheckAuthorizationToGetAllUsers();

        var allUsers = GetAllUsersImpl();
        retResult = Result<ICollection<UserDto>>.Success(allUsers);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.GetAllFailedException(ex);
        retResult = Result<ICollection<UserDto>>.FailureWithInfo(null, wrappedEx);
      }

      return retResult;
    }

    protected abstract bool? VerifyUserImpl(string username, string password);
    protected abstract UserDto GetUserImpl(string username);
    protected abstract ICollection<RoleDto> GetRolesImpl(string username);
    protected abstract UserDto AddUserImpl(string username, string password);
    protected abstract bool? DeleteUserImpl(string username);
    protected abstract ICollection<UserDto> GetAllUsersImpl();
  }
}
