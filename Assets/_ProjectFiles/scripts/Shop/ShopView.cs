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
    [SerializeField] TMP_InputField itemCountInputField;
    [SerializeField] private Button addButton;
    [SerializeField] private Button subtractButton;



    private void Start()
    {
        shopModel = new ShopModel();
        shopModel.items = Resources.LoadAll<ItemSo>("itemSO");
        shopController = new ShopController(shopModel,this, itemCountInputField);
        addButton.onClick.AddListener(shopController.AddBuySellItems);
        subtractButton.onClick.AddListener(shopController.SubtractBuySellItems);




    }
    private void OnEnable()
    {
       
        itemtypeButtonArmour.onClick.AddListener(delegate { shopController.UpdateShop(ItemType.Armours); });
        itemtypeButtonconsumables.onClick.AddListener(delegate { shopController.UpdateShop(ItemType.Consumables); });
        itemtypeButtonMaterial.onClick.AddListener(delegate { shopController.UpdateShop(ItemType.Materials); });
        itemtypeButtonWeapons.onClick.AddListener(delegate { shopController.UpdateShop(ItemType.Weapons); });
        itemtypeButtonAll.onClick.AddListener(delegate{shopController.UpdateShop(ItemType.All); });
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
            
           ItemDisplay itemDisplay = Instantiate(itemPrefab, itemParent).GetComponent<ItemDisplay>();
          
            itemDisplay.UpdateItem(itemSo, itemSo.maxStackSize);
        }
    }


    public void UpdateBuySellCountInputField(int count)
    {
        itemCountInputField.text = count.ToString();

    }
}
