using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliCopter : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    public bool ladyDead = false;
    void Update()
    {
        if(!ladyDead)
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
