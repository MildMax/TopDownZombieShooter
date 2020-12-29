using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1Spawner : RoomSpawner {

    Coroutine lastCoroutine;

    protected override void MakeGo()
    {
        lastCoroutine = StartCoroutine(SpawnByRoom(GameController.instance.room2, GameController.instance.room3, 
            GameController.instance.room6, GameController.instance.room8, GameController.instance.room1));
        //Debug.Log("Coroutine started");
    }

    protected override void MakeNo()
    {
        StopCoroutine(lastCoroutine);
        //Debug.Log("Coroutine stopped");
    }
}
