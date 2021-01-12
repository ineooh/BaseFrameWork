//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright Â© 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
//using Spine.Unity;
/// <summary>
/// Tween the object's color.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Color")]
public class TweenColor : UITweener
{
	public Color from = Color.white;
    public Color to = Color.white;
    public bool isHaveChild = false;
    SpriteRenderer[] imgsSpriteRenders;// = itemUI.transform.GetComponentsInChildren<SpriteRenderer>();
    Image[] imgImages;
	bool mCached = false;
    //UIWidget mWidget;
	Material mMat;
	Light mLight;
	SpriteRenderer mSr;
    MaskableGraphic mUI;
    //Spine.Skeleton skeleton;
	void Cache ()
	{
		mCached = true;
        //mWidget = GetComponent<UIWidget>();
        //if (mWidget != null) return;

		mSr = GetComponent<SpriteRenderer>();
        if (mSr != null)
        {
            if (isHaveChild)
            {
                imgsSpriteRenders = GetComponentsInChildren < SpriteRenderer>();
            }
            return;
        }

        //if( GetComponent<SkeletonAnimation>()!=null)
        //    skeleton = GetComponent<SkeletonAnimation>().skeleton;

        //if (skeleton != null) return;
        MeshRenderer ren = GetComponent<MeshRenderer>();
        if (ren != null)
		{
			mMat = ren.material;
			return;
		}
      
        mLight = GetComponent<Light>();
        //if (mLight == null) mWidget = GetComponentInChildren<UIWidget>();

        mUI = GetComponent<MaskableGraphic>();
	}

	[System.Obsolete("Use 'value' instead")]
	public Color color { get { return this.value; } set { this.value = value; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public Color value
	{
		get
		{
			if (!mCached) Cache();
            if (mUI != null) return mUI.color;
            //if (mWidget != null) return mWidget.color;
            //else if (skeleton != null)
            //{
            //    //Debug.Log("a");
            //    skeleton.GetColor();
            //}
            else if (mMat != null) return mMat.color;
            else if (mSr != null) return mSr.color;
            else if (mLight != null) return mLight.color;
            
			return Color.black;
		}
		set
		{
			if (!mCached) Cache();
			if (mUI != null)
			{
				//mUI.color = value;
				SetColor(mUI, value);
			}
			//else if (mWidget != null) mWidget.color = value;
			//else if (skeleton != null)
			//{
			//    //Debug.Log("b");

			//    skeleton.SetColor(value);
			//}
			else if (mMat != null)
			{
				//mMat.color = value;
				mMat.SetColor("_Color", value);
			}
			else if (mSr != null)
			{
				mSr.color = value;
				if (isHaveChild)
					foreach (SpriteRenderer item in imgsSpriteRenders)
					{
						item.color = value;
					}
			}
			else if (mLight != null)
			{
				mLight.color = value;
				mLight.enabled = (value.r + value.g + value.b) > 0.01f;
			}
		}
	}

    void SetColor(MaskableGraphic itemUI, Color color)
    {
        itemUI.color = color;
		if (isHaveChild)
		{
			MaskableGraphic[] imgs = itemUI.transform.GetComponentsInChildren<MaskableGraphic>();
			if (imgs.Length > 0)
				foreach (MaskableGraphic image in imgs)
				{
					if (image != itemUI)
						SetColor(image, color);
				}
		}

    }
    void SetColor(SpriteRenderer itemUI, Color color)
    {
        itemUI.color = color;
        SpriteRenderer[] imgs = itemUI.transform.GetComponentsInChildren<SpriteRenderer>();
        if (imgs.Length > 0)
            foreach (SpriteRenderer image in imgs)
            {
                if (image != itemUI)
                    image.color = color;
            }

    }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) { value = Color.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenColor Begin (GameObject go, float duration, Color color)
	{
#if UNITY_EDITOR
		if (!Application.isPlaying) return null;
#endif
		TweenColor comp = UITweener.Begin<TweenColor>(go, duration);
		comp.from = comp.value;
		comp.to = color;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value; }

	[ContextMenu("Assume value of 'From'")]
	void SetCurrentValueToStart () { value = from; }

	[ContextMenu("Assume value of 'To'")]
	void SetCurrentValueToEnd () { value = to; }
}
