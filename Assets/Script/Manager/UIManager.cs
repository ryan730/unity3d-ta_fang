using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 所有UI 共享方法
public class UIManager : MonoBehaviour
{
    private static GameObject mUIRoot;
    private static string UIPath = "UIPrefabs/";
    private static List<MonoBehaviour> mUIList = new List<MonoBehaviour>();

    private static void InitUI()
    {
        mUIRoot = GameObject.Find("Canvas");
    }

    //添加脚本绑定到gameobject
    public static T EnterUI<T>() where T : MonoBehaviour
    {
        if (mUIRoot == null)
        {
            InitUI();
        }
        GameObject obj = Resources.Load<GameObject>(UIPath + typeof(T).ToString());
        GameObject ui = GameObject.Instantiate(obj);

        //GameObject ui_parent = GameObject.Find("Canvas");

        RectTransform uiTran = ui.transform as RectTransform;
        uiTran.SetParent(mUIRoot.transform);
        uiTran.localPosition = Vector3.zero;
        uiTran.localScale = Vector3.one;
        uiTran.localRotation = Quaternion.identity;

        RectTransform rect = ui.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;

        T t = ui.GetComponent<T>();
        if (t == null)
        {
            t = ui.AddComponent<T>();
            mUIList.Add(t);
        }

        Debug.Log(typeof(T).ToString() + "加载成功");
        return ui.GetComponent<T>();

        // return ui.AddComponent<T>();
    }

    public static void ExitUI(MonoBehaviour mono){
        mUIList.Remove(mono);
        Destroy(mono.gameObject);
    }

     public static void ExitAllUI(){
        for (var i = 0; i < mUIList.Count; i++)
        {
            MonoBehaviour mono = mUIList[i];
            Destroy(mono.gameObject);
        }
        mUIList.Clear();
    }

}
