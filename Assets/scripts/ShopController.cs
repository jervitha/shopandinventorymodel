using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(ShopView))]
public class ShopController : GenericSingleton<ShopController>
{
   
    public ShopModel shopModel;
    private ShopView shopView;
    
    public GameObject itemPrefab;
    public Transform itemParent;
    public float Money;





    private void Start()
    {
        shopModel = GetComponent<ShopModel>();
        shopView = GetComponent<ShopView>();

        
    
    }

    public void OnArmourClicked()
    {
        UpdateShop(ItemType.Armours);
    }

    public void OnConsumablesClicked()
    {
        UpdateShop(ItemType.Consumables);
    }

    public void OnMaterialClicked()
    {
        UpdateShop(ItemType.Materials);
    }

    public void OnAllClicked()
    {
        UpdateShop(ItemType.All);
    }

   public void OnWeaponClicked()
    {
        UpdateShop(ItemType.Weapons);
    }

    private void UpdateShop(ItemType itemType)
    {
       
        List<ItemSo> itemsToDisplay = new List<ItemSo>();

        if (itemType==ItemType.All)
        {
            itemsToDisplay.AddRange(shopModel.items);
        }

            foreach (ItemSo itemSo in shopModel.items)
            {
            if(itemType==itemSo.itemType)
            {
                itemsToDisplay.Add(itemSo);
            }
            
        }
        InstantiateItems(itemsToDisplay);

    }

    public void InstantiateItems(List<ItemSo> itemsToDisplay)
    {
    
        foreach (Transform child in itemParent)
        {
            Destroy(child.gameObject);
        }

        
        foreach (ItemSo itemSo in itemsToDisplay)
        {
            GameObject newItem = Instantiate(itemPrefab, itemParent);
            ItemDisplay itemDisplay = newItem.GetComponent<ItemDisplay>();
            itemDisplay.UpdateItem(itemSo,itemSo.maxStackSize);
        }
    }

   


}
