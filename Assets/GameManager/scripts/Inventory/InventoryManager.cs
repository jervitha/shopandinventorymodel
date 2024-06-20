using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Random=UnityEngine.Random;
public class InventoryManager : GenericSingleton<InventoryManager>
{
    public  Action<ItemDisplay> OnitemSelected;
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI priceText; 
    [SerializeField] private TextMeshProUGUI weightText;
    




    [Header("Buttons")]
  
    [SerializeField] private Button AddItemCount;
    [SerializeField] private Button SubstractItemCount;
     public TMP_InputField ItemCountInputField;
    [SerializeField] private GameObject itemDescription;
    [HideInInspector]public List<ItemDisplay> items;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemParent;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;

    public ItemSo SelecteditemSo;
    private ItemSo[] itemSoArray;
   

    [Header("weight")]
    [SerializeField] private float maximumWeight;
     private float currentWeight = 0;
    

    private void Start()
    {
       
        itemDescription.SetActive(false);
        weightText.text= currentWeight.ToString() + "/" + maximumWeight.ToString();
    

}
    private void OnEnable()
    {
        OnitemSelected += OnItemPressed;
        itemSoArray=Resources.LoadAll<ItemSo>("itemSO");
        
       
      



    }
    private void OnDisable()
    {
       OnitemSelected -= OnItemPressed;
    }

   
    private void OnItemPressed(ItemDisplay itemDisplay)
    {
        SelecteditemSo = itemDisplay.itemSo;
        if (itemDisplay.transform.parent == itemParent)
        {
            sellButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
        }
        else
        {
            sellButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
        }

            
            itemDescription.SetActive(true);
            nameText.text = itemDisplay.itemSo.name;
            rarityText.text = itemDisplay.itemSo.itemRarity.ToString();
            priceText.text = itemDisplay.itemSo.price.ToString();
        

    }
   public void GetItem(ItemSo _itemSo, int count)
    {
        
        int itemIndex = -1;
        int i = 0;
        foreach (ItemDisplay itemDisplay in items)
        {
            bool isStackFull = itemDisplay.itemCurrentCount >= itemDisplay.itemSo.maxStackSize;


            if (itemDisplay.itemSo == _itemSo && !isStackFull) 
            {

                itemIndex = i;
            }

            i++;
        }
  

        if (itemIndex == -1)
        {
           
            GameObject newItem = Instantiate(itemPrefab, itemParent);
            newItem.gameObject.GetComponent<ItemDisplay>().UpdateItem(_itemSo, count);
            items.Add(newItem.GetComponent<ItemDisplay>());
        }
        else
        {
            ItemDisplay currentItem = items[itemIndex];
            if (currentItem.itemCurrentCount + count <= currentItem.itemSo.maxStackSize)
            {

                items[itemIndex].UpdateItem(_itemSo, count + currentItem.itemCurrentCount);

            }
            else
            {
                int itemsToFillStack=currentItem.itemSo.maxStackSize-currentItem.itemCurrentCount;
                items[itemIndex].UpdateItem(_itemSo,currentItem.itemSo.maxStackSize);
                GetItem(_itemSo, count - itemsToFillStack);

            }
        }
        weightText.text = currentWeight.ToString() + "/" + maximumWeight.ToString();
    }

    public void GatherRandomItems()
    {
        int stacksToadd = Random.Range(2, 4);
        for(int stack=0;stack<stacksToadd-1;stack++)
        {
            int itemsToAdd = Random.Range(2, 5);
            int itemId = Random.Range(0, itemSoArray.Length);
           if (CanItemFitInTheInventory(itemSoArray[itemId], itemsToAdd))
                GetItem(itemSoArray[itemId],itemsToAdd);
           
        }
       
    }

    private bool CanItemFitInTheInventory(ItemSo itemSo,int count)
    {
        currentWeight = 0;
        foreach(ItemDisplay itemDisplay in items)
        {
            currentWeight += itemDisplay.itemSo.weight *itemDisplay.itemCurrentCount;

        }
        float weightAfterAddingItem;
        weightAfterAddingItem = currentWeight + itemSo.weight * count;

        if(weightAfterAddingItem>maximumWeight)
        {
            return false;
        }
        else
        {
            currentWeight = weightAfterAddingItem;
            return true;
        }
        
    }

   
     public void RemoveItems(ItemSo itemSo,int  count)
    {
        List<ItemDisplay> itemDisplayList = new List<ItemDisplay>();
        int totalItemsInInventoryMatchingType=0;
        foreach (ItemDisplay itemDisplay in items)
        {
          
            if(itemSo==itemDisplay.itemSo)
            {
                itemDisplayList.Add(itemDisplay);
                totalItemsInInventoryMatchingType += itemDisplay.itemCurrentCount;
            }
           

        }
       if(totalItemsInInventoryMatchingType<count)
        {
            ToolTipManager.Instance.ShowTooltip("Items are not sufficient to sell");

            return;
        }
        else
        {
            
        }
       
        ItemDisplay itemDisplayLast = itemDisplayList[itemDisplayList.Count - 1];
        if (itemDisplayLast.itemCurrentCount>count)
        {
            itemDisplayLast.UpdateItem(itemSo, itemDisplayLast.itemCurrentCount - count);
        }
        else if(itemDisplayLast.itemCurrentCount == count)
        {
            items.Remove(itemDisplayLast);
            Destroy(itemDisplayLast.gameObject);
           
        }
        else
        {
            items.Remove(itemDisplayLast);
            Destroy(itemDisplayLast.gameObject);
            
            RemoveItems(itemSo, count-itemDisplayLast.itemCurrentCount);
        }
    }


    
   
} 