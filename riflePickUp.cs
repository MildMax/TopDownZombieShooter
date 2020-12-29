using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class riflePickUp : MonoBehaviour {

    float time;

    private void Update()
    {
        if(time >= 7f)
        {
            Destroy(gameObject);
        }
        time += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerMover shoot = other.GetComponentInChildren<PlayerMover>();
            shoot.rifleTotalAmmo += 60;
            Destroy(gameObject);
        }
    }

}
