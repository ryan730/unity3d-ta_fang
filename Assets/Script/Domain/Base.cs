using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class EnemyInfo
{
    public string ModelName;
    public int HP;
    public double MoveSpeed;
    public int Count;
    public double Size = 1;
    // 属于第几波敌人
    public int Wave;
    // 出生间隔
    public double BornCD;
}
[Serializable]
public enum TowerType    //枚举类型
{
    Melee = 0,
    Shooter = 1,
    Magic = 2,
}

// 防御塔的属性描述
public class TowerInfo
{
    public string ModelName;
    public double Attack;
    public double AttackCD;
    public double Attack_CD;
    public double AttackRange;

    public string BulletModelName;
    public double BullerMoveSpd;
    public double BulletFlyLimit;// 子弹飞出多远会失效
    // 攻击动画名称
    public string AttckAnimName;
    // 防守菜单
    public TowerMenusInfo menus;

}
[Serializable]
public class TowerMenusInfo
{
    public string name;
    public int level;
    public List<object> skin;

}