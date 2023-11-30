using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_TimeLimit : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int minutes = (int)(GameMaster.instance.timerCount / 60);
        int seconds = GameMaster.instance.timerCount - (minutes*60);
        //print("minutes: " + seconds);
        text.SetText(minutes + " : " + seconds);    
    }
}
