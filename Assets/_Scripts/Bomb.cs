using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour{

    private int color;

    [SerializeField]
    private MeshRenderer ms;
    [SerializeField]
    private ParticleSystem idlePS0, idlePS1;

    public void GetColored(int color) {
        if (this.color != color) {
            this.color = color;
            if (color == 1) {
                ms.material = FightManager.Instance.red;
                var m = idlePS0.main;
                m.startColor = new Color(255, 66, 66, 255);
                m = idlePS1.main;
                m.startColor = new Color(255, 66, 66, 255);
            }
            else if (color == 2) {
                ms.material = FightManager.Instance.green;
                var m = idlePS0.main;
                m.startColor = new Color(66, 255, 66, 255);
                m = idlePS1.main;
                m.startColor = new Color(66, 255, 66, 255);
            }
        }
        else {
            GameManager.Instance.CreateExplosion(color, transform.position);
            Destroy(gameObject);
        }
    }

}
