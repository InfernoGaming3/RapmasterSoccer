using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "CreateCharacterStat")]
public class CharacterStats : ScriptableObject
{
    public string charName;
    public RuntimeAnimatorController animatorController;

    public float initialSpeed;
    public float runSpeed;
    public float airSpeed;
    public float accel;
    public float deccel;

    public float jumpHeight;
    public float midairJumpHeight;
    public float fallSpeed;

    public int weight;

    public float bcXOffset;
    public float bcYOffset;
    public float bcXSize;
    public float bcYSize;

    public Sprite cssSprite;
    public Sprite stockIcon;
    
}
