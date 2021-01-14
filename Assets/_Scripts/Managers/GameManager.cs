using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMono<GameManager> {

    public float g;
    public Vector3 startVelocity;

    /// <summary>
    /// 0 tap to start, 1 drag to aim, 2 throwing, 3 lining up, 4 gauging power, 5 fighting, 6 result UI, 7 camera to new map
    /// </summary>

    public int phase;

    public List<Collider> boxColliders;
    public Collider groundCollider;

    public Bomb[] bombs;

    float phase3StartTime;

    private void Start() {
        int level = GSystem.userData.level;
        int stage = GSystem.userData.stage;
        int mapIndex = ((level - 1) * 4 + (stage - 1)) % 5 + 1;
        if (SceneManager.GetActiveScene().name != "Gameplay" + mapIndex) {
            SceneManager.LoadScene("Gameplay" + mapIndex);
        }
        FightManager.Instance.Startup();
        PrepareLevel(level, stage);
        if (GlobalManager.Instance.inLevel) {
            startupPanel.SetActive(false);
            dragToAimPanel.SetActive(true);
            scoreboardPanel.SetActive(true);
            StartLevel(level, stage);
        }
    }

    private void Update() {
        //return;
        if (phase == 0) {       // tap to start
            if (Input.GetMouseButtonDown(0)) {
                phase = 1;
                startupPanel.SetActive(false);
                dragToAimPanel.SetActive(true);
                scoreboardPanel.SetActive(true);
                StartLevel(GSystem.userData.level, GSystem.userData.stage);
            }
        }
        else if (phase == 1) {       // drag to aim
            if (Input.GetMouseButtonDown(0)) {
                phase = 2;
                dragToAimPanel.SetActive(false);
                Opponent.Instance.AutoThrow();
            }
        }
        else if (phase == 3) {
            if (Time.time - phase3StartTime > 3f) {
                GaugePower();
                foreach (Fighter f in FightManager.Instance.fighters) {
                    f.State = 3;
                }
            }
        }
        if (phase != 0 && phase != 3) {

            if (Input.GetKeyDown(KeyCode.T)) {
                Test();
            }
            else if (Input.GetKeyDown(KeyCode.A)) {
                PrevLevel();
            }
            else if (Input.GetKey(KeyCode.S)) {
                RestartLevel();
            }
            else if (Input.GetKeyDown(KeyCode.D)) {
                NextLevel();
            }
        }
    }


    private void ChangeColorSettings() {
        RenderSettings.ambientLight = new Color32(149, 120, 136, 255);
        RenderSettings.ambientIntensity = 0;
        RenderSettings.fogColor = new Color32(255, 85, 120, 255);
    }


    #region Levels

    // call on startup
    private void PrepareLevel(int level, int stage) {
        StopAllCoroutines();
        phase = 0;

        LevelBuilder.Instance.BuildLevel(level, stage);
    }

    private void StartLevel(int level, int stage) {

        StopAllCoroutines();
        phase = 1;

        ToggleVictoryUI(0);
        lossPanel.SetActive(false);
        powerGaugePanel.SetActive(false);
        scoreboardPanel.SetActive(true);
        dragToAimPanel.SetActive(true);

        UIManager.Instance.ChangeOpponentID(level, stage);


        boxColliders = new List<Collider>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Box")) {
            if (obj.activeInHierarchy)
                boxColliders.Add(obj.GetComponent<Collider>());
        }

        bombs = GameObject.FindObjectsOfType<Bomb>();


        ObjectPool.Instance.StartLevel();
        if (stage != 4)
            FightManager.Instance.StartLevel();
        else
            FightManager.Instance.StartLevelStage4(level);
        Player.Instance.StartLevel();


        Opponent.Instance.StartLevel(LevelBuilder.Instance.map);

    }

    private void PrevLevel() {
        int stage = GSystem.userData.stage;
        int level = GSystem.userData.level;
        stage = (stage + 2) % 4 + 1;
        if (stage == 4)
            level--;
        if (stage == 3)
            stage = 2;
        if (level < 1)
            level = 1;
        GSystem.userData.stage = stage;
        GSystem.userData.level = level;
        GSystem.SaveUserData();
        PrepareLevel(level, stage);
        StartLevel(level, stage);
    }

    public void RestartLevel() {
        PrepareLevel(GSystem.userData.level, GSystem.userData.stage);
        StartLevel(GSystem.userData.level, GSystem.userData.stage);
    }

    public void NextLevel() {
        int stage = GSystem.userData.stage;
        int level = GSystem.userData.level;
        stage = (stage) % 4 + 1;
        if (stage == 1)
            level++;
        if (stage == 3)
            stage = 4;
        GSystem.userData.stage = stage;
        GSystem.userData.level = level;
        GSystem.SaveUserData();

        int mapIndex = ((level - 1) * 4 + (stage - 1)) % 5 + 1;
        GlobalManager.Instance.inLevel = true;
        SceneManager.LoadScene("Gameplay" + mapIndex);
        //PrepareLevel(level, stage);
        //StartLevel(level, stage);
    }

    [SerializeField]
    private ParticleSystem fireworksPS0, fireworksPS1, fighterHitPS;
    [SerializeField]
    GameObject cameraShadeObj;
    public void EndLevel(int result, float delay0 = 0, float delay1 = 0) {
        StartCoroutine(IEndLevel(result, delay0, delay1));
    }

    public IEnumerator IEndLevel(int result, float delay0, float delay1) {      // 0 loss, 1  victory
        yield return new WaitForSeconds(delay0);
        FightManager.Instance.Celebrate();
        yield return new WaitForSeconds(delay1);
        scoreboardPanel.SetActive(false);


        if (result == 1) {
            GSystem.userData.money += 300;
            GSystem.SaveUserData();
            ToggleVictoryUI(1);
        }
        else {
            lossPanel.SetActive(true);
        }
        phase = 6;
        Player.Instance.aimingPS.gameObject.SetActive(false);
    }

    public void ToggleVictoryUI(int toggle) {
        if (toggle == 0) {
            victoryPanel.SetActive(false);
            victoryBossPanel.SetActive(false);
        }
        else {
            ((GSystem.userData.stage == 4) ? victoryBossPanel : victoryPanel).SetActive(true);
        }
        cameraShadeObj.SetActive(toggle != 0);
        if (toggle == 1) {
            UIManager.Instance.PlayVictoryAnim(GSystem.userData.level, GSystem.userData.stage);
        }
    }

    #endregion

    


    #region Coloring

    [SerializeField]
    private float ballRange, bombRange;

    [SerializeField]
    private Transform colorTrans;

    public void CreateExplosion(int color, Vector3 p) {
        GameObject exObj = ObjectPool.Instance.GetObject("explosion_" + color, colorTrans);
        exObj.transform.position = p + Vector3.up * 2;
        Fighter f;
        int count = FightManager.Instance.fighters.Count;
        for (int i = 0; i < count; i++) {
            f = FightManager.Instance.fighters[i];
            if (f.color == 0) {
                if (!f.walkingOnBoxes) {
                    if (Vector2.Distance(new Vector2(p.x, p.z), new Vector2(f.transf.position.x, f.transf.position.z)) < bombRange)
                        f.GetColored(color);
                }
                else {
                    if (Vector2.Distance(new Vector2(p.x, p.z), new Vector2(f.transf.position.x, f.transf.position.z)) < bombRange)
                        f.GetColored(color);
                }
            }
        }
        ObjectPool.Instance.ReleaseObject(exObj, 5); 
        GameObject colorPoolObj = ObjectPool.Instance.GetObject("color_pool_" + color, colorTrans);
        colorPoolObj.transform.position = p;
        colorPoolObj.transform.localScale = new Vector3(bombRange / ballRange, 1, bombRange / ballRange);
        ObjectPool.Instance.ReleaseObject(colorPoolObj, 5);
    }

    public void CreateColorPool(int color, Vector3 p) {
        GameObject bloodPSObj = ObjectPool.Instance.GetObject("blood_" + color, colorTrans);
        bloodPSObj.transform.position = p + 2 * Vector3.up;
        ObjectPool.Instance.ReleaseObject(bloodPSObj, 5);

        GameObject colorPoolObj = ObjectPool.Instance.GetObject("color_pool_" + color, colorTrans);
        colorPoolObj.transform.position = p;
        colorPoolObj.transform.localScale = Vector3.one;
        ObjectPool.Instance.ReleaseObject(colorPoolObj, 5);

        Fighter f;
        int count = FightManager.Instance.fighters.Count;
        for (int i = 0; i < count; i++) {
            f = FightManager.Instance.fighters[i];
            if (f.color == 0 && !f.walkingOnBoxes) {
                if (Vector2.Distance(new Vector2(p.x, p.z), new Vector2(f.transf.position.x, f.transf.position.z)) < ballRange)
                    f.GetColored(color);
            }
        }

        foreach (Bomb b in bombs) {
            if (b == null)
                continue;
            if (Vector2.Distance(new Vector2(p.x, p.z), new Vector2(b.transform.position.x, b.transform.position.z)) < ballRange)
                b.GetColored(color);
        }
    }

    #endregion

    public void StartPhase3() {
        phase = 3;
        phase3StartTime = Time.time;
        Player.Instance.aimingPS.gameObject.SetActive(false);
        StartCoroutine(IHideBoxes());
    }

    [SerializeField]
    float boxSpeed;
    private IEnumerator IHideBoxes() {
        if (LevelBuilder.Instance.map.boxesTransf == null)
            yield break;
        float d = 0;
        Vector3 v = LevelBuilder.Instance.map.boxesTransf.position;
        while (d < LevelBuilder.Instance.boxHeight * 4) {
            d += boxSpeed * Time.deltaTime;
            LevelBuilder.Instance.map.boxesTransf.position = v + Vector3.down * d;
            yield return new WaitForEndOfFrame();
        }
        LevelBuilder.Instance.map.boxesTransf.gameObject.SetActive(false);
        yield break;
    }


    public void GaugePower() {
        StartCoroutine(IGaugePower());
    }


    private IEnumerator IGaugePower() {
        phase = 4;
        Player.Instance.aimingPS.gameObject.SetActive(false);
        powerGaugePanel.SetActive(true);
        float[] rots = new float[] { 105, 0, -105, 0 };
        float rot = 0;
        float t;
        float a = -720;
        float halfcycle = Mathf.Sqrt(210 / Mathf.Abs(a));
        float v0 = -a * halfcycle;
        float t0 = Time.time - halfcycle;
        while (!Input.GetMouseButton(0)) {
            t = Time.time - t0;
            if (t < halfcycle * 2) {
                rot = v0 * t + a * t * t / 2;
            }
            else {
                t0 = Time.time;
                v0 = -v0;
                a = -a;
                rot = 0;
            }
            powerPointer.eulerAngles = Vector3.forward * rot;
            yield return new WaitForEndOfFrame();
        }
        int power = (int)((rot + 105) / 30);
        if (power < 4)
            FightManager.Instance.SetFriendshipLevel(power);
        else
            FightManager.Instance.SetFriendshipLevel(6 - power);
        yield return new WaitForSeconds(.5f);
        powerGaugePanel.SetActive(false);
        FightManager.Instance.StartFight(1);
        yield break;
    }

    #region UI

    [SerializeField]
    private GameObject startupPanel, dragToAimPanel, victoryPanel, victoryBossPanel, lossPanel, powerGaugePanel, opponentIDPanel, scoreboardPanel;
    [SerializeField]
    private RectTransform powerPointer, opponentIDRect;
    [SerializeField]
    private RectTransform[] scoreBG;
    [SerializeField]
    private Transform[] scoreTransfs;
    public Transform plusSignsTransf;

    public void UpdateScoreBoard(int color, int score) {
        if (scoreTransfs == null || scoreTransfs.Length == 0)
            return;
        scoreBG[color - 1].sizeDelta = new Vector2(180 + 25 * score, 40);
        int maxScore = scoreTransfs[0].childCount;
        for (int i = 0; i < Mathf.Min(score, maxScore); i++) {
            scoreTransfs[color - 1].GetChild(i).gameObject.SetActive(true);
        }
        for (int i = score; i < maxScore; i++) {
            scoreTransfs[color - 1].GetChild(i).gameObject.SetActive(false);
        }
    }

    #endregion

    public void Test() {

    }

}
