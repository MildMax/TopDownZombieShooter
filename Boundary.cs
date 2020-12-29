﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Shot")
        {
            Destroy(other.gameObject);
        }
    }
}
