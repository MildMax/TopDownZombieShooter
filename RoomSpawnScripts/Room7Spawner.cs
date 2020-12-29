﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room7Spawner : RoomSpawner {

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room6, GameController.instance.room8));
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
    }
}
