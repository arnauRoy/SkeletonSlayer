using System;
using System.Collections.Generic;
public class Inventory
{
    private readonly Dictionary<ItemData, int> items = new();

    public event Action<ItemData, int> Changed;      // (qué objeto, nueva cantidad)

    public int Count(ItemData item)
    {
        if (item == null) return 0;
        return items.TryGetValue(item, out int quantity) ? quantity : 0;
    }
    public int FreeSpace(ItemData item)
    {
        if (item == null) return 0;
        return Math.Max(0, item.maxStack - Count(item));
    }
    
    public int Add(ItemData item, int amount)
    {
        if (item == null || amount <= 0) return 0;

        int toAdd = Math.Min(amount, FreeSpace(item));
        
        if (toAdd == 0) return 0;

        items[item] = Count(item) + toAdd;      // el indexador crea o actualiza
        Changed?.Invoke(item, items[item]);
        return toAdd;
    }      // devuelve cuántos ha podido guardar
    public bool TryConsume(ItemData item, int amount = 1)
    {
        if(Count(item)<amount)return false;

        int remaining = Count(item) - amount;
        if (remaining > 0)
        {
            items[item] = remaining;
            Changed?.Invoke(item, remaining);
            return true;
        }
        else
        {
            items.Remove(item);
            Changed?.Invoke(item, 0);
            return true;

        }
    }
}
