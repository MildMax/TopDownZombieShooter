using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Pathfinding;

public class ZombieMover : MonoBehaviour {

    public GameObject rifleAmmo;
    public GameObject shotgunAmmo;
    public AudioClip[] clips;
    public AudioClip deathClip;

    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public int health = 100;
    [HideInInspector] public int pickUpStatus = 0;
    [HideInInspector] public bool inRange = false;

    Transform player;
    PlayerMover playerMover;
    NavMeshAgent nav;
    NavMeshPath path;
    Rigidbody body;
    Animator animator;
    Slider healthSlider;
    AIPath aiPath;
    AudioSource damageSound;
    CapsuleCollider capsule;
    float wait;
    float waitTime = 0.25f;
    bool isPlayingDamageSound = false;
    bool isDying = false;

    // Use this for initialization
    void Awake()
    {
        //nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMover = player.GetComponent<PlayerMover>();
        body = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        healthSlider = GetComponentInChildren<Slider>();
        wait = waitTime;
        path = new NavMeshPath();
        aiPath = GetComponent<AIPath>();
        damageSound = GetComponent<AudioSource>();
        capsule = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (isAttacking == true || player == null)
        {
            animator.SetBool("IsWalking", false);
            body.velocity = Vector3.zero;
            body.isKinematic = true;
            //nav.velocity = new Vector3(0f, 0f, 0f);
        }
        else
        {
            if(body.isKinematic == true)
            {
                body.isKinematic = false;
            }
            if (health > 0)
            {
                animator.SetBool("IsWalking", true);
            }
            //nav.CalculatePath(player.position, path);
            //if (path.status == NavMeshPathStatus.PathComplete)
            //{
            //    nav.SetDestination(player.position);
            //    //wait = 0f;
            //}
            //nav.SetDestination(player.position);
        }

        wait += Time.deltaTime;
        StartCoroutine(CheckHealth());
    }


    private void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(90f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        healthSlider.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        healthSlider.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.55f);
    }

    public void DamageHealth(int damage)
    {
        if (playerMover.isOnePunchMan == true)
        {
            health = 0;
        }
        else
        {
            StartCoroutine(PlaySound());
            health -= damage;
            healthSlider.value = health;
        }
    }
    
    private IEnumerator PlaySound()
    {
        if (isPlayingDamageSound == false)
        {
            isPlayingDamageSound = true;
            damageSound.clip = clips[Random.Range(0, clips.Length)];
            damageSound.Play();
            yield return new WaitForSeconds(damageSound.clip.length);
            isPlayingDamageSound = false;
        }
    }

    private IEnumerator CheckHealth()
    {
        if (health <= 0 && isDying == false)
        {
            isDying = true;
            MakeInert();
            damageSound.Stop();
            damageSound.clip = deathClip;
            damageSound.Play();
            GameController.instance.zombiesLeft -= 1;

            yield return new WaitForSeconds(deathClip.length);
            
            MakePickup();
            
            Destroy(gameObject);
        }
    }
    
    private void MakePickup()
    {
        if(pickUpStatus == 1)
        {
            Instantiate(rifleAmmo, transform.position, Quaternion.Euler(90f, 0f, 0f));
        }
        else if (pickUpStatus == 2)
        {
            Instantiate(shotgunAmmo, transform.position, Quaternion.Euler(90f, 0f, 0f));
        }
    }

    private void MakeInert()
    {
        capsule.enabled = false;
        animator.SetBool("IsWalking", false);
        body.velocity = Vector3.zero;
        body.isKinematic = true;
        aiPath.enabled = false;
    }
}
