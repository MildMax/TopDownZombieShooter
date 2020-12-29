using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteAmmoScript : MonoBehaviour {

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
            playerMover.isInfinite = true;
            playerMover.infiniteCountDown = 5f;
            playerMover.slidersOnScreen.Add(playerMover.infiniteScreenPos);
            Destroy(gameObject);
        }
    }
}
