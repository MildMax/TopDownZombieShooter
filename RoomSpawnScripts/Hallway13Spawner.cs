using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway13Spawner : RoomSpawner {

    public Vector3[] Room8;
    public Vector3[] Room9;

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room8, GameController.instance.room9));
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
    }
}
