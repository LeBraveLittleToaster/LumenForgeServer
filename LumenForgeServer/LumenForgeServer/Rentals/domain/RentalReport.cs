using System;

namespace LumenForgeServer.Rentals.domain;

public class RentalReport
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }

    public long RentalId { get; set; }
    public Rental Rental { get; set; } = null!;

    public DateTimeOffset GeneratedAt { get; set; }
    public string? GeneratedByUserId { get; set; } // Keycloak user id

    public string? ReportSummary { get; set; }
    public string? ReportDocumentUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
