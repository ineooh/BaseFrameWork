using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMono<Player> {

    public float g;
    [SerializeField]
    private Vector3 v0;

    public ParticleSystem aimingPS;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;

    private bool mouseDown, recharging;

    [SerializeField]
    private float weightX, weightY, minChange;
    private float x, y, dx, dy;

    [SerializeField]
    private Ball ball;
    [SerializeField]
    private Transform redBallsTransf;
    [SerializeField]
    private float throwingCD;

    private void Start() {
        velocityModule = aimingPS.velocityOverLifetime;
        aimingPS.gameObject.SetActive(false);
    }

    private void Update() {
        if (GameManager.Instance.phase != 2)
            return;
        if (!mouseDown) {
            if (Input.GetMouseButton(0)) {
                mouseDown = true;
                GetBallToPosition();
                x = Input.mousePosition.x;
                y = Input.mousePosition.y;
            }
        }
        else if (Input.GetMouseButton(0)) {
            dx = Input.mousePosition.x - x;
            dy = Input.mousePosition.y - y;
            if (dx * dx + dy * dy < minChange)
                return;
            x = Input.mousePosition.x;
            y = Input.mousePosition.y;
            SetVelocity(v0 + Vector3.right * dx * weightX + Vector3.forward * dy * weightY);
        }
        else {
            mouseDown = false;
            if (!recharging)
                ThrowBall();
        }
    }

    public void StartLevel() {
        aimingPS.gameObject.SetActive(true);
        GetBall();
    }

    private void SetVelocity(Vector3 v) {
        v0 = v;
        velocityModule.xMultiplier = v.x;
        velocityModule.yMultiplier = v.y;
        velocityModule.zMultiplier = v.z;
        aimingPS.gameObject.SetActive(false);
        aimingPS.gameObject.SetActive(true);
    }
    private void ThrowBall() {
        StartCoroutine(IThrowBall());
    }

    private IEnumerator IThrowBall() {
        recharging = true;
        ball.Throw(v0);
        GetBall();

        yield break;
    }

    public void GetFirstBall() {
        ball = ObjectPool.Instance.GetObject("ball_red").GetComponent<Ball>();
        ball.transf.SetParent(redBallsTransf);
        recharging = false;
    }

    private void GetBall() {
        getBallCor = StartCoroutine(IGetBall());
    }

    [SerializeField]
    private Vector3 ballOffset;
    [SerializeField]
    private float ballSlideTime;
    private Coroutine getBallCor;
    private IEnumerator IGetBall() {
        ball = ObjectPool.Instance.GetObject("ball_red").GetComponent<Ball>();
        ball.transf.SetParent(redBallsTransf);
        recharging = false;
        ball.transf.localPosition = ballOffset;
        float elapsedTime = 0;
        while (elapsedTime < ballSlideTime) {
            ball.transf.localPosition = ballOffset * (1 - elapsedTime / ballSlideTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        ball.transf.localPosition = Vector3.zero;

        yield break;
    }

    private void GetBallToPosition() {
        if (getBallCor != null)
            StopCoroutine(getBallCor);
        ball.transf.localPosition = Vector3.zero;
    }

}
