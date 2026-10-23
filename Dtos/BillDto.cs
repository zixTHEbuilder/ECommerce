namespace ECommerce.Dtos
{
    public record BillItemDto
    (
        string ProductName,
        decimal Price,
        int Quantity,
        decimal Subtotal
    );
    public record BillDto
    (
        string Name,
        List<BillItemDto> Items,
        decimal TotalAmount,
        DateTime IssuedAt
    );

}
