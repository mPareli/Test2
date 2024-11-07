


using Emne_7___Arbeidskrav_2.Data;
using Emne_7___Arbeidskrav_2.Features.Comments;
using Emne_7___Arbeidskrav_2.Features.Comments.Interfaces;
using Emne_7___Arbeidskrav_2.Features.Posts;
using Emne_7___Arbeidskrav_2.Features.Posts.Interfaces;
using Emne_7___Arbeidskrav_2.Features.Users;
using Emne_7___Arbeidskrav_2.Features.Users.Interfaces;
using Emne_7___Arbeidskrav_2.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Serilog;



Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build())
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<StudentBloggDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services and repositories
builder.Services.AddScoped<IUserService, UserService>();  // Example: User service
builder.Services.AddScoped<IPostService, PostService>();  // Example: Post service
builder.Services.AddScoped<ICommentService, CommentService>();  // Example: Comment service

// Configure custom authentication middleware
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, StudentBloggBasicAuthentication>("BasicAuthentication", null);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<StudentBloggExceptionHandling>();



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting the application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start correctly");
}
finally
{
    Log.CloseAndFlush(); 
}

app.Run();