using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldRandomRoomGenerator : MonoBehaviour {

    //spawnpoints by room
    public Vector3[] room1;
    public Vector3[] room2;
    public Vector3[] room3;
    public Vector3[] room4;
    public Vector3[] room5;
    public Vector3[] room6;
    public Vector3[] room7;
    public Vector3[] room8;
    public Vector3[] room9;

    public GameObject[] objectMaterials;

    List<GameObject> objectsInScene = new List<GameObject>();
    List<Vector3> worldPointsLeft = new List<Vector3>();

    //objects to be randomly generated

    public GameObject[] worldPickUps;
    int[] rots = new int[4] { 0, 90, 180, 270 };

    Vector3[] boxedIn = {
        //up, down, left, right
        new Vector3(1, 0, 0),
        //new Vector3(2, 0, 0),
        new Vector3(-1, 0, 0),
        //new Vector3(-2, 0 , 0),
        new Vector3(0, 0, 1),
        //new Vector3(0, 0, 2),
        new Vector3(0, 0, -1)
        //new Vector3(0, 0, -2),
    };

    private void RoomByGeneration()
    {

        //room 1
        ObjectSpawner(-6.5f, -2.5f, 17, 7);
        //room 2
        ObjectSpawner(-3.5f, -13.5f, 12, 5);
        //room 3
        ObjectSpawner(-17.5f, -8.5f, 3, 11);
        //room 4
        ObjectSpawner(-26.5f, -8.5f, 3, 11);
        //room 5
        ObjectSpawner(-25.5f, 7.5f, 5, 6);
        //room 6
        ObjectSpawner(-12.5f, 10.5f, 9, 4);
        //room 7
        ObjectSpawner(3.5f, 8.5f, 5, 6);
        //room 8
        ObjectSpawner(16.5f, 2.5f, 8, 8);
        //room 9
        ObjectSpawner(18.5f, -13.5f, 5, 8);

    }



    //experimental function that spawns large items
    private void ObjectSpawner(float xPos, float yPos, int width, int height)
    {
        int numOfObjects = width * height / 12;

        //find available points in map
        List<Vector3> set = new List<Vector3>();
        float ogX = xPos;

        for (int i = 0; i != height; ++i)
        {
            for (int j = 0; j != width; ++j)
            {
                set.Add(new Vector3(xPos, 0f, yPos));
                ++xPos;
            }
            ++yPos;
            xPos = ogX;
        }

        //find objects that will fit in map
        int numInstantiated = 0;

        while (numInstantiated != numOfObjects)
        {
            int randomPoint = Random.Range(0, set.Count - 1);

            GameObject temp = objectMaterials[Random.Range(0, objectMaterials.Length - 1)];
            bool fits = DetermineIfFits(temp, set[randomPoint], ref set);

            if (fits == false)
            {
                continue;
            }
            else
            {
                ++numInstantiated;
            }
        }

        //put remaining world points in a list to randomly generate world pickups
        for (int i = 0; i != set.Count; i++)
        {
            worldPointsLeft.Add(set[i]);
        }
    }

    private bool DetermineIfFits(GameObject temp, Vector3 randomPoint, ref List<Vector3> set)
    {
        int randomRotate = rots[Random.Range(0, 3)];

        Vector3[] size = ObjectMaterialSizes.GetSizeByName(temp.name, randomPoint, randomRotate);

        bool fits = true;

        //see if the size of the object fits within constraints of the room
        for (int i = 0; i != size.Length; ++i)
        {
            fits = set.Contains(size[i]);
            if (fits == false)
            {
                return fits;
            }
        }

        objectsInScene.Add(Instantiate(temp, randomPoint, Quaternion.Euler(0f, randomRotate, 0f)));

        //remove points from set
        for (int i = 0; i != size.Length; ++i)
        {
            set.Remove(size[i]);

        }

        return fits;
    }

    private void ClearObjects()
    {
        for (int i = 0; i != objectsInScene.Count; ++i)
        {
            Destroy(objectsInScene[i]);
        }
        worldPointsLeft.Clear();
    }

    private void SetWorldPickUps()
    {
        for (int i = 0; i != worldPickUps.Length; ++i)
        {
            int randomPoint = Random.Range(0, worldPointsLeft.Count);
            bool fits = CheckIfBoxedIn(worldPointsLeft[randomPoint]);
            if (fits == true)
            {
                Instantiate(worldPickUps[i], worldPointsLeft[randomPoint], Quaternion.Euler(90f, 0f, 0f));
                worldPointsLeft.Remove(worldPointsLeft[randomPoint]);
            }
            else
            {
                --i;
            }
        }
    }

    private bool CheckIfBoxedIn(Vector3 point)
    {
        int count = 0;

        for (int i = 0; i != boxedIn.Length; i++)
        {
            if (!worldPointsLeft.Contains(point + boxedIn[i]))
            {
                count++;
            }
        }

        if (count == 3)
        {
            return true;
        }

        return false;
    }
}
