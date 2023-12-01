using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_PowerupSelect : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI descriptionText;
    // Start is called before the first frame update
    void Start()
    {
        image.sprite = GameMaster.instance.p1Character.cssSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameMaster.instance.showPowerupDesc)
        {
            descriptionText.SetText(GameMaster.instance.powerupDescriptions[GameMaster.instance.hoverIndex]);
        } else
        {
            descriptionText.SetText("");
        }
    }

    public void GoToScene()
    {
        GameMaster.instance.StartGame();
        SceneManager.LoadScene("GrassField");
    }
}
