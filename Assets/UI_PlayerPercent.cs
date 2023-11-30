using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_PlayerPercent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI percentText;
    [SerializeField] TextMeshProUGUI percentShadow;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] PlayerController playerController;
    [SerializeField] Image image;
    [SerializeField] Image ballImage;
    [SerializeField] bool p1;

    // Start is called before the first frame update
    void Start()
    {
        image.sprite = (p1)? GameMaster.instance.p1Character.cssSprite: GameMaster.instance.p2Character.cssSprite;
        nameText.SetText((p1)? GameMaster.instance.p1Character.name: GameMaster.instance.p2Character.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (p1) percentText.SetText(GameMaster.instance.p1percentage.ToString("F1") + "%"); else percentText.SetText(GameMaster.instance.p2percentage.ToString("F1") + "%");
        percentShadow.SetText(percentText.text);

        if (playerController.playerMoveset.IsHoldingItem()) ballImage.color = new Color(1, 1, 1, 1); else ballImage.color = new Color(1,1,1,0);
    }
}
