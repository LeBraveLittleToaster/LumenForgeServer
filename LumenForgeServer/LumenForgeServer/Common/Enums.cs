namespace LumenForgeServer.Common;

/// <summary>
/// Units used to represent stock quantities.
/// </summary>
public enum StockUnitType
{
    /// <summary>
    /// Individual unit count.
    /// </summary>
    UNIT,
    /// <summary>
    /// Weight in kilograms.
    /// </summary>
    KG,
    /// <summary>
    /// Volume in liters.
    /// </summary>
    LITER
}

/// <summary>
/// Checklist types associated with rental workflow stages.
/// </summary>
public enum ChecklistType
{
    /// <summary>
    /// Checklist completed at pickup.
    /// </summary>
    PICKUP,
    /// <summary>
    /// Checklist completed at dropoff.
    /// </summary>
    DROPOFF
}

/// <summary>
/// Supported payment methods for billing.
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// Cash payment.
    /// </summary>
    CASH,
    /// <summary>
    /// Card payment.
    /// </summary>
    CARD,
    /// <summary>
    /// Bank transfer or equivalent.
    /// </summary>
    TRANSFER,
    /// <summary>
    /// Any other payment method.
    /// </summary>
    OTHER
}
