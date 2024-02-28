using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enemy_HP : MonoBehaviour
{
    public Scrollbar mBarHP;
    public Enemy mTarget;

    public void InitData(Enemy enemy)
    {
        this.mTarget = enemy;
        enemy.item_hp = this;
        this.mBarHP = this.transform.Find("bar_hp").GetComponent<Scrollbar>();
        this.RefershHP();
        this.RefershPos();
        //Debug.Log("(enemy.HP * 1f / enemy.MAXHP)====" + (enemy.HP * 1f / enemy.MAXHP));
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void lateUpdate()
    {
        if (!this.mTarget)
        {
            Destroy(gameObject);
        }
        else
        {
            this.RefershPos();
        }

    }

    void LateUpdate()
    {
        if (this.mTarget == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.RefershPos();
        }
    }

    public void RefershPos()
    {
        Vector3 pos = this.mTarget.mHP_Pos.position;
        pos = Camera.main.WorldToScreenPoint(pos);

        // pos.y += 15;
        // pos.x += 5;

        // 一下算法只适合 canvas render mode 为 screen space - overly
        // pos.x -= Screen.width >> 1;
        // pos.y -= Screen.height >> 1;
        // pos = pos * 640 / Screen.height;
        //this.transform.localPosition = pos;

        Vector2 guiPos = new Vector2();

        // rect: 对应的 RectTransform 的引用
        // screenPoint: 位置，基于屏幕坐标系
        // cam: 相机的引用，如果Canvas的Render Mode 为 Screen Space - Camera 模式，则需要填入 Render Camera 对应的引用
        // localPoint: rect 本地坐标系下的坐标（原点(0,0)位置受Anchor的影响）
        // // canvas 的屏幕坐标转换
        bool isInRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent as RectTransform,
                pos,
                Camera.main,// 当前摄像机
                out guiPos
            );
        //Debug.Log("guiPos:" + pos+'/'+guiPos);

        RectTransform rect = transform.parent as RectTransform;

        //Debug.Log("guiPos:" + rect.rect);

        this.transform.localPosition = guiPos;
    }

    public void RefershHP()
    {

        //Debug.Log("this.mTarget.HP * 1f / this.mTarget.MAXHP==="+this.mTarget.HP+"/"+this.mTarget.MAXHP);
        this.mBarHP.size = (this.mTarget.HP * 1f / this.mTarget.MAXHP);//整型转浮点
    }
}
