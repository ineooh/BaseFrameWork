using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPS : MonoBehaviour
{
	#if UNITY_IOS 
	public const float fixDeltaTime = 1f / 60;
		public static float deltaTime   = 1f/60;
	#elif UNITY_EDITOR
	public const float fixDeltaTime = 0.025f;//1f/60f;
	public static float deltaTime =0.025f;// 1f/60f;
	#else
    	public const float fixDeltaTime = 0.025f;
    	public static float deltaTime = 0.025f;
	#endif
    public static float max = 40;
    public static float min = 30;
    public Text guiTxt;
    private int frames = 0;
    static public int AverageFPS = 60;
    static public bool isSlowDevice = false;
    int countframes = 10;
    Queue stack = new Queue();
    public static int quality = 2;
    public static int curQuality = -1;
    public string curFps = "";
    public GUIStyle style;
    void Start()
    {
//#if UNITY_EDITOR
//#else
//        guiTxt.gameObject.SetActive(false);
//#endif
		Application.targetFrameRate = 60;
        InvokeRepeating("updateText", 0, 1.0f);
    

    }
    void updateText()
    {
        //return ;
        //if (PlayScene.instance.isState_Run() == false) return;
        if (Time.timeScale != 1) return;
        if (frames == 0) return;
        if (stack.Count < countframes)
        {
            stack.Enqueue(frames);
        }
        else
        {
            stack.Dequeue();
            stack.Enqueue(frames);

            int count = 0;
            foreach (int f in stack)
            {
                count += f;
            }
            AverageFPS = count / stack.Count;
        }

        //guiTxt.text = frames.ToString() + " FPS";
        //guiTxt.text = frames.ToString() + " FPS";
        curFps = frames.ToString() + " FPS";
        frames = 0;
      
    }
    private void OnGUI()
    {

		GUI.Label(new Rect(Screen.width / 2, Screen.height - 70, 200, 30), curFps, style);
	}

    void Update()
    {
        if( Time.timeScale == 1)
            ++frames;
    }
    static public bool checkSlow()
    {
        return isSlowDevice;
    }
}
