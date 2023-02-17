using System;
using Web.Middleware;

namespace Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.UseCustomAuthMiddleware();
        app.Run(async context =>
        {
            await context.Response.WriteAsync("" + context.Request.HttpContext.Items["userdetails"]?.ToString());
        });
        app.Run();
    }
}
