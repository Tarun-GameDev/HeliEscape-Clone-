using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    Camera mainCamera;
    [SerializeField] Transform targetTrans;
    [SerializeField] Transform aimgun;
    public bool ladyDead = false;

    public bool gamePaused = false;
    [SerializeField] LayerMask mouseNonTouchLayer;
    bool animSpeedZero;

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
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (ladyDead)
        {
            if(!animSpeedZero)
            {
                animSpeedZero = true;
            }        
            return;
        }

        if(!gamePaused)
        {
            Aim(targetTrans);
            aimgun.LookAt(targetTrans);
        }
    }


    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, float.MaxValue,mouseNonTouchLayer))
        {
            return (true, hitInfo.point);
        }
        else
            return (false, Vector3.zero);
    }

    void Aim(Transform _target)
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            //var direc = position - _target.position;
            //_target.forward = direc;

            _target.transform.position = position;
            //Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.position);
            //crosshair.anchoredPosition = screenPoint - canvasRectT.sizeDelta / 2f;
        }
    }


}
