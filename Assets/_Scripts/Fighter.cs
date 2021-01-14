using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour {

    public int color;
    public bool boss;

    public Transform transf;
    public SkinnedMeshRenderer meshRenderer;

    [SerializeField]
    protected Animator anim;

    public bool carryingCup;
    public GameObject cupObj;
    [SerializeField]
    private Transform feetTrans;
    private bool onGround {
        get {
            return GameManager.Instance.groundCollider.bounds.Contains(feetTrans.position); 
        }
    }
    private bool onBox {
        get {
            foreach (Collider c in GameManager.Instance.boxColliders) {
                if (c.bounds.Contains(feetTrans.position)) {
                    boxY = c.transform.position.y + LevelBuilder.Instance.boxHeight;
                    return true;
                }
            }
            return false;
        }
    }
    private float boxY;

    private int state;

    /// <summary>
    /// 0 chilling, 1 wandering, 2 lining up, 3 taunting, 4 chasing, 5 fighting, 6 celebrating
    /// </summary>
    public int State {
        get { return state; }
        set {
            if (dead)
                return;
            state = value;
            switch (value) {
                case 0: anim.Play("Walk"); break;
                case 1: anim.Play("Walk"); break;
                case 2: anim.Play("Run"); break;
                case 3: transf.rotation = Quaternion.Euler(0, (color - 1) * 180, 0); anim.Play("Taunt" + Random.Range(1, 4).ToString()); break;
                case 4: if (fightingCor != null) StopCoroutine(fightingCor); anim.Play("Run"); break;
                case 5: fightingCor = StartCoroutine(IFight()); break;
                case 6: if (fightingCor != null) StopCoroutine(fightingCor); anim.Play("Celebrate" + Random.Range(1, 4).ToString()); break;
            }
        }
    }

    [SerializeField]
    private Vector3 des;
    public Vector3 Des { 
        get { return des; }
        set {
            des = new Vector3(value.x, transf.position.y, value.z);
        }
    }

    public Vector3 velocity;

    public bool walkingOnBoxes;
    public int boxIndex;
    public List<Transform> boxes;

    float vy;
    float g = 1;
    protected virtual void Update() {
        if (GameManager.Instance.phase > 4 || GameManager.Instance.phase < 2)
            return;
        if (onGround) {
            vy = 0;
            transf.position = new Vector3(transf.position.x, LevelBuilder.Instance.y, transf.position.z);
        }
        else if (onBox) {
            vy = 0;
            transf.position = new Vector3(transf.position.x, boxY, transf.position.z);
        }
        else {
            vy -= g * Time.deltaTime;
            transf.position += Vector3.up * vy;
            if (Des != Vector3.zero)
                Des = new Vector3(des.x, transf.position.y, des.z);
        }
    }

    public void Move(Vector3 move) {
        transf.position += move;
        transf.rotation = Quaternion.Euler(0, 90 - Mathf.Atan2(move.z, move.x) * Mathf.Rad2Deg, 0);
    }

    #region Fighting

    public Fighter target;
    public int hit;

    public float attackSpeed;

    public bool dead;

    public int linePosition;

    protected string[] moves = new string[] { "MmaKick", "MmaKick2", "Punch" };
    private Coroutine fightingCor;

    public void EngageOpposite() {
        int n = (color == 1) ? FightManager.Instance.GreenCount : FightManager.Instance.RedCount;
        if (linePosition + 2 < n) {
            n = Random.Range(0, 3);
            if (n == 0) {
                if (linePosition < 2)
                    target = FightManager.Instance.indexedFighters[2 - color][0];
                else
                    target = FightManager.Instance.indexedFighters[2 - color][linePosition - 2];
            }
            else if (n == 1)
                target = FightManager.Instance.indexedFighters[2 - color][linePosition];
            else
                target = FightManager.Instance.indexedFighters[2 - color][linePosition + 2];
        }
        else {
            if (n == 0) {
                if (linePosition < 2)
                    target = FightManager.Instance.indexedFighters[2 - color][0];
                else
                    target = FightManager.Instance.indexedFighters[2 - color][linePosition - 2];
            }
            else
                target = FightManager.Instance.indexedFighters[2 - color][linePosition];
        }
    }

    public void EngageNearest() {
        State = 4;
        float dMin = float.MaxValue;
        target = null;
        foreach (Fighter f in FightManager.Instance.fighters) {
            if (color == f.color || f.dead)
                continue;
            float d = Vector3.Distance(transf.position, f.transf.position);
            if (d < dMin) {
                dMin = d;
                target = f;
            }
        }
    }

    private IEnumerator IFight() {
        yield return new WaitForSeconds(Random.Range(0, .7f) / attackSpeed);

        PlayRandomMove();
        while (!target.dead) {
            if (Vector3.Distance(target.transf.position, transf.position) > ((!boss) ? FightManager.Instance.attackRange : (FightManager.Instance.attackRange * 2))
                || target.color == color) {
                EngageNearest();
                yield break;
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .95) {
                target.hit++;
                if (target.hit > ((target.boss) ? 19 : 9) && !FightManager.Instance.fightersFalling) {
                    FightManager.Instance.LetFightersFall();
                }
                Instantiate(FightManager.Instance.fighterHitPS, target.transf).transform.localPosition = new Vector3(0, 1.24f, .3f);
                PlayRandomMove();
            }
            yield return new WaitForEndOfFrame();
        }
        //target.Die();
        if (FightManager.Instance.RedCount == 0 || FightManager.Instance.GreenCount == 0)
            yield break;
        if (GameManager.Instance.phase > 4|| GameManager.Instance.phase < 2)
            yield break;
        EngageNearest();
        yield break;
    }

    protected virtual void PlayRandomMove() {
        anim.Play(moves[Random.Range(0, 3)]);
    }

    public void Die() {
        if (dead)
            return;
        dead = true;
        StopAllCoroutines();
        if (color == 1)
            FightManager.Instance.RedCount--;
        else if (color == 2)
            FightManager.Instance.GreenCount--;
        if (FightManager.Instance.RedCount == 0) {
            GameManager.Instance.EndLevel(0, .5f, 1.3f);
        }
        else if (FightManager.Instance.GreenCount == 0) {
            GameManager.Instance.EndLevel(1, .5f, 1.3f);
        }
        meshRenderer.material = FightManager.Instance.grey;
        anim.Play("SweepFall");
    }

    #endregion

    public void GetColored(int color) {
        this.color = color;
        State = 2;
        //Des = FightManager.Instance.GetNextSlotPosition(color);
        FightManager.Instance.AssignPosition(this);
        meshRenderer.material = (color == 1) ? FightManager.Instance.red : FightManager.Instance.green;
        int count;
        if (carryingCup) {
            cupObj.SetActive(false);
            for (int i = 0; i < 2; i++)
                FightManager.Instance.AddFighter(color, transf.position);
            count = 3;
            Instantiate(FightManager.Instance.cupHitPS0).transform.position = transf.position + Vector3.up * 2;
            Instantiate(FightManager.Instance.cupHitPS1).transform.position = transf.position + Vector3.up * 2;
        }
        else
            count = 1;
        if (color == 1) {
            GameObject plusSign = ObjectPool.Instance.GetObject("plus" + count.ToString(), GameManager.Instance.plusSignsTransf);
            plusSign.GetComponent<Plus1>().MoveUp(Camera.main.WorldToScreenPoint(transf.position));
            FightManager.Instance.RedCount += count;
        }
        else
            FightManager.Instance.GreenCount += count;

        if (FightManager.Instance.RedCount + FightManager.Instance.GreenCount == FightManager.Instance.totalCount) {
            GameManager.Instance.StartPhase3();
        }

        if (GSystem.userData.stage == 4) {
            FightManager.Instance.waveNeutralCount -= count;
            if (FightManager.Instance.waveNeutralCount == 0)
                FightManager.Instance.NextWave();
        }
    }


}
