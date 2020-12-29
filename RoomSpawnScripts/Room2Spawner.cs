using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2Spawner : RoomSpawner {

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room1, GameController.instance.room3, GameController.instance.room9));
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
    }

}
