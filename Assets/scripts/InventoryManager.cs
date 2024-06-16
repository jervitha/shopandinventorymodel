using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText, rarityText, priceText;
    [SerializeField] private TextMeshProUGUI weightText;
    [SerializeField] private TextMeshProUGUI confirmationText;




    [Header("Buttons")]
    [SerializeField] private Button buyButton, sellButton;
    [SerializeField] private Button AddItemCount, SubstractItemCount;
    [SerializeField] private TMP_InputField ItemCountInputField;
    [SerializeField] private GameObject itemDescription;
    [SerializeField] private List<ItemDisplay> items;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemParent;
    [SerializeField] private GameObject confirmationParent;

    public ItemSo SelecteditemSo;
    private ItemSo[] itemSoArray;
    [SerializeField] private Button AddButton;
    [SerializeField] private Button SubtractButton;
    [SerializeField] private Button YesButtonConfirmation;
    [SerializeField] private Button NoButtonConfirmation;

    [Header("weight")]
    [SerializeField] private float maximumWeight;
     private float currentWeight = 0;
    private bool isBuying;

    private void Start()
    {
        confirmationParent.SetActive(false);
        itemDescription.SetActive(false);
        weightText.text= currentWeight.ToString() + "/" + maximumWeight.ToString();
    

}
    private void OnEnable()
    {
        Actions.OnitemSelected += OnItemPressed;
        itemSoArray=Resources.LoadAll<ItemSo>("itemSO");
        buyButton.onClick.AddListener(delegate  { isBuying = true; ConfirmationMessage(); });
       
        sellButton.onClick.AddListener(delegate { isBuying = false; ConfirmationMessage(); });
        AddButton.onClick.AddListener(AddBuySellItems);
        SubtractButton.onClick.AddListener(SubtractBuySellItems);
        YesButtonConfirmation.onClick.AddListener(YesConfirmtionButton);
        NoButtonConfirmation.onClick.AddListener(NoConfirmtionButton);



    }
    private void OnDisable()
    {
        Actions.OnitemSelected -= OnItemPressed;
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
    private void GetItem(ItemSo _itemSo, int count)
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

    public void BuyItems()
    {
      
        
       if(!int.TryParse(ItemCountInputField.text,out int count))
        {
            
            return;
        }

       float TotalPrice=SelecteditemSo.price*count;
        if(TotalPrice<=ShopController.Instance.Money)
        {
            ToolTipManager.Instance.ShowTooltip("You bought :"+count +" "+SelecteditemSo.itemName);
            ShopView.Instance.UpdateUiMoneyAmount(ShopController.Instance.Money-TotalPrice);
            GetItem(SelecteditemSo,count);


        }
        else
        {
            ToolTipManager.Instance.ShowTooltip("Items are not sufficient to Buy");
        }


    }
    public void SellItems()
    {
        if (!int.TryParse(ItemCountInputField.text, out int count))
        {
            return;
        }


        int totalItemsInInventoryMatchingType = 0;
        foreach (ItemDisplay itemDisplay in items)
        {
            if (SelecteditemSo == itemDisplay.itemSo)
            {
                totalItemsInInventoryMatchingType += itemDisplay.itemCurrentCount;
            }
        }

        if (totalItemsInInventoryMatchingType < count)
        {
            ToolTipManager.Instance.ShowTooltip("Items are not sufficient to sell");
            return;
        }
        float TotalPrice = SelecteditemSo.price * count;
        ShopView.Instance.UpdateUiMoneyAmount(ShopController.Instance.Money +TotalPrice);
        RemoveItems(SelecteditemSo,count);
    }

    private void AddBuySellItems()
    {
        int count;
       if(int.TryParse(ItemCountInputField.text,out count))
        {
            count++;
           ItemCountInputField.text = count.ToString();
        }
       else
        {
            ItemCountInputField.text = "1";
        }
    }
    private void SubtractBuySellItems()
    {
        int count;
        if (int.TryParse(ItemCountInputField.text, out count))
        {
         
            if (count>1)
            {
                count--;
                ItemCountInputField.text = count.ToString();
            }
        }
        else
        {
            ItemCountInputField.text = "1";
        }
    }
    void RemoveItems(ItemSo itemSo,int  count)
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


    void ConfirmationMessage()
    {
     
        
       if (int.TryParse(ItemCountInputField.text, out int count))
        {
            confirmationParent.SetActive(true);
            if (isBuying)
            {

                confirmationText.text = "Are you sure to buy" +count+ SelecteditemSo.itemName;
            }
            else
            {
                confirmationText.text = "Are you sure to sell" +count + SelecteditemSo.itemName;
            }
        }
    }

    void NoConfirmtionButton()
    {
        confirmationParent.SetActive(false);
        SoundManager.Instance.PlaySound(SoundType.Cancellation);
    }
    void YesConfirmtionButton()
    {
        confirmationParent.SetActive(false);
        SoundManager.Instance.PlaySound(SoundType.confirmation);
        if (isBuying)
        {
            BuyItems();
            
        }
        else
        {
            SellItems();
           
        }

    }
} 