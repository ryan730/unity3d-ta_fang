using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILayer : MonoBehaviour {

    public void __init_node(Transform tf) {
        this.OnNodeAsset(tf.name, tf.gameObject);
        Button btn = tf.GetComponent<Button>();
        if(btn != null) {
            btn.onClick.AddListener(() => {
                this.OnButtonClick(btn.name, btn.gameObject);
            });
        }
        for(int i = 0; i < tf.childCount; i++) {
            this.__init_node(tf.GetChild(i));
        }
    }

    void Awake() {
        Debug.Log("Awake");
        this.OnNodeLoad();
    }
    void Start() {
        Debug.Log("Start");
        this.OnEnter();
    }

    public void OnDestroy() {
        this.OnExit();
    }

    public void Close() {
        UIManager.ExitUI(this);
    }

    public virtual void OnNodeLoad() {//UI加载
        Debug.Log("UI加载");
    }
    public virtual void OnEnter() {//UI启动完成
        Debug.Log("UI启动完成");
    }
    public virtual void OnExit() {
    }

    public virtual void OnButtonClick(string name, GameObject obj) {
    }
    public virtual void OnNodeAsset(string name, GameObject obj) {
    }
}
