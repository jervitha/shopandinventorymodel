using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class ToolTipManager : GenericSingleton<ToolTipManager>
{

    [SerializeField] private TextMeshProUGUI messageText;
   public async void ShowTooltip(string message)
    {
        messageText.text = message;

        await Task.Delay(1000);
        messageText.text = string.Empty;
    }
   
}
