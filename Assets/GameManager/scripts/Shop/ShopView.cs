using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ShopView:GenericSingleton<ShopView>
{

    [SerializeField] private TextMeshProUGUI priceText;
    public ShopController shopController;
    private ShopModel shopModel;
    [SerializeField] private Button itemtypeButtonArmour;
    [SerializeField] private Button itemtypeButtonconsumables;
    [SerializeField] private Button itemtypeButtonMaterial;
    [SerializeField] private Button itemtypeButtonWeapons;
    [SerializeField] private Button itemtypeButtonAll;
    [SerializeField] private Transform itemParent;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] TMP_InputField ItemCountInputField;
    [SerializeField] private Button AddButton;
    [SerializeField] private Button SubtractButton;



    private void Start()
    {
        shopModel = new ShopModel();
        shopModel.items = Resources.LoadAll<ItemSo>("itemSO");
        shopController = new ShopController(shopModel,this, ItemCountInputField);
       
       

    }
    private void OnEnable()
    {
       
        itemtypeButtonArmour.onClick.AddListener(delegate { shopController.UpdateShop(ItemType.Armours); });
        itemtypeButtonconsumables.onClick.AddListener(delegate { shopController.UpdateShop(ItemType.Consumables); });
        itemtypeButtonMaterial.onClick.AddListener(delegate { shopController.UpdateShop(ItemType.Materials); });
        itemtypeButtonWeapons.onClick.AddListener(delegate { shopController.UpdateShop(ItemType.Weapons); });
        itemtypeButtonAll.onClick.AddListener(delegate{shopController.UpdateShop(ItemType.All); });

        AddButton.onClick.AddListener(AddBuySellItems);
        SubtractButton.onClick.AddListener(SubtractBuySellItems);



    }

    private void OnDisable()
    {
        itemtypeButtonArmour.onClick.RemoveAllListeners();
        itemtypeButtonconsumables.onClick.RemoveAllListeners();
        itemtypeButtonMaterial.onClick.RemoveAllListeners();
        itemtypeButtonWeapons.onClick.RemoveAllListeners();
        itemtypeButtonAll.onClick.RemoveAllListeners();
    }

    public void UpdateMoneyText(float money)
    {
        priceText.text= money.ToString();

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
            itemDisplay.UpdateItem(itemSo, itemSo.maxStackSize);
        }
    }


    private void AddBuySellItems()
    {
        int count;
        if (int.TryParse(ItemCountInputField.text, out count))
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

            if (count > 1)
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
}
