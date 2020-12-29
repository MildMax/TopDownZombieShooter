using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHitKillScript : MonoBehaviour {

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
            playerMover.isOnePunchMan = true;
            playerMover.onePunchManCountDown = 5f;
            playerMover.slidersOnScreen.Add(playerMover.oneHitKillScreenPos);
            Destroy(gameObject);
        }
    }
}
