using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway10Spawner : RoomSpawner {

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room5, GameController.instance.room6));
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
    }
}
