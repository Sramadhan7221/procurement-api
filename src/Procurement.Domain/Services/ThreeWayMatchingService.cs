using Procurement.Domain.Enums;

namespace Procurement.Domain.Services;

public record MatchingInput(
    Guid ProcurementItemId,
    string ItemName,
    decimal OrderedQuantity,
    decimal ReceivedQuantity,
    decimal InvoicedQuantity,
    decimal ExpectedUnitPrice,
    decimal InvoicedUnitPrice);

public record DiscrepancyDetail(
    string ItemName,
    decimal OrderedQuantity,
    decimal ReceivedQuantity,
    decimal InvoicedQuantity,
    decimal ExpectedUnitPrice,
    decimal InvoicedUnitPrice,
    DiscrepancyType DiscrepancyType);

public record MatchingResult(bool IsMatched, IReadOnlyList<DiscrepancyDetail> Discrepancies);

public static class ThreeWayMatchingService
{
    public static MatchingResult Match(IEnumerable<MatchingInput> items, decimal priceTolerance = 0m)
    {
        var discrepancies = new List<DiscrepancyDetail>();

        foreach (var item in items)
        {
            var quantityMismatch = item.InvoicedQuantity > item.ReceivedQuantity;
            var priceDiff = Math.Abs(item.InvoicedUnitPrice - item.ExpectedUnitPrice);
            var priceMismatch = priceDiff > priceTolerance;

            if (!quantityMismatch && !priceMismatch)
                continue;

            var type = (quantityMismatch, priceMismatch) switch
            {
                (true, true) => DiscrepancyType.Both,
                (true, false) => DiscrepancyType.QuantityMismatch,
                _ => DiscrepancyType.PriceMismatch,
            };

            discrepancies.Add(new DiscrepancyDetail(
                item.ItemName,
                item.OrderedQuantity,
                item.ReceivedQuantity,
                item.InvoicedQuantity,
                item.ExpectedUnitPrice,
                item.InvoicedUnitPrice,
                type));
        }

        return new MatchingResult(discrepancies.Count == 0, discrepancies.AsReadOnly());
    }
}
