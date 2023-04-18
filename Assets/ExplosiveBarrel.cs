using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject ExplosiveEffect;
    [SerializeField]
    float explosionForce = 3000f;
    [SerializeField]
    float explosionRange;
    [SerializeField]
    float upForce = 1f;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionUpForce;
    [SerializeField] LayerMask NotLadyLayerMask;

    public void Explode()
    {
        if (ExplosiveEffect != null)
            Instantiate(ExplosiveEffect, transform.position, Quaternion.identity);

        GameManager.instance.audioManager.Play("Blast");

        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        Collider[] explosionCollider = Physics.OverlapSphere(transform.position, explosionRange);

        foreach (Collider nearbyexplsionObj in explosionCollider)
        {
            /*
            LadyController lady = nearbyexplsionObj.GetComponent<LadyController>();
            if (lady != null)
            {
                lady.Dead();
            }
            */
            
            EnemyController _enemy = nearbyexplsionObj.GetComponent<EnemyController>();
            if (_enemy != null)
                _enemy.EnemyDead();

        }

        yield return new WaitForEndOfFrame();

        Collider[] explosionCollider1 = Physics.OverlapSphere(transform.position, explosionRange,NotLadyLayerMask);

        foreach (Collider nearbyexplsionObj in explosionCollider1)
        {
            //explosion Force
            Rigidbody rbofObj = nearbyexplsionObj.GetComponent<Rigidbody>();
            if (rbofObj != null)
            {
                rbofObj.AddExplosionForce(explosionForce * Time.fixedDeltaTime, transform.position, explosionRange, upForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }

        Destroy(gameObject);
    }
    
    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, explosionRange);
    }*/
}
