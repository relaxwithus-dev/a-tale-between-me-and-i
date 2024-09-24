namespace ATBMI.Inventory
{
    public interface IInventorySlotFactory<T> where T : class
    {
        T CreateInventorySlot();
    }
}
