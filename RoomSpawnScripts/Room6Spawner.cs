using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room6Spawner : RoomSpawner {

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room1, GameController.instance.room5, GameController.instance.room7));
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
    }
}
