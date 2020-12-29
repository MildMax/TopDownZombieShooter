using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShoot : ShootScript {

    public float[] shotAngles = new float[5];

    int shotgunAmmo = 7;

    PlayerMover player;

    protected override void Awake()
    {
        base.Awake();
        waitShot = shootSound.clip.length;
    }

    private void Start()
    {
        player = GetComponentInParent<PlayerMover>();
    }

    protected override void Update()
    {
        CheckConditions();
        UpdateAmmoText();
    }

    protected override void FixedUpdate()
    {
        if (canShoot == true)
        {
            Shoot();
        }
        nextShot += Time.deltaTime;
        if (isReloading == false && player.shotgunTotalAmmo != 0)
        {
            checkReload(shotgunAmmo);
        }
    }

    protected override void CheckConditions()
    {
        if (Input.GetButtonDown("Melee") && isMelee == false && isRunning == false)
        {
            StartCoroutine(Melee());
        }

        else if (isMelee == false && isRunning == false && shotgunAmmo != 0 && Input.GetButtonDown("Fire1") && waitShot <= nextShot && playerMover.health > 0)
        {
            canShoot = true;
        }
    }

    protected override void Shoot()
    {
        
        if (waitShot <= nextShot)
        {
            shootSound.Play();
            for (int i = 0; i < shotAngles.Length; i++)
            {
                GameObject setDamage = Instantiate(shot, transform.position, transform.rotation * Quaternion.Euler(1f, 1f, shotAngles[i]));
                ShotMover script = setDamage.GetComponent<ShotMover>();
                script.damage = 35;
                Destroy(setDamage, 0.5f);
            }
            nextShot = 0f;
            shotgunAmmo -= 1;
            canShoot = false;
            
        }
    }

    protected override void checkReload(int rifleAmmo)
    {
        if (shotgunAmmo == 0 || (shotgunAmmo != 7 && Input.GetButton("Reload")))
        {
            StartCoroutine(Reload());
        }
    }

    protected override IEnumerator Reload()
    {
        isReloading = true;
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(reload.length);

        if(player.isInfinite == true)
        {
            shotgunAmmo = 7;
        }
        else if (player.shotgunTotalAmmo >= 7)
        {
            player.shotgunTotalAmmo -= 7 - shotgunAmmo;
            shotgunAmmo = 7;
        }
        else
        {
            if (shotgunAmmo + player.shotgunTotalAmmo > 7)
            {
                int diff = 7 - shotgunAmmo;
                shotgunAmmo += diff;
                player.shotgunTotalAmmo -= diff;
            }
            else
            {
                shotgunAmmo += player.shotgunTotalAmmo;
                player.shotgunTotalAmmo = 0;
            }
        }
        isReloading = false;

    }

    protected override void UpdateAmmoText()
    {
        ammoText.text = shotgunAmmo + "/" + player.shotgunTotalAmmo;
    }
}
