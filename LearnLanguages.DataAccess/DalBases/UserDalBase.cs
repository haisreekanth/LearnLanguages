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
  public abstract class UserDalBase : IUserDal
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
    public Result<ICollection<RoleDto>> GetRoles(string username)
    {
      Result<ICollection<RoleDto>> retResult = Result<ICollection<RoleDto>>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
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

    public Result<UserDto> AddUser(Csla.Security.UsernameCriteria criteria)
    {
      Result<UserDto> retResult = Result<UserDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        DalHelper.CheckAuthorizationToAddUser();

        var userDto = AddUserImpl(criteria);
        if (userDto == null)
          throw new Exception();
        retResult = Result<UserDto>.Success(userDto);
      }
      catch (Exception ex)
      {
        var wrapperEx = new Exceptions.AddUserFailedException(ex, criteria.Username);
        retResult = Result<UserDto>.FailureWithInfo(null, wrapperEx);
      }

      return retResult;
    }
    public Result<UserDto> New(object criteria)
    {
      Result<UserDto> retResult = Result<UserDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        DalHelper.CheckAuthorizationToAddUser();

        var dto = NewImpl(criteria);
        retResult = Result<UserDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.CreateFailedException(ex);
        retResult = Result<UserDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<UserDto> Fetch(string username)
    {
      Result<UserDto> retResult = Result<UserDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();

        var userDto = FetchImpl(username);
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
    public Result<UserDto> Fetch(Guid id)
    {
      Result<UserDto> retResult = Result<UserDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        DalHelper.CheckAuthorizationToGetAllUsers();

        var dto = FetchImpl(id);
        retResult = Result<UserDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<UserDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<UserDto> Insert(UserDto dto)
    {
      Result<UserDto> retResult = Result<UserDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        DalHelper.CheckAuthorizationToAddUser();

        var insertedDto = InsertImpl(dto);
        retResult = Result<UserDto>.Success(insertedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.InsertFailedException(ex);
        retResult = Result<UserDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<UserDto> Update(UserDto dto)
    {
      Result<UserDto> retResult = Result<UserDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        DalHelper.CheckAuthorizationToAddUser();
        DalHelper.CheckAuthorizationToDeleteUser();

        var updatedDto = UpdateImpl(dto);
        retResult = Result<UserDto>.Success(updatedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.UpdateFailedException(ex);
        retResult = Result<UserDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<UserDto> Delete(Guid id)
    {
      Result<UserDto> retResult = Result<UserDto>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        DalHelper.CheckAuthorizationToDeleteUser();

        var dto = DeleteImpl(id);
        retResult = Result<UserDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.DeleteFailedException(ex);
        retResult = Result<UserDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<bool?> Delete(string username)
    {
      Result<bool?> retResult = Result<bool?>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        DalHelper.CheckAuthorizationToDeleteUser();
        var deleteResult = DeleteImpl(username);
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
    public Result<ICollection<UserDto>> GetAll()
    {
      Result<ICollection<UserDto>> retResult = Result<ICollection<UserDto>>.Undefined(null);
      try
      {
        Common.CommonHelper.CheckAuthentication();
        DalHelper.CheckAuthorizationToGetAllUsers();

        var allUsers = GetAllImpl();
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
    protected abstract ICollection<RoleDto> GetRolesImpl(string username);

    protected abstract UserDto AddUserImpl(Csla.Security.UsernameCriteria criteria);
    protected abstract bool? DeleteImpl(string username);
    protected abstract ICollection<UserDto> GetAllImpl();

    protected abstract UserDto NewImpl(object criteria);
    protected abstract UserDto FetchImpl(string username);
    protected abstract UserDto FetchImpl(Guid id);
    protected abstract UserDto InsertImpl(UserDto dto);
    protected abstract UserDto UpdateImpl(UserDto dto);
    protected abstract UserDto DeleteImpl(Guid id);

    
  }
}
