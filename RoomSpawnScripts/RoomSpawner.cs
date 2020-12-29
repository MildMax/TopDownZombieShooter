using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomSpawner : MonoBehaviour {

    Collider coll;

    private void Awake()
    {
        coll = GetComponent<BoxCollider>();
        StartCoroutine(Wait());
    }

    private void Update()
    {
        if(GameController.instance.zombiesSpawned == GameController.instance.zombiesStart)
        {
            coll.enabled = false;
        }
        else if(coll.enabled == false && GameController.instance.zombiesSpawned < GameController.instance.zombiesStart)
        {
            coll.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            MakeGo();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            MakeNo();
        }
    }

    protected virtual void MakeGo()
    {
        StartCoroutine(SpawnByRoom());
    }

    protected virtual void MakeNo()
    {
        StopCoroutine(SpawnByRoom());
    }

    public IEnumerator SpawnByRoom(params Vector3[][] list)
    {
        while (GameController.instance.player != null)
        {
            if (GameController.instance.waitSpawn >= GameController.instance.spawnTime && GameController.instance.betweenRounds == false
                && GameController.instance.zombiesSpawned != GameController.instance.zombiesStart)
            {

                SpawnZombies(list[Random.Range(0, list.Length)]);
                GameController.instance.waitSpawn = 0f;
                yield return null;
                
            }
            GameController.instance.waitSpawn += Time.deltaTime;
            yield return null;
        }
        //yield return null;
    }

    private void SpawnZombies(Vector3[] spawnPoints)
    {
        int pos = Random.Range(0, spawnPoints.Length - 1);
        Instantiate(GameController.instance.zombie, spawnPoints[pos], Quaternion.identity);
        Debug.Log("x: " + spawnPoints[pos].x + " y: " + spawnPoints[pos].y + " z: " + spawnPoints[pos].z);
        Debug.Log(gameObject.name);
        ++GameController.instance.zombiesSpawned;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        coll.enabled = true;
    }
}
