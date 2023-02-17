using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Web.Middleware;
namespace Middleware.Tests;
public class AuthTests : IAsyncLifetime
{
    IHost? host;
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    public async Task InitializeAsync()
    {
        host = await new HostBuilder()
        .ConfigureWebHost(webBuilder =>
        {
            webBuilder
    .UseTestServer()
    .ConfigureServices(services =>
    {
        })
    .Configure(app =>
        {
            app.UseCustomAuthMiddleware();
            app.Run(async context =>
            {
                await context.Response.WriteAsync("" + context.Request.HttpContext.Items["userdetails"]?.ToString());
            });
        });
        })
        .StartAsync();
    }

    [Fact]
    public async Task MiddlewareTestNoCredentials()
    {
        var response = await host.GetTestClient().GetAsync("/");
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Not authorized.", result);
    }

    [Fact]
    public async Task MiddlewareTestAuthenticated()
    {
        var response = await host.GetTestClient().GetAsync("/?username=user1&password=password1");
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Authorized.", result);
    }

    [Fact]
    public async Task MiddlewareTestNoPassword()
    {
        var response = await host.GetTestClient().GetAsync("/?username=user1");
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Not authorized.", result);
    }

    [Fact]
    public async Task MiddlewareTestWrongCredentials()
    {
        var response = await host.GetTestClient().GetAsync("/?username=user5&password=password2");
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Not authorized.", result);
    }
}