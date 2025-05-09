using Microsoft.EntityFrameworkCore;
using MyRazorApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages ve oturum servislerini ekle
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// SQL Server bağlantısı (appsettings.json içindeki connection string kullanılmalı)
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDbConnection")));

var app = builder.Build();

// HTTP istekleri için middleware yapılandırması
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();       // Oturum yönetimi
app.UseAuthorization(); // Yetkilendirme (gerekirse)

// Razor Pages için endpoint yönlendirme
app.MapRazorPages();

app.Run(); // Uygulamayı başlat
