using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopModel : SingletonMono<ShopModel>  {

    private int[] equippedItems;
    private int equippedSkin;

    [SerializeField]
    GameObject meshObj;
    private Animator anim;

    [SerializeField]
    private DefaultSkin defaultSkin;

    private int prefabIndex;

    private void Start() {
        equippedItems = new int[3] { GSystem.userData.itemIndices[0], GSystem.userData.itemIndices[1], GSystem.userData.itemIndices[2] };
        equippedSkin = GSystem.userData.itemIndices[3];
        ChangeSkin(equippedSkin);
        for (int i = 0; i < 3; i++)
            EquipItem(i, equippedItems[i]);
    }

    public void EquipItem(int itemSlot, int itemIndex) {
        if (itemSlot > 2 || itemSlot < 0)
            return;
        int n = defaultSkin.itemTransfs[itemSlot].childCount;
        for (int i = n - 1; i >= 0; i--) {
            Destroy(defaultSkin.itemTransfs[itemSlot].GetChild(i).gameObject);
        }
        GameObject itemObj;
        if (itemSlot == 0)
            itemObj = Instantiate(GlobalPrefab.Instance.weaponPrefabs[itemIndex], defaultSkin.itemTransfs[0]);
        else if (itemSlot == 1)
            itemObj = Instantiate(GlobalPrefab.Instance.shieldPrefabs[itemIndex], defaultSkin.itemTransfs[1]);
        else
            itemObj = Instantiate(GlobalPrefab.Instance.hatPrefabs[itemIndex], defaultSkin.itemTransfs[2]);
    }

    public void ChangeSkin(int skinIndex) {
        int newPrefabIndex;
        if (skinIndex < 3) {            // default, cowboy and police
            newPrefabIndex = 0;
        }
        else if (skinIndex == 3) {      // amongus
            newPrefabIndex = 1;
        }
        else {                          // others
            newPrefabIndex = 2;
        }
        if (newPrefabIndex == prefabIndex) {
            defaultSkin.meshRenderer.sharedMesh = GlobalPrefab.Instance.skinMeshes[skinIndex];
        }
        else {
            Destroy(meshObj);
            prefabIndex = newPrefabIndex;
            meshObj = Instantiate(GlobalPrefab.Instance.skinPrefabs[prefabIndex], transform);
            defaultSkin = meshObj.GetComponent<DefaultSkin>();
            defaultSkin.meshRenderer.sharedMesh = GlobalPrefab.Instance.skinMeshes[skinIndex];
            anim = meshObj.GetComponent<Animator>();
            for (int i = 0; i < 3; i++)
                EquipItem(i, equippedItems[i]);
        }

    }

}
