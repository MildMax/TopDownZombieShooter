using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMaterialSizes : ScriptableObject {

	static public Vector3[] GetSizeByName(string name, Vector3 rP, int rR)
    {
        switch(name)
        {
            //case ("Box"):
            //    return new Vector3[1] { rP };
            case ("SquareBox"):
                return SquareRot(rP, rR);
            case ("RegularLBox"):
                return RegularLRot(rP, rR);
            case ("UBox"):
                return URot(rP, rR);
            case ("StraightBox"):
                return StraightRot(rP, rR);
            case ("CrossBox"):
                return CrossRot(rP, rR);
            case ("TheWorstTetrisBlockBox"):
                return TWTBRot(rP, rR);
            case ("HumpedBox"):
                return HumpedRot(rP, rR);
            case ("ZBox"):
                return ZRot(rP, rR);
        }

        Debug.Log("GetSizeByName did not find object: " + name);
        return null;
    }

    private static Vector3[] SquareRot(Vector3 rP, int rR)
    {
        Vector3[] rValue = new Vector3[4];

        switch(rR)
        {
            case (0):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z + 1),
                    new Vector3(rP.x + 1, rP.y, rP.z),
                    new Vector3(rP.x + 1, rP.y, rP.z + 1)
                };
                break;
            case (90):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x + 1, rP.y, rP.z),
                    new Vector3(rP.x + 1, rP.y, rP.z - 1),
                    new Vector3(rP.x, rP.y, rP.z -1)
                };
                break;
            case (180):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z - 1),
                    new Vector3(rP.x - 1, rP.y, rP.z - 1),
                    new Vector3(rP.x - 1, rP.y, rP.z)
                };
                break;
            case (270):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x - 1, rP.y, rP.z),
                    new Vector3(rP.x - 1, rP.y, rP.z + 1),
                    new Vector3(rP.x, rP.y, rP.z + 1)
                };
                break;
            default:
                Debug.Log("SquareRot is not valid: " + rR);
                rValue = null;
                break;
        }

        return rValue;
    }

    private static Vector3[] RegularLRot(Vector3 rP, int rR)
    {
        Vector3[] rValue = new Vector3[4];

        switch(rR)
        {
            case (0):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z + 1),
                    new Vector3(rP.x, rP.y, rP.z + 2),
                    new Vector3(rP.x + 1, rP.y, rP.z)
                };
                break;
            case (90):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x + 1, rP.y, rP.z),
                    new Vector3(rP.x + 2, rP.y, rP.z),
                    new Vector3(rP.x, rP.y, rP.z - 1)
                };
                break;
            case (180):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z - 1),
                    new Vector3(rP.x, rP.y, rP.z - 2),
                    new Vector3(rP.x - 1, rP.y, rP.z)
                };
                break;
            case (270):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z + 1),
                    new Vector3(rP.x - 1, rP.y, rP.z),
                    new Vector3(rP.x - 2, rP.y, rP.z)
                };
                break;
            default:
                rValue = null;
                Debug.Log("RegularLRot is not valid: " + rR);
                break;
        }

        return rValue;
    }

    private static Vector3[] URot(Vector3 rP, int rR)
    {
        Vector3[] rValue = new Vector3[5];

        switch(rR)
        {
            case (0):
                rValue = new Vector3[5]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z + 1),
                    new Vector3(rP.x + 1, rP.y, rP.z),
                    new Vector3(rP.x + 2, rP.y, rP.z),
                    new Vector3(rP.x + 2, rP.y, rP.z + 1)
                };
                break;
            case (90):
                rValue = new Vector3[5]
                {
                    rP,
                    new Vector3(rP.x + 1, rP.y, rP.z),
                    new Vector3(rP.x, rP.y, rP.z - 1),
                    new Vector3(rP.x, rP.y, rP.z - 2),
                    new Vector3(rP.x + 1, rP.y, rP.z - 2)
                };
                break;
            case (180):
                rValue = new Vector3[5]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z - 1),
                    new Vector3(rP.x - 1, rP.y, rP.z),
                    new Vector3(rP.x - 2, rP.y, rP.z),
                    new Vector3(rP.x - 2, rP.y, rP.z - 1)
                };
                break;
            case (270):
                rValue = new Vector3[5]
                {
                    rP,
                    new Vector3(rP.x - 1, rP.z, rP.y),
                    new Vector3(rP.x, rP.y, rP.z + 1),
                    new Vector3(rP.x, rP.y, rP.z + 2),
                    new Vector3(rP.x - 1, rP.y, rP.z)
                };
                break;
            default:
                Debug.Log("URot is not valid: " + rR);
                rValue = null;
                break;
        }

        return rValue;
    }

    private static Vector3[] StraightRot(Vector3 rP, int rR)
    {
        Vector3[] rValue = new Vector3[4];

        switch (rR)
        {
            case (0):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z + 1),
                    new Vector3(rP.x, rP.y, rP.z + 2),
                    new Vector3(rP.x, rP.y, rP.z + 3)
                };
                break;
            case (90):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x + 1, rP.y, rP.z),
                    new Vector3(rP.x + 2, rP.y, rP.z),
                    new Vector3(rP.x + 3, rP.y, rP.z)
                };
                break;
            case (180):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z - 1),
                    new Vector3(rP.x, rP.y, rP.z - 2),
                    new Vector3(rP.x, rP.y, rP.z - 3)
                };
                break;
            case (270):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x - 1, rP.y, rP.z),
                    new Vector3(rP.x - 2, rP.y, rP.z),
                    new Vector3(rP.x - 3, rP.y, rP.z)
                };
                break;
            default:
                Debug.Log("StraightRot is not valid: " + rR);
                rValue = null;
                break;
        }

        return rValue;
    }

    private static Vector3[] CrossRot(Vector3 rP, int rR)
    {
        Vector3[] rValue = new Vector3[5];

        switch(rR)
        {
            case (0):
                rValue = new Vector3[5]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z + 1),
                    new Vector3(rP.x - 1, rP.y, rP.z + 1),
                    new Vector3(rP.x + 1, rP.y, rP.z + 1),
                    new Vector3(rP.x, rP.y, rP.z + 2)
                };
                break;
            case (90):
                rValue = new Vector3[5]
                {
                    rP,
                    new Vector3(rP.x + 1, rP.y, rP.z),
                    new Vector3(rP.x + 1, rP.y, rP.z + 1),
                    new Vector3(rP.x + 1, rP.y, rP.z - 1),
                    new Vector3(rP.x + 2, rP.y, rP.z)
                };
                break;
            case (180):
                rValue = new Vector3[5]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z - 1),
                    new Vector3(rP.x - 1, rP.y, rP.z - 1),
                    new Vector3(rP.x + 1, rP.y, rP.z - 1),
                    new Vector3(rP.x, rP.y, rP.z - 2)
                };
                break;
            case (270):
                rValue = new Vector3[5]
                {
                    rP,
                    new Vector3(rP.x - 1, rP.y, rP.z),
                    new Vector3(rP.x - 1, rP.y, rP.z - 1),
                    new Vector3(rP.x - 1, rP.y, rP.z + 1),
                    new Vector3(rP.x - 2, rP.y, rP.z)
                };
                break;
            default:
                Debug.Log("CrossRot is not valid: " + rR);
                rValue = null;
                break;
        }

        return rValue;
    }

    private static Vector3[] TWTBRot(Vector3 rP, int rR)
    {
        Vector3[] rValue = new Vector3[4];

        switch(rR)
        {
            case (0):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z + 1),
                    new Vector3(rP.x + 1, rP.y, rP.z + 1),
                    new Vector3(rP.x + 1, rP.y, rP.z + 2)
                };
                break;
            case (90):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x + 1, rP.y, rP.z),
                    new Vector3(rP.x + 1, rP.y, rP.z - 1),
                    new Vector3(rP.x + 2, rP.y, rP.z - 1)
                };
                break;
            case (180):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z - 1),
                    new Vector3(rP.x - 1, rP.y, rP.z - 1),
                    new Vector3(rP.x - 1, rP.y, rP.z - 2)
                };
                break;
            case (270):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x - 1, rP.y, rP.z),
                    new Vector3(rP.x -1, rP.y, rP.z + 1),
                    new Vector3(rP.x - 2, rP.y, rP.z + 1)
                };
                break;
            default:
                Debug.Log("TWTBRot is not valid: " + rR);
                rValue = null;
                break;
        }

        return rValue;
    }

    private static Vector3[] HumpedRot(Vector3 rP, int rR)
    {
        Vector3[] rValue = new Vector3[4];

        switch(rR)
        {
            case (0):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x + 1, rP.y, rP.z),
                    new Vector3(rP.x + 1, rP.y, rP.z + 1),
                    new Vector3(rP.x + 2, rP.y, rP.z)
                };
                break;
            case (90):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z - 1),
                    new Vector3(rP.x + 1, rP.y, rP.z - 1),
                    new Vector3(rP.x, rP.y, rP.z - 2)
                };
                break;
            case (180):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x - 1, rP.y, rP.z),
                    new Vector3(rP.x - 1, rP.y, rP.z - 1),
                    new Vector3(rP.x - 2, rP.y, rP.z)
                };
                break;
            case (270):
                rValue = new Vector3[4]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z + 1),
                    new Vector3(rP.x - 1, rP.y, rP.z + 1),
                    new Vector3(rP.x, rP.y, rP.z + 2)
                };
                break;
            default:
                Debug.Log("HumpedRot is not valid: " + rR);
                rValue = null;
                break;
        }

        return rValue;
    }

    private static Vector3[] ZRot(Vector3 rP, int rR)
    {
        Vector3[] rValue = new Vector3[5];

        switch(rR)
        {
            case (0):
                rValue = new Vector3[5]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z + 1),
                    new Vector3(rP.x + 1, rP.y, rP.z + 1),
                    new Vector3(rP.x + 2, rP.y, rP.z + 1),
                    new Vector3(rP.x + 1, rP.y, rP.z + 2)
                };
                break;
            case (90):
                rValue = new Vector3[5]
                {
                    rP,
                    new Vector3(rP.x + 1, rP.y, rP.z),
                    new Vector3(rP.x + 1, rP.y, rP.z - 1),
                    new Vector3(rP.x + 1, rP.y, rP.z - 2),
                    new Vector3(rP.x + 2, rP.y, rP.z - 2)
                };
                break;
            case (180):
                rValue = new Vector3[5]
                {
                    rP,
                    new Vector3(rP.x, rP.y, rP.z - 1),
                    new Vector3(rP.x - 1, rP.y, rP.z - 1),
                    new Vector3(rP.x - 2, rP.y, rP.z - 1),
                    new Vector3(rP.x - 2, rP.y, rP.z - 2)
                };
                break;
            case (270):
                rValue = new Vector3[5]
                {
                    rP,
                    new Vector3(rP.x - 1, rP.y, rP.z),
                    new Vector3(rP.x - 1, rP.y, rP.z + 1),
                    new Vector3(rP.x - 1, rP.y, rP.z + 2),
                    new Vector3(rP.x - 2, rP.y, rP.z + 2)
                };
                break;
            default:
                Debug.Log("ZRot is not valid: " + rR);
                rValue = null;
                break;
        }

        return rValue;
    }
}
