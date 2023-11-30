using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ScoreCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreCounterText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreCounterText.SetText(GameMaster.instance.p1Score + "-" + GameMaster.instance.p2Score);
    }
}
