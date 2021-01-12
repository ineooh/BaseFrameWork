using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreEffect : MonoBehaviour {

    public static ScoreEffect instance;

    public GameObject pf_text;
    public GameObject pf_textLive;
    public GameObject pf_text_new;
    public GameObject pf_text_levelup;

    public Color[] Colors;
    public Color[] ColorTeam;

    // Use this for initialization
    void Awake ()
    {
        instance = this;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
    public void createText_new(Vector3 pos)
    {
        pos.z += 0.5f;
        GameObject g = Instantiate(pf_text_new, pos, Quaternion.identity) as GameObject;
        g.transform.SetParent(gameObject.transform, false);
        TweenPosition tw = g.GetComponent<TweenPosition>();
        tw.from = pos;
        tw.to = pos;
        tw.to.z += 0.5f;
        tw.ResetToBeginning();
        tw.PlayForward();
    }
    public void createText_levelup(Vector3 pos)
    {
        pos.z += 0.8f;
        GameObject g = Instantiate(pf_text_levelup, pos, Quaternion.identity) as GameObject;
        g.transform.SetParent(gameObject.transform, false);
        TweenPosition tw = g.GetComponent<TweenPosition>();
        tw.from = pos;
        tw.to = pos;
        tw.to.z += 0.5f;
        tw.ResetToBeginning();
        tw.PlayForward();
    }
   
    public void createTextScoreEffect(string value , Vector3 pos , Color color , float scale = 1f , int id = 0)
    {
        pos.y += 1.0f;

        GameObject g = Instantiate(pf_text, pos, Quaternion.identity) as GameObject;
        Text textControl = g.GetComponent<Text>();
        textControl.text = value;
        g.transform.SetParent(gameObject.transform, false);
        TweenPosition tw = g.GetComponent<TweenPosition>();
        tw.from = pos;
        tw.to = pos;
        tw.to.z += 0.5f;
        tw.ResetToBeginning();
        tw.PlayForward();
        if (scale !=1)
        
        {
            TweenScale t = g.GetComponent<TweenScale>();
            t.to = Vector3.one * scale;
        }
        //if (color != null)
            textControl.color = ColorTeam[id];


        if (id == 0)
        {

        }
        else if (id == 1)
        {
            textControl.transform.Rotate(0, 0, -90);
        }
        else if (id == 2)
        {
            textControl.transform.Rotate(0, 0, -180);
        }
        else if (id == 3)
        {
            textControl.transform.Rotate(0, 0, 90);
        }
    }
   
    public void createTextLiveEffect(string value, Vector3 pos)
    {
        GameObject g = Instantiate(pf_textLive, pos, Quaternion.identity) as GameObject;
        Text textControl = g.GetComponent<Text>();

        TweenPosition tw = g.GetComponent<TweenPosition>();
        pos.x += 0.3f;
        tw.from = pos;
        tw.to = pos;
        tw.to.x += 0.3f;

        textControl.text = value;
        g.transform.SetParent(gameObject.transform, false);

        //if (color != null)
        //    textControl.color = color;
    }
}
