using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMap : MonoBehaviour
{
    public static List<Vector3> mList = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        return;
        //Debug.Log("OnDrawGizmos");
        Gizmos.color = Color.green;
        Vector3 lineWidth = new Vector3(0.98f, 0.1f, 0.98f);
        if (mList.Count > 0)
        {
            Gizmos.DrawCube(mList[0], lineWidth);

        }
        else
        {
            return;
        }
        Gizmos.color = Color.red;
        int _x;
        int _z;
        for (int i = 1; i < mList.Count; i++)
        {
            _x = (int)mList[i - 1].x;
            _z = (int)mList[i - 1].z;
            while (_x > mList[i].x)
            {
                _x--;
                Gizmos.DrawCube(new Vector3(_x, 0, _z), lineWidth);
            }
            while (_x < mList[i].x)
            {
                _x++;
                Gizmos.DrawCube(new Vector3(_x, 0, _z), lineWidth);

            }
            while (_z > mList[i].z)
            {
                _z--;
                Gizmos.DrawCube(new Vector3(_x, 0, _z), lineWidth);
            }
            while (_z < mList[i].z)
            {
                _z++;
                Gizmos.DrawCube(new Vector3(_x, 0, _z), lineWidth);

            }
            Gizmos.DrawCube(new Vector3(_x, 0, _z), lineWidth);
        }
        if (mList.Count > 0)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(mList[mList.Count - 1], lineWidth);
        }

    }
}
