using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_PowerupSelect : MonoBehaviour
{
    [SerializeField] Image image;
    // Start is called before the first frame update
    void Start()
    {
        image.sprite = GameMaster.instance.p1Character.cssSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToScene()
    {
        GameMaster.instance.StartGame();
        SceneManager.LoadScene("GrassField");
    }
}
