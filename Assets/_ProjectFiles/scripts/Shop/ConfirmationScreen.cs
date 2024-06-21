using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmationScreen : MonoBehaviour
{
    [SerializeField] private GameObject confirmationParent;
    [SerializeField] private Button yesButtonConfirmation;
    [SerializeField] private Button noButtonConfirmation;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;
    private bool isBuying;



    private void Start()
    {
        confirmationParent.SetActive(false);
    }

    private void OnEnable()
    {

        buyButton.onClick.AddListener(delegate { isBuying = true; ShowConfirmationMessage(); });

        sellButton.onClick.AddListener(delegate { isBuying = false; ShowConfirmationMessage(); });
        yesButtonConfirmation.onClick.AddListener(YesConfirmtionButton);
        noButtonConfirmation.onClick.AddListener(NoConfirmtionButton);
    }
    void NoConfirmtionButton()
    {
       
        confirmationParent.SetActive(false);
        SoundManager.Instance.PlaySound(SoundType.Cancellation);
    }
    void YesConfirmtionButton()
    {
        confirmationParent.SetActive(false);
        SoundManager.Instance.PlaySound(SoundType.Confirmation);
        if (isBuying)
        {
            ShopView.Instance.shopController.BuyItems();

        }
        else
        {
            ShopView.Instance.shopController.SellItems();

        }

    }
    public void ShowConfirmationMessage()
    {


        if (int.TryParse(InventoryManager.Instance.itemCountInputField.text, out int count))
        {
            confirmationParent.SetActive(true);
            if (isBuying)
            {

                confirmationText.text = "Are you sure to buy" + count +InventoryManager.Instance.selectedItemSo.itemName;
            }
            else
            {
                confirmationText.text = "Are you sure to sell" + count + InventoryManager.Instance.selectedItemSo.itemName;
            }
        }
    }
}
