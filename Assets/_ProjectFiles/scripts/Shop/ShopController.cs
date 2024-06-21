using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController
{
   
    private ShopModel shopModel;
    private ShopView shopView;
    
   
    
    private TMP_InputField itemCountField;
   
            

    public  ShopController(ShopModel _shopModel, ShopView _shopView, TMP_InputField _itemCountField)
    {
        shopModel = _shopModel;
        shopView = _shopView;
        itemCountField = _itemCountField;
        shopView.UpdateMoneyText(shopModel.money);
        itemCountField.onValueChanged.AddListener(OnBuySellItemCount);
        shopView.UpdateBuySellCountInputField(shopModel.buySellItemCount);

    }

   

    public void BuyItems()
    {
        ItemSo _selecteditemSo = InventoryManager.Instance.selectedItemSo;

        if (!int.TryParse(itemCountField.text, out int count))
        {

            return;
        }

        float TotalPrice = _selecteditemSo.price * count;
        if (TotalPrice <= shopModel.money)
        {
            ToolTipManager.Instance.ShowTooltip("You bought :" + count + " " + _selecteditemSo.itemName);
            shopModel.money -= TotalPrice;
            shopView.UpdateMoneyText(shopModel.money);
            InventoryManager.Instance.GetItem(_selecteditemSo, count);


        }
        else
        {
            ToolTipManager.Instance.ShowTooltip("Items are not sufficient to Buy");
        }


    }
    public void SellItems()
    {
        ItemSo _selecteditemSo = InventoryManager.Instance.selectedItemSo;
        if (!int.TryParse(itemCountField.text, out int count))
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
        shopModel.money += TotalPrice;
        shopView.UpdateMoneyText(shopModel.money);
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

    public void AddBuySellItems()
    {
        shopModel.buySellItemCount++;
        shopView.UpdateBuySellCountInputField(shopModel.buySellItemCount);
    }

    public void SubtractBuySellItems()
    {
       if(shopModel.buySellItemCount>1)
        {
            shopModel.buySellItemCount--;
            shopView.UpdateBuySellCountInputField(shopModel.buySellItemCount);
        }
       
    }
    private void OnBuySellItemCount(string str)
    {
        if (int.TryParse(str, out int count))
        {
            shopModel.buySellItemCount = count;
            shopView.UpdateBuySellCountInputField(shopModel.buySellItemCount);
        }
    }
   


}
