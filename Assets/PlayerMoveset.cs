using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMoveset : MonoBehaviour
{
    [SerializeField] public string character;
    public bool attacking = false;
    public bool aerialAttack = false;
    public bool hurt;
    [SerializeField] Animator anim;
    [SerializeField] GameObject[] hitboxes;
    bool holdingItem;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject smokePrefab;

    bool spawningSmoke;

    private void Start()
    {
    }

    public void SetHeldItem(bool holdingItem)
    {
        this.holdingItem = holdingItem;
    }

    public bool IsHoldingItem()
    {
        return holdingItem;
    }

    public void SetRuntimeAnimatorController(RuntimeAnimatorController runAnimController)
    {
        anim.runtimeAnimatorController = runAnimController;
    }

    public void SetCharacterName(string character)
    {
        this.character = character;
    }

    public void BasicMovementAnimation(Vector2 input, bool grounded, int jumps, int maxjumps)
    {
        if (attacking || hurt) return;
        if (grounded)
        {
            if (input.x != 0) anim.Play(character + "_walk"); else anim.Play(character + "_idle");
        }
        else
        {
            if (jumps >= maxjumps - 1) anim.Play(character + "_jump"); else anim.Play(character + "_djump");
        }
    }

    public void StopAerialOnGround(bool grounded)
    {
        if (grounded && aerialAttack) attacking = false;
    }

    public void DetectAttackInput(Vector2 input, bool grounded, float direction)
    {
        if (attacking || hurt) return;
        attacking = true;
        if (holdingItem)
        {
            DetectItemThrow(input, direction);
        } else
        {
            if (grounded) DetectedGroundedAttack(input); else DetectAerialAttack(input, direction);
        }
    }

    void DetectItemThrow(Vector2 input, float direction)
    {
        aerialAttack = false;
        bool forward = (direction == 1 && input.x > 0 || direction == -1 && input.x < 0);
        if (input.x == 0 && input.y == 0)
        {
            anim.Play(character + "_itemfthrow");
        }
        else if (forward && input.y == 0)
        {
            anim.Play(character + "_itemfthrow");
        } else if (!forward && input.y == 0)
        {
            anim.Play(character + "_itembthrow");
        }
        else if (input.y < 0)
        {
            anim.Play(character + "_itemdthrow");
        }
        else if (input.y > 0)
        {
            anim.Play(character + "_itemuthrow");
        }
    }

    void DetectedGroundedAttack(Vector2 input)
    {
        aerialAttack = false;
        if (input.x == 0 && input.y == 0)
        {
           anim.Play(character + "_jab");
        }
        else if (input.x != 0 && input.y == 0)
        {
            anim.Play(character + "_ftilt");
        }
        else if (input.y < 0)
        {
            anim.Play(character + "_dtilt");
        }
        else if (input.y > 0)
        {
            anim.Play(character + "_utilt");
        }
    }

    void DetectAerialAttack(Vector2 input, float direction)
    {
        aerialAttack = true;
        bool forward = (direction == 1 && input.x > 0 || direction == -1 && input.x < 0);
        if (input.x == 0 && input.y == 0)
        {
            anim.Play(character + "_nair");
        }
        else if (forward && input.y == 0)
        {
            anim.Play(character + "_fair");
        }
        else if (!forward && input.y == 0)
        {
            anim.Play(character + "_bair");
        }
        else if (input.y < 0)
        {
            anim.Play(character + "_dair");
        }
        else if (input.y > 0)
        {
            anim.Play(character + "_uair");
        }
    }

    public void AttackEnd()
    {
        attacking = false;
    }

    public void ThrowItem(string vectorDir)
    {
        GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);

        string[] vectors = vectorDir.Split(",");
        int xSpeed =  Int32.Parse(vectors[0]);
        int ySpeed = Int32.Parse(vectors[1]);

        //print("xSpeed: " + xSpeed + ": ySpeed: " + ySpeed);

        int attackerLayer = (GetComponentInParent<PlayerController>().GetPlayerIndex() == 0) ? 8 : 9;
        ball.GetComponent<Ball>().EnableHitbox(attackerLayer);
        float xDir = GetComponentInParent<PlayerController>().direction;

        ball.GetComponent<Ball>().ApplyKnockback(new Vector2(xSpeed * xDir, ySpeed), 1);
        holdingItem = false;

    }

    public void SpawnTrailSmoke()
    {
        StartCoroutine(SpawnSmokes());
    }

    IEnumerator SpawnSmokes()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity);
        Destroy(smoke, 1f);
        if(hurt) SpawnTrailSmoke();
    }

}
