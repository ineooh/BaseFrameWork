//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
//using Spine.Unity;

/// <summary>
/// Tween the object's alpha. Works with both UI widgets as well as renderers.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha2 : UITweener
{
    //public bool isSpriteRenderer = false;

	[Range(0f, 1f)] public float from = 1f;
	[Range(0f, 1f)] public float to = 1f;

    public bool FindInActive = true;

	bool mCached = false;
	Material mMat;
	SpriteRenderer spriteRenderer;
    //SkeletonAnimation SpineAnim;
	[System.Obsolete("Use 'value' instead")]
	public float alpha { get { return this.value; } set { this.value = value; } }

	void Cache ()
	{
		mCached = true;
        //mRect = GetComponent<UIRect>();
		spriteRenderer = GetComponent<SpriteRenderer>();
        //SpineAnim = GetComponent<SkeletonAnimation>();
        if ( spriteRenderer == null)
		{
			Renderer ren = GetComponent<Renderer>();
			if (ren != null) mMat = ren.material;
		}
	}

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public float value
	{
		get
		{
            MaskableGraphic itemUI = GetComponent<MaskableGraphic>();
            if ( itemUI == null )
            {
                if ( !mCached ) Cache();
                if ( spriteRenderer != null ) return spriteRenderer.color.a;
                return mMat != null ? mMat.color.a : 1f;
            }
            else
            {
                return itemUI.color.a;
            }
		}
		set
		{
            MaskableGraphic itemUI = GetComponent<MaskableGraphic>();
            if ( itemUI == null )
            {
                if ( !mCached ) Cache();

  
                if ( spriteRenderer != null )
                {
                    SetAlpha(spriteRenderer, value);
                }

                //else if (SpineAnim != null && SpineAnim.skeleton != null)
                //{
                //    SpineAnim.skeleton.a = value;

                //    setAlphaAnimChilren(gameObject, value);
                //}

                else if ( mMat != null )
                {
                    if (mMat.color != null)
                    {
                        Color c = mMat.color;
                        c.a = value;
                        mMat.color = c;
                    }
                }
               
            }
            else
            {
            
                SetAlpha(itemUI , value);
               
            }

		}
	}

    void setAlphaAnimChilren(GameObject tmp , float alpha)
    {
        SpriteRenderer itemUI = tmp.GetComponent<SpriteRenderer>();
        if( itemUI!=null)
        {
             Color c = itemUI.color;
            c.a = alpha;
            itemUI.color = c;
        }
        SpriteRenderer[] imgs = tmp.transform.GetComponentsInChildren<SpriteRenderer>(FindInActive);
        if ( imgs.Length > 0 )
            foreach ( SpriteRenderer image in imgs )
            {
                if ( image != itemUI )
                    setAlphaAnimChilren(image.gameObject, alpha);
            }
    
    }

    void SetAlpha(MaskableGraphic itemUI, float alpha)
    {
        //if ( itemUI.gameObject.activeInHierarchy )
        //{
            Color c = itemUI.color;
            c.a = alpha;
            itemUI.color = c;

            MaskableGraphic[] imgs = itemUI.transform.GetComponentsInChildren<MaskableGraphic>(FindInActive);
            //SkeletonAnimation[] anims = itemUI.transform.GetComponentsInChildren<SkeletonAnimation>(FindInActive);
            SpriteRenderer[] sprs = itemUI.transform.GetComponentsInChildren<SpriteRenderer>(FindInActive);
            if ( imgs.Length > 0 )
            foreach ( MaskableGraphic image in imgs )
            {
                if (image != itemUI && image.gameObject.name != "LeFade")
                {    //SetAlpha(image, alpha);
                    Color tmp = image.color;
                    tmp.a = alpha;
                    image.color = tmp;
                }

            }
/*
           if (anims.Length > 0)
                foreach (SkeletonAnimation anim in anims)
                {
                    if (anim != itemUI && anim.skeleton != null && anim.gameObject.name != "LeFade")
                        anim.skeleton.a = alpha;
                }
                */
            if (sprs.Length > 0)
                foreach (SpriteRenderer spr in sprs)
                {
                    if (spr != itemUI && spr.gameObject.name != "LeFade")
                    {
                        Color a = spr.color;
                        a.a = alpha;
                        spr.color = a;
                  }
                }
      

    }

    void SetAlpha(SpriteRenderer itemUI, float alpha)
    {
        //if ( itemUI.gameObject.activeInHierarchy )
        //{
        Color c = itemUI.color;
        c.a = alpha;
        itemUI.color = c;

        SpriteRenderer[] imgs = itemUI.transform.GetComponentsInChildren<SpriteRenderer>(FindInActive);
        if ( imgs.Length > 0 )
            foreach ( SpriteRenderer image in imgs )
            {
                if ( image != itemUI )
                    SetAlpha(image, alpha);
            }
    }

    void SetAlpha(Renderer itemUI, float alpha)
    {
        //if ( itemUI.gameObject.activeInHierarchy )
        //{
        Color c = itemUI.material.color;
        c.a = alpha;
        itemUI.material.color = c;

      
       
    }
	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) { 
        
        value = Mathf.Lerp(from, to, factor); 
    }

    public override void PlayForward()
    {
        //if (Global.is_BUILDSD)
        //duration = 0;
        Play(true);
    }
    public override void PlayReverse()
    {
        //if (Global.is_BUILDSD)
        //duration = 0;
        Play(false);
    }


	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenAlpha Begin (GameObject go, float duration, float alpha)
	{
		TweenAlpha comp = UITweener.Begin<TweenAlpha>(go, duration);
		comp.from = comp.value;
		comp.to = alpha;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	public override void SetStartToCurrentValue () { from = value; }
	public override void SetEndToCurrentValue () { to = value; }
}
