public class WarehouseTransaction
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }

    public enum TransactionType
    {
        Ingoing,
        Outgoing
    }
}