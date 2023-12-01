using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float yFallDistance;
    [SerializeField] float timeTillScore;
    [SerializeField] GameObject hitbox;
    [SerializeField] BoxCollider2D bc;

    bool carried = false;
    public bool collectable = false;
    GameObject carryParent;

    [SerializeField] float fallSpeed;
    [SerializeField] float waterFallSpeed;
    [SerializeField] Vector3 startPos;
    string inCement = "";

    public delegate void BallCollected();
    public static event BallCollected onBallCollect;

    public delegate void BallRespawn();
    public static event BallRespawn onBallRespawn;

    bool scoringBall = false;


    // Start is called before the first frame update
    void Start()
    {
        //startPos = transform.position;
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        if (balls.Length >= 2) Destroy(this.gameObject);
        StartCoroutine(AllowContact());
        rb.gravityScale = fallSpeed;
    }

    IEnumerator AllowContact()
    {
        yield return new WaitForSeconds(1f);
        gameObject.layer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(carried)
        {
            transform.position = carryParent.transform.position;
        }

        if(transform.position.y < yFallDistance)
        {
            BallScored();
        }

        //print(rb.velocity.sqrMagnitude);
        if(rb.velocity.sqrMagnitude < 10)
        {
           // bc.enabled = true;
            hitbox.SetActive(false);
        }


        collectable = rb.velocity.sqrMagnitude < 10;
        
    }

    public void ApplyKnockback(Vector2 knockback, float kbMul)
    {
        rb.velocity = new Vector2(knockback.x * kbMul, knockback.y * kbMul);

    }

    public float GetRigidbodyVelSqrtMag()
    {
        return rb.velocity.sqrMagnitude;
    }

    public void ResetBallSpeed()
    {
        rb.velocity = Vector3.zero;
    }

    public void MirrorBallSpeed()
    {
        rb.velocity *= -0.5f;
    }

    public void EnableHitbox(int hitboxLayer)
    {
       // bc.enabled = false;
        hitbox.SetActive(true);
        hitbox.layer = hitboxLayer;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController _playerController = collision.collider.GetComponent<PlayerController>();
        if (_playerController != null) 
        {
            if(!_playerController.IsAttacking() && !hitbox.activeInHierarchy)
            {
                GameMaster.instance.PlaySound(15);
                Destroy(this.gameObject);
                _playerController.SetHeldItem(true);
                onBallCollect();
                print("Player is carrying the ball!");
            } 

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Colliding with waiter, slow down fall speed.
        if (collision.gameObject.layer == 12)
        {
            rb.velocity = Vector2.zero;
            inCement = collision.gameObject.name;
            if(!scoringBall) StartCoroutine(ScoreBallInZone());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            // rb.gravityScale = fallSpeed;
            //scoringBall = false;
            //inCement = "";
        }
    }

    IEnumerator ScoreBallInZone()
    {
        print("scoring the ball...");
        scoringBall = true;
        yield return new WaitForSeconds(timeTillScore);
        scoringBall = false;
        BallScored();
    }

    void BallScored()
    {
        GameMaster.instance.PlaySound(13);
        rb.velocity = Vector3.zero;
        transform.position = startPos;
        if(GameMaster.instance.CheckBallsOnField()) onBallRespawn();
        if (inCement == "WaterL")
        {
            GameMaster.instance.p2Score++;
        }
        else if (inCement == "WaterR")
        {
            GameMaster.instance.p1Score++;
        }
    }

}