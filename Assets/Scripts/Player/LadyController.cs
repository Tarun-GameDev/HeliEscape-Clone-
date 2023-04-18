using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LadyController : MonoBehaviour
{
    public static LadyController instance;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] Transform[] movePointsTran;
    [SerializeField] Transform activePointTran;
    [SerializeField] float distToPoint;
    Rigidbody rb;
    public static bool ladyDead = false;
    [SerializeField] TimeManager timeManager;
    [SerializeField] UIManager uiManager;
    

    int i = 0;
    int finalPoint;
    bool finalPointReached;
    //Quaternion initialRota;
    [SerializeField] bool inSlowMo = false;
    [SerializeField] LayerMask enemyLayerMask;
    [SerializeField] EnemyController _enemy;
    [SerializeField] RagdollPhysics ragdoll;

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

    private void Start()
    {
        inSlowMo = false;
        ladyDead = false;
        //initialRota = Quaternion.Euler(new Vector3(this.transform.localRotation.eulerAngles.x, this.transform.localRotation.eulerAngles.y, this.transform.localRotation.eulerAngles.z));
        finalPoint = movePointsTran.Length;

        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (uiManager == null)
            uiManager = UIManager.instance;
        if (ragdoll == null)
            ragdoll = GetComponent<RagdollPhysics>();

    }

    void Update()
    {
        if (ladyDead)
            return;

        if(!finalPointReached)
        {
            if (activePointTran != null)
            {
                distToPoint = (activePointTran.position - transform.position).magnitude;
                if (distToPoint >= 2)
                {
                    //transform.LookAt(activePointTran);
                    var lookPos = activePointTran.position - transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

                    transform.position += transform.forward * moveSpeed * Time.deltaTime;

                }
                else
                {
                    if (i != finalPoint)
                        activePointTran = MoveToNextPoint();
                    else
                        finalPointReached = true;
                }

            }
            else
                activePointTran = MoveToNextPoint();

            EnemyNearAlert();
        }
    }

    Transform MoveToNextPoint()
    {
        Transform _movePoint = movePointsTran[i];
        i++;
        return _movePoint;
    }

    public void Dead()
    {
        ladyDead = true;
        //GameManager.instance.GameOver();
        Player.instance.ladyDead = true;
        GameManager.instance.StopHeliMove();
        EnemyAimGun();
        if (ragdoll != null)
            ragdoll.DoRagdoll();
        StartCoroutine(delayDead());
    }

    IEnumerator delayDead()
    {
        yield return new WaitForSeconds(1f);
        if (timeManager != null)
            timeManager.StopSlowMotion();

        if (uiManager != null)
            uiManager.LevelLost();
        //UIManager.instance.LevelLost();
    }

    void EnemyAimGun()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, 2f,enemyLayerMask);
        foreach (Collider nearbyObject in collider)
        {
            EnemyController _enemy1 = nearbyObject.GetComponent<EnemyController>();
            if (_enemy1 != null)
                _enemy1.AimGunAtEnd();
        }
    }

    void EnemyNearAlert()
    {
        _enemy = null;
        Collider[] collider = Physics.OverlapSphere(transform.position, 3f,enemyLayerMask);
        foreach (Collider nearbyObject in collider)
        {
            _enemy = nearbyObject.GetComponent<EnemyController>();
            if (_enemy != null && !inSlowMo)
            {
                inSlowMo = true;
                timeManager.StartSlowMotion();
            }
        }

        if (_enemy == null && inSlowMo)
        {
            inSlowMo = false;
            timeManager.StopSlowMotion();
        }
    }

    public void LevelCompleted()
    { 

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position, 10f);
    }
}
