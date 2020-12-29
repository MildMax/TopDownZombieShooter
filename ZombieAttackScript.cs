using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackScript : MonoBehaviour {

    public AnimationClip zombieMelee;

    ZombieMover zombieMover;
    BoxCollider myCollider;
    float colliderDistance;
    Animator animator;
    PlayerMover playerMover;

    private void Start()
    {
        //do nothing
    }

    private void Awake()
    {
        zombieMover = GetComponentInParent<ZombieMover>();
        myCollider = GetComponent<BoxCollider>();
        colliderDistance = myCollider.size.x * 0.75f;
        animator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (zombieMover.inRange == true && zombieMover.isAttacking == false && zombieMover.health > 0)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        zombieMover.isAttacking = true;
        animator.SetTrigger("Melee");

        yield return new WaitForSeconds(0.5f);
        myCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        myCollider.enabled = false;
        yield return new WaitForSeconds(0.15f);

        zombieMover.isAttacking = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger accessed");
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            
            //Debug.Log("if statement accessed");
            Debug.Log("Successful zombie attack\n");
            playerMover = other.GetComponent<PlayerMover>();
            playerMover.TakeDamage();

        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Player" && zombieMover.isAttacking == false)
    //    {
    //        //Debug.Log("if statement accessed");
    //        StartCoroutine(Attack(other));

    //    }
    //}

    //private IEnumerator Attack(Collider other)
    //{
    //    //Debug.Log("coroutine accessed");
    //    zombieMover.isAttacking = true;
    //    animator.SetTrigger("Melee");

    //    yield return new WaitForSeconds(zombieMelee.length);

    //    zombieMover.isAttacking = false;

        //float animTime = zombieMelee.length / 2;
        //float waitTime = 0f;
        //while (waitTime < animTime)
        //{
        //    waitTime += Time.deltaTime;
        //    yield return null;
        //}
        //waitTime = 0f;

        ////Debug.Log("Distance: " + Vector3.Distance(transform.position, other.gameObject.transform.position)
        ////        + "\nCollider Distance: " + colliderDistance);
        ////if (Vector3.Distance(transform.position, other.gameObject.transform.position) / 2 < colliderDistance)
        ////{
        ////    Debug.Log("Successful zombie attack\n");
        ////    playerMover = other.GetComponent<PlayerMover>();
        ////    playerMover.TakeDamage();
        ////}

        //while (waitTime < animTime)
        //{
        //    waitTime += Time.deltaTime;
        //    yield return null;
        //}
        //zombieMover.isAttacking = false;
        //Debug.Log("Coroutine finished");
    //}
}
