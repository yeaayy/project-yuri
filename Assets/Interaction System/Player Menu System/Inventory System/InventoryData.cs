
public class InventoryData
{
    public ItemSO item { get; private set; }
    public int quantity;

    public InventoryData(ItemSO item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
