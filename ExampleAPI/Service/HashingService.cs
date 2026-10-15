using ExampleAPI.Entities;
using Microsoft.AspNetCore.Identity;

namespace ExampleAPI.Service;

public class HashingService
{
    private static PasswordHasher<AppUser> _hasher =  new PasswordHasher<AppUser>();

    /// <summary>
    /// Проверяет соответствие пароля сохранённому хешу
    /// </summary>
    /// <param name="user"><see cref="AppUser"/> Пользователь из базы данных</param>
    /// <param name="password"><see cref="string"/> Пароль для проверки</param>
    /// <returns>Если пароль подходит, то вернет - true <see cref="bool"/></returns>
    public static bool CheckPassword(AppUser? user, string password)
    {
        if(user == null|| string.IsNullOrWhiteSpace(password)) 
            return false;
        return _hasher.VerifyHashedPassword(user,user.PassHash, password) != PasswordVerificationResult.Failed;
    }

    /// <summary>
    /// Создаёт хеш пароля и записывает его в объект пользователя.
    /// </summary>
    /// <param name="user"><see cref="AppUser"/> Пользователь, которому устанавливается пароль</param>
    /// <param name="password"><see cref="string"/> Новый пароль</param>
    public static void HashPassword(AppUser? user, string password)
    {
        if(user == null|| string.IsNullOrWhiteSpace(password)) 
            return;
        user.PassHash = _hasher.HashPassword(user, password);
        
    }
}