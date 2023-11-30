using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_PowerupButton : MonoBehaviour
{
    public int powerupIndex;
    public int powerupCost;

    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI costText;

    private void Start()
    {
        costText.SetText("$" + powerupCost);
        if(GameMaster.instance.p1cash < powerupCost)
        {
            image.color = new Color(1, 1, 1, 0.5f);
        }
    }

    public void OnPowerupButtonClicked()
    {
        if (GameMaster.instance.p1cash < powerupCost) return;
        GameMaster.instance.IncreaseP1Stats(powerupIndex, powerupCost);
    }
}
