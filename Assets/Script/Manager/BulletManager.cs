using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 子弹管理器
public class BulletManager : MonoBehaviour
{
    public static List<Bullet> list = new List<Bullet>();
    public const string Path = "Model/";
    //public static Bullet Create(Enemy enemy, float speed, float atv)
    public static Bullet Create(Enemy enemy, string modelName)
    {
        for (var i = 0; i < list.Count; i++)
        {
            Bullet bul = list[i];
            if (bul.gameObject.activeSelf == false && bul.gameObject.name == modelName)
            {// 已经击中过的子弹，回收一个，重制状态，减少实例化次数
                //bul.InitData(enemy, speed, atv);
                bul.gameObject.SetActive(true);
                return bul;
            }
        }

        GameObject obj;
        // 如果是空值
        if (string.IsNullOrEmpty(modelName))
        {
            obj = new GameObject();
        }
        else
        {
            obj = Resources.Load<GameObject>(Path + modelName);
            obj = GameObject.Instantiate(obj);
        }

        obj.name = modelName;
        Bullet bullet = obj.AddComponent<Bullet>();
        //bullet.InitData(enemy, speed, atv);
        list.Add(bullet);
        return bullet;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //删除所有子弹
    public static void DestroyAll()
    {
        for (var i = 0; i < list.Count; i++)
        {
            if (list[i])
            {
                Destroy(list[i].gameObject);
            }

        }
        list.Clear();
    }
}
