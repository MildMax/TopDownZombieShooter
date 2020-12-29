using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootScript : MonoBehaviour {

    public GameObject shot;
    public float waitShot;
    public AnimationClip melee;
    public AnimationClip reload;
    public AudioClip meleeSound;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool isReloading = false;
    [HideInInspector] public Animator animator;

    protected Text ammoText;

    protected float nextShot = 0f;
    protected bool canShoot = false;
    protected bool isMelee = false;

    protected int ammo = 12;

    
    BoxCollider boxCollider;
    ZombieMover zombieMover;
    protected PlayerMover playerMover;
    protected AudioSource shootSound;

    protected virtual void Awake()
    {
        nextShot = waitShot;
        animator = GetComponentInParent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        playerMover = GetComponentInParent<PlayerMover>();
        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
        shootSound = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        CheckConditions();
        UpdateAmmoText();
        
    }

    protected virtual void FixedUpdate()
    {
        if(canShoot == true)
        {
            Shoot();
        }
        nextShot += Time.deltaTime;
        if (isReloading == false)
        {
            checkReload(ammo);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Zombie")
        {
            zombieMover = other.GetComponent<ZombieMover>();
            if (playerMover.isOnePunchMan == true)
            {
                zombieMover.DamageHealth(100);
            }
            else
            {
                zombieMover.DamageHealth(50);
            }
        }
    }

    protected virtual void CheckConditions()
    {
        if (Input.GetButtonDown("Melee") && isMelee == false && isRunning == false)
        {
            StartCoroutine(Melee());
        }

        else if (isMelee == false && isRunning == false && isReloading == false && Input.GetButtonDown("Fire1") && waitShot <= nextShot && playerMover.health > 0)
        {
            canShoot = true;
        }
    }

    protected virtual void Shoot()
    {
        animator.SetTrigger("Shoot");
        shootSound.Play();
        GameObject setDamage = Instantiate(shot, transform.position, transform.rotation);
        ShotMover script = setDamage.GetComponent<ShotMover>();
        script.damage = 25;
        nextShot = 0f;
        ammo -= 1;
        canShoot = false;
    }

    protected IEnumerator Melee()
    {
        canShoot = false;
        isMelee = true;
        animator.SetTrigger("Melee");
        AudioClip current = shootSound.clip;
        shootSound.clip = meleeSound;
        

        float animTime = melee.length - 0.1f;
        float waitTime = 0f;

        while(waitTime < animTime * 0.5f)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }

        shootSound.Play();
        waitTime = 0f;

        while (waitTime < animTime * 0.1f)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }

        waitTime = 0f;
        //attack frames
        
        boxCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        boxCollider.enabled = false;

        while (waitTime < animTime * 0.4f)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }
        shootSound.clip = current;

        isMelee = false;
        canShoot = true;
    }

    protected virtual void checkReload(int ammo)
    {
        if(ammo == 0 || (ammo!= 12 && Input.GetButton("Reload")))
        {
            StartCoroutine(Reload());
        }
    }

    protected virtual IEnumerator Reload()
    {

        isReloading = true;
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(reload.length);
        ammo = 12;
        isReloading = false;
    }

    virtual protected void UpdateAmmoText()
    {
        ammoText.text = ammo + "/inf";
    }
}
