using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawnPoints : ScriptableObject {

    

	public static List<Vector3> FindPoints(GameObject temp)
    {
        Transform[] objectList = temp.GetComponentsInChildren<Transform>();

        //Debug.Log(objectList.Length);
        List<Vector3> posList = new List<Vector3>();

        foreach( Transform child in objectList)
        {
            posList.Add(child.position);
        }

        for(int i = 0; i != posList.Count; i++)
        {
            float x = Mathf.Round(posList[i].x * 10.0f) * 0.1f;
            float y = Mathf.Round(posList[i].y * 10.0f) * 0.1f;
            float z = Mathf.Round(posList[i].z * 10.0f) * 0.1f;
            posList[i] = new Vector3(x, y, z);
        }

        //debug
        //for (int i = 0; i != posList.Count; i++)
        //{
        //    Debug.Log(posList[i].x + " " + posList[i].y + " " + posList[i].z);
        //}

        List<Vector3> roomPos = FindLayout(temp, ref posList);

        //Debug.Log(roomPos.Count);

        List<Vector3> validPos = new List<Vector3>();

        for(int i = 0; i != roomPos.Count; ++i)
        {
            bool valid = CheckSpace(roomPos[i], posList);
            if(valid == true)
            {
                validPos.Add(roomPos[i]);
            }
        }

        //Debug.Log(validPos.Count);

        return validPos;
    }

    private static List<Vector3> FindLayout(GameObject layout, ref List<Vector3> posList)
    {
        List<Vector3> current = new List<Vector3>();

        switch (layout.tag)
        {
            case "Room1":
                current = PopulateRoom(-7.5f, -3.5f, 19, 9, ref posList);
                RemoveHallwayPos(ref current, layout.tag);
                break;
            case "Room2":
                current = PopulateRoom(-4.5f, -14.5f, 14, 7, ref posList);
                RemoveHallwayPos(ref current, layout.tag);
                break;
            case "Room3":
                current = PopulateRoom(-18.5f, -9.5f, 5, 13, ref posList);
                RemoveHallwayPos(ref current, layout.tag);
                break;
            case "Room4":
                current = PopulateRoom(-27.5f, -9.5f, 5, 13, ref posList);
                RemoveHallwayPos(ref current, layout.tag);
                break;
            case "Room5":
                current = PopulateRoom(-26.5f, 6.5f, 7, 8, ref posList);
                RemoveHallwayPos(ref current, layout.tag);
                break;
            case "Room6":
                current = PopulateRoom(-13.5f, 9.5f, 11, 6, ref posList);
                RemoveHallwayPos(ref current, layout.tag);
                break;
            case "Room7":
                current = PopulateRoom(2.5f, 7.5f, 7, 8, ref posList);
                RemoveHallwayPos(ref current, layout.tag);
                break;
            case "Room8":
                current = PopulateRoom(15.5f, 1.5f, 10, 11, ref posList);
                RemoveHallwayPos(ref current, layout.tag);
                break;
            case "Room9":
                current = PopulateRoom(17.5f, -14.5f, 7, 10, ref posList);
                RemoveHallwayPos(ref current, layout.tag);
                break;

        }

        return current;
    }

    private static bool CheckSpace(Vector3 roomPos, List<Vector3> posList)
    {
        int count = 0;

        if(posList.Contains(roomPos))
        {
            return false;
        }

        if(posList.Contains(new Vector3(roomPos.x + 1, roomPos.y, roomPos.z)))
        {
            count++;
        }
        if(posList.Contains(new Vector3(roomPos.x - 1, roomPos.y, roomPos.z)))
        {
            count++;
        }
        if(posList.Contains(new Vector3(roomPos.x, roomPos.y, roomPos.z + 1)))
        {
            count++;
        }
        if(posList.Contains(new Vector3(roomPos.x, roomPos.y, roomPos.z - 1)))
        {
            count++;
        }

        if(count >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //this is the code from the original room generator -- needs adjusting for entire room
    //
    //private void RoomByGeneration()
    //{

    //    //room 1
    //    PopulateRoom(-6.5f, -2.5f, 17, 7);
    //    //room 2
    //    PopulateRoom(-3.5f, -13.5f, 12, 5);
    //    //room 3
    //    PopulateRoom(-17.5f, -8.5f, 3, 11);
    //    //room 4
    //    PopulateRoom(-26.5f, -8.5f, 3, 11);
    //    //room 5
    //    PopulateRoom(-25.5f, 7.5f, 5, 6);
    //    //room 6
    //    PopulateRoom(-12.5f, 10.5f, 9, 4);
    //    //room 7
    //    PopulateRoom(3.5f, 8.5f, 5, 6);
    //    //room 8
    //    PopulateRoom(16.5f, 2.5f, 8, 8);
    //    //room 9
    //    PopulateRoom(18.5f, -13.5f, 5, 8);

    //}

    //experimental function that spawns large items
    private static List<Vector3> PopulateRoom(float xPos, float yPos, int width, int height, ref List<Vector3> posList)
    {

        //add borders to the posList using dimensions given
        float ogX = xPos;
        float ogY = yPos;

        for (int i = 0; i != height + 2; ++i)
        {
            for (int j = 0; j != width + 2; ++j)
            {
                if (i == 0 || j == 0 || i == height + 1 || j == width + 1)
                {
                    posList.Add(new Vector3(xPos - 1, 0, yPos - 1));
                }

                ++xPos;
            }
            ++yPos;
            xPos = ogX;
        }


        //find available points in map
        List<Vector3> set = new List<Vector3>();
        xPos = ogX;
        yPos = ogY;

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

        return set;
    }

    //current is whats returned as the valid positions to use, so this eliminates the positions in each hallway location
    private static void RemoveHallwayPos(ref List<Vector3> current, string tag)
    {
        switch(tag)
        {
            case "Room1":
                for (int i = 0; i != room1.Length; i++)
                {
                    if(current.Contains(room1[i]))
                    {
                        current.Remove(room1[i]);
                    }
                }
                break;
            case "Room2":
                for (int i = 0; i != room2.Length; i++)
                {
                    if (current.Contains(room2[i]))
                    {
                        current.Remove(room2[i]);
                    }
                }
                break;
            case "Room3":
                for (int i = 0; i != room3.Length; i++)
                {
                    if (current.Contains(room3[i]))
                    {
                        current.Remove(room3[i]);
                    }
                }
                break;
            case "Room4":
                for (int i = 0; i != room4.Length; i++)
                {
                    if (current.Contains(room4[i]))
                    {
                        current.Remove(room4[i]);
                    }
                }
                break;
            case "Room5":
                for (int i = 0; i != room5.Length; i++)
                {
                    if (current.Contains(room5[i]))
                    {
                        current.Remove(room5[i]);
                    }
                }
                break;
            case "Room6":
                for (int i = 0; i != room6.Length; i++)
                {
                    if (current.Contains(room6[i]))
                    {
                        current.Remove(room6[i]);
                    }
                }
                break;
            case "Room7":
                for (int i = 0; i != room7.Length; i++)
                {
                    if (current.Contains(room7[i]))
                    {
                        current.Remove(room7[i]);
                    }
                }
                break;
            case "Room8":
                for (int i = 0; i != room8.Length; i++)
                {
                    if (current.Contains(room8[i]))
                    {
                        current.Remove(room8[i]);
                    }
                }
                break;
            case "Room9":
                for (int i = 0; i != room9.Length; i++)
                {
                    if (current.Contains(room9[i]))
                    {
                        current.Remove(room9[i]);
                    }
                }
                break;

        }
    }

    //vector positions in front of hallways
    static Vector3[] room1 =
    {
        new Vector3(-7.5f, 0, 4.5f),
        new Vector3(-6.5f, 0, 4.5f),
        new Vector3(10.5f, 0, 4.5f),
        new Vector3(10.5f, 0, 3.5f),
        new Vector3(3.5f, 0, -3.5f),
        new Vector3(4.5f, 0, -3.5f),
        new Vector3(-7.5f, 0, 2.5f),
        new Vector3(-7.5f, 0, 1.5f)
    };

    static Vector3[] room2 =
    {
        new Vector3(3.5f, 0, -8.5f),
        new Vector3(4.5f, 0, -8.5f),
        new Vector3(8.5f, 0, -13.5f),
        new Vector3(8.5f, 0, -14.5f),
        new Vector3(-4.5f, 0, -8.5f),
        new Vector3(-4.5f, 0, -9.5f)
    };

    static Vector3[] room3 =
    {
        new Vector3(-14.5f, 0, -8.5f),
        new Vector3(-14.5f, 0, -9.5f),
        new Vector3(-18.5f, 0, -7.5f),
        new Vector3(-18.5f, 0, -6.5f),
        new Vector3(-18.5f, 0, -0.5f),
        new Vector3(-18.5f, 0, 0.5f),
        new Vector3(-14.5f, 0, 2.5f),
        new Vector3(-14.5f, 0, 1.5f)
    };

    static Vector3[] room4 =
    {
        new Vector3(-23.5f, 0, -7.5f),
        new Vector3(-23.5f, 0, -6.5f),
        new Vector3(-23.5f, 0, -0.5f),
        new Vector3(-23.5f, 0, 0.5f),
        new Vector3(-25.5f, 0, 2.5f),
        new Vector3(-24.5f, 0, 2.5f)
    };

    static Vector3[] room5 =
    {
        new Vector3(-25.5f, 0, 6.5f),
        new Vector3(-24.5f, 0, 6.5f),
        new Vector3(-20.5f, 0, 10.5f),
        new Vector3(-20.5f, 0, 9.5f)
    };

    static Vector3[] room6 =
    {
        new Vector3(-14.5f, 0, 10.5f),
        new Vector3(-13.5f, 0, 9.5f),
        new Vector3(-7.5f, 0, 9.5f),
        new Vector3(-6.5f, 0, 9.5f),
        new Vector3(-3.5f, 0, 14.5f),
        new Vector3(-3.5f, 0, 13.5f)
    };

    static Vector3[] room7 =
    {
        new Vector3(2.5f, 0, 14.5f),
        new Vector3(2.5f, 0, 13.5f),
        new Vector3(8.5f, 0, 11.5f),
        new Vector3(8.5f, 0, 10.5f)
    };

    static Vector3[] room8 =
    {
        new Vector3(15.5f, 0, 11.5f),
        new Vector3(15.5f, 0, 10.5f),
        new Vector3(15.5f, 0, 4.5f),
        new Vector3(15.5f, 0, 3.5f),
        new Vector3(19.5f, 0, 1.5f),
        new Vector3(20.5f, 0, 1.5f)
    };

    static Vector3[] room9 =
    {
        new Vector3(19.5f, 0, -5.5f),
        new Vector3(20.5f, 0, -5.5f),
        new Vector3(17.5f, 0, -13.5f),
        new Vector3(17.5f, 0, -14.5f)
    };
}


