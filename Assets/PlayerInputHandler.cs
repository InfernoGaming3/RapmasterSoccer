using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;


public class PlayerInputHandler : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        var players = FindObjectsOfType<PlayerController>();
        var index = playerInput.playerIndex;
        player = players.FirstOrDefault(m => m.GetPlayerIndex() == index);

        //playerInput.user.ActivateControlScheme(index == 0 ? "PlayerKeyboard" : "Player2Keyboard");
    }

    public void Move(InputAction.CallbackContext context)
    {
        player.Move(context.ReadValue<Vector2>());
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.started) player.Jump();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if(context.started) player.Attack();
    }
}
