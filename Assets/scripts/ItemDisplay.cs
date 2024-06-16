using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay : MonoBehaviour
{
    public ItemSo itemSo;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCountText;
    public int itemCurrentCount;


    public void UpdateItem(ItemSo _itemSo,int count)
    {
        itemSo = _itemSo;
        itemImage.sprite = itemSo.sprite;
        itemCurrentCount=count;

        itemCountText.text = itemCurrentCount.ToString();

    }
    public void itemPress()
    {
        if(Actions.OnitemSelected!=null)
        {
            Actions.OnitemSelected(this);
        }
       
    }
}
