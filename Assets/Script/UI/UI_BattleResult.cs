using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleResult : UILayer
{
    public Text mTextResult;

    public void InitData(bool is_win)
    {
        if (is_win)
        {
            this.mTextResult.text = "You Win!";
            AudioManager.PlaySound("battle_success");
        }

        else
        {
            this.mTextResult.text = "You Lose!";
            AudioManager.PlaySound("battle_fail");
        }

    }

    public override void OnNodeLoad()
    {
        this.mTextResult = gameObject.GetComponentInChildren<Text>();
        Debug.Log("UI_BattleResult OnNodeLoad");
        Debug.Log(this.mTextResult);

        Button btn_back = Utils.FindComponetInObject<Button>(transform, "btn_back", () =>
        {
            BattleManager.Clear();
            UIManager.ExitAllUI();
            UIManager.EnterUI<UI_Main>();
            // 切换回主场景音乐
            AudioManager.PlayMusic("music_login");
        }) as Button;
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
