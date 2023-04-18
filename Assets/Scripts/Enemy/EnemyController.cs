using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent navMesh;
    [SerializeField] Animator animator;
    [SerializeField] LadyController ladyController;
    [SerializeField] GameObject lady;
    [SerializeField] RagdollPhysics ragdollPhy;
    [SerializeField]
    bool canMove = true;
    bool enemyDead = false;

    bool enemyDeadcheck = false;
    public bool gameOver = false;
    [SerializeField] bool navDisabled = false;
    [SerializeField] bool ladyDead = false;
    [SerializeField] GameObject alertCanvas;
    [SerializeField] float dist = 10f;
    bool alertON = false;

    void Start()
    {
        enemyDead = false;
        navDisabled = false;
        ladyDead = false;
        if (navMesh == null)
            navMesh = GetComponent<NavMeshAgent>();
        if (ladyController == null)
            ladyController = LadyController.instance;
        if (lady == null)
            if (ladyController != null)
                lady = ladyController.gameObject;
    }

    void Update()
    {
        if (enemyDead)
            return;

        canMove = !LadyController.ladyDead;
        if (lady != null && canMove)
        {
            Movement();
            animator.SetFloat("Speed", Vector3.Project(navMesh.desiredVelocity, transform.forward).magnitude);
            LadyNearAlert();
        }
        else if(!navDisabled)
        {
            animator.SetFloat("Speed",0f);
            navMesh.enabled = false;
            navDisabled = true;
        }
    }

    void Movement()
    {
        navMesh.SetDestination(lady.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Lady") && !ladyDead)
        {
            LadyController.instance.Dead();
            ladyDead = true;
        }
    }

    public void EnemyDead()
    {
        enemyDead = true;
        alertCanvas.SetActive(false);
        GameManager.instance.uiManager.KillsText();
        ActivateRagdoll();
        //activate ragdoll
    }

    public void AimGunAtEnd()
    {
        animator.SetTrigger("AimGun");
    }

    void LadyNearAlert()
    {
        dist = (transform.position - lady.transform.position).magnitude;
        if (dist <= 4f)
        {
            if(!alertON)
            {
                alertCanvas.SetActive(true);
                alertON = true;
            }
        }
        else
        {
            if (alertON)
            {
                alertCanvas.SetActive(false);
            }               
            alertON = false;
        }
           
        
    }


    void ActivateRagdoll()
    {
        navMesh.enabled = false;
        ragdollPhy.DoRagdoll();
    }
}
