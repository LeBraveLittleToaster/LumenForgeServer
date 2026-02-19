using Microsoft.AspNetCore.Mvc.Testing;

namespace LumenForgeServer.IntegrationTests.Auth;

public class AuthUserTest: IClassFixture<WebApplicationFactory<Program>>
{
    
    
    [Fact]
    public async Task GET_user_returns_ok()
    {
        
    }
}