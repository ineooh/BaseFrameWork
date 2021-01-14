using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMono<UIManager> {


    private void Start() {
        keyP0 = newKeyRect.transform.position;
        keyScale0 = 3;
        //PlayVictoryAnim(1, 3);
    }

    [SerializeField]
    private Image opponentFlagImg;
    [SerializeField]
    private Text opponentNameText;
    public void ChangeOpponentID(int level, int stage) {
        string nameStr = "Player " + stage.ToString();
        if (stage == 4)
            nameStr = "Player 3";
        opponentNameText.text = nameStr;
        opponentFlagImg.sprite = GlobalPrefab.Instance.opponentFlagSprites[Random.Range(0, GlobalPrefab.Instance.opponentFlagSprites.Length)];

    }

    #region Victory
    [SerializeField]
    private GameObject[] stageTxtObjs, stageWonUIObjs;
    [SerializeField]
    private Image[] progressKeyImgs;
    [SerializeField]
    private Sprite keySprite;
    [SerializeField]
    private RectTransform newKeyRect, checkmarkBossRect;

    [SerializeField]
    private Text moneyAcquiredText, totalMoneyText, moneyAcquiredTextBoss, totalMoneyTextBoss;
    [SerializeField]
    private Transform flyingMoneyTransf0, flyingMoneyTransf1, flyingMoneyTransfBoss0, flyingMoneyTransfBoss1;

    private Vector2 keyP0;
    private int keyScale0;

    private int level, stage;


    

    public void PlayVictoryAnim(int level, int stage) {
        this.level = level;
        this.stage = stage;
        StartCoroutine(IPlayVictoryAnim());
        StartCoroutine(IPlayMoneyAnim());
    }

    private IEnumerator IPlayVictoryAnim() {
        if (stage == 4) {
            StartCoroutine(IPlayVictoryAnimBoss());
            yield break;
        }
        for (int i = 0; i < stage; i++) {
            stageTxtObjs[i].SetActive(false);
            stageWonUIObjs[i].SetActive(true);
        }
        for (int i = stage; i < 3; i++) {
            stageTxtObjs[i].SetActive(true);
            stageWonUIObjs[i].SetActive(false);
        }
        for (int i = 0; i < stage - 1; i++) {
            progressKeyImgs[i].sprite = keySprite;
        }
        newKeyRect.position = keyP0;
        newKeyRect.SetParent(progressKeyImgs[stage - 1].transform);
        Transform glowTransf = newKeyRect.transform.GetChild(0);
        glowTransf.gameObject.SetActive(true);
        Transform checkmarkTransf = stageWonUIObjs[stage - 1].transform.GetChild(0);
        Vector2 p = newKeyRect.localPosition;
        float scale = keyScale0;
        float elapsedTime = 0;
        float animTime0 = 1, animTime1 = .5f, animTime2 = .5f;
        float ratio;
        while (elapsedTime < animTime0) {
            ratio = elapsedTime / animTime0;
            checkmarkTransf.localScale = Vector2.one * ratio;
            newKeyRect.localScale = new Vector2(scale, scale) * ratio;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        checkmarkTransf.localScale = Vector2.one;
        elapsedTime = 0;
        yield return new WaitForSeconds(animTime1);
        while (elapsedTime < animTime2) {
            ratio = (1 - elapsedTime / animTime2);
            newKeyRect.localPosition = p * ratio;
            newKeyRect.localScale = Vector2.one * (1 + (scale - 1) * ratio);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        newKeyRect.localPosition = Vector2.zero;
        newKeyRect.localScale = Vector2.one;
        glowTransf.gameObject.SetActive(false);
    }
    
    private IEnumerator IPlayVictoryAnimBoss() {
        float elapsedTime = 0;
        float animTime0 = 1;
        float ratio;
        while (elapsedTime < animTime0) {
            ratio = elapsedTime / animTime0;
            checkmarkBossRect.localScale = Vector2.one * ratio;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        checkmarkBossRect.localScale = Vector2.one;
    }

    private IEnumerator IPlayMoneyAnim() {
        Text txt = (stage == 4) ? moneyAcquiredTextBoss : moneyAcquiredText;
        int moneyAcquired = 300;
        float animTime0 = 2;
        float elapsedTime = 0;
        int moneyCount = 0;
        while (elapsedTime < animTime0) {
            txt.text = ((int)(moneyAcquired * elapsedTime / animTime0)).ToString();
            if (elapsedTime > moneyCount * animTime0 / 9) {
                StartCoroutine(ISpawnMoney(moneyCount++));
            }
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        txt.text = moneyAcquired.ToString();
    }

    private IEnumerator ISpawnMoney(int index) {
        Transform transf0 = (stage == 4) ? flyingMoneyTransfBoss0 : flyingMoneyTransf0;
        Transform transf1 = (stage == 4) ? flyingMoneyTransfBoss1 : flyingMoneyTransf1;

        Transform moneyTransf = transf0.GetChild(index);
        moneyTransf.gameObject.SetActive(true);
        float animTime = 1.5f;
        float elapsedTime = 0;
        Vector2 p0 = moneyTransf.position;
        Vector2 p1 = transf1.position;
        float rotZ = moneyTransf.rotation.eulerAngles.z;
        float rotSpd = 180;
        float ratio;
        while (elapsedTime < animTime) {
            ratio = elapsedTime / animTime;
            moneyTransf.position = Vector2.Lerp(p0, p1, ratio);
            moneyTransf.rotation = Quaternion.Euler(Vector3.forward * (rotZ + rotSpd * elapsedTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (index == 0) {
            StartCoroutine(IIncreaseTotalMoney());
        }
        moneyTransf.gameObject.SetActive(false);
        moneyTransf.position = p0;
    }

    private IEnumerator IIncreaseTotalMoney() {
        Text txt = (stage == 4) ? totalMoneyTextBoss : totalMoneyText;
        int moneyAcquired = 300;
        float animTime = 2;
        float elapsedTime = 0;
        while (elapsedTime < animTime) {
            txt.text = ((int)(GSystem.userData.money + moneyAcquired * (elapsedTime / animTime - 1))).ToString();
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        txt.text = GSystem.userData.money.ToString();
    }

    #endregion


}