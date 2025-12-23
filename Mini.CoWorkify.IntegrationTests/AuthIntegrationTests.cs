using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.IntegrationTests.Utilities;
using Shouldly;

namespace Mini.CoWorkify.IntegrationTests;

public class AuthIntegrationTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Register_Should_ReturnOk_When_DataIsValid()
    {
        var registerDto = new RegisterUserDto
        {
            Email = "newuser@test.com",
            Password = "Password123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        await ExecuteDbContextAsync(async context =>
        {
            var count = await context.Users.CountAsync(u => u.Email == registerDto.Email);
            count.ShouldBe(1);
        });
    }

    [Fact]
    public async Task Register_Should_ReturnBadRequest_When_PasswordIsWeak()
    {
        var registerDto = new RegisterUserDto
        {
            Email = "weakuser@test.com",
            Password = "123"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        var responseString = await response.Content.ReadAsStringAsync();
        responseString.ShouldContain("Password");
    }
    
    [Fact]
    public async Task Register_Should_ReturnBadRequest_When_Empty()
    {
        var registerDto = new RegisterUserDto
        {
            Email = "",
            Password = ""
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        var responseString = await response.Content.ReadAsStringAsync();
        
        responseString.ShouldContain("Email should not be empty");
        responseString.ShouldContain("Email address is not valid");
        responseString.ShouldContain("Password should not be empty");
    }
    
    [Fact]
    public async Task Login_Should_ReturnOk_When_UserExists()
    {
        var registerDto = new RegisterUserDto
        {
            Email = "newuser@test.com",
            Password = "Password123!"
        };
        
        var loginDto = new LoginUserDto()
        {
            Email = "newuser@test.com",
            Password = "Password123!"
        };
        
        await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var content = await response.Content.ReadFromJsonAsync<JsonObject>();
    
        content?["token"]?.ToString().ShouldNotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task Login_Should_ReturnUnauthorized_When_UserDoesNotExist()
    {
        var loginDto = new LoginUserDto()
        {
            Email = "nonexist@test.com",
            Password = "123"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Login_Should_ReturnBadRequest_When_Empty()
    {
        var loginDto = new LoginUserDto
        {
            Email = "",
            Password = ""
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        var responseString = await response.Content.ReadAsStringAsync();
        
        responseString.ShouldContain("Email should not be empty");
        responseString.ShouldContain("Email address is not valid");
        responseString.ShouldContain("Password should not be empty");
    }
}