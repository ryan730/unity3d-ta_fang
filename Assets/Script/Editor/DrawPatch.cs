using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DrawPatch : MonoBehaviour
{
    public List<Vector3> mList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        //mList.Add(new Vector3(0, 0, 0));
        // mList.Add(new Vector3(10, 0, 0));
        //mList.Add(new Vector3(10, 0, 10));
        //mList.Add(new Vector3(5, 0, 2));
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())// 防UI穿透，点击按钮阻止鼠标事件
                return;
            // canvas 的屏幕坐标转换
            //         bool isInRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            //        canvasRt,
            //        mousePos,
            //        eventData.enterEventCamera,// 当前摄像机
            //        out guiPos
            //    );


            Vector3 pos = Input.mousePosition; //鼠标坐标
            pos.z = 10;

            Vector3 transPos = Camera.main.ScreenToWorldPoint(pos);// 屏幕坐标转世界坐标
            Debug.Log("down：" + pos + "/" + transPos);

            float _x = Mathf.RoundToInt(transPos.x);
            float _y = Mathf.RoundToInt(transPos.y);
            float _z = Mathf.RoundToInt(transPos.z);

            mList.Add(new Vector3(_x, _y, _z));

        }
        if (Input.GetMouseButtonDown(1))
        {
            if (mList.Count > 0)
            {
                mList.RemoveAt(mList.Count - 1);// 参数是个index

                Debug.Log("mList" + mList.Count);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Debug.Log("down move");
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("up");
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // 滚轮控制视野
        {
            //Debug.Log("ScrollWheel up");
            //Camera.main.fieldOfView *= 0.9f;// 透视投影
            Camera.main.orthographicSize *= 0.9f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //Debug.Log("ScrollWheel down");
            //Camera.main.fieldOfView *=1.1f;
            Camera.main.orthographicSize *= 1.1f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("W1");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W2");
            moveCamera("W");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("W3");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S");
            moveCamera("S");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A");
            moveCamera("A");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D");
            moveCamera("D");
        }
    }

    public void outputText()
    {
        Debug.Log("--outputText--");
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < mList.Count; i++)
        {
            Vector3 vec = mList[i];
            sb.AppendLine(string.Format("{0},{1}", vec.x, vec.z));
        }
        Debug.Log("--->" + sb.ToString());
        string filePath = EditorUtility.SaveFilePanel("保存地图文件", "./Assets/Resources/Map", DateTime.Now.ToString("yyyyMMddHHmm"), "txt");
        Debug.Log("filePath>" + filePath);
        System.IO.File.WriteAllText(filePath, sb.ToString());
    }

    // 清空所有
    public void clearAll(){
        mList.Clear();
    }

    void moveCamera(string type)
    {
        Transform trans = Camera.main.transform;
        Vector3 transPos = trans.localPosition;
        switch (type)
        {
            case "W":
                transPos.z += 1;
                break;
            case "S":
                transPos.z -= 1;
                break;
            case "A":
                transPos.x -= 1;
                break;
            case "D":
                transPos.x += 1;
                break;
        }
        trans.localPosition = transPos;
    }

    void OnDrawGizmos()
    {
        Debug.Log("OnDrawGizmos");
        Gizmos.color = Color.green;
        Vector3 lineWidth = new Vector3(0.98f, 0.98f, 0.98f);
        if (mList.Count > 0)
        {
            Gizmos.DrawCube(mList[0], lineWidth);

        }else {
            return;
        }
        Gizmos.color = Color.red;
        int _x;
        int _z;
        _x = (int)mList[0].x;
        _z = (int)mList[0].z;
        for (int i = 1; i < mList.Count; i++)
        {

            while (_x > mList[i].x)
            {
                //Debug.Log(_x + "===" + mList[i].x);
                _x--;
                Gizmos.DrawCube(new Vector3(_x, 0, _z), lineWidth);
            }
            while (_x < mList[i].x)
            {
                //Debug.Log(_x + "___" + _z + "____" + mList[i].x);
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
        }
        if (mList.Count > 0)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(mList[mList.Count - 1], lineWidth);

        }


    }
}
