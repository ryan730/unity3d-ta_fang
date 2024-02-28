using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemy mTarget;
    private float mMoveSpeed;

    public float mAttackValue;

    private Vector3 prevTargetPos;

    private Vector3 sourcePos;// 子弹发出的位置

    private float bulletFlyLimit = 5;// 子弹飞出多远会失效

    // Start is called before the first frame update
    public void InitData(Object enemy, Vector3 pos, float speed, float atv, float flyLimit)
    {
        if (enemy)
        {
            this.mTarget = enemy as Enemy;
            this.prevTargetPos = this.mTarget.transform.localPosition;
        }
        else // 如果敌人已经死亡，延迟造成，取敌人死之前的坐标
        {
            this.mTarget = null;
            this.prevTargetPos = pos;
        }
        this.mMoveSpeed = speed;
        this.mAttackValue = atv;
        this.sourcePos = pos;
        this.bulletFlyLimit = flyLimit;

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (!this.mTarget)
        // {
        //     return;
        // }
        Vector3 selfPos = this.transform.localPosition;
        Vector3 targetPos = this.mTarget ? this.mTarget.transform.localPosition : this.prevTargetPos;
        float attackRange = Vector3.Distance(selfPos, targetPos);
        float flyRange = Vector3.Distance(selfPos, sourcePos);
        float current_time = this.mMoveSpeed > 0 ? attackRange / this.mMoveSpeed : 0;// 剩余的时间
        if (this.mTarget)
        {
            this.prevTargetPos = this.mTarget.transform.localPosition;
        }
        if (Time.deltaTime >= current_time)// 如果剩余时间小于两帧最小间隔
        {
            //this.transform.localPosition = targetPos;

            if (this.mTarget)
            {
                this.mTarget.AddHP(-this.mAttackValue);
                AudioManager.PlaySound("hit");
            }

            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
        else if (flyRange < bulletFlyLimit)// 子弹飞行的距离内
        {
            this.transform.localPosition = Vector3.Lerp(selfPos, targetPos, Time.deltaTime / current_time);
        }
        else
        {
            this.transform.localPosition = sourcePos;// 子弹飞行的距离太远
            gameObject.SetActive(false);
        }
    }
}
