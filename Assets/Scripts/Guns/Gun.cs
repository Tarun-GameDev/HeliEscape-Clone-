using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] Player _player;
    [Header("Gun Status")]
    public string GunName;
    [SerializeField]
    string BulletName = "Bullet";
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    bool normalBullet = true;
    [Range(0f, 100f)]
    public float velocity = 20f;
    [Range(0f, 100f)]
    public int damage;
    [SerializeField] float timeBetweenShooting, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    [SerializeField] bool allowButtonToHold;

    [Header("Bug Fixing")]
    [SerializeField] bool allowInvoke = true;

    [Header("Manager")]
    public bool playerEquipped;
    public bool enemyEquipped;

    [Space(10)]
    [Header("For Graphics")]
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] ParticleSystem bulletsEffect;



    bool shooting,reloading, readyToShoot;
    [HideInInspector]
    public int bulletsLeft,bulletsShot;
    Cinemachine.CinemachineImpulseSource source;
    Camera cam;

    [SerializeField] AudioSource shootAudio;
        
    private void Awake()
    {
        shooting = false;
        readyToShoot = true;
        bulletsLeft = magazineSize;
    }
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if(playerEquipped || !enemyEquipped)
        {
            MyInput();
            playerEquipped = true;
            
        }
        else if(enemyEquipped)
        {
            EnemyInput();
        }
    }

    void MyInput()
    {
        #region PC Controllers

        //Inpuy
        if (allowButtonToHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);


        //reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //Reload autimatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        #endregion


        //Shooting
        if (shooting && readyToShoot && !reloading && !UIManager.GameIsPaused && !LadyController.ladyDead)
        {

            Shoot();
            bulletsShot = 0;
        }
    }

    void EnemyInput()
    {
        //atomaticaly shoot
        if (readyToShoot && !reloading)
        {
            Shoot();
            bulletsShot = 0;
        }

        //Reload autimatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();
    }

    void Shoot()
    {
        readyToShoot = false;
        if (shootAudio != null)
            shootAudio.Play();
        source = GetComponent<Cinemachine.CinemachineImpulseSource>();
        if(source != null && cam != null)
            source.GenerateImpulse(cam.transform.forward);

        #region BulletSpawning

        var go = Instantiate(bullet, transform.position, transform.rotation);
        var _bullet = go.GetComponent<Bullet>();
        _bullet.Fire(attackPoint.transform.position, attackPoint.eulerAngles,velocity,gameObject.layer, damage);

        //muzzleFlash
        if (muzzleFlash != null)
        {
            Transform _flash = Instantiate(muzzleFlash, attackPoint.position, transform.rotation).transform;
            _flash.SetParent(this.transform);

        }

        if (bulletsEffect != null)
            bulletsEffect.Play();
        
        
        #endregion

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);


    }

    private void ResetShot()
    {
        //allow shootin and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

}
