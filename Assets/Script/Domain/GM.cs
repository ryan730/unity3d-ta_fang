using System.Collections.Specialized;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;   // 必须先引用 namespace

[Serializable]
public class LoadResult
{
    public List<Data> data;
}
[Serializable]
public class Data
{
    public Level level;
}

[Serializable]
public class Level
{
    public string name;
    public List<TowerInfo> tower_info;
    public List<EnemyInfo> enemy_info;
}

public class GM : MonoBehaviour
{
    public static LoadResult loadResult;
    //public static Hashtable parseData = new Hashtable();
    void Awake()
    {


    }

    public static void initData(int name)
    {
        loadResult = Utils.LoadJsonFromFile<LoadResult>("Config/Level_"+name);

        var list = loadResult.data[0].level.tower_info;
        Debug.Log("result : " + list);

        // Hashtable aa = childrenInJSON(loadResult.initData);

        // Debug.Log("aaaaaaaaaaaaaa : " + aa);
        foreach (var item in list)
        {
            Debug.Log("===>" + item.ToString());
        }

    }

    // Hashtable childrenInJSON(Dictionary all)
    // {
    //     Hashtable parseData = new Hashtable();
    //     foreach (DictionaryEntry item in all)
    //     {
    //         Debug.Log(item.Key + "===>" + item.Value);
    //         Debug.Log("====>" + item.Value.GetType());
    //         if (item.Value is string || item.Value is int || item.Value is float)
    //         {
    //             parseData[item.Key] = item.Value;
    //         }
    //         else
    //         {
    //             parseData[item.Key] = childrenInJSON(item.Value as Dictionary);
    //         }

    //     }
    //     return parseData;
    // }
    // 获取某一关的信息
    public static Level getLevelInfo(string name)
    {
        if (GM.loadResult?.data?.Count > 0)
        {
            int index = int.Parse(name) - 1;
            return loadResult.data[0].level;
        }
        return null;
    }
}