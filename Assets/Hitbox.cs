using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] float damage = 5;
    [SerializeField] Vector2 knockback;
    [SerializeField] float hitstun;
    [SerializeField] Hitboxes hitboxParent;
    [SerializeField] bool isBall;
    [SerializeField] int playSoundIndex;

    [SerializeField] PlayerController ownerController;


    public void SetOwnerController(PlayerController playerController)
    {
        ownerController = playerController;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitboxCollision(collision);
    }

    void HitboxCollision(Collider2D collision)
    {
        if(hitboxParent != null)
        {
            if (hitboxParent.deactivateHitboxes) return;
        }

        //print("collided with: " + collision.gameObject.name);
        PlayerController _playerController = collision.gameObject.GetComponent<PlayerController>();
        Ball _ball = collision.gameObject.GetComponent<Ball>();

        float xMul = (transform.position.x > collision.gameObject.transform.position.x) ? -1 : 1;
        Vector2 kb = new Vector2(knockback.x * xMul, knockback.y);

        if(isBall)
        {
            kb *= GetComponentInParent<Ball>().GetRigidbodyVelSqrtMag() / 20;
        }

        if (_playerController != null)
        {
            GameMaster.instance.PlaySound(playSoundIndex);
            if (isBall) 
            {
                if (ownerController != null) _playerController.DamgePlayer(damage, kb, hitstun, ownerController.GetSoccerballBoost());
                else _playerController.DamgePlayer(damage, kb, hitstun, 1);
                GetComponentInParent<Ball>().MirrorBallSpeed();
            } else
            {
                _playerController.DamgePlayer(damage, kb, hitstun, ownerController.GetAttackBoost());
            }

            if (hitboxParent != null) {
                hitboxParent.deactivateHitboxes = true;
                hitboxParent.CreateHitEffect(transform.position, hitstun);
            }
        }

        if (_ball != null)
        {
            GameMaster.instance.PlaySound(playSoundIndex);
            _ball.ApplyKnockback(kb, 1.5f);
            int attackerLayer = (GetComponentInParent<PlayerController>().GetPlayerIndex() == 0) ? 8 : 9;
            _ball.EnableHitbox(attackerLayer);
        }
    }

    private void OnDisable()
    {
        if (hitboxParent != null) hitboxParent.deactivateHitboxes = false;
    }
}
