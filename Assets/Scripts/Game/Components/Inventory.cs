using Unity.Collections;
using Unity.Entities;

public struct InventoryComponent : IComponentData
{
    public int inventorySize;
    public NativeList<Item> items;
}
public struct Item
{
    public Hash128 type;
    public int count;
}

/// <summary>
/// Points to entities that contain <see cref="InventoryContentComponent"/> 
/// </summary>
public struct TransferItemEventComponent : IComponentData
{
    public Entity from;
    public int fromIndex;
    public Entity to;
    public int toIndex;
    public int count;
}