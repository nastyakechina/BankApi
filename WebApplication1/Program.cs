using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Storage;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Указываем строку подключения к базе данных
var connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=nastya02032005";

// Добавление сервисов в контейнер зависимостей
builder.Services.AddControllers(); // Добавляем поддержку контроллеров

// Регистрация хранилищ в DI
builder.Services.AddScoped<ITransactionStorage, TransactionStorage>();
builder.Services.AddScoped<IWalletStorage, WalletStorage>();
builder.Services.AddScoped<ICoinStorage, CoinStorage>();

// Регистрация IDatabaseStorage<T> для всех необходимых моделей
builder.Services.AddScoped<IDatabaseStorage<Transaction>>(
    _ => new DatabaseStorage<Transaction>(connectionString, "Transactions"));
builder.Services.AddScoped<IDatabaseStorage<Wallet>>(
    _ => new DatabaseStorage<Wallet>(connectionString, "Wallets"));
builder.Services.AddScoped<IDatabaseStorage<Coin>>(
    _ => new DatabaseStorage<Coin>(connectionString, "Coins"));
builder.Services.AddScoped<IDatabaseStorage<CoinAmount>>(
    _ => new DatabaseStorage<CoinAmount>(connectionString, "CoinAmounts"));

// Настройка Swagger для документации API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Настройка конвейера обработки запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();

// using Microsoft.AspNetCore.Builder;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Storage;
// using Models;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Получаем строку подключения и имя таблицы из конфигурации (например, appsettings.json)
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//
// // Регистрация хранилищ в DI
// builder.Services.AddScoped<ITransactionStorage, TransactionStorage>();
// builder.Services.AddScoped<IWalletStorage, WalletStorage>();
// builder.Services.AddScoped<ICoinStorage, CoinStorage>();
// builder.Services.AddScoped<IDatabaseStorage<Transaction>>(provider => new DatabaseStorage<Transaction>(connectionString, "Transactions"));
// builder.Services.AddScoped<IDatabaseStorage<Wallet>>(provider => new DatabaseStorage<Wallet>(connectionString, "Wallets"));
// builder.Services.AddScoped<IDatabaseStorage<CoinAmount>>(provider => new DatabaseStorage<CoinAmount>(connectionString, "CoinAmounts"));
// builder.Services.AddScoped<IDatabaseStorage<Coin>>(provider => new DatabaseStorage<Coin>(connectionString, "Coins"));
//
//
// // Настройка Swagger для документации API
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//
// builder.Services.AddControllers();
//
// var app = builder.Build();
//
// // Настройка конвейера обработки запросов
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
// app.UseHttpsRedirection();
//
// // Добавление маршрутизации для контроллеров
// app.UseRouting();
//
// //app.UseAuthorization();
//
// // Подключаем маршруты для всех контроллеров
// app.MapControllers();
//
// app.Run();
