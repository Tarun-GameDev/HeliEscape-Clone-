using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{

    public GameObject Lady;
    [SerializeField] Animator ladyAnimator;
    public GameObject mainHeliCopter;
    public GameObject EndHeliCopter;
    [SerializeField] Animator heliCopterAnimator;
    [SerializeField] TimeManager timeManager;
    [SerializeField] GameObject allEnemies;
    

    private void Start()
    {
        if (Lady == null)
            Lady = LadyController.instance.gameObject;

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Lady"))
        {
            StartCoroutine(LevelCompleted());
            //activae level complete menu after 2 sec
        }
    }

    IEnumerator LevelCompleted()
    {
        Lady.SetActive(false);
        EndHeliCopter.SetActive(true);
        mainHeliCopter.SetActive(false);
        ladyAnimator.SetTrigger("Hang");
        yield return new WaitForSecondsRealtime(.5f);
        timeManager.StartSetSlowMotion(.25f);
        allEnemies.SetActive(false);
        yield return new WaitForSecondsRealtime(2f);
        GameManager.instance.uiManager.LevelCompleted();
        yield return new WaitForSecondsRealtime(3f);
        timeManager.StopSlowMotion();
        heliCopterAnimator.SetTrigger("MoveHeli");
    }
}
