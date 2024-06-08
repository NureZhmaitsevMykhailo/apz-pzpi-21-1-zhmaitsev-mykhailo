using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using IoT.Entities;
using Microsoft.Extensions.Configuration;

// Без цього ми не побачимо українськи символи
Console.OutputEncoding = Encoding.UTF8;

// Завантаження конфігурації з файлу appsettings.json
var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);
IConfiguration configBuilder = builder.Build();
var configuration = new Configuration();
configBuilder.Bind(configuration);

Console.WriteLine("Ласкаво просимо до системи OncoBound!");

// Аутентифікація та отримання JWT
using var client = new HttpClient();
var loginData = new
{
    email = configuration.Credentials.Email,
    password = configuration.Credentials.Password,
};

var loginContent = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
var loginResponse = await client.PostAsync($"{configuration.ApiUrl}/User/login", loginContent);

if (!loginResponse.IsSuccessStatusCode)
{
    Console.WriteLine($"Помилка: {loginResponse.StatusCode} - Неможливо увійти.");
    return;
}

var jwt = await loginResponse.Content.ReadAsStringAsync();
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

Console.WriteLine("Будь ласка, проведіть карткою для продовження ->>");

var userId = Convert.ToInt32(Console.ReadLine());

Console.WriteLine("Перевірка даних користувача");
for (int i = 0; i < 3; i++)
{
    Console.WriteLine("...");
    await Task.Delay(1000);
}

try
{
    // Отримання даних користувача
    var userResponse = await client.GetAsync($"{configuration.ApiUrl}/User/{userId}");

    // Перевірка чи користувача не знайдено
    if (!userResponse.IsSuccessStatusCode)
    {
        Console.WriteLine($"Помилка: {userResponse.StatusCode} - Неможливо знайти користувача.");
        return;
    }
    
    var userResponseBody = await userResponse.Content.ReadAsStringAsync();
    var user = JsonSerializer.Deserialize<User>(userResponseBody);

    Console.WriteLine($"Привіт, {user.fullname}!");

    Console.WriteLine("Отримання ваших призначених ліків...");
    for (int i = 0; i < 2; i++)
    {
        Console.WriteLine("...");
        await Task.Delay(1000);
    }
    
    var response = await client.PostAsync($"{configuration.ApiUrl}/Medicine/takeMedicines/{userId}", null);
    
    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine($"Помилка: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        return;
    }

    var responseBody = await response.Content.ReadAsStringAsync();
    var medications = JsonSerializer.Deserialize<PrescriptedMedicines>(responseBody)!.prescriptedMedicineDtos;
    
    Console.WriteLine("Ліки успішно видані:");
    foreach (var medication in medications)
    {
        Console.WriteLine($"Ідентифікатор рецепта: {medication.prescriptionId}");
        Console.WriteLine($"Ідентифікатор ліків: {medication.medicationId}");
        Console.WriteLine($"Дозування: {medication.dosage}");
        Console.WriteLine($"Частота прийому: {medication.frequency}");
        Console.WriteLine($"Тривалість курсу: {medication.duration}");
        Console.WriteLine($"Дата призначення: {medication.datePrescribedUTC}");
        Console.WriteLine("-------------------");
    }
    Console.WriteLine("Будьте здорові!");
}
catch (Exception ex)
{
    // Відображення будь-якої виняткової ситуації, яка може виникнути
    Console.WriteLine($"Виникла несподівана помилка: {ex.Message}");
}
