using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

//让Component菜单下出现你自定义的类
//[AddComponentMenu("UIManager/MapManager")]

//在编辑界面让你的功能（类）起作用，编辑状态下启动 start()
//[ExecuteInEditMode]

//默认添加组件
[RequireComponent(typeof(EventTrigger))]
public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    //SerializeField]
    // int test1;

    [NonSerialized]
    public int test2;

    [HideInInspector]
    public int test3;

    //在Inspector版面中，右击包含这条标记的类，在菜单中会出现名为“XXX”的选项，点击选项，会执行被标记的功能（注：此乃标记功能也，非标记类）
   //[ContextMenu("test")]
    // public void addTest()
    // {
    //     test1 = -11;
    // }

    //MenuItem是一个特性，修饰静态方法，可以在Unity顶部菜单出现相应的按钮。
    // [MenuItem("EditorTest/EditorTest3")]
    public static void MenuItemTest()
    {
        Debug.Log("MenuItemTest");
    }


    void Start()
    {
        //test1 = -1;
        AddUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddUI()
    {
        // GameObject obj = Resources.Load<GameObject>("UIPrefabs/UI_Main");
        // GameObject ui = GameObject.Instantiate(obj);

        // GameObject ui_parent = GameObject.Find("Canvas");

        // RectTransform uiTran = ui.transform as RectTransform;
        // uiTran.SetParent(ui_parent.transform);
        // uiTran.localPosition = Vector3.zero;
        // uiTran.localScale = Vector3.one;
        // uiTran.localRotation = Quaternion.identity;

        // RectTransform rect = ui.GetComponent<RectTransform>();
        // rect.offsetMin = Vector2.zero;
        // rect.offsetMax= Vector2.zero;

        // ui.AddComponent<UI_Main>();
        AudioManager.initData();
        // AudioManager 初始化声音
        AudioManager.PlayMusic("music_login");
        MapManager.mMapObj = GameObject.Find("Map");

        Component ui = UIManager.EnterUI<UI_Main>() as Component;

    }

}
