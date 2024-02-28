using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 防御塔管理器
public class TowerManager
{
    public const string Path = "Model/Role/";

    public static Tower CreateTower(TowerInfo info)
    {
        GameObject obj = Resources.Load<GameObject>(Path + info.ModelName);
        GameObject tower = GameObject.Instantiate(obj);
        Tower tr = tower.AddComponent<Tower>();
        tr.InitData(info);
        return tr;
    }

    // 创建防守塔选择菜单

    public static void CreateTowerMenus(Transform parant, GameObject container, GameObject mBtnRole, List<TowerInfo> tower_list)
    {
        for (int i = 0; i < tower_list.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(mBtnRole);
            obj.SetActive(true);
            obj.GetComponentInChildren<Text>().text = tower_list[i].menus?.name;
            RectTransform objMenu = obj.transform as RectTransform;
            int index = i;
            obj.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                TowerInfo tower_info = tower_list[index];
                CreateTowerInMap(parant, container, tower_info);

            });
            objMenu.SetParent(container.transform);
            objMenu.localPosition = Vector3.zero;
            objMenu.localRotation = Quaternion.identity;
            objMenu.localScale = Vector3.one;
            ///this.mMenusList.Add(obj);
        }
    }

    // 创建防守塔,参数是一个对象
    public static void CreateTowerInMap(Transform parant, GameObject mBtnTower, TowerInfo towerInfo)
    {
        Vector3 currentPos = mBtnTower.transform.localPosition;
        /////mTextLife.text = string.Format("坐标" + currentPos.ToString());
        Vector3 pos = Vector3.zero;
        bool isHit = Utils.getPositionByRay(parant, currentPos, out pos);
        if (isHit && !BattleManager.isCreatedTower(pos))
        {
            BattleManager.CreateTower(pos, towerInfo);
            mBtnTower.SetActive(false);
            UI_Battle.is_click_button = true;
        }

    }
    
}
