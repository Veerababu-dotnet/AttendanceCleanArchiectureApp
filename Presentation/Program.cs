
using Application.Interfaces;
using Application.Services;
using Infrastructure.Configuration;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Presentation.Menus;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                // Load secrets.json
                config.AddUserSecrets<Program>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            })
            .ConfigureServices((context, services) =>
            {
                // Bind DatabaseSettings from secrets.json

                services.Configure<DatabaseSettings>(
                    context.Configuration.GetSection("DatabaseSettings"));

                // Register DbContext using DatabaseSettings
                services.AddDbContext<AppDbContext>((sp, options) =>
                {
                    var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                    options.UseSqlServer(settings.ConnectionString);
                    options.EnableSensitiveDataLogging(false);
                    options.LogTo(_ => { });
                });

                // Register repositories
                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

               
                services.AddScoped<IEmployeeService, EmployeeService>();
                services.AddScoped<IAttendanceService, AttendanceService>();
                services.AddScoped<IAuthService, AuthService>();

                // Register presentation layer
                services.AddScoped<MainMenu>();
            })
            .Build();

        await RunAsync(host.Services);
    }

    private static async Task RunAsync(IServiceProvider services)
    {
        try
        {
            var authService = services.GetRequiredService<IAuthService>();

            Console.WriteLine("===== LOGIN =====");
            Console.Write("Username: ");
            var username = Console.ReadLine() ?? string.Empty;

            Console.Write("Password: ");
            var password = ReadPassword();

            var isAuthenticated = await authService.LoginAsync(username, password);

            if (!isAuthenticated)
            {
                Console.WriteLine("\nInvalid username or password");
                return;
            }

            Console.WriteLine("\nLogin successful");

            var menu = services.GetRequiredService<MainMenu>();
            await menu.ShowAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error:");
            Console.WriteLine(ex.Message);
        }
    }

   
    private static string ReadPassword()
    {
        var password = string.Empty;
        ConsoleKey key;

        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                password += keyInfo.KeyChar;
                Console.Write("*");
            }
        } while (key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }
}
