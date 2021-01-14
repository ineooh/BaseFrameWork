using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : SingletonMono<FightManager> {

    [SerializeField]
    private float[] friendshipLevels;
    public float friendshipPowerup;
    public bool fightersFalling;

    public List<Fighter> fighters;
    [SerializeField]
    private GameObject fighterPrefab, fighterCupPrefab;
    private int tauntingCount;
    public int totalCount;

    private int redCount;
    public int RedCount {
        get { return redCount; }
        set { 
            redCount = value;
            if (GameManager.Instance.phase < 4)
                GameManager.Instance.UpdateScoreBoard(1, value);
        }
    }

    private int greenCount;
    public int GreenCount {
        get { return greenCount; }
        set {
            greenCount = value;
            if (GameManager.Instance.phase < 4)
                GameManager.Instance.UpdateScoreBoard(2, value);
        }
    }

    public Material grey, red, green;
    public ParticleSystem bloodPSRed, bloodPSGreen, cupHitPS0, cupHitPS1, fighterHitPS;
    [SerializeField]
    private Vector3 center;
    [SerializeField]
    private float lengthX, lengthZ, lineInterval;
    private float x0, x1, z0, z1, y;

    public void Start() {
        
    }

    public void Startup() {
        x0 = center.x - lengthX / 2;
        x1 = x0 + lengthX;
        z0 = center.z - lengthZ / 2;
        z1 = z0 + lengthZ;
        y = center.y;
    }

    private void Update() {
        if (GameManager.Instance.phase > 4 || GameManager.Instance.phase < 2)
            return;
        HandleMovement();
    }

    private Vector3 velocity;
    private void HandleMovement() {
        foreach (Fighter unit in fighters) {
            if (unit.dead)
                continue;
            if (unit.State == 1) {              // wandering
                unit.Move(GetMoveWander(unit));
            }
            else if (unit.State == 2) {         // lining up
                unit.Move(GetMoveLine(unit));
                if (Vector3.Distance(unit.transform.position, unit.Des) < .5f) {
                    unit.transform.position = unit.Des;
                    unit.transform.rotation = Quaternion.Euler(0, 180 * (unit.color - 1), 0);
                    unit.State = 3;
                    tauntingCount++;
                    if (tauntingCount == totalCount) {
                        if (GreenCount == 0)
                            GameManager.Instance.EndLevel(1);
                        else if (RedCount == 0)
                            GameManager.Instance.EndLevel(0);
                        else
                            GameManager.Instance.GaugePower();
                    }
                }
            }
            else if (unit.State == 3) {         // taunting
                continue;
            }
            else if (unit.State == 4) {         // chasing
                if (unit.target == null) {
                    return;
                }
                if (unit.color == unit.target.color) {
                    unit.EngageNearest();
                    continue;
                }
                unit.Move(GetMoveEngage(unit));
                Vector3 d = unit.target.transf.position - unit.transf.position;
                if (d.magnitude < ((!unit.boss) ? attackRange : (attackRange * 2))) {
                    unit.transf.rotation = Quaternion.Euler(0, 90 - Mathf.Atan2(d.z, d.x) * Mathf.Rad2Deg, 0);
                    unit.State = 5;
                }
            }
            else if (unit.State == 5) {         // fighting
                if (Vector3.Distance(unit.transform.position, unit.target.transform.position) > attackRange) {
                    unit.State = 4;
                    continue;
                }
            }
        }
    }

    [SerializeField]
    private float fDir, fSep, fBox, dSep, dBox, aMax, fSteer;     // direction, separation, box avoidance
    public float speed, speedRunning, attackRange;
    Vector3 vDir, vSep, vBox;
    float d; 

    private Vector3 GetMove(Fighter f, Vector3 des, float speed) {
        vDir = (des - f.transform.position).normalized;

        vSep = Vector3.zero;
        foreach (Fighter f1 in fighters) {
            if (f1 == f || f1.dead)
                continue;
            d = Vector3.Distance(f.transform.position, f1.transform.position);
            if (d < .1f)
                d = .1f;
            if (d < dSep) {
                vSep += (f.transform.position - f1.transform.position) / Mathf.Pow(d, 3);
            }
        }

        vBox = Vector3.zero;
        foreach (Collider c in GameManager.Instance.boxColliders) {
            d = Vector3.Distance(f.transform.position, c.transform.position);
            if (d < 1f)
                d = 1f;
            if (d < dBox) {
                vBox += (f.transform.position - c.transform.position) / Mathf.Pow(d, 3);
            }
        }

        velocity = vDir * fDir + vSep * fSep + vBox * fBox;
        velocity = new Vector3(velocity.x, 0, velocity.z);
        velocity = velocity.normalized * speed;

        Vector3 vA = velocity - f.velocity;
        if (vA.magnitude > aMax) {
            velocity = f.velocity + vA.normalized * aMax;
            if (velocity.magnitude > speed) {
                velocity = velocity.normalized * speed;
            }
        }

        if (f.velocity != Vector3.zero) {
            float rot0, rot1;
            rot0 = Mathf.Atan2(f.velocity.z, f.velocity.x) * Mathf.Rad2Deg;
            rot1 = Mathf.Atan2(velocity.z, velocity.x) * Mathf.Rad2Deg;
            while (rot1 > rot0 + 180)
                rot0 += 360;
            while (rot0 > rot1 + 180)
                rot1 += 360;
            if (rot1 < rot0 - fSteer * Time.deltaTime) {
                rot1 = rot0 - fSteer * Time.deltaTime;
            }
            else if (rot1 > rot0 + fSteer * Time.deltaTime) {
                rot1 = rot0 + fSteer * Time.deltaTime;
            }
            velocity = new Vector3(Mathf.Cos(rot1 * Mathf.Deg2Rad), 0, Mathf.Sin(rot1 * Mathf.Deg2Rad)).normalized * velocity.magnitude;
            float rot2 = Mathf.Atan2(velocity.z, velocity.x);
            //Debug.Log(rot0 + ", " + rot1 + ", " + rot2);
        }

        f.velocity = velocity;
        velocity *= Time.deltaTime;

        return velocity;
    }

    private Vector3 GetMoveWander(Fighter f) {
        if (f.walkingOnBoxes) {
            if (f.Des == Vector3.zero || Vector3.Distance(f.Des, f.transform.position) < .5f) {
                if (f.boxIndex == -1) {
                    f.Des = f.boxes[Random.Range(0, f.boxes.Count)].position;
                }
                else {
                    f.boxIndex = (f.boxIndex + 1) % f.boxes.Count;
                    f.Des = f.boxes[f.boxIndex].position;
                }
            }
            return (f.Des - f.transform.position).normalized * speed * Time.deltaTime;
        }
        else {
            if (f.Des == Vector3.zero || Vector3.Distance(f.Des, f.transform.position) < 3f) {
                f.Des = GetRandomDes();
            }
            if (GSystem.userData.stage != 4)
                return GetMove(f, f.Des, speed);
            else
                return GetMove(f, f.Des, speed * (1 + .15f * (currentWave)));
        }
    }

    
    private Vector3 GetMoveLine(Fighter f) {
        return GetMove(f, f.Des, speedRunning);
    }

    private Vector3 GetMoveEngage(Fighter f) {
        return GetMove(f, f.target.transform.position, speedRunning);
    }


    int[] nextSlot;
    public Vector3 GetNextSlotPosition(int color) {
        int x = (nextSlot[color - 1] + 1) / 2 * ((nextSlot[color - 1] % 2) * 2 - 1);
        nextSlot[color - 1]++;
        return new Vector3(center.x + x * lineInterval, y, (color == 1) ? z0 : z1);
    }

    public List<Fighter>[] indexedFighters;
    public void AssignPosition(Fighter f) {
        if (f.color == 1)
            indexedFighters[0].Add(f);
        else
            indexedFighters[1].Add(f);
        int index = nextSlot[f.color - 1];
        int z = index / 9 * (3 - f.color * 2);
        index %= 9;
        int x = (index + 1) / 2 * ((index % 2) * 2 - 1);
        f.Des = new Vector3(center.x + x * lineInterval, y, ((f.color == 1) ? z0 : z1) + z * lineInterval);
        f.linePosition = nextSlot[f.color - 1];
        nextSlot[f.color - 1]++;
    }

    public void SetFriendshipLevel(int level) {
        friendshipPowerup = friendshipLevels[level];
    }

    public void LetFightersFall() {
        if (GSystem.userData.stage != 4)
            StartCoroutine(ILetFightersFall());
        else
            StartCoroutine(ILetFightersFallStage4());
    }

    private int fallenRed, fallenGreen;
    private IEnumerator ILetFightersFall() {
        fightersFalling = true;
        float t = 0;
        while (RedCount != 0 && GreenCount != 0) {
            if (fallenRed == 0) {
                if (GreenCount > 0) {
                    LetNextFighterFall(1);
                }
                else if (friendshipPowerup < 1) {
                    LetNextFighterFall(1);
                }
                else {
                    LetNextFighterFall(2);
                }
            }
            else if ((float)fallenGreen / fallenRed > friendshipPowerup) {
                LetNextFighterFall(1);
            }
            else {
                LetNextFighterFall(2);
            }
            t = Random.Range(0f, 1f);
            yield return new WaitForSeconds(t);
        }
    }

    private IEnumerator ILetFightersFallStage4() {
        fightersFalling = true;
        float t = 0;
        while (RedCount != 0 && GreenCount != 0) {
            if (fallenRed < 2) {
                LetNextFighterFall(1);
            }
            else if ((float)fallenGreen / fallenRed > friendshipPowerup) {
                LetNextFighterFall(1);
            }
            else
                LetNextFighterFall(2);
            t = Random.Range(0f, 1f);
            yield return new WaitForSeconds(t);
        }
    }

    private void LetNextFighterFall(int color) {
        int maxHit = 0;
        Fighter nextFighter = null;
        List<Fighter> oppositeTeam = indexedFighters[2 - color];

        foreach (Fighter f in oppositeTeam) {
            if (f != null && !f.dead && f.target != null && !f.target.dead && f.State == 5) {
                if (f.target.hit > maxHit) {
                    maxHit = f.target.hit;
                    nextFighter = f.target;
                }
            }
        }
        if (nextFighter != null) {
            nextFighter.Die();
            if (color == 1) {
                fallenRed++;
            }
            else {
                if (nextFighter.GetComponent<Boss>() == null)
                    fallenGreen++;
                else
                    fallenGreen += 3;
            }
        }
    }

    public void StartLevel() {
        nextSlot = new int[] { 0, 0 };
        indexedFighters = new List<Fighter>[] { new List<Fighter>(), new List<Fighter>() };
        fighters = new List<Fighter>();
        for (int i = 0; i < LevelBuilder.Instance.map.fightersTransf.childCount; i++) {
            fighters.Add(LevelBuilder.Instance.map.fightersTransf.GetChild(i).GetComponent<Fighter>());
        }
        foreach (Fighter f in fighters) {
            if (f == null) {
                Debug.Log(fighters.Count);
            }
            if (!f.boss) {
                f.color = 0;
                f.State = 1;
            }
            else {
                f.color = 2;
                f.State = 3;
            }
        }
        totalCount = fighters.Count;
        foreach (Fighter f in fighters) {
            if (f.carryingCup) {
                totalCount += 2;
            }
        }
        tauntingCount = 0;
        RedCount = 0;
        GreenCount = 0;
        fightersFalling = false;
        fallenRed = 0;
        fallenGreen = 0;
    }

    private int waves;
    private int currentWave;
    public int waveNeutralCount;
    public void StartLevelStage4(int level) {
        waves = Mathf.Min(level + 3, 7);
        currentWave = 0;
        nextSlot = new int[] { 0, 1 };
        fighters = new List<Fighter>() { LevelBuilder.Instance.map.fightersTransf.GetChild(0).GetComponent<Fighter>() };
        indexedFighters = new List<Fighter>[] { new List<Fighter>(), new List<Fighter>() { fighters[0] } };
        fighters[0].State = 3;
        fighters[0].color = 2;
        totalCount = waves * 4 + 1;
        tauntingCount = 1;
        RedCount = 0;
        GreenCount = 1;
        fightersFalling = false;
        fallenRed = 0;
        fallenGreen = 0;
        NextWave();
    }

    public void NextWave() {
        if (currentWave == waves) {
            return;
        }
        int cupSide = Random.Range(0, 2);
        AddFighter(0, GetRandomPosition(cupSide), true);
        AddFighter(0, GetRandomPosition(1 - cupSide));
        waveNeutralCount = 4;
        currentWave++;
    }

    public void StartFight(float delay) {
        StartCoroutine(IStartFight(delay));
    }

    private IEnumerator IStartFight(float delay) {
        yield return new WaitForSeconds(delay);
        if (GreenCount == 0) {
            GameManager.Instance.EndLevel(1);
            yield break;
        }
        else if (RedCount == 0) {
            GameManager.Instance.EndLevel(0);
            yield break;
        }
        GameManager.Instance.phase = 4;
        foreach (Fighter f in fighters) {
            f.velocity = Vector3.zero;
            f.State = 4;
            f.EngageNearest();
        }
    }

    public void Celebrate() {
        foreach (Fighter f in fighters) {
            f.State = 6;
        }
    }

    public void AddFighter(int color, Vector3 pos, bool hasCup = false) {
        Fighter f = Instantiate((!hasCup) ? fighterPrefab : fighterCupPrefab, LevelBuilder.Instance.map.fightersTransf).GetComponent<Fighter>();
        f.gameObject.transform.position = pos;
        f.color = color;
        if (color == 0) {
            f.meshRenderer.material = FightManager.Instance.grey;
            f.State = 1;
        }
        else {
            f.State = 2;
            if (color == 1)
                f.meshRenderer.material = FightManager.Instance.red;
            else
                f.meshRenderer.material = FightManager.Instance.green;
        }
        

        //f.Des = GetNextSlotPosition(color);
        fighters.Add(f);
        if (color != 0)
            FightManager.Instance.AssignPosition(f);
    }

    public Vector3 GetRandomPosition(int side) {
        if (side == 0) {
            Vector3 v = new Vector3(x0, y, Random.Range(z0, z1));
            return v;
        }
        return new Vector3(x1, y, Random.Range(z0, z1));
    }

    public Vector3 GetRandomDes() {
        int i = Random.Range(0, 4);
        if (i == 0)
            return new Vector3(Random.Range(x0, x1), y, z1);
        else if (i == 1)
            return new Vector3(x0, y, Random.Range(z0, z1));
        else if (i == 2)
            return new Vector3(Random.Range(x0, x1), y, z0);
        else
            return new Vector3(x1, y, Random.Range(z0, z1));
    }

}