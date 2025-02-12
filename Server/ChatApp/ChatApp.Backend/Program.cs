using ChatApp.Backend.Api.Middleware;
using ChatApp.Backend.Core.Authentication;
using ChatApp.Backend.Core.Users;
using ChatApp.Backend.Infrastructure.Data;
using FirebaseAdmin;
using FluentValidation;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

FirebaseApp.Create(
    new AppOptions()
    {
        Credential = GoogleCredential.FromFile("config/firebase-service-account.json"),
    }
);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseSqlServer(connectionString: connectionString)
);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IValidator<RegisterData>, UserValidator>();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddAuthorization();
builder.Services.AddSignalR();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<AuthMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
