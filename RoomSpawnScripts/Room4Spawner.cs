using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room4Spawner : RoomSpawner {

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room3, GameController.instance.room5));
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
    }
}
