using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectLevel : UILayer
{
    public void InitData(bool is_win)
    {

    }

    public override void OnNodeLoad()
    {
        // this.mTextResult = gameObject.GetComponentInChildren<Text>();
        // Debug.Log("UI_BattleResult OnNodeLoad");
        // Debug.Log(this.mTextResult);

        Button btn_back = Utils.FindComponetInObject<Button>(transform, "btn_back", () =>
        {
            this.Close();
            UIManager.EnterUI<UI_Main>();
        }) as Button;

        for (int i = 1; i < 10; i++)
        {
            int index = i;//注意，这里的index每次循环内是独立的一个值，不是同一个引用
            Button btn_leve = Utils.FindComponetInObject<Button>(transform, "btn_lv/btn_lv_" + i, () =>
            {
                jumpLevel(index.ToString());// 跳到各个关
            }) as Button;
        }

    }

    private void jumpLevel(string number)
    {
        Debug.Log("-------------------------------------");
        Debug.Log(this);
        this.Close();
        UIManager.EnterUI<UI_Battle>().InitData(number);
    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }

    public override void OnButtonClick(string name, GameObject obj)
    {

    }

    public override void OnNodeAsset(string name, GameObject obj)
    {

    }
}
