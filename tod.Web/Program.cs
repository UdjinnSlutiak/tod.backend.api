using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Tod.Domain;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Implementations.Sql;
using Tod.Domain.Repositories.Realisations.Sql;
using Tod.Services.Abstractions;
using Tod.Services.Implementations;
using Tod.Services.Jwt;
using Tod.Services.Redis;

var builder = WebApplication.CreateBuilder(args);

// Adding all necessary services into web project

var services = builder.Services;

services.AddDbContext<ProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

services.AddJwtTokenConfiguration(out var jwtTokenConfiguration);

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtTokenConfiguration.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtTokenConfiguration.Audience,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfiguration.Secret))
    };
});

services.AddRedisConfiguration(out var redisConfig);

services.AddStackExchangeRedisCache(options =>
{
    options.InstanceName = redisConfig.InstanceName;
    options.ConfigurationOptions = new ConfigurationOptions
    {
        AllowAdmin = true,
        EndPoints =
        {
            {
                redisConfig.Address,
                redisConfig.Port
            }
        },
        Password = redisConfig.Password,
        ConnectRetry = 5,
        ReconnectRetryPolicy = new LinearRetry(1500),
        AbortOnConnectFail = false,
        ConnectTimeout = 5000,
        SyncTimeout = 5000
    };
});

services.AddControllers();

services.AddTransient<IRepository<User>, SqlBaseRepository<User>>();
services.AddTransient<IRepository<Topic>, SqlBaseRepository<Topic>>();
services.AddTransient<IRepository<Commentary>, SqlBaseRepository<Commentary>>();
services.AddTransient<IRepository<UserTopic>, SqlBaseRepository<UserTopic>>();
services.AddTransient<IRepository<TopicTag>, SqlBaseRepository<TopicTag>>();

services.AddTransient<IUserRepository, SqlUserRepository>();
services.AddTransient<ITopicRepository, SqlTopicRepository>();
services.AddTransient<ICommentaryRepository, SqlCommentaryRepository>();
services.AddTransient<IReactionRepository, SqlReactionRepository>();
services.AddTransient<ITagRepository, SqlTagRepository>();
services.AddTransient<IUserTopicRepository, SqlUserTopicRepository>();
services.AddTransient<ITopicTagRepository, SqlTopicTagRepository>();
services.AddTransient<ITopicReactionRepository, SqlTopicReactionRepository>();
services.AddTransient<ITopicCommentaryRepository, SqlTopicCommentaryRepository>();
services.AddTransient<IUserCommentaryRepository, SqlUserCommentaryRepository>();
services.AddTransient<ICommentaryReactionRepository, SqlCommentaryReactionRepository>();

services.AddTransient<IAccountService, JwtAccountService>();
services.AddTransient<ITokenService, JwtTokenService>();
services.AddTransient<IUserService, UserService>();
services.AddTransient<ITopicService, TopicService>();
services.AddTransient<ICommentaryService, CommentaryService>();
services.AddTransient<ITagService, TagService>();
services.AddTransient<IReactionService, ReactionService>();
services.AddTransient<IRedisService, RedisService>();
services.AddTransient<IPasswordHasher, Rfc2898PasswordHasher>();

services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "tod Backend API", Version = "v1" });
});

// Building web application and adding all necessary middlewares

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "tod Backend API"));

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
