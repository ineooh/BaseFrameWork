using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : SingletonMono<LevelBuilder> {
    
    public float x0, x1, z0, z1, y, boxHeight;
    public Map map;

    [SerializeField]
    private Map bossMapPrefab;
    [SerializeField]
    private Map[] firstLevelMapPrefabs, normalMapPrefabs;

    [SerializeField]
    private Transform worldTrans;

    public void BuildLevel(int level, int stage) {
        if (level == 1 && stage != 4) {
            BuildLevel(firstLevelMapPrefabs[stage - 1]);
        }
        else if (stage == 4) {
            BuildLevel(bossMapPrefab);
        }
        else {
            BuildLevel(normalMapPrefabs[(level * 3 + stage - 4) % normalMapPrefabs.Length]);
        }
    }

    public void BuildLevel(Map mapPrefab) {
        if (map != null) {
            map.gameObject.SetActive(false);
            Destroy(map.gameObject);
        }
        map = Instantiate(mapPrefab, worldTrans);
    }

}
