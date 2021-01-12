using UnityEngine;
using System.Collections;

public class ProcessBar : MonoBehaviour {

    public SpriteRenderer bar;
    public SpriteRenderer barFull;
    public bool horizontal = true;
    public bool center = false;
    Vector3 beginScale;
    bool isInit = false;
    float xBegin;
    float curRatio = 1;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        if (isInit == false)
        {
            isInit = true;
            beginScale = barFull.transform.localScale;
            if (horizontal)
                xBegin = -barFull.bounds.size.x / 2;
            else
                xBegin = -barFull.bounds.size.y / 2;
        }
    }

    public void set(float value)
    {
        Init();
        value = value > 1 ? 1 : value;
        value = value < 0 ? 0 : value;

        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash("from", curRatio,
                                            "to", value,
                                            "time", 0.2f,
                                            "onupdatetarget", gameObject,
                                            "onupdate", "onUpdate",
                                            //"oncomplete", "onComplete",
                                            "easetype", iTween.EaseType.linear
                                           ));

        
    }

    void onUpdate(float value)
    {
        curRatio = value;
        Vector3 scale = beginScale;
        if (horizontal)
            scale.x = value * beginScale.x;
        else
            scale.y = value * beginScale.y;
        barFull.transform.localScale = scale;
    }

    public void setColor(Color color)
    {
        barFull.color = color;
    }

    public void SetOrder(int order )
    {
        bar.sortingOrder = order;
        barFull.sortingOrder = order+1;
    }
}
