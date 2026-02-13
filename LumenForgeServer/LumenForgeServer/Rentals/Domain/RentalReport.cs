namespace LumenForgeServer.Rentals.Domain;

public class RentalReport
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }

    public long RentalId { get; set; }
    public Rental Rental { get; set; } = null!;

    public DateTime GeneratedAt { get; set; }
    public string? GeneratedByUserId { get; set; } // Keycloak user id

    public string? ReportSummary { get; set; }
    public string? ReportDocumentUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
