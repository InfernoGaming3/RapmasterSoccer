using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerInputPrefab;
    // Start is called before the first frame update
    void Start()
    {
        var player1 = PlayerInput.Instantiate(prefab: playerInputPrefab, playerIndex: 0, 
            controlScheme: "PlayerKeyboard", pairWithDevice: Keyboard.current);
        var player2 = PlayerInput.Instantiate(prefab: playerInputPrefab, playerIndex: 1, 
            controlScheme: "Player2Keyboard", pairWithDevice: Keyboard.current);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
