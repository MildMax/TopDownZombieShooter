﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway11Spawner : RoomSpawner {

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room6, GameController.instance.room7));
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
    }
}
