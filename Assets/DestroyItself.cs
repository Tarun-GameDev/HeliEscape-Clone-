using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : MonoBehaviour
{

    [SerializeField] float destroyTime = 3f;

    void Start()
    {
        Invoke("Destroy", destroyTime);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
 
}
