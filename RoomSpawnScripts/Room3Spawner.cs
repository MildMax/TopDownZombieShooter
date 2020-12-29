using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room3Spawner : RoomSpawner {

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room1, GameController.instance.room2, GameController.instance.room4));
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
    }
}
