using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryAttackScript : MonoBehaviour {

    ZombieMover zombieMover;

    private void Awake()
    {
        zombieMover = GetComponentInParent<ZombieMover>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            zombieMover.inRange = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            zombieMover.inRange = false;
        }
    }
}
