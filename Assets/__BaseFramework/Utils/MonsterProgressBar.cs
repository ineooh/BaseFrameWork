using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterProgressBar : MonoBehaviour {
    public static MonsterProgressBar instance;
    public Image bar;
    public Image barfull;

    Vector2 beginsize;

    float curRatio = 1;
	void Awake ()
    {
        instance = this;
        beginsize = bar.rectTransform.sizeDelta;
	}

   public void Set( float ratio)
    {
        ratio = Mathf.Min(1, ratio);
        ratio = Mathf.Max(0.001f, ratio);
        Vector2 size = beginsize;
        size.x = size.x * ratio;

        //bar.rectTransform.sizeDelta = size;
        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash("from", curRatio,
                                            "to", ratio,
                                            "time", 0.5f,
                                            "onupdatetarget", gameObject,
                                            "onupdate", "onUpdate",
                                            //"oncomplete", "onComplete",
                                            "easetype", iTween.EaseType.easeInOutCubic
                                           ));
    }


	void onUpdate( float value)
    {
        if (value == 0)
            barfull.enabled = false;
        else
            barfull.enabled = true;

            curRatio = value;
        Vector2 size = beginsize;
        size.x = size.x * value;
        bar.rectTransform.sizeDelta = size ;
    }
}
