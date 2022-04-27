﻿using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Tod.Domain;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;
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

// services.AddSwaggerGen();

// Building web application and adding all necessary middlewares

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();