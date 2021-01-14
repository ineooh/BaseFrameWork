using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Fighter {

    private void Start() {
        anim.Play("SwordAndShieldIdle");
    }

    protected override void Update() {
        
    }

    private string[] bossMoves = new string[] { "SwordAndShieldSlash", "SwordAndShieldSlash2", "Punch" };
    protected override void PlayRandomMove() {
        anim.Play(bossMoves[Random.Range(0, 3)]);
    }

}
