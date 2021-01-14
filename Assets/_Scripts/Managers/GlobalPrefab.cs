using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPrefab : SingletonMonoPersistent<GlobalPrefab> {

    public bool inLevel;
    public Sprite[] opponentFlagSprites;

    public GameObject[] weaponPrefabs, shieldPrefabs, hatPrefabs, skinPrefabs;

    public Mesh[] skinMeshes;

}
