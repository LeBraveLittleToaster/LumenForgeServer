This folder contains shared domain types (enums, etc.).

Notes:
- All Keycloak user references are stored as string IDs (Keycloak `sub`), without a local user table.
- EF Core mapping is done via Fluent API in `Persistence/AppDbContext.cs`.
