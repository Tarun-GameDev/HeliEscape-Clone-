using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UIManager uiManager;
    public AudioManager audioManager;
    [SerializeField] HeliCopter heliCopter;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StopHeliMove()
    {
        heliCopter.ladyDead = true;
    }

    /*
    public void GameOver()
    {
        EnemyController[] _enemy = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemies in _enemy)
        {
            if (enemies != null)
                enemies.gameOver = true;
        }
    }*/
}
