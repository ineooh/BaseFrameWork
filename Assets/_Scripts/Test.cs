using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    [SerializeField]
    private Transform teamTransfRed, teamTransfGreen;
    [SerializeField]
    int hpLower, hpUpper;
    [SerializeField]
    float friendshipPowerup;

    // Start is called before the first frame update
    private void Start() {
        List<Fighter> fighters = new List<Fighter>();
        FightManager.Instance.indexedFighters = new List<Fighter>[] { new List<Fighter>(), new List<Fighter>() };
        Fighter f;
        int n = teamTransfRed.childCount;
        FightManager.Instance.RedCount = n;
        for (int i = 0; i < n; i++) {
            f = teamTransfRed.GetChild(i).GetComponent<Fighter>();
            fighters.Add(f);
            f.color = 1;
            f.meshRenderer.material = FightManager.Instance.red;
            FightManager.Instance.indexedFighters[0].Add(f);
            f.linePosition = i;
        }
        n = teamTransfGreen.childCount;
        FightManager.Instance.GreenCount = n;
        for (int i = 0; i < n; i++) {
            f = teamTransfGreen.GetChild(i).GetComponent<Fighter>();
            fighters.Add(f);
            f.color = 2;
            f.meshRenderer.material = FightManager.Instance.green;
            FightManager.Instance.indexedFighters[1].Add(f);
            f.linePosition = i;
        }
        FightManager.Instance.fighters = fighters;
        foreach (Fighter f1 in FightManager.Instance.fighters) {
            f1.State = 4;
            f1.EngageOpposite();
        }
        foreach (Fighter f1 in FightManager.Instance.fighters) {
            if (f1.color == 1) {
                f1.hit = 0;
            }
        }
        GameManager.Instance.phase = 4;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
            Reset();
    }

    private void Reset() {
        List<Fighter> fighters = new List<Fighter>();
        FightManager.Instance.indexedFighters = new List<Fighter>[] { new List<Fighter>(), new List<Fighter>() };
        FightManager.Instance.fightersFalling = false;
        Fighter f;
        int n = teamTransfRed.childCount;
        FightManager.Instance.RedCount = n;
        for (int i = 0; i < n; i++) {
            f = teamTransfRed.GetChild(i).GetComponent<Fighter>();
            fighters.Add(f);
            f.color = 1;
            f.meshRenderer.material = FightManager.Instance.red;
            f.transf.localPosition = Vector3.zero + Vector3.right * (i - 2);
            f.dead = false;
            FightManager.Instance.indexedFighters[0].Add(f);
            f.linePosition = i;
        }
        n = teamTransfGreen.childCount;
        FightManager.Instance.GreenCount = n;
        for (int i = 0; i < n; i++) {
            f = teamTransfGreen.GetChild(i).GetComponent<Fighter>();
            fighters.Add(f);
            f.color = 2;
            f.meshRenderer.material = FightManager.Instance.green;
            f.transf.localPosition = Vector3.zero + Vector3.right * (i - 2);
            f.transf.eulerAngles = Vector3.up * (-180);
            f.dead = false;
            FightManager.Instance.indexedFighters[1].Add(f);
            f.linePosition = i;
        }
        FightManager.Instance.fighters = fighters;
        foreach (Fighter f1 in FightManager.Instance.fighters) {
            f1.State = 4;
            f1.EngageOpposite();
        }
        foreach (Fighter f1 in FightManager.Instance.fighters) {
            if (f1.color == 1) {
                f1.hit = 0;
            }
        }
        GameManager.Instance.phase = 4;
    }

}
