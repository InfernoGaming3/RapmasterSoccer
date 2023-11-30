using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Cash : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText("Cash $" + GameMaster.instance.p1cash);
    }
}
