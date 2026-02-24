using LumenForgeServer.IntegrationTests.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace LumenForgeServer.IntegrationTests.Client;

public class KcOptions
{

    public KeycloakOptions KeycloakOptions { get; init; }
    public string AdminPass { get; init; }
    public string AdminUser { get; init; }
    
    public string AdminRealm { get; init; }

    private KcOptions() { }

    public static KcOptions FromEnvironment()
    {
        return new KcOptions
        {
            KeycloakOptions = new KeycloakOptions
            {
                BaseUrl = Environment.GetEnvironmentVariable("KC_BASEURL") ?? "http://localhost:8080",
                Realm = Environment.GetEnvironmentVariable("KC_REALM") ?? "lumenforge-realm",
                ClientId = Environment.GetEnvironmentVariable("KC_CLIENTID") ?? "lumenforge-test",
                Username = Environment.GetEnvironmentVariable("KC_USER") ?? "alice",
                Password = Environment.GetEnvironmentVariable("KC_PASS") ?? "alice123",
            },
            AdminRealm = Environment.GetEnvironmentVariable("KC_ADMIN_REALM") ?? "master",
            AdminUser = Environment.GetEnvironmentVariable("KC_ADMIN_USER") ?? "admin",
            AdminPass = Environment.GetEnvironmentVariable("KC_ADMIN_PASS") ?? "adminpassword"
        };        
    }
}

