using System.Collections.Generic;
using UnityEngine;

// 战斗界面管理
public class BattleManager
{
   public static  List<EnemyInfo> Enemies = new List<EnemyInfo>();
 
    public static UI_Battle ui_Battle;
    public static List<Vector3> list;
    private static float _time;
    public static int HomeLife;
    public static List<Enemy> mEnemyList = new List<Enemy>();

    private static List<GameObject> mTowerList = new List<GameObject>();

    public static bool mBattleStop;// 战斗结束，false 为未结束

    // 当前的波数
    public static int mCurrentWaveCount;
    // 波次倒计时
    public static int mWaveCountDown;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //清空所有游戏元素
    public static void Clear()
    {
        MapManager.ClearMapBox();
        for (int i = 0; i < mTowerList.Count; i++)
        {
            if (mTowerList[i])
            {
                GameObject.Destroy(mTowerList[i].gameObject);
            }

        }
        for (int i = 0; i < mEnemyList.Count; i++)
        {
            if (mEnemyList[i])
            {
                GameObject.Destroy(mEnemyList[i].gameObject);
            }

        }
        mTowerList.Clear();
        mEnemyList.Clear();
        BulletManager.DestroyAll();
    }

    public static void EnemyAttack()
    {
        if (HomeLife > 1)
        {
            HomeLife--;

        }
        else
        {
            Debug.Log("战斗失败!");
            Debug.Log(mEnemyList.Count);
            UIManager.EnterUI<UI_BattleResult>().InitData(false);
            ui_Battle.BattleStop();
            HomeLife = 0;
            mBattleStop = true;
            ui_Battle.RefershLife();
        }
    }

    public static void InitData(UI_Battle ui, List<Vector3> mlist, List<EnemyInfo> enemy_infos)
    {
        ui_Battle = ui;
        list = mlist;
        ///BattleEnemyCount = enemy_count;// 敌人数量
        _time = 0;
        HomeLife = 5;// 基地生命
        mCurrentWaveCount = 0;// 当前波数

        Enemies = EnemyManager.CreateEnemies(enemy_infos);

        for (int i = mEnemyList.Count - 1; i >= 0; i--)
        {
            mEnemyList[i].DeleteObj();
        }
        mEnemyList.Clear();
        mBattleStop = false;
    }

    public static void BattleUpdate()
    {
        if (mBattleStop)
        {
            return;
        }
        if (Enemies.Count > 0)
        {
            _time += Time.deltaTime;
            if (mEnemyList.Count == 0 && mCurrentWaveCount > 1)
            {
                _time += Time.deltaTime * 2;
            }
            if (_time >= 1)
            {
                _time -= 1;
                if (mWaveCountDown > 0) // 是否在倒计时内
                {
                    mWaveCountDown--;
                    ui_Battle.RefreshCountDown(mWaveCountDown);
                }
                else if (mCurrentWaveCount < Enemies[0].Wave) // 当前Wave数小于所有Enemys'Wave数
                {
                    mWaveCountDown = 5;
                    mCurrentWaveCount++;
                    ui_Battle.RefreshCountDown(mWaveCountDown);
                    ui_Battle.RefreshWave(mCurrentWaveCount);
                    EnemyManager.CurrentCountTotal = Enemies[0].Count;// 当前一波开始的总敌人数
                }
                else // 既不在倒计时又是当前波次
                {
                    Enemy enemy = EnemyManager.CreateEnemy(Enemies[0], list);// 生成敌人
                    ui_Battle.BindEnemy(enemy);
                    mEnemyList.Add(enemy);// 生成敌人
                                          //BattleEnemyCount--;
                    if (Enemies[0].Count > 1)
                    {
                        Enemies[0].Count--;
                    }
                    else
                    {
                        Enemies.RemoveAt(0);
                    }
                }

            }
        }
        for (int i = mEnemyList.Count - 1; i >= 0; i--)
        {
            if (mEnemyList[i] == null)
            {
                mEnemyList.RemoveAt(i);
            }
            else
            {
                mEnemyList[i].BattleUpdate();
                if (mEnemyList[i].HP <= 0)
                {
                    mEnemyList.RemoveAt(i);
                }
            }
        }
        if (HomeLife > 0 && Enemies.Count == 0 && mEnemyList.Count == 0)// 敌人已经完全消灭
        {
            mBattleStop = true;
            UIManager.EnterUI<UI_BattleResult>().InitData(true);
            ui_Battle.BattleStop();
            Debug.Log("战斗胜利");
        }
    }

    public static void CreateTower(Vector3 pos, TowerInfo info)
    {

        Tower tower = TowerManager.CreateTower(info);// 生成守卫
        tower.gameObject.transform.position = pos;
        mTowerList.Add(tower.gameObject);

    }
    public static bool isCreatedTower(Vector3 pos)
    {
        for (int i = 0; i < mTowerList.Count; i++)
        {
            if (pos == mTowerList[i].transform.localPosition)
            {
                Utils.ShowAlert("不能重复创建!");
                return true;
            }
        }
        List<GameObject> mMapRunBox = MapManager.mMapRunBox;

        for (int j = 0; j < mMapRunBox.Count; j++)
        {
            float distance = Vector3.Distance(pos, mMapRunBox[j].transform.localPosition);
            if (Mathf.RoundToInt(distance) <= 0)
            {
                Utils.ShowAlert("不能创建在道路上!");
                return true;
            }
        }
        return false;
    }
}
