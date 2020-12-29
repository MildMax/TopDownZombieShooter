using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway1Spawner : RoomSpawner {

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room1, GameController.instance.room3));
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
    }
}
