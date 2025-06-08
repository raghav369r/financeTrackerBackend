using financeTrackerBackned.Data;
using financeTrackerBackned.Domain;
using financeTrackerBackned.Dtos;
using financeTrackerBackned.Errors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace financeTrackerBackned.Services
{
  public class UserService
  {
    private readonly DataContext _dataContext;
    private readonly PasswordService _passwordService;
    public UserService(DataContext dataContext, PasswordService passwordService)
    {
      _dataContext = dataContext;
      _passwordService = passwordService;
    }
    public async Task<Object?> Login(LoginDto _user)
    {
      var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == _user.Email);
      var dummyUser = new User
      {
        Email = _user.Email,
      };
      if (user == null || !_passwordService.IsEqual(user, user.Password, _user.Password)) return null;
      return user;
    }
    public async Task<Object?> Register(RegisterDto _user)
    {
      if (await _dataContext.Users.AnyAsync(u => u.Email == _user.Email))
        return new UserAlreadyExistError();
      var dummyUser = new User
      {
        Email = _user.Email,
      };
      string hashedPassword = _passwordService.HashPassword(dummyUser, _user.Password);
      var user = new User(_user.Email, _user.FullName, hashedPassword);
      var addedUser=await _dataContext.Users.AddAsync(user);
      await _dataContext.SaveChangesAsync();
      return addedUser.Entity;
    }
  }
}