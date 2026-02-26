using NodaTime;

namespace LumenForgeServer.Rentals.Domain;

/// <summary>
/// Represents a generated report summarizing the outcome of a rental.
/// </summary>
public class RentalReport
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }

    public long RentalId { get; set; }
    public Rental Rental { get; set; } = null!;

    public Instant GeneratedAt { get; set; }
    public string? GeneratedByUserId { get; set; } // Keycloak user id

    public string? ReportSummary { get; set; }
    public string? ReportDocumentUrl { get; set; }

    public Instant CreatedAt { get; set; }
    public Instant UpdatedAt { get; set; }
}
