using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_DeathDialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;
    public string dialogueMessage;
    public Vector3 playerDeathPos;
    public int deathType = 1;
    // Start is called before the first frame update
    void Start()
    {
        if(deathType != 2)
        {
            float yPos = Random.Range(-0.9f, 0.9f);
            transform.position = new Vector3(10 * deathType, playerDeathPos.y + yPos, 0);
        } else
        {
            float yPos = Random.Range(-0.3f, 0.3f);
            transform.position = new Vector3(playerDeathPos.x, -6.5f + yPos, 0);

        }
        StartCoroutine(DialogueEnd());
        dialogueText.SetText(dialogueMessage);

    }

    IEnumerator DialogueEnd()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
