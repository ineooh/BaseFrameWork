using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : SingletonMono<Opponent> {

    [SerializeField]
    private Ball ball;
    [SerializeField]
    private Transform handTrans, greenBallsTransf;
    [SerializeField]
    private float ballCD;
    public float throwingCDLower, throwingCDUpper;


    [SerializeField]
    private float vy, throwingAnimTime;
    [SerializeField]
    private Vector3 ballPosition;
    private float ballFlyingTime;
    
    private float error;

    private Animator anim;
    void Start() {
        float t1 = vy / Player.Instance.g;
        float h = t1 * vy / 2 + ballPosition.y - LevelBuilder.Instance.y;
        float t2 = Mathf.Sqrt(2 * h / Player.Instance.g);
        ballFlyingTime = (t1 + t2);

        anim = GetComponent<Animator>();
    }

    public void StartLevel(Map map) {
        error = map.opponentError;
        throwingCDLower = map.opponentCDLowerBound;
        throwingCDUpper = map.opponentCDUpperBound;

        ball = ObjectPool.Instance.GetObject("ball_green").GetComponent<Ball>();
        ball.transf.SetParent(handTrans);
        ball.transf.localPosition = Vector3.zero;

        StopAllCoroutines();
    }

    public void AutoThrow() {
        StartCoroutine(IAutoThrow());
    }

    private Vector3 target;
    private IEnumerator IAutoThrow() {
        yield return new WaitForSeconds(Random.Range(throwingCDLower, throwingCDUpper));
        while (GameManager.Instance.phase < 3) {

            Fighter targetF = null;
            foreach (Fighter f in FightManager.Instance.fighters) {
                if (f.color == 0) {
                    targetF = f;
                    if (f.carryingCup)
                        break;                 
                }
            }
            if (ball != null && targetF != null) {
                anim.Play("Throw");
                yield return new WaitForSeconds(throwingAnimTime);
                target = targetF.transform.position + (targetF.Des - targetF.transform.position).normalized
                    * FightManager.Instance.speed * ballFlyingTime / ball.tMultiplier;
                if (FightManager.Instance.GreenCount > 0)
                    target += Vector3.left * Random.Range(-error, error) + Vector3.forward * Random.Range(-error, error);
                Vector3 v = new Vector3((target.x - ball.transf.position.x) / ballFlyingTime, vy, (target.z - ball.transf.position.z) / ballFlyingTime);
                ball.transf.SetParent(greenBallsTransf);
                ball.Throw(v);

                ball = ObjectPool.Instance.GetObject("ball_green").GetComponent<Ball>();
                ball.transf.SetParent(handTrans);
                ball.transf.localPosition = Vector3.zero;
                yield return new WaitForSeconds(Random.Range(throwingCDLower, throwingCDUpper));
            }
            else
                yield return new WaitForEndOfFrame();
        }
        yield break;
    }



}
