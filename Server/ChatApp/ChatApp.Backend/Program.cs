using System.Text.Json;
using System.Text.Json.Serialization;
using ChatApp.Backend.Api.Hubs;
using ChatApp.Backend.Api.Middleware;
using ChatApp.Backend.Core.Authentication;
using ChatApp.Backend.Core.Conversations;
using ChatApp.Backend.Core.Enums;
using ChatApp.Backend.Core.Messages;
using ChatApp.Backend.Core.Users;
using ChatApp.Backend.Infrastructure.Data;
using FirebaseAdmin;
using FluentValidation;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;

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
builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter<MessageStatus>(JsonNamingPolicy.CamelCase)
        );
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter<ConversationType>(JsonNamingPolicy.CamelCase)
        );
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IConversationService, ConversationService>();
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
app.MapHub<ChatHub>("/chat");
app.MapControllers();

app.Run();
