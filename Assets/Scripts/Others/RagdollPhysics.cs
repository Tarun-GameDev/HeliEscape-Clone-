using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPhysics : MonoBehaviour
{
    [SerializeField] Collider[] MainCollider;
    [SerializeField] Collider[] AllColliders;
    [SerializeField] Rigidbody[] AllRb;
    [SerializeField] Rigidbody mainRb;
    [SerializeField] Animator animator;

    private void Awake()
    {
        //MainCollider = GetComponents<Collider>();
        AllColliders = GetComponentsInChildren<Collider>();
        //mainRb = GetComponent<Rigidbody>();
        AllRb = GetComponentsInChildren<Rigidbody>();

        foreach (var rb in AllRb)
        {
            rb.isKinematic = true;
        }

        if(mainRb != null)
            mainRb.isKinematic = false;
        animator.enabled = true;

        DoRagdol(false);
    }


    public void DoRagdoll()
    {
        StartCoroutine(dor(true));
    }

    IEnumerator dor(bool isRagdoll)
    {
        DoRagdol(isRagdoll);
        yield return new WaitForEndOfFrame();
        DoRagdoll(isRagdoll);
    }

    void DoRagdoll(bool isRagdoll)
    {
        foreach (var rb in AllRb)
        {
            rb.isKinematic = !isRagdoll;
        }

        if (mainRb != null)
            mainRb.isKinematic = isRagdoll;
        animator.enabled = !isRagdoll;
    }

    void DoRagdol(bool isRagdoll)
    {
        
        foreach (var col in AllColliders)
        {
            col.enabled = isRagdoll;
        }

        foreach (var col in MainCollider)
        {
            col.enabled = !isRagdoll;
        }
        if (mainRb != null)
            mainRb.useGravity = !isRagdoll;
        animator.enabled = !isRagdoll;
    }
}
