using A_AspNetCore;
using A_AspNetCore.Data;
using A_AspNetCore.Models;
using A_AspNetCore.Models.Users;
using EFCore.ChangeTriggers.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<SampleDbContext>(options =>
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("SampleDbContext") ?? throw new InvalidOperationException("Connection string 'SampleDbContext' not found."))
        .UseSqlServerChangeTriggers(options =>
            options
                .UseChangedBy<SampleChangedByProvider, User>()
                .UseChangeSource<SampleChangeSourceProvider, ChangeSource>()));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();

dbContext.Database.Migrate();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
