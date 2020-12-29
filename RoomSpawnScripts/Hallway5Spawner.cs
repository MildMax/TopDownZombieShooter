using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway5Spawner : RoomSpawner {

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room2, GameController.instance.room9));
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
    }
}
