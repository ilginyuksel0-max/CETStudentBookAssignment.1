using CetStudentBook.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CetStudentBook.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    if (!context.Categories.Any())
    {
        var categories = new List<Category>
        {
            new Category { Name = "Literature" },
            new Category { Name = "Children & Youth" },
            new Category { Name = "History & Research" },
            new Category { Name = "Foreign Language Books" }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();
    }

    if (!context.Books.Any())
    {
        var literature = context.Categories.FirstOrDefault(c => c.Name == "Literature");
        var children = context.Categories.FirstOrDefault(c => c.Name == "Children & Youth");
        var history = context.Categories.FirstOrDefault(c => c.Name == "History & Research");
        var foreign = context.Categories.FirstOrDefault(c => c.Name == "Foreign Language Books");

        var books = new List<Book>
        {
            new Book
            {
                Name = "Crime and Punishment",
                Author = "Fyodor Dostoevsky",
                PageCount = 220,
                PublishDate = DateTime.Now,
                IsSecondHand = false,
                ImageUrl = "https://images.unsplash.com/photo-1544947950-fa07a98d237f",
                CategoryId = literature?.Id
            },
            new Book
            {
                Name = "Madonna in a Fur Coat",
                Author = "Sabahattin Ali",
                PageCount = 180,
                PublishDate = DateTime.Now,
                IsSecondHand = false,
                ImageUrl = "https://images.unsplash.com/photo-1512820790803-83ca734da794",
                CategoryId = literature?.Id
            },
            new Book
            {
                Name = "Harry Potter and the Philosopher's Stone",
                Author = "J.K. Rowling",
                PageCount = 250,
                PublishDate = DateTime.Now,
                IsSecondHand = false,
                ImageUrl = "https://images.unsplash.com/photo-1524578271613-d550eacf6090",
                CategoryId = children?.Id
            },
            new Book
            {
                Name = "The Little Prince",
                Author = "Antoine de Saint-Exupéry",
                PageCount = 140,
                PublishDate = DateTime.Now,
                IsSecondHand = false,
                ImageUrl = "https://images.unsplash.com/photo-1516979187457-637abb4f9353",
                CategoryId = children?.Id
            },
            new Book
            {
                Name = "Sapiens",
                Author = "Yuval Noah Harari",
                PageCount = 300,
                PublishDate = DateTime.Now,
                IsSecondHand = false,
                ImageUrl = "https://images.unsplash.com/photo-1495640388908-05fa85288e61",
                CategoryId = history?.Id
            },
            new Book
            {
                Name = "1984",
                Author = "George Orwell",
                PageCount = 210,
                PublishDate = DateTime.Now,
                IsSecondHand = false,
                ImageUrl = "https://images.unsplash.com/photo-1507842217343-583bb7270b66",
                CategoryId = foreign?.Id
            }
        };

        context.Books.AddRange(books);
        context.SaveChanges();
    }
}

app.Run();