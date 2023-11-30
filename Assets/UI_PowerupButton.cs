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

        switch(powerupIndex)
        {
            case 0:
                GameMaster.instance.p1percentage -= GameMaster.instance.p1percentage / 4;
                break;
            case 1:
                GameMaster.instance.p1StatBoosts[0] += 0.1f;
                break;
            case 2:
                GameMaster.instance.p1StatBoosts[2] += 0.1f;
                break;
            case 3:
                GameMaster.instance.p1StatBoosts[3] += 0.1f;
                break;
            case 4:
                GameMaster.instance.p1StatBoosts[1] += 1f;
                break;
            case 5:
                GameMaster.instance.p1StatBoosts[4] += 0.1f;
                break;
            case 6:
                GameMaster.instance.p1StatBoosts[5] += 0.1f;
                break;
            case 7:
                GameMaster.instance.p1stocks++;
                break;
        }
        GameMaster.instance.p1cash -= powerupCost;
    }
}
