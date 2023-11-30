using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCPU : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject playerTarget;
    [SerializeField] GameObject ballTarget;
    [SerializeField] GameObject goalTarget;

    [SerializeField] float attackRange = 3;

    Vector2 input;

   // [SerializeField] int jumpCooldownSecs;
   // bool canJump = false;

    public enum PlayerAction {
        Idle,
        ChaseBall,
        ChasePlayer,
        ChaseSpawn,
        ThrowTowardsGoal,
        DropInGoal
    }

    public PlayerAction curAction;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating(nameof(GoToState), 5.0f, 5.0f);
    }

    private void OnEnable()
    {
        PlayerController.onPlayerDeath += StopChasingPlayer;
        PlayerController.onPlayerHurt += GoToState;
        Ball.onBallCollect += GoToState;
        Ball.onBallRespawn += GoToState;
    }

    private void OnDisable()
    {
        PlayerController.onPlayerDeath -= StopChasingPlayer;
        PlayerController.onPlayerHurt -= GoToState;
        Ball.onBallCollect -= GoToState;
        Ball.onBallRespawn -= GoToState;
    }

    // Update is called once per frame
    void Update()
    {
        if(curAction==PlayerAction.ChaseBall)
        {
            GoAfterBall();
        } else if (curAction==PlayerAction.ChasePlayer)
        {
            GoAfterPlayer();
        } else if (curAction==PlayerAction.ChaseSpawn)
        {
            GoToSpawn();
        } else if (curAction==PlayerAction.DropInGoal)
        {
            GoToGoal();
        }
    }

    void GoToState()
    {
        //print("GoToState called");
        ballTarget = GameObject.FindGameObjectWithTag("Ball");
        PlayerMoveset enemyMoveset = playerTarget.GetComponent<PlayerController>().playerMoveset;
        if (!playerController.playerMoveset.IsHoldingItem() && !enemyMoveset.IsHoldingItem())
        {
            curAction = PlayerAction.ChaseBall;
        } else if(playerController.playerMoveset.IsHoldingItem())
        {
            curAction = PlayerAction.DropInGoal;
        } else
        {
            curAction = PlayerAction.ChasePlayer;
        }
    }


    void StopChasingPlayer()
    {
        if(curAction == PlayerAction.ChasePlayer) curAction = PlayerAction.ChaseSpawn;
    }

    void GoAfterBall()
    {
        if (ballTarget == null) return;
        if (transform.position.x > ballTarget.transform.position.x)
        {
            input = new Vector2(-1, 0);
            playerController.Move(input);
        } else
        {
            input = new Vector2(1, 0);
            playerController.Move(input);
        }

        if (transform.position.y < ballTarget.transform.position.y)
        {
            playerController.Jump();
        }

        float yDistance = Mathf.Abs(transform.position.y - playerTarget.transform.position.y);
        ThrowOutRandomAttacks(0.2f, yDistance, false);
    }

    void GoToSpawn()
    {
        if (ballTarget == null) return;
        if (transform.position.x > playerController.GetSpawnPos().x)
        {
            input = new Vector2(-1, 0);
            playerController.Move(input);
        }
        else
        {
            input = new Vector2(1, 0);
            playerController.Move(input);
        }

        if (transform.position.y < playerController.GetSpawnPos().y)
        {
            playerController.Jump();
        }

        float yDistance = Mathf.Abs(transform.position.y - playerTarget.transform.position.y);
        ThrowOutRandomAttacks(attackRange, yDistance, false);
    }

    void GoAfterPlayer()
    {
        if (playerTarget == null) return;

        bool inAttackRange = Vector2.Distance(transform.position, playerTarget.transform.position) < attackRange;
        float yDistance = Mathf.Abs(transform.position.y - playerTarget.transform.position.y);

        if (transform.position.x > playerTarget.transform.position.x && !inAttackRange)
        {
            input = new Vector2(-1, 0);
            playerController.Move(input);

        }
        else if (!inAttackRange)
        {
            input = new Vector2(1, 0);
            playerController.Move(input);
        }
        int randomJumps = Random.Range(0, 100);
        if (randomJumps <= 5 && yDistance < 1)
        {
            playerController.Jump();
        }

        if (transform.position.y < playerTarget.transform.position.y)
        {
            playerController.Jump();
        }

        ThrowOutRandomAttacks(attackRange, yDistance, false);
    }

    void GoToGoal()
    {
        float xDistance = Mathf.Abs(transform.position.x - goalTarget.transform.position.x);
        //print("xDistance: " + xDistance);
        if (transform.position.x > goalTarget.transform.position.x && xDistance > 2f)
        {
            input = new Vector2(-1, 0);
            playerController.Move(input);
        }
        else if (xDistance > 2f)
        {
            input = new Vector2(1, 0);
            playerController.Move(input);
        }

        if(xDistance < 2f)
        {
            playerController.Move(new Vector2(0, -1));
            playerController.Attack();
            curAction = PlayerAction.ChasePlayer;
        }

        float yDistance = Mathf.Abs(transform.position.y - playerTarget.transform.position.y);
        ThrowOutRandomAttacks(1, yDistance, true);
    }

    void ThrowOutRandomAttacks(float attackRange, float yDistance, bool itemInHand)
    {
        if (Vector2.Distance(transform.position, playerTarget.transform.position) < attackRange)
        {
            if(!itemInHand)
            {
                int side = (transform.position.x > playerTarget.transform.position.x) ? -1 : 1;
                int randomX = (side == 1) ? Random.Range(0, side) : Random.Range(side, 0);
                int randomY = Random.Range(-1, 2);

                if (transform.position.y < playerTarget.transform.position.y && yDistance > 3)
                {
                    int uAirChance = Random.Range(0, 100);
                    if (uAirChance <= 60) randomY = 1;
                }
                else if (transform.position.y < playerTarget.transform.position.y && yDistance > 3)
                {
                    int dAirChance = Random.Range(0, 100);
                    if (dAirChance <= 60) randomY = -1;
                }

                input = new Vector2(randomX, randomY);

                playerController.Move(input);
                playerController.Attack();
            } else
            {
                int side = (transform.position.x > playerTarget.transform.position.x) ? -1 : 1;
                int randomX = (side == 1) ? Random.Range(0, side) : Random.Range(side, 0);
                int randomY = 0;

                if (transform.position.y < playerTarget.transform.position.y && yDistance > 3)
                {
                    randomY = 1;
                }
                else if (transform.position.y < playerTarget.transform.position.y && yDistance > 3)
                {
                    randomY = -1;
                }

                input = new Vector2(randomX, randomY);

                playerController.Move(input);
                playerController.Attack();
            }

        }
    }
}
