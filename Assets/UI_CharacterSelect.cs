using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_CharacterSelect : MonoBehaviour
{
    int p1Index = 0;
    int p2Index = 0;

    [SerializeField] Button rapperButton;
    [SerializeField] Button djButton;
    [SerializeField] Button cheetahButton;

    [SerializeField] Button startGameButton;

    [SerializeField] Image p1Image;
   // [SerializeField] Image p2Image;


    // Start is called before the first frame update
    void Start()
    {
        p1Image.sprite = GameMaster.instance.p1Character.cssSprite;

        rapperButton.onClick.AddListener(() =>
        {
            print("rapper clicked!");
            GameMaster.instance.UpdateP1Character("rap");
            p1Image.sprite = GameMaster.instance.p1Character.cssSprite;
        });

        djButton.onClick.AddListener(() =>
        {
            print("dj clicked!");
            GameMaster.instance.UpdateP1Character("dj");
            p1Image.sprite = GameMaster.instance.p1Character.cssSprite;
        });

        cheetahButton.onClick.AddListener(() =>
        {
            print("cheetah clicked!");
            GameMaster.instance.UpdateP1Character("cheetah");
            p1Image.sprite = GameMaster.instance.p1Character.cssSprite;
        });


        startGameButton.onClick.AddListener(() =>
        {
            GameMaster.instance.RandomlyPickP2Character();
            //p2Image.sprite = GameMaster.instance.p2Character.cssSprite;
            GameMaster.instance.StartGame();
            SceneManager.LoadScene("GrassField");
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
