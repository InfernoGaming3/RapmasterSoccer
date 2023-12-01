using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WinScreen : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] bool winScreen;
    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("Win", winScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
