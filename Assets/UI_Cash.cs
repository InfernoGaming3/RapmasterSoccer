using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Cash : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI percentageText;
    [SerializeField] TextMeshProUGUI stockText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText("Cash $" + GameMaster.instance.p1cash);
        percentageText.SetText(GameMaster.instance.p1percentage.ToString("F1") + "%");
        stockText.SetText("Stocks: " + GameMaster.instance.p1stocks);
    }
}
