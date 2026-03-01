using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Dto.Command;
using LumenForgeServer.Auth.Dto.Views;
using LumenForgeServer.Common;
using LumenForgeServer.IntegrationTests.Collections;
using LumenForgeServer.IntegrationTests.Fixtures;
using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.TestSupport;

namespace LumenForgeServer.IntegrationTests.Auth;

/// <summary>
/// Integration tests for group membership and role assignment endpoints.
/// </summary>
[Collection(AuthCollection.Name)]
public class AssignUsersToGroupTests(AuthFixture fixture)
{
    [Fact]
    public async Task Assign_user_to_group_and_query_membership()
    {

    }

    [Fact]
    public async Task Assign_user_to_group_twice_returns_conflict()
    {
      
    }

    [Fact]
    public async Task Remove_user_from_group_then_not_listed()
    {
        
    }

    [Fact]
    public async Task Assign_role_to_group_and_query_roles()
    {
        
    }

    [Fact]
    public async Task User_roles_include_group_roles()
    {
        
    }

    [Fact]
    public async Task Assign_role_twice_returns_conflict()
    {
        
    }

    [Fact]
    public async Task Remove_role_not_assigned_returns_not_found()
    {
        
    }

    [Fact]
    public async Task Assign_invalid_role_returns_bad_request()
    {
        
    }

    [Fact]
    public async Task Assign_user_not_found_returns_not_found()
    {
        
    }

}
