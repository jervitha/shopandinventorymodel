using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



[RequireComponent(typeof(ShopController))]
public class ShopView:GenericSingleton<ShopView>
{

    [SerializeField] private TextMeshProUGUI priceText;
    private ShopController shopController;
    [SerializeField] private Button itemtypeButtonArmour;
    [SerializeField] private Button itemtypeButtonconsumables;
    [SerializeField] private Button itemtypeButtonMaterial;
    [SerializeField] private Button itemtypeButtonWeapons;
    [SerializeField] private Button itemtypeButtonAll;
    


    private void Start()
    {
        UpdateUiMoneyAmount(0);   
    }
    private void OnEnable()
    {
        shopController = GetComponent<ShopController>();
        itemtypeButtonArmour.onClick.AddListener(shopController.OnArmourClicked);
        itemtypeButtonconsumables.onClick.AddListener(shopController.OnConsumablesClicked);
        itemtypeButtonMaterial.onClick.AddListener(shopController.OnMaterialClicked);
        itemtypeButtonWeapons.onClick.AddListener(shopController.OnWeaponClicked);
        itemtypeButtonAll.onClick.AddListener(shopController.OnAllClicked);
        
    }

    private void OnDisable()
    {
        itemtypeButtonArmour.onClick.RemoveAllListeners();
        itemtypeButtonconsumables.onClick.RemoveAllListeners();
        itemtypeButtonMaterial.onClick.RemoveAllListeners();
        itemtypeButtonWeapons.onClick.RemoveAllListeners();
        itemtypeButtonAll.onClick.RemoveAllListeners();
    }

   public void UpdateUiMoneyAmount(float newValue)
    {
        priceText.text = newValue.ToString();
        shopController.Money = newValue;
    }
}
