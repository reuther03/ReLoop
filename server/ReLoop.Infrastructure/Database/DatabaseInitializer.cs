using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReLoop.Api.Domain.Item;
using ReLoop.Api.Domain.User;

namespace ReLoop.Infrastructure.Database;

public class DatabaseInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ReLoopDbContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);

        await SeedDataAsync(dbContext, cancellationToken);
    }

    private static async Task SeedDataAsync(ReLoopDbContext dbContext, CancellationToken cancellationToken)
    {
        try
        {
            if (await dbContext.Users.AnyAsync(cancellationToken))
                return;

            // Create admin
            var admin = User.CreateAdmin("admin@reloop.com", "Admin", "User", "Admin123!");

            // Create users
            var user1 = User.CreateUser("jan.kowalski@email.com", "Jan", "Kowalski", "User123!");
            var user2 = User.CreateUser("anna.nowak@email.com", "Anna", "Nowak", "User123!");

            dbContext.Users.AddRange(admin, user1, user2);
            await dbContext.SaveChangesAsync(cancellationToken);
            Console.WriteLine("Users seeded successfully.");

            var currentDir = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current directory: {currentDir}");

            var seedPath = Path.GetFullPath(Path.Combine(currentDir, "..", "..", "..", "seed"));
            Console.WriteLine($"Looking for seed folder at: {seedPath}");

            if (!Directory.Exists(seedPath))
            {
                Console.WriteLine($"Seed folder not found! Trying alternative path...");
                seedPath = Path.GetFullPath(Path.Combine(currentDir, "..", "..", "seed"));
                Console.WriteLine($"Trying: {seedPath}");
            }

            if (!Directory.Exists(seedPath))
                return;

            // Create items
            var items = new List<Item>
            {
                Item.Create("Nike Jordan 4 Retro Thunder", "Classic Jordan 4 sneakers in Thunder colorway, size 42, excellent condition",
                    await File.ReadAllBytesAsync(Path.Combine(seedPath, "thumb_xlarge_1696715965Jordan_4_Retro_Thunder_2.jpg"), cancellationToken),
                    850m, ItemCategory.Clothes, user1.Id),

                Item.Create("Nike Air Jordan 1", "Original Air Jordan 1, size 43, slightly used",
                    await File.ReadAllBytesAsync(Path.Combine(seedPath, "jordan.png"), cancellationToken),
                    650m, ItemCategory.Clothes, user1.Id),

                Item.Create("Michael Jordan Hoodie", "Black hoodie with Michael Jordan print, size L, brand new",
                    await File.ReadAllBytesAsync(Path.Combine(seedPath, "MESKA-BLUZA-MICHAEL-JORDAN-Z-KAPTUREM-CZARNA-color000000.jpg"), cancellationToken),
                    120m, ItemCategory.Clothes, user2.Id),

                Item.Create("Gaming Laptop ASUS ROG", "Powerful gaming laptop, RTX 3070, 16GB RAM, 512GB SSD",
                    await File.ReadAllBytesAsync(Path.Combine(seedPath, "laptop.jpg"), cancellationToken),
                    3500m, ItemCategory.Electronics, user1.Id),

                Item.Create("Logitech G920 Racing Wheel", "Racing wheel for PC and Xbox, excellent for sim racing",
                    await File.ReadAllBytesAsync(Path.Combine(seedPath, "Kierownica-LOGITECH-G920-PC-XBOX-ONE-front-1x.jpg"), cancellationToken),
                    800m, ItemCategory.Electronics, user2.Id),

                Item.Create("Mechanical Gaming Keyboard", "RGB mechanical keyboard, Cherry MX switches, like new",
                    await File.ReadAllBytesAsync(Path.Combine(seedPath, "klawa.jpg"), cancellationToken),
                    250m, ItemCategory.Electronics, user1.Id),

                Item.Create("Metallica - Master of Puppets CD", "Original CD, collector's edition, mint condition",
                    await File.ReadAllBytesAsync(Path.Combine(seedPath, "depositphotos_188822810-stock-photo-metallica-master-of-puppets-cd.jpg"), cancellationToken),
                    45m, ItemCategory.Music, user2.Id),

                Item.Create("George Orwell - 1984", "Classic dystopian novel, paperback, good condition",
                    await File.ReadAllBytesAsync(Path.Combine(seedPath, "orwell.jpg"), cancellationToken),
                    25m, ItemCategory.Books, user1.Id),

                Item.Create("George Orwell - Animal Farm", "Political satire classic, hardcover edition",
                    await File.ReadAllBytesAsync(Path.Combine(seedPath, "folwark.jpg"), cancellationToken),
                    30m, ItemCategory.Books, user2.Id),

                Item.Create("Vintage Collectible Figure", "Rare collectible figure, limited edition",
                    await File.ReadAllBytesAsync(Path.Combine(seedPath, "351667a.jpg"), cancellationToken),
                    150m, ItemCategory.Collectibles, user1.Id)
            };

            dbContext.Items.AddRange(items);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding database: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}