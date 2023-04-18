using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    float velocity = 20f;
    int damage;

    [SerializeField]
    GameObject impactEffect;
    [SerializeField]
    ParticleSystem bloodEffect;

    [Space(10)]
    [SerializeField]
    bool drawGizmos;
    [SerializeField]
    LayerMask collideWithBulletMask;

    float lifeTimer;
    [SerializeField]
    float life = 3f;
    [SerializeField]
    float detectingRadius = .2f;
    int fireByLayer;

    Rigidbody rb;

    private void Update()
    {
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);

        //this makes destroy bullet atomatically
        if (Time.deltaTime > lifeTimer + life)
        {
            Destroy(gameObject);
        }
        NormalBulletCollision();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Lady"))
        {
            collision.gameObject.GetComponent<LadyController>().Dead();
        }
        
        Dead();
    }

    void NormalBulletCollision()
    {
        //better collision
        Collider[] collider = Physics.OverlapSphere(transform.position, detectingRadius, collideWithBulletMask);
        foreach (Collider nearbyObject in collider)
        {
            LadyController lady = nearbyObject.GetComponent<LadyController>();
            if(lady != null)
            {
                Blood();
                lady.Dead();
            }

            EnemyController _enemy = nearbyObject.GetComponent<EnemyController>();
            if (_enemy != null)
            {
                Blood();
                _enemy.EnemyDead();
               
            }

            EnemyHead _enemyHead = nearbyObject.GetComponent<EnemyHead>();
            if(_enemyHead != null)
            {
                Blood();
                _enemyHead.EnemyDead();
            }
           

            ExplosiveBarrel _barrel = nearbyObject.GetComponent<ExplosiveBarrel>();
            if (_barrel != null)
                _barrel.Explode();
            
            Dead();
        }
    }

    void Blood()
    {
        GameManager.instance.audioManager.Play("Blood");
        if (bloodEffect != null)
            Instantiate(bloodEffect, transform.position, Quaternion.identity);
    }

    public void Fire(Vector3 position, Vector3 eular,float bulletVelocity,int layermask, int damageAmount)
    {
        string bulletLayerMask = LayerMask.LayerToName(layermask);
        lifeTimer = Time.deltaTime;
        
        if(bulletLayerMask == "Player")
        {
            collideWithBulletMask &= ~(1 << LayerMask.NameToLayer(bulletLayerMask));
            collideWithBulletMask |= 1 << LayerMask.NameToLayer("Enemy");
        }
        else
        {
            collideWithBulletMask &= ~(1 << LayerMask.NameToLayer(bulletLayerMask));
            collideWithBulletMask |= 1 << LayerMask.NameToLayer("Player");
        }

        transform.position = position;
        transform.eulerAngles = eular;
        velocity = bulletVelocity;
        damage = damageAmount;
    }
    
    void Dead()
    {
        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, detectingRadius);
        }
    }
}
