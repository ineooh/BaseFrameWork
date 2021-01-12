using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIBase : MonoBehaviour
{
    public GUIHandlerBase handler;

    public Animation animController;
    public bool isCheckScale = true;

    private void CheckScale()  //Suitable for all device resolution
    {
        if (!isCheckScale) return;
        float ratio = (float)Screen.width / (float)Screen.height;
        if (ratio > 0.5625f)     //Minh.ho: 0.5625 is the ratio of FullHD resolution: 1080*1920
        {
            this.GetComponent<CanvasScaler>().matchWidthOrHeight = .8f;
        }
        else
        {
            this.GetComponent<CanvasScaler>().matchWidthOrHeight = .4f;
        }
    }


    protected void AddActionToAllButton(UnityEngine.Events.UnityAction action)
    {
        Button[] listButton = gameObject.GetComponentsInChildren<Button>(true);
        foreach (Button button in listButton)
        {
            button.onClick.AddListener(action);
        }
    }

    public virtual bool Show(params object[] @parameter)
    {
        if (handler == null)
            return false;
        CheckScale();
        return handler.Show(@parameter);
    }

    public virtual void Hide(params object[] @parameter)
    {
        if (handler == null)
            return;
        handler.Hide(@parameter);
    }
    public virtual void HideNoAnim(params object[] @parameter)
    {
        if (handler == null)
            return;
        handler.Hide(@parameter);
    }
    public virtual void OnBeginShowing() { }
    public virtual void OnEndShowing() { }
    public virtual void OnBeginHidding() { }
    public virtual void OnEndHidding() { }
}