using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace VahanX.Tests.Integration.Auth;

/// <summary>
/// Test server factory that swaps SQL Server for an in-memory database.
/// Program.cs seeds auth data (roles, permissions, super admin) at startup,
/// which therefore lands in the in-memory store.
/// </summary>
public class AuthTestFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        // Route tests to a dedicated database; migrations + auth seed run on startup.
        builder.UseSetting("ConnectionStrings:DefaultConnection",
            "Server=localhost,1445;Database=VahanXDb_Test;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=True;");
    }
}
