using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    [SerializeField] EnemyController _enemyController;
    

    public void EnemyDead()
    {
        GameManager.instance.uiManager.HeadShot(transform.position);
        _enemyController.EnemyDead();
    }
}
