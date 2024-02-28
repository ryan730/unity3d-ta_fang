using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public List<Vector3> mPathList = new List<Vector3>();
    public float mMoveSpeed = 2.0f;
    public float HP = 20.0f;// 敌人血量
    public float MAXHP = 20;// 完整血量

    public UI_Enemy_HP item_hp;
    public Transform mHP_Pos;

    private Vector3 tempPosition;

    public void InitData(List<Vector3> path, int hp, EnemyInfo info)
    {
        mPathList.AddRange(path);//将项追加到数组的末尾
        if (mPathList.Count > 0)
        {
            transform.localPosition = mPathList[0];// 放在路径起始位置
            tempPosition = mPathList[0];
            mPathList.RemoveAt(0);// 位置用完就丢掉
        }

        this.MAXHP = this.HP = hp;

        Animation anim = this.GetComponent<Animation>();

        if (anim["RunFront"])
        {
            anim.PlayQueued("RunFront", QueueMode.PlayNow);
            anim.Play("RunFront");
        }

        if (anim["Run"])
        {
            AnimationState animState = anim.PlayQueued("Run", QueueMode.PlayNow);
            //var stateinfo = anim.GetCurrentAnimatorStateInfo(0);
            Animator m_Animator = gameObject.GetComponent<Animator>();
            //m_Animator.speed = 0.0f;
            anim["Run"].speed = 0.3f;
            anim.Play("Run");
        }
        Debug.Log("Index_id::" + EnemyManager.CurrentCountTotal + "///" + info.Count);
        this.mHP_Pos = this.transform.Find("hp_pos");

        if (info.BornCD > 0 && EnemyManager.CurrentCountTotal > 0)
        {
            this.mMoveSpeed = 0;//info.move_speed;
            transform.localPosition = new Vector3(9999, -100, 9999);// 仍到很远的地方
            StartCoroutine(delay(info));
        }
        else
        {

            this.mMoveSpeed = (float)info.MoveSpeed;
        }

    }

    private IEnumerator delay(EnemyInfo info)
    {
        float delayTime = (EnemyManager.CurrentCountTotal - info.Count);
        Debug.Log("Delay" + this.gameObject);
        yield return new WaitForSeconds((float)(delayTime * info.BornCD));
        this.mMoveSpeed = (float)info.MoveSpeed;
        transform.localPosition = tempPosition;
    }

    public void AddHP(float hp)
    {
        this.HP += hp;
        if (this.HP <= 0)
        {
            DeleteObj();
        }
        item_hp.RefershHP();
        Debug.Log("hp:" + this.HP);
    }

    public void BattleUpdate()
    {
        // Debug.Log("Time.deltaTime:" + Time.deltaTime);
        if (mPathList.Count > 0)
        {
            Vector3 targetPos = mPathList[0];// 每次都取第一个
            float distance = 0;
            if (transform.localPosition.x == targetPos.x)
            {
                // x 到位，沿y 行走
                distance = Mathf.Abs(transform.position.z - targetPos.z);
                if (this.transform.localPosition.z > targetPos.z)
                {//下
                    //this.transform.localEulerAngles = new Vector3(0, 180, 0);// 结果一样
                    //this.transform.localRotation = Quaternion.Euler(0, 180, 0);// 结果一样
                }
                else
                {//上
                    //this.transform.localEulerAngles = new Vector3(0, 0, 0);
                    //this.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

            }
            else
            {
                // 沿 x 行走
                targetPos.z = transform.position.z;// y 不动
                distance = Mathf.Abs(transform.position.x - targetPos.x);
                if (this.transform.localPosition.x > targetPos.x)
                {//左
                    //this.transform.localEulerAngles = new Vector3(0, 270, 0);
                    //this.transform.localRotation = Quaternion.Euler(0, 270, 0);
                }
                else
                {//右
                    //this.transform.localEulerAngles = new Vector3(0, 90, 0);
                    //this.transform.localRotation = Quaternion.Euler(0, 90, 0);
                }

            }

            //Vector3 relative = target1.InverseTransformPoint(target.position);
            //float angle = Mathf.Atan2(targetPos.x, targetPos.z) * Mathf.Rad2Deg;
            //transform.Rotate(0, angle, 0);

            Vector3 dir = transform.localPosition - targetPos;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + 180;
            //Debug.Log("angle:" + angle);
            //this.transform.localRotation = Quaternion.Euler(0, angle, 0);
            transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);// 第二个参数代表绕轴



            float total_time = distance / mMoveSpeed;// 需要花费的时间

            //Debug.Log("Time.deltaTime:" + Time.deltaTime + "::" + total_time);

            if (Time.deltaTime >= total_time)// 总时间小于单位时间 
            {
                transform.localPosition = targetPos;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime / total_time);
            }
            if (transform.localPosition == mPathList[0])
            {
                mPathList.RemoveAt(0);
            }

        }
        else
        {// 路径走完，销毁对象
            DeleteObj();
            BattleManager.EnemyAttack();
        }
    }

    public void DeleteObj()
    {
        Destroy(gameObject);
    }
}