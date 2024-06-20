using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController
{
   
    private ShopModel shopModel;
    private ShopView shopView;
    
   
    
    private TMP_InputField ItemCountField;
   
            

    public  ShopController(ShopModel _shopModel, ShopView _shopView, TMP_InputField _ItemCountField)
    {
        shopModel = _shopModel;
        shopView = _shopView;
        ItemCountField = _ItemCountField;
        shopView.UpdateMoneyText(shopModel.Money);

    }

   

    public void BuyItems()
    {
        ItemSo _selecteditemSo = InventoryManager.Instance.SelecteditemSo;

        if (!int.TryParse(ItemCountField.text, out int count))
        {

            return;
        }

        float TotalPrice = _selecteditemSo.price * count;
        if (TotalPrice <= shopModel.Money)
        {
            ToolTipManager.Instance.ShowTooltip("You bought :" + count + " " + _selecteditemSo.itemName);
            shopModel.Money -= TotalPrice;
            shopView.UpdateMoneyText(shopModel.Money);
            InventoryManager.Instance.GetItem(_selecteditemSo, count);


        }
        else
        {
            ToolTipManager.Instance.ShowTooltip("Items are not sufficient to Buy");
        }


    }
    public void SellItems()
    {
        ItemSo _selecteditemSo = InventoryManager.Instance.SelecteditemSo;
        if (!int.TryParse(ItemCountField.text, out int count))
        {
            return;
        }


        int totalItemsInInventoryMatchingType = 0;
        foreach (ItemDisplay itemDisplay in InventoryManager.Instance.items)
        {
            if (_selecteditemSo == itemDisplay.itemSo)
            {
                totalItemsInInventoryMatchingType += itemDisplay.itemCurrentCount;
            }
        }

        if (totalItemsInInventoryMatchingType < count)
        {
            ToolTipManager.Instance.ShowTooltip("Items are not sufficient to sell");
            return;
        }
        float TotalPrice = _selecteditemSo.price * count;
        shopModel.Money += TotalPrice;
        shopView.UpdateMoneyText(shopModel.Money);
        InventoryManager.Instance.RemoveItems(_selecteditemSo, count);
    }



    public void UpdateShop(ItemType itemType)
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
        shopView.InstantiateItems(itemsToDisplay);

    }

    

   


}
