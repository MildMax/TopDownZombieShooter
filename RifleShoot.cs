using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShoot : ShootScript {

    int rifleAmmo = 30;
    PlayerMover player;
    AudioSource[] sources;

    protected override void Awake()
    {
        base.Awake();
        //shootSound = null;
        sources = GetComponents<AudioSource>();
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
        if (isReloading == false && player.rifleTotalAmmo != 0)
        {
            checkReload(rifleAmmo);
        }
    }

    protected override void CheckConditions()
    {
        if (Input.GetButtonDown("Melee") && isMelee == false && isRunning == false)
        {
            StartCoroutine(Melee());
        }

        else if (isMelee == false && isRunning == false && isReloading == false && Input.GetButton("Fire1") && waitShot <= nextShot && playerMover.health > 0)
        {
            canShoot = true;
        }
    }

    protected override void Shoot()
    {
        if (rifleAmmo != 0 || player.rifleTotalAmmo != 0)
        {
            animator.SetTrigger("Shoot");
            for(int i = 0; i != sources.Length; i++)
            {
                if(sources[i].isPlaying == false)
                {
                    sources[i].Play();
                    break;
                }
            }
            GameObject setDamage = Instantiate(shot, transform.position, transform.rotation);
            ShotMover script = setDamage.GetComponent<ShotMover>();
            script.damage = 15;
            nextShot = 0f;
            rifleAmmo -= 1;
            canShoot = false;
        }
    }

    protected override void checkReload(int rifleAmmo)
    {
        if (rifleAmmo == 0 || (rifleAmmo != 30 && Input.GetButton("Reload")))
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
            rifleAmmo = 30;
        }
        else if (player.rifleTotalAmmo >= 30)
        {
            player.rifleTotalAmmo -= 30 - rifleAmmo;
            rifleAmmo = 30;
        }
        else
        {
            if (rifleAmmo + player.rifleTotalAmmo > 30)
            {
                int diff = 30 - rifleAmmo;
                rifleAmmo += diff;
                player.rifleTotalAmmo -= diff;
            }
            else
            {
                rifleAmmo += player.rifleTotalAmmo;
                player.rifleTotalAmmo = 0;
            }
        }
        isReloading = false;
    }

    protected override void UpdateAmmoText()
    {
        ammoText.text = rifleAmmo + "/" + player.rifleTotalAmmo;
    }
}
