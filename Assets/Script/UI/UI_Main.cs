using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main : UILayer
{
    public Text mTextTitle;
    public Button mBtnStart;
    // 
    public Image mImageMusic;
    public Image mImageSound;
    // 设置背景音乐开关的函数
    public void SetMusicSwitch(bool isOn)
    {
        // 打开与关闭的判断
        if (isOn)
        {
            mImageMusic.rectTransform.anchoredPosition = new Vector3(-23, 0, 0);
            // 还原成当前颜色,颜色值 #3C7BD9
            mImageMusic.color = new Color32(60, 123, 217, 255);
        }
        else
        {
            mImageMusic.rectTransform.anchoredPosition = new Vector3(23, 0, 0);
            // 修改为灰色
            mImageMusic.color = Color.gray;
        }
    }
    // 设置背景声音开关的函数
    public void SetSoundSwitch(bool isOn)
    {
        if (isOn)
        {
            mImageSound.rectTransform.anchoredPosition = new Vector3(-23, 0, 0);
            // 还原成当前颜色,颜色值 #3C7
            mImageSound.color = new Color32(60, 123, 217, 255);
        }
        else
        {
            mImageSound.rectTransform.anchoredPosition = new Vector3(23, 0, 0);
            // 修改为灰色
            mImageSound.color = Color.gray;
        }
    }
    public override void OnNodeLoad()
    {
        // this.mTextTitle.transform.localPosition = new Vector3(Random.Range(-300, 300), Random.Range(-300, 300));
        mTextTitle = transform.Find("text_title").GetComponent<Text>();
        //mBtnStart = transform.Find("btn_play").GetComponent<Button>();
        mTextTitle.text = "3D 塔防教程案例";
        Debug.Log("OnNodeLoad");

        //获取声音组件
        // mImageMusic = transform.Find("btn_music").GetComponent<Image>();
        // mImageSound = transform.Find("btn_sound").GetComponent<Image>();

        mImageMusic = transform.Find("btn_music/Image").GetComponent<Image>();
        mImageSound = transform.Find("btn_sound/Image").GetComponent<Image>();
    }
    public override void OnEnter()
    {
        Debug.Log("OnEnter");
        // 使用上面的开关函数，设置为打开
        SetMusicSwitch(AudioManager.mIsMusicOn);
        SetSoundSwitch(AudioManager.mIsSoundOn);

    }

    public void aa(object a)
    {
        Debug.Log("aa");
    }

    public void aa(int a) // 重载
    {
        Debug.Log("bb");
    }
    public override void OnExit()
    {
        Debug.Log("OnExit");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start-zi");
        //aa("1");
        // mBtnStart.onClick.AddListener(() =>
        // {
        //     print("123");
        // });
        Button btn_play = Utils.FindComponetInObject<Button>(transform, "btn_play", () =>
        {
            print("123");
            // Component ui = UIManager.EnterUI<UI_Battle>() as Component;
            UIManager.EnterUI<UI_SelectLevel>();
            Close();
        }) as Button;

        Button btn_close = Utils.FindComponetInObject<Button>(transform, "btn_close", () =>
        {
            UIManager.ExitUI(this);
        }) as Button;
        //btn_close.onClick.AddListener();

        Button btn_All = Utils.FindComponetInObject<Button>(transform, "btn_closeAll", () =>
        {
            UIManager.ExitAllUI();
        }) as Button;

        Button btn_start = Utils.FindComponetInObject<Button>(transform, "btn_start", () =>
        {
            Component ui = UIManager.EnterUI<UI_Main>() as Component;
            ui.transform.localPosition = new Vector3(Random.Range(-300, 300), Random.Range(-300, 200));
        }) as Button;

        Button btn_music = Utils.FindComponetInObject<Button>(transform, "btn_music", () =>
       {
           bool bol = !AudioManager.mIsMusicOn;
           SetMusicSwitch(bol);
           AudioManager.setMusicOn(bol);
       }) as Button;

        Button btn_sound = Utils.FindComponetInObject<Button>(transform, "btn_sound", () =>
        {
            bool bol = !AudioManager.mIsSoundOn;
            SetSoundSwitch(bol);
            AudioManager.setSoundOn(bol);
        }) as Button;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
