using ExampleAPI.Entities;
using Microsoft.AspNetCore.Identity;

namespace ExampleAPI.Service;

public class HashingService
{
    private static PasswordHasher<AppUser> _hasher =  new PasswordHasher<AppUser>();

    /// <summary>
    /// Пересылать уже найденых пользователей
    /// </summary>
    /// <param name="user"><see cref="AppUser"/></param>
    /// <param name="password"><see cref="string"/></param>
    /// <returns>Если пароль подходит, то вернет - true <see cref="bool"/></returns>
    public static bool CheckPassword(AppUser? user, string password)
    {
        if(user == null|| string.IsNullOrWhiteSpace(password)) 
            return false;
        return _hasher.VerifyHashedPassword(user,user.PassHash, password) == PasswordVerificationResult.Success;
    }

    /// <summary>
    /// Помогает создать хеш пароля нового пользователя
    /// При не удаче вернет null
    /// </summary>
    /// <param name="user"><see cref="AppUser"/></param>
    /// <param name="password"><see cref="string"/></param>
    /// <returns>При успехе возвращает <see cref="AppUser"/>? с захешированным паролем</returns>
    public static AppUser? HashPassword(AppUser? user, string password)
    {
        if(user == null|| string.IsNullOrWhiteSpace(password)) 
            return null;
        user.PassHash = _hasher.HashPassword(user, password);
        return user;
    }
}