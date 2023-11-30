using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_PlayerPercent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI textShadow;
    [SerializeField] bool p1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (p1) text.SetText(GameMaster.instance.p1percentage + "%"); else text.SetText(GameMaster.instance.p2percentage + "%");
        textShadow.SetText(text.text);
    }
}
