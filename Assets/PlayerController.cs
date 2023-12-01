using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerType
    {
        Player1, Player2, CPU
    }
    public PlayerType curPlayer = PlayerType.Player1;
    [SerializeField] int playerIndex;

    Vector2 input;
    public float direction;
    [SerializeField] CharacterStats curStats;
    [SerializeField] float speed;
    [SerializeField] float smoothingAccel;
    [SerializeField] float smoothingDeccel;
    [SerializeField] float airspeed;
    [SerializeField] float jumpForce;
    [SerializeField] float airJumpForce;
    [SerializeField] int maxJumps;
    [SerializeField] float fallSpeed;
    [SerializeField] int weight;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D bc;

    public GameObject groundRayObject;
    public GameObject wallRayObject;
    [SerializeField] float wallRayDistance;
    [SerializeField] LayerMask groundMask;

    [SerializeField] SpriteRenderer sr;
    [SerializeField] Animator anim;
    public PlayerMoveset playerMoveset;

    [SerializeField] float xDieDistance;
    [SerializeField] float yFallDistance;

    [SerializeField] GameObject deathDialogue;

    [SerializeField] int jumps;
    [SerializeField] bool jumpCooldown;
    [SerializeField] float jumpCooldownTimer;
    [SerializeField] float coyoteTime = 10;
    [SerializeField] float respawnTimer;
    bool roundStart = false;

    [SerializeField] AudioSource audioSource;


    //Stat boosts refer to the boosts you get at the shop
    //0 is for power, 1 is for weight, 2 is speed, 3 is jump, 4 is soccer,
    //5 is haste.
    [SerializeField] float[] statBoosts = new float[5];

    float coyoteTimeCounter = 0;  
    bool jumpedOffGround = false;
    bool jumpOn;
    bool dead = false;

    float velocityXSmoothing;
    public Coroutine playerHurtCO;
    Vector3 spawnPos;

    public delegate void PlayerDeath();
    public static event PlayerDeath onPlayerDeath;

    public delegate void PlayerHurt();
    public static event PlayerHurt onPlayerHurt;


    void Awake()
    {
        curStats = (playerIndex == 0) ? GameMaster.instance.p1Character : GameMaster.instance.p2Character;
        InitializeStats();
    }

    // Start is called before the first frame update
    void Start()
    {
        jumpOn = false;
        direction = 1;
        spawnPos = transform.position;
        jumps = maxJumps;
        Invoke(nameof(StartRound), 3.0f);

    }

    void StartRound()
    {
        roundStart = true;
    }

    void InitializeStats()
    {
        float[] gameMasterBoosts = (playerIndex == 0) ? GameMaster.instance.p1StatBoosts : GameMaster.instance.p2StatBoosts;

        for (int i = 0; i < statBoosts.Length; i++)
        {
            statBoosts[i] = gameMasterBoosts[i];
        }

        speed = curStats.runSpeed * statBoosts[2];
        airspeed = curStats.airSpeed * statBoosts[2];
        jumpForce = curStats.jumpHeight * statBoosts[3];
        airJumpForce = curStats.midairJumpHeight * statBoosts[3];
        smoothingAccel = curStats.accel;
        smoothingDeccel = curStats.deccel;
        weight = curStats.weight + (int)statBoosts[1];

        playerMoveset.SetCharacterName(curStats.charName);
        playerMoveset.SetRuntimeAnimatorController(curStats.animatorController);

        bc.offset = new Vector2(curStats.bcXOffset, curStats.bcYOffset);
        bc.size = new Vector2(curStats.bcXSize, curStats.bcYSize);

        //rb.mass = curStats.weight / 100;
        rb.gravityScale = curStats.fallSpeed;
    }

    public Sprite GetPlayerPortrait()
    {
        return curStats.cssSprite;
    }

    public bool IsDead()
    {
        return dead;
    }

    public Vector3 GetSpawnPos()
    {
        return spawnPos;
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public float GetAttackBoost()
    {
        return statBoosts[0];
    }

    public float GetSoccerballBoost()
    {
        return statBoosts[4];
    }

    void FindDirection()
    {
        if (jumpOn && !playerMoveset.attacking) direction = (input.x > 0) ? 1 : (input.x < 0) ? -1 : direction;
        gameObject.transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
       // sr.flipX = direction == -1;
    }

    void CalculateSpeed()
    {
        float curSpeed = (jumpOn) ? speed : airspeed;
        float targetSpeedX = input.x * curSpeed;
        float smoothTime = (input.x != 0) ? smoothingAccel : smoothingDeccel;
        if (playerMoveset.attacking && jumpOn && !playerMoveset.dashAttack) {
            targetSpeedX = 0;
            smoothTime += 0.5f;
        } 
        float smoothSpeed = Mathf.SmoothDamp(rb.velocity.x, targetSpeedX, ref velocityXSmoothing, smoothTime);

        if (!playerMoveset.hurt && !playerMoveset.dashAttack) rb.velocity = new Vector2(smoothSpeed, rb.velocity.y);
    }

    void DetectGround()
    {
        float extraHeight = .05f;
        RaycastHit2D hitGround = Physics2D.Raycast(bc.bounds.center, -Vector2.up, bc.bounds.extents.y + extraHeight, groundMask);

        Color rayColor;

        if (hitGround.collider != null)
        {
            //print(hitGround.collider.gameObject.name);
            jumpOn = true;
            if (jumpOn) jumps = maxJumps;
            rayColor = Color.green;
            coyoteTimeCounter = coyoteTime;

        } else
        {
            rayColor = Color.red;
            coyoteTimeCounter -= Time.deltaTime;
            bool checkCoyoteTIme = (coyoteTimeCounter <= 0 || jumpedOffGround);

            if(checkCoyoteTIme || playerMoveset.hurt) {
                jumpOn = false;
                if (!jumpOn && jumps > maxJumps - 1) jumps = maxJumps - 1;
            }

        }
        Debug.DrawRay(bc.bounds.center, -Vector2.up * (bc.bounds.extents.y + extraHeight), rayColor);

        /*
        RaycastHit2D hitWall = Physics2D.Raycast(wallRayObject.transform.position, Vector2.right * direction, wallRayDistance);
        Debug.DrawRay(wallRayObject.transform.position, -Vector2.right * hitWall.distance * direction, Color.red);
        if (hitWall.collider != null)
        {
            wallDetected = true;
        }
        */

        //Make sure when ur in the air, it costs u atleast one jump. Even while falling

    }


    private void FixedUpdate()
    {
        // if (curPlayer == PlayerType.Player1) input = playerInputActions.General.Move.ReadValue<Vector2>();
        if (dead) {
            playerMoveset.attacking = false;
            playerMoveset.aerialAttack = false;
            anim.Play(playerMoveset.character + "_idle");
            return;
        }
        if (!roundStart) return;

        FindDirection();
        CalculateSpeed();
        DetectGround();
        playerMoveset.BasicMovementAnimation(input, jumpOn, jumps, maxJumps);
        playerMoveset.StopAerialOnGround(jumpOn);


        bool leftRightBounds = (transform.position.x > xDieDistance || transform.position.x < -xDieDistance);
        if (transform.position.y < yFallDistance && !dead)
        {
            GameObject deathD = Instantiate(deathDialogue, transform.position, Quaternion.identity);
            UI_DeathDialogue dDialogue = deathD.GetComponent<UI_DeathDialogue>();
            dDialogue.deathType = 2;
            dDialogue.playerDeathPos = transform.position;
            dDialogue.dialogueMessage = "Bummer.";
            KillPlayer();

        } 
        else if (leftRightBounds && !dead)
        {
            GameObject deathD = Instantiate(deathDialogue, transform.position, Quaternion.identity);
            UI_DeathDialogue dDialogue = deathD.GetComponent<UI_DeathDialogue>();
            dDialogue.deathType = (transform.position.x > xDieDistance) ? 1 : -1;
            dDialogue.playerDeathPos = transform.position;
            dDialogue.dialogueMessage = "Bogus.";
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        GameMaster.instance.PlaySound(14);
        StartCoroutine(RespawnPlayer());
        dead = true;
        if (playerMoveset.IsHoldingItem()) playerMoveset.ThrowItem("0,0");
        if (playerIndex == 0) GameMaster.instance.SetP1Percentage(0); else GameMaster.instance.SetP2Percentage(0);
        if(playerIndex == 0)GameMaster.instance.RemoveStock();
        onPlayerDeath();
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(respawnTimer);
        transform.position = spawnPos;
        dead = false;
    }

    public void Move(Vector2 moveVector)
    {
        input = moveVector;
    }

    public void Jump()
    {
        if (playerMoveset.attacking || playerMoveset.hurt || jumpCooldown || !roundStart) return;
        if (jumpOn)
        {
            jumps--;
            rb.velocity = Vector2.up * jumpForce;
            jumpedOffGround = true;
            StartCoroutine(ResetJumpOffGround());
            jumpCooldown = true;
            StartCoroutine(EndJumpCooldown());

        } else if(jumps > 0)
        {
            jumps--;
            rb.velocity = Vector2.up * airJumpForce;
            jumpCooldown = true;
            StartCoroutine(EndJumpCooldown());
        }
    }

    IEnumerator EndJumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldownTimer);
        jumpCooldown = false;
    }

    IEnumerator ResetJumpOffGround()
    {
        yield return new WaitForSeconds(0.5f);
        jumpedOffGround = false;
    }

    public void Attack()
    {
        playerMoveset.DetectAttackInput(input, jumpOn, direction, statBoosts[5]);
    }

    public void DamgePlayer(float damage, Vector2 knockback, float hitstun, float damageMultiplier)
    {
        if(playerHurtCO != null) StopCoroutine(playerHurtCO);
        if (playerMoveset.IsHoldingItem()) playerMoveset.ThrowItem("0,0");

        playerMoveset.attacking = false;
        playerMoveset.dashAttack = false;
        float percentage = (playerIndex == 0) ? GameMaster.instance.p1percentage : GameMaster.instance.p2percentage;
        percentage += damage * damageMultiplier;
        print("statBoost: " + damageMultiplier + ",damage: " + damageMultiplier * damage);
        playerMoveset.hurt = true;
        if(knockback.x > 3 || knockback.y > 3) playerMoveset.SpawnTrailSmoke();
        anim.Play(playerMoveset.character + "_hurt");

        float knockbackWeightReduc = 1 - (float)weight / 300f;
        float percentMul = percentage / 40;
        print("kbkbck: " + knockbackWeightReduc + "percentMul: " + percentMul);
        rb.velocity = new Vector2(knockback.x, knockback.y) * knockbackWeightReduc * percentMul;
        print("vel: x" + rb.velocity.x + " y: " + rb.velocity.y + " mul:" + knockbackWeightReduc + " weight: " + curStats.weight);
        playerHurtCO = StartCoroutine(EndPlayerHitstun(hitstun * percentMul));
        onPlayerHurt();
        if (playerIndex == 0) GameMaster.instance.SetP1Percentage(percentage); else GameMaster.instance.SetP2Percentage(percentage);
    }

    IEnumerator EndPlayerHitstun(float hitstun)
    {
        print("hitstun: " + hitstun);
        yield return new WaitForSeconds(hitstun);
        print("playerHurt reset");
        playerMoveset.attacking = false;
        playerMoveset.aerialAttack = false;
        playerMoveset.hurt = false;
    }

    public bool IsAttacking()
    {
        return playerMoveset.attacking;
    }

    public void SetHeldItem(bool holdingItem)
    {
        playerMoveset.SetHeldItem(holdingItem);
    }

}
