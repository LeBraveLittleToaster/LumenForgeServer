namespace LumenForgeServer.Auth;

/// <summary>
/// Authorization policy names used by the auth layer.
/// </summary>
public static class AuthConstants
{
    /// <summary>
    /// Policy name restricting endpoints to administrative users.
    /// </summary>
    public const String POLICY_ADMIN_ONLY = "IS_ADMIN_ONLY";
    /// <summary>
    /// Policy name restricting endpoints to authenticated users.
    /// </summary>
    public const String POLICY_USER_ONLY = "IS_USER_ONLY";
}
