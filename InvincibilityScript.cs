using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityScript : MonoBehaviour {

    private void Awake()
    {
        StartCoroutine(DestroyObject());
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(7);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMover playerMover = other.GetComponent<PlayerMover>();
            playerMover.isInvincible = true;
            playerMover.invincibilityCountDown = 5f;
            playerMover.slidersOnScreen.Add(playerMover.invincibilityScreenPos);
            Destroy(gameObject);
        }
    }
} 
