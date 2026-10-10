# ExampleAPI

Учебный REST API на ASP.NET Core 9 с использованием Entity Framework Core и SQLite.

**Возможности:**

- Регистрация и вход с выдачей JWT-токена.
- Разграничение доступа по ролям.
- Получение списка пользователей администратором.
- Загрузка, просмотр и скачивание файлов.
- Тестирование методов через Swagger.

**Запуск:** установите .NET SDK 9 и выполните из каталога `ExampleAPI`:

```bash
dotnet restore
dotnet run --launch-profile http
```

Swagger: http://localhost:5239/swagger

При первом запуске тестовая БД автоматически копируется в папку `TestAPI` в документах пользователя.