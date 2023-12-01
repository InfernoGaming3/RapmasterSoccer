using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSong : MonoBehaviour
{
    public int songNumber = 1;
    // Start is called before the first frame update
    void Start()
    {
        switch(songNumber)
        {
            case 0:
                GameMaster.instance.PlayIntroMusic();
                break;
            case 1:
                GameMaster.instance.PlayBattleMusic();
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
