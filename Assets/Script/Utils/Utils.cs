using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;
using Newtonsoft.Json;   // 必须先引用 namespace

public class Utils
{
    public static UnityAction on_Click
    {
        get { return on_Click; }
        set
        {

            on_Click = value;
        }
    }

    public static Component FindComponetInObject<T>(Transform transform, string objectName, UnityAction call = null) where T : Component
    {
        Component t = transform.Find(objectName).GetComponent<T>();

        // if(t == null){
        //     Component t = transform.Find(objectName).GetComponentInChildren<T>();
        // }

        if (t is Button && call is UnityAction)
        {
            Button but = t as Button;
            but.onClick.AddListener(call);
        }


        return t;
    }

    public static void Test()
    {

    }

    public static void ShowAlert(string txt)
    {
        Alert alert = GameObject.FindObjectOfType<Alert>();
        alert.Show(txt);
    }

    public static string SystemInfoScript()
    {
        string infoString = "";
        infoString += "Battery Level: " + SystemInfo.batteryLevel + "\n";
        infoString += "Battery Status: " + SystemInfo.batteryStatus + "\n";
        infoString += "Device Model: " + SystemInfo.deviceModel + "\n";
        infoString += "Device Name: " + SystemInfo.deviceName + "\n";
        infoString += "Device Type: " + SystemInfo.deviceType + "\n";
        infoString += "Device Unique Identifier: " + SystemInfo.deviceUniqueIdentifier + "\n";
        infoString += "Operating System: " + SystemInfo.operatingSystem + "\n";
        infoString += "Processor Type: " + SystemInfo.processorType + "\n";
        infoString += "Memory Size: " + SystemInfo.systemMemorySize + "\n";
        infoString += "Acceleromer Support: " + SystemInfo.supportsAccelerometer + "\n";
        infoString += "Gyroscope Support: " + SystemInfo.supportsGyroscope + "\n";
        infoString += "Vibration Support: " + SystemInfo.supportsVibration + "\n";
        return infoString;
    }


    // json 转对象
    public static T LoadJsonFromFile<T>(string path) where T : class
    {
        // if (!File.Exists(Application.dataPath + path))
        // {
        //     Debug.LogError(Application.dataPath + path + " - Don't Find");

        //     return null;
        // }
        // StreamReader sr = new StreamReader(Application.dataPath + path);
        // if (sr == null)
        // {
        //     return null;
        // }
        // string json = sr.ReadToEnd();
        TextAsset res = Resources.Load<TextAsset>(path);// 修改成多平台都能适配的方法
        string json = res.text;
        if (string.IsNullOrEmpty(json)) return null;
        if (json.Length > 0)
        {
            //T res = JsonUtility.FromJson<T>(json);

            return JsonConvert.DeserializeObject<T>(json);
        }
        return null;
    }

    // canvas 坐标上的点，通过射线，获取三维坐标位置
    public static bool getPositionByRay(Transform transform, Vector3 currentPos, out Vector3 outVec)
    {
        RectTransform rect = transform as RectTransform;
        currentPos.x = currentPos.x / rect.rect.width + 0.5f;
        currentPos.y = currentPos.y / rect.rect.height + 0.5f;

        ///currentPos = Camera.main.ScreenToViewportPoint(currentPos);
        ///currentPos = currentPos + new Vector3(0.5f, 0.5f, 0.0f);

        Ray ray = Camera.main.ViewportPointToRay(currentPos);// 摄像机发射一条射线
        RaycastHit[] hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.name == "map_show")
            {
                Vector3 pos = hits[i].point;

                pos.x = Mathf.RoundToInt(pos.x); // 四舍五入
                pos.z = Mathf.RoundToInt(pos.z); // 四舍五入
                Debug.DrawLine(Camera.main.transform.position, pos, Color.blue);// 绘制射线
                outVec = pos;
                return true;
            }
        }
        outVec = Vector3.zero;
        return false;
    }

    // 放大缩小手势的判断
    public static bool isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2) // 函数判断放大缩小手势
    {
        // 取平方根
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));

        if (leng1 < leng2)
        {
            // 放大手势  
            return true;
        }
        else
        {
            // 缩小手势  
            return false;
        }
    }

    // 手指的判断标准
    public static bool isTouchAction(bool th, string flag = "")
    {
        DeviceType systemInfo = SystemInfo.deviceType;
        // if (systemInfo == DeviceType.Desktop)
        // {

        // }
        if (systemInfo == DeviceType.Handheld)
        {
            if (flag == "up")
            {
                return Input.touchCount == 1 && th && Input.GetTouch(0).phase == TouchPhase.Ended;// 移动端判断几个手指
            }
            return Input.touchCount == 1 && th;// 移动端判断几个手指
        }
        else
        {
            return th;
        }
    }


}