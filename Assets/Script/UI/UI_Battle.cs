using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_Battle : UILayer
{
    public static Text mTextLife;
    // 波数
    public Text mTextWave;
    // 波数倒计时
    public Text mTextWavesCountDown;
    public Button mBtnTowerNeary;
    public Button mBtnTowerShoot;
    public GameObject mBtnTower;
    List<Vector3> list;

    private GameObject mEnemyHPInfo;//血槽

    private GameObject mBtnRole;
    public static bool is_click_button = false;

    // 守护菜单
    //public List<GameObject> mMenusList = new List<GameObject>();

    //初始化数据
    public void InitData(string map_name)
    {
        Debug.Log("UI_Battle-InitData：" + map_name);

        // 播放音乐
        AudioManager.PlayMusic("music_fuben");
        Camera.main.transform.localPosition = new Vector3(0, 10, 0);
        Camera.main.fieldOfView = 60;

        MapManager.mMapObj.transform.localPosition = Vector3.zero;

        GM.initData(int.Parse(map_name));// 加载 gameConfig 文件
                                         // 获取GM配置
        Level level = GM.getLevelInfo(map_name);
        Debug.Log(level);

        BattleManager.InitData(this, MapManager.GetMapPath(map_name), level.enemy_info);// 创建敌人数量

        TowerManager.CreateTowerMenus(this.transform, mBtnTower, mBtnRole, level.tower_info);

        RefershLife();
        mEnemyHPInfo.SetActive(false);
        mBtnTower.SetActive(false);

        mTextWave.text = "";
        mTextWavesCountDown.text = "";

    }

    public override void OnNodeLoad()
    {

        Debug.Log("UI_Battle-OnNodeLoad");

        mTextLife = transform.Find("text_life").GetComponent<Text>();// 基地生命

        mTextWave = transform.Find("text_wave").GetComponent<Text>();// 波数

        mTextWavesCountDown = transform.Find("count_down_wave").GetComponent<Text>();// 倒计时
        // mTextLife.text = string.Format("剩余生命值:{0}", BattleManager.HomeLife);

        mEnemyHPInfo = transform.Find("enemy_hp_info").gameObject;
        mBtnTower = transform.Find("tower_menus").gameObject;
        mBtnRole = transform.Find("tower_menus/btn_role").gameObject;

        // 建造防御塔按钮
        // mBtnTowerNeary = Utils.FindComponetInObject<Button>(mBtnTower.transform, "btn_battle_neary", () =>
        // {

        //     CreateTower(new TowerInfo
        //     {
        //         ModelName = "Warrior_example1",
        //         Attack = 5,
        //         AttackCD = 0.8f,
        //         AttackRange = 2,

        //         BulletModelName = "bullet",//bullet
        //         BullerMoveSpd = 0,
        //         BulletFlyLimit = 5,
        //         AttckAnimName = "AttackMelee2",
        //     });
        // }) as Button;

        // mBtnTowerShoot = Utils.FindComponetInObject<Button>(mBtnTower.transform, "btn_battle_shoot", () =>
        // {
        //     CreateTower(new TowerInfo
        //     {
        //         ModelName = "Archer_example2",
        //         Attack = 2,
        //         AttackCD = 0.6f,
        //         AttackRange = 5,

        //         BulletModelName = "bullet",//bullet
        //         BullerMoveSpd = 5,
        //         BulletFlyLimit = 5,
        //         AttckAnimName = "AttackRange1",
        //     });
        // }) as Button;
    }

    // 刷新倒计时的方法
    public void RefreshCountDown(int time)
    {
        if (time <= 0)
        {
            mTextWavesCountDown.gameObject.SetActive(false);
        }
        else
        {
            mTextWavesCountDown.gameObject.SetActive(true);
            mTextWavesCountDown.text = time.ToString();
        }
    }

    // 刷新波数
    public void RefreshWave(int wave)
    {
        mTextWave.text = string.Format("第{0}波", wave);
    }



    public void RefershLife()
    {
        mTextLife.text = string.Format("剩余生命值:{0}", BattleManager.HomeLife);
    }
    public override void OnEnter()
    {//UI启动完成

    }

    // 战斗结束回归初始状态
    public void BattleStop()
    {
        this.mBtnTower.gameObject.SetActive(false);// 隐藏创建防御塔按钮
    }

    public void BindEnemy(Enemy enemy)
    {
        GameObject obj = Instantiate(this.mEnemyHPInfo);
        obj.transform.SetParent(this.transform);
        // 重要，这里要重置一下scale 和 rotation,不然默认和父组件一致
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;
        obj.SetActive(true);
        UI_Enemy_HP hp = obj.AddComponent<UI_Enemy_HP>();
        hp.InitData(enemy);
    }

    public override void OnExit()
    {
    }

    void Update()
    {

        MapManager.MapUpdate();
        BattleManager.BattleUpdate();
        //UI_Battle.mTextLife.text = string.Format("Input.touchCount:{0}", Input.touchCount + "/" + Input.GetMouseButtonUp(0) + "/" + Input.GetTouch(0).phase);
        //if (BattleManager.mBattleStop == false && (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Canceled)) && !EventSystem.current.IsPointerOverGameObject())

        if (is_click_button == false && BattleManager.mBattleStop == false && Utils.isTouchAction(Input.GetMouseButtonUp(0), "up") && !EventSystem.current.IsPointerOverGameObject())
        {// 点击鼠标弹出塔菜单

            if (MapManager.mMapDarg)
            {// 如果拖动就不执行下面的内容,同时释放鼠标，结束拖动
                MapManager.mMapDarg = false;
                return;
            }
            if (mBtnTower.gameObject.activeSelf // 按钮的 active 状态
            && !EventSystem.current.IsPointerOverGameObject() // 是否点中的是按钮本身, 防止点击穿透
            )
            {
                mBtnTower.SetActive(false);
            }
            else if (!mBtnTower.gameObject.activeSelf)
            {
                Vector3 mousePos = Input.mousePosition; //鼠标坐标
                //mousePos.z = 10;
                // Vector3 pos = Input.mousePosition;
                // Vector3 transPos = Camera.main.ScreenToWorldPoint(pos);// 屏幕坐标转世界坐标
                // Debug.Log("down：" + pos + "/" + transPos);

                Vector3 transPos = Camera.main.ScreenToWorldPoint(mousePos);// 屏幕坐标转世界坐标
                                                                            //Debug.Log("GetMouseButtonDown-down：" + mousePos + "/" + transPos)
                Vector2 guiPos = new Vector2();
                // canvas 的屏幕坐标转换
                bool isInRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        transform.parent as RectTransform,
                        mousePos,
                        Camera.main,// 当前摄像机
                        out guiPos
                    );
                //Debug.Log("guiPos:" + guiPos);

                Vector3 pos = Vector3.zero;
                bool isHit = Utils.getPositionByRay(transform, guiPos, out pos);
                if (isHit && BattleManager.isCreatedTower(pos))
                {
                    Debug.Log("不能重复创建!");
                    return;
                }

                mBtnTower.SetActive(true);
                mBtnTower.transform.localPosition = new Vector3(guiPos.x, guiPos.y, 0);
            }
            //
        }
        is_click_button = false;
    }

}
