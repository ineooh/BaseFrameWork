using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarFill : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Image img;
    public bool emptyBegin = false;
    public float speed = 4; 
    public TextMeshOutLine textHp;
    float curRatio = 1;
    float ratioDisplay = 1;

    Vector2 MaxSize;
    Vector2 curSize;

    // Use this for initialization
    void Awake ()
    {
      
        if (!emptyBegin)
        {
            curRatio = ratioDisplay = 1;
            if (sprite != null)
            {
                curSize = MaxSize = sprite.size;
                curSize.x = MaxSize.x;
                sprite.size = curSize;
            }
            if (img != null)
                img.fillAmount = 1;
        }
        else
        {
            curSize.x = 0;
            curRatio = ratioDisplay = 0;
            if (sprite != null)
                sprite.size = curSize;
            if (img != null)
                img.fillAmount =0;
        }
    }


    // Update is called once per frame
    void Update ()
    {
		if(ratioDisplay != curRatio)
        {
            ratioDisplay = Mathf.Lerp(ratioDisplay, curRatio, Time.deltaTime * speed);
            curSize.x = ratioDisplay* MaxSize.x;
            if (sprite != null)
                sprite.size = curSize;
            else if (img != null)
                img.fillAmount = ratioDisplay;
            if (maxHp > 0)
            {
                int hp = Mathf.CeilToInt(10 * ratioDisplay * maxHp);
                int max = Mathf.CeilToInt(10 * maxHp);
                if (Mathf.FloorToInt(10 * ratioDisplay * maxHp) == 0)
                    hp = 0;
                string a = string.Format("{0}/{1}", hp, max);
                textHp.Set(a);
            }
        }
	}
    float maxHp =1;
    public void Set( float ratio , float max = 0 )
    {
        maxHp = max;
        curRatio = ratio;
        curRatio = Mathf.Min(1, ratio);
        curRatio = Mathf.Max(0, ratio);
       
    }
    
}
