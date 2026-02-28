using System.Text.Json.Serialization;

namespace LumenForgeServer.Auth.Domain;

public record KcUser
{
        [JsonPropertyName("username")] public string Username;
        [JsonPropertyName("password")] public string Password;
        [JsonPropertyName("email")] public string Email;
        [JsonPropertyName("firstname")] public string FirstName;
        [JsonPropertyName("lastname")] public string LastName;
        [JsonPropertyName("groups")] public string[] Groups;
        [JsonPropertyName("realm_roles")] public string[] RealmRoles;
    }