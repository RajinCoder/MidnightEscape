using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public enum AllItems
    {
        Key
    }

    public List<AllItems> inventoryItems = new List<AllItems>();

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(AllItems item)
    {
        if (!inventoryItems.Contains(item))
        {
            inventoryItems.Add(item);
        }
    }

    public void RemoveItem(AllItems item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
        }
    }
}
