using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    public CanvasGroup tips;
    public void Show(string txt)
    {
        ShowTips(tips, txt);
    }
    private void ShowTips(CanvasGroup tipCanvasGroup, string msg)
    {
        tipCanvasGroup.GetComponentInChildren<Text>().text = msg;

        tipCanvasGroup.alpha = 0;
        DOTween.Kill(tipCanvasGroup);
        Sequence sequence = tipCanvasGroup.DOSequence();
        sequence.Append(tipCanvasGroup.DOFade(1, 0.8f));
        sequence.AppendInterval(2.0f);
        sequence.Append(tipCanvasGroup.DOFade(0, 1.0f));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public static class GameObjExt
{
    public static Sequence DOSequence(this UnityEngine.Object seq)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.target = seq;

        return sequence;
    }
}