using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeWeaponScript : MonoBehaviour {

    public AnimationClip melee;
    public int damageAmount;

    private bool isMelee = false;

    Animator animator;
    BoxCollider boxCollider;
    ZombieMover zombieMover;
    Text ammoText;
    AudioSource meleeSound;


	// Use this for initialization
	void Awake () {
        animator = GetComponentInParent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
        meleeSound = GetComponent<AudioSource>();

	}

    // Update is called once per frame
    void Update () {
		if((Input.GetButtonDown("Melee") || Input.GetButtonDown("Fire1")) && isMelee == false)
        {
            StartCoroutine(Melee());
        }
        ammoText.text = "";
	}

    private IEnumerator Melee()
    {
        isMelee = true;
        animator.SetTrigger("Melee");

        float animTime = melee.length - 0.15f;
        float waitTime = 0f;

        while (waitTime < animTime * 0.5f)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }

        waitTime = 0f;
        meleeSound.Play();

        while (waitTime < animTime * 0.1f)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }
        waitTime = 0f;

        //attack frames
        boxCollider.enabled = true;
        yield return new WaitForSeconds(0.15f);
        boxCollider.enabled = false;

        while (waitTime < animTime * 0.4f)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }

        isMelee = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Zombie")
        {
            zombieMover = other.GetComponent<ZombieMover>();
            zombieMover.DamageHealth(damageAmount);
        }
    }
}
