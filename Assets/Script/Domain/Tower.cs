using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float mAttackRange = 5.0f;//攻击范围
    public float mAttackCD = 0.8f;//攻击cd
    public float mAttack; // 攻击力
    public string mBulletModelName;// 子弹模型
    public string mAttckAnimName;// 子弹模型
    public float mBulletMoveSpd;// 子弹速度
    public float mBulletFlyLimit;// 子弹失效距离
    public Enemy mAttackTarget;
    private float __time = 0;
    private Animation attackAnim;

    public void InitData(TowerInfo info)
    {
        // 对象初始化
        mAttack = (float)info.Attack;
        mAttackCD = (float)(info.Attack_CD);
        mAttackRange = (float)info.AttackRange;
        mBulletMoveSpd = (float)info.BullerMoveSpd;
        mBulletModelName = info.BulletModelName;
        mBulletFlyLimit = (float)info.BulletFlyLimit;
        mAttckAnimName = info.AttckAnimName;
    }

    // Start is called before the first frame update
    void Start()
    {
        attackAnim = gameObject.GetComponent<Animation>();
        //attackAnim["Idle"].layer =-1;//调整动画层次
        //attackAnim["AttackMelee2"].layer = 1;
        attackAnim[mAttckAnimName].layer = 1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * mAttackRange;
        Gizmos.DrawRay(transform.localPosition, direction);
    }

    void DrawSightUpdate()
    {
        Vector3 direction = transform.TransformDirection(Vector3.forward) * mAttackRange;
        Debug.DrawRay(transform.position, direction, Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        // this.__time += Time.deltaTime;
        // if(this.__time >= this.mAttackCD) {
        //     this.__time = 0;
        //     if(this.mAttackTarget == null) {
        //         this.mAttackTarget = this.GetAttackTarget();
        //         if(this.mAttackTarget != null) {
        //             this.transform.LookAt(this.mAttackTarget.transform);
        //             AttackEnemy(this.mAttackTarget);
        //         }
        //     } else if(this.IsAttackEnemy(this.mAttackTarget)) {
        //         AttackEnemy(this.mAttackTarget);
        //     } else {
        //         this.mAttackTarget = this.GetAttackTarget();
        //         if(this.mAttackTarget != null) {
        //             this.transform.LookAt(this.mAttackTarget.transform);
        //             AttackEnemy(this.mAttackTarget);
        //         }
        //     }
        // }

        if (IsAttackEnemy(this.mAttackTarget))
        {

            this.transform.LookAt(this.mAttackTarget.transform);
            this.__time += Time.deltaTime;
            if (this.__time > this.mAttackCD)
            {
                this.__time = 0;
                Debug.Log("冷却后攻击敌人");
                AttackEnemy(this.mAttackTarget);
            }

        }
        else
        {
            this.mAttackTarget = GetAttackTarget();
            if (this.mAttackTarget != null)
            {
                this.transform.LookAt(this.mAttackTarget.transform);
                Debug.Log("立即攻击敌人");
                AttackEnemy(this.mAttackTarget);
            }
        }
        DrawSightUpdate();
    }

    private void AttackEnemy(Enemy target)
    {
        AnimationState animState = this.attackAnim.PlayQueued(mAttckAnimName, QueueMode.PlayNow, PlayMode.StopSameLayer);
        animState.speed = this.mAttackCD > 1 ? this.mAttackCD : 1 / this.mAttackCD;// 控制播放速度，动画原始时长30毫秒
        this.StartCoroutine(WaitAttack(target, animState.speed * 0.5f));


    }

    public IEnumerator WaitAttack(Enemy target, float time)
    {
        Vector3 pos = target.transform.localPosition;
        yield return new WaitForSeconds(time);
        AudioManager.PlaySound("attack");
        Bullet bullet = BulletManager.Create(target, mBulletModelName);
        bullet.InitData(target, pos, mBulletMoveSpd, mAttack, mBulletFlyLimit);
        bullet.transform.localPosition = this.transform.localPosition;
    }

    // 当前敌人是否还在射程内
    public bool IsAttackEnemy(Enemy enemy)
    {
        if (enemy == null)
        {
            return false;
        }
        Vector3 towerPos = this.transform.localPosition;
        Vector3 enemyPos = enemy.transform.localPosition;
        if (Vector3.Distance(towerPos, enemyPos) < this.mAttackRange)
        {
            return true;
        }
        return false;
    }

    // 获取当前在射程内的敌人
    public Enemy GetAttackTarget()
    {
        for (var i = 0; i < BattleManager.mEnemyList.Count; i++)
        {
            Vector3 towerPos = this.transform.localPosition;
            Enemy enemy = BattleManager.mEnemyList[i];
            if (enemy)
            {
                Vector3 enemyPos = enemy.transform.localPosition;
                if (Vector3.Distance(towerPos, enemyPos) < this.mAttackRange)
                {
                    return enemy;
                }
            }

        }
        return null;
    }
}
