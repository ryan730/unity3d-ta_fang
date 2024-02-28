using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 地图场景管理器
public class MapManager
{
    public const string MapPath = "Map/";
    public static GameObject mMapObj;

    public static bool mMapDarg = false;

    public static List<GameObject> mMapRunBox = new List<GameObject>();

    private static Vector2 oldPosition1 = new Vector2(0, 0);
    private static Vector2 oldPosition2 = new Vector2(0, 0);

    private static float _camerafov;

    public static List<Vector3> GetMapPath(string fileName)
    {
        TextAsset ta = Resources.Load<TextAsset>(MapPath + fileName);
        string text = ta.text;
        string[] pos_strs = text.TrimEnd('\n').Split('\n');
        string[] _pos = new string[2];
        List<Vector3> pos = new List<Vector3>();
        for (var i = 0; i < pos_strs.Length; i++)
        {
            _pos = pos_strs[i].Split(',');
            float x = int.Parse(_pos[0]);
            float z = int.Parse(_pos[1]);
            pos.Add(new Vector3(x, 0, z));
        }
        DrawLand(pos);
        return pos;
    }

    public static void DrawLand(List<Vector3> mList)
    {
        for (var i = 0; i < mMapRunBox.Count; i++)
        {
            GameObject.Destroy(mMapRunBox[i]);
        }

        GameObject map_box = Resources.Load<GameObject>("Model/map_box");
        if (mList.Count > 0)
        {

        }
        else
        {
            return;
        }

        Vector3 pos = mList[0];
        CreateMapBox(map_box, pos);

        for (int i = 1; i < mList.Count; i++)
        {
            //pos = mList[i-1];
            while (pos.x > mList[i].x)
            {
                pos.x--;
                CreateMapBox(map_box, pos);
            }
            while (pos.x < mList[i].x)
            {
                pos.x++;
                CreateMapBox(map_box, pos);
            }
            while (pos.z > mList[i].z)
            {
                pos.z--;
                CreateMapBox(map_box, pos);
            }
            while (pos.z < mList[i].z)
            {
                pos.z++;
                CreateMapBox(map_box, pos);
            }
        }
        Debug.Log("mMapRunBox:" + mMapRunBox.Count);
        // if (mList.Count > 0)
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawCube(mList[mList.Count - 1], lineWidth);
        // }

    }

    public static void CreateMapBox(GameObject box, Vector3 pos)
    {
        GameObject map_box = GameObject.Instantiate(box);
        map_box.transform.SetParent(mMapObj.transform);
        map_box.transform.localPosition = pos;
        mMapRunBox.Add(map_box);
    }

    // 清空整个map
    public static void ClearMapBox()
    {
        for (int i = 0; i < mMapRunBox.Count; i++)
        {
            GameObject.Destroy(mMapRunBox[i]);
        }
        mMapRunBox.Clear();
    }

    public static void MapUpdate()
    {
        // 判断设备是否是移动端
        DeviceType systemInfo = SystemInfo.deviceType;
        // if (systemInfo == DeviceType.Desktop)
        // {
        //     setViewField();
        // }
        // else if (systemInfo == DeviceType.Handheld)
        // {
        //     setMobiViewField();
        // }
        setViewField();
        setMobiViewField();
    }

    static void moveCamera(string type)
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

    //PC视野操作
    public static void setViewField()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0) // 滚轮控制视野
        {
            Camera.main.fieldOfView *= 0.9f;// 透视投影
                                            //Camera.main.orthographicSize *= 0.9f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.fieldOfView *= 1.1f;
            //Camera.main.orthographicSize *= 1.1f;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W2");
            moveCamera("W");
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


    private static float mPrevRange = -1;
    private static Vector3 mTouchClickPos = Vector3.zero;

    // 移动端视野操作
    public static void setMobiViewField()
    {
        // 判断多个手指的操作逻辑
        if (Input.touchCount >= 2)
        {
            Touch touch_1 = Input.GetTouch(0);
            Touch touch_2 = Input.GetTouch(1);


            if (mPrevRange < 0)
            {
                mPrevRange = Vector2.Distance(touch_1.position, touch_2.position);
            }
            else
            {
                //float _distance = Vector2.Distance(touch_1.position, touch_2.position);
                //UI_Battle.mTextLife.text = string.Format("两指距离:{0}", _distance);
                //Camera.main.fieldOfView *= _range / mPrevRange;
                //float _range = Camera.main.fieldOfView * (_distance / mPrevRange / 3);
                //Camera.main.fieldOfView = Mathf.Clamp(_range, 10, 70);

                if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
                {

                    //计算出当前两点触摸点的位置
                    Vector2 tempPosition1 = Input.GetTouch(0).position;
                    Vector2 tempPosition2 = Input.GetTouch(1).position;

                    // 改变相机FOV值 实现放大缩小
                    if (Utils.isEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                    {
                        _camerafov -= 80f * Time.deltaTime;
                        if (_camerafov <= 40f)
                        {
                            _camerafov = 40;

                        }
                        Camera.main.fieldOfView = _camerafov;
                    }
                    else
                    {
                        _camerafov += 80f * Time.deltaTime;

                        if (_camerafov >= 95f)
                        {
                            _camerafov = 95f;
                        }
                        Camera.main.fieldOfView = _camerafov;

                    }
                    oldPosition1 = tempPosition1;
                    oldPosition2 = tempPosition2;

                }

            }
        }
        else if (Utils.isTouchAction(Input.GetMouseButton(0)))
        {
            if (mPrevRange >= 0)
            {
                mPrevRange = -1;
            }

            //Touch touch_1 = Input.GetTouch(0);
            //Ray ray = Camera.main.ScreenPointToRay(touch_1.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.name == "map_show")
                {
                    if (mTouchClickPos == Vector3.zero)
                    {
                        mTouchClickPos = hit.point;
                    }
                    else if (mTouchClickPos != hit.point)
                    {
                        mMapDarg = true;
                        Camera.main.transform.localPosition += mTouchClickPos - hit.point;
                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.name == "map_show")
                            {
                                mTouchClickPos = hit.point;
                            }
                            else
                            {
                                mTouchClickPos = Vector3.zero;
                            }
                        }
                        else
                        {
                            mTouchClickPos = Vector3.zero;
                        }
                    }
                }

            }
        }
        else
        {
            if (mPrevRange >= 0)
            {
                mPrevRange = -1;
            }
            if (mTouchClickPos != Vector3.zero)
            {
                mTouchClickPos = Vector3.zero;
            }
        }
    }

}
