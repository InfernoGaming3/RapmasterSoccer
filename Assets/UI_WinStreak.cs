using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WinStreak : MonoBehaviour
{
    [SerializeField] Image[] trophyImages;

    // Start is called before the first frame update
    void Start()
    {
        int count = GameMaster.instance.p1wins;
        foreach(Image image in trophyImages)
        {
            if (count > 0) image.color = new Color(1, 1, 1, 1);
            count--;
        }
    }
}
