using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway7Spawner : RoomSpawner {

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room3, GameController.instance.room4));
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
    }
}
