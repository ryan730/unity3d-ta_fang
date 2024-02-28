using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敌人管理器
public class EnemyManager
{
    public const string EnemyPath = "Model/Role/";

    public static int CurrentCountTotal; // 每一波的敌人人数;

    // 每波敌人的数量List


    public static Enemy CreateEnemy(EnemyInfo info, List<Vector3> list)
    {

        GameObject obj = Resources.Load<GameObject>(EnemyPath + info.ModelName);
        GameObject enemy = GameObject.Instantiate(obj);
        Enemy en = enemy.AddComponent<Enemy>();
        // enemy 的尺寸变成 x 倍
        enemy.transform.localScale = new Vector3((float)info.Size, (float)info.Size, (float)info.Size);

        en.InitData(list, info.HP, info);

        // MonoBehaviour owner = enemy.GetComponent<MonoBehaviour>();//  静态方法里做延迟
        // owner.StartCoroutine(delay(en, info, list));

        return en;
    }


    private static IEnumerator delay(Enemy en, EnemyInfo info, List<Vector3> list)
    {
        yield return new WaitForSeconds(2);
        en.InitData(list, info.HP, info);
    }

    public static List<EnemyInfo> CreateEnemies(List<EnemyInfo> enemies)
    {
        //List<EnemyInfo> Enemies1 = new List<EnemyInfo>();
        // for (int i = 0; i < enemyInfos.Count; i++)
        // {
        //     EnemyInfo enemyInfo = enemyInfos[i];
        //     // 初始化各个波的敌人属性
        //     Enemies.Add(new EnemyInfo
        //     {
        //         ModelName = enemyInfo.ModelName,
        //         HP = enemyInfo.HP,
        //         MoveSpeed = 1,
        //         Count = 10,
        //         Wave = 1,
        //     });
        // }
        // 初始化各个波的敌人属性
        // Enemies1.Add(new EnemyInfo
        // {
        //     ModelName = "Warrior",
        //     HP = 15,
        //     MoveSpeed = 1,
        //     Count = 10,
        //     Wave = 1,
        // });

        // Enemies1.Add(new EnemyInfo
        // {
        //     ModelName = "Archer",
        //     HP = 10,
        //     MoveSpeed = 3.5f,
        //     Count = 5,
        //     Wave = 2,
        // });

        // Enemies1.Add(new EnemyInfo // boss
        // {
        //     ModelName = "Birder",
        //     HP = 50,
        //     MoveSpeed = 0.3f,
        //     Count = 5,
        //     Size = 2,
        //     Wave = 1,
        // });
        return enemies;
    }
}
