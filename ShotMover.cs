using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMover : MonoBehaviour {

    ZombieMover zombieMover;
    Rigidbody body;
    public float speed;

    [HideInInspector] public int damage = 0;

	// Use this for initialization
	void Awake () {
        body = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.Euler(90f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        body.velocity = transform.right * speed;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shot" || other.tag == "NonInteractable")
        {
            return;
        }

        else if (other.tag == "Zombie")
        {
            zombieMover = other.GetComponent<ZombieMover>();
            zombieMover.DamageHealth(damage);
        }

        Destroy(gameObject);
    }
}
