using Microsoft.Identity.Client.TelemetryCore.TelemetryClient;
using System.Runtime.InteropServices;

namespace ECommerce.Helpers
{
    public enum PurchaseError
    {
        None,
        NotFound,
        InsufficientBalance,
        DatabaseError
    }
    public record PurchaseResult<TData>(PurchaseError Error, string? Message = null, TData? Data = default);
}
