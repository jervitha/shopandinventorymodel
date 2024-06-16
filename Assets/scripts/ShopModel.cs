using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopModel:MonoBehaviour
{

    public ItemSo[] items;
    private void OnEnable()
    {
        items = Resources.LoadAll<ItemSo>("itemSO");
    }

}


public enum ItemType
{
    Consumables,
    Weapons,
    Materials,
    Armours,
        All
}