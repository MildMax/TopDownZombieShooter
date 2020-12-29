using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway9Spawner : RoomSpawner {

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room4, GameController.instance.room5));
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
    }
}
