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
    private Transform weaponTransf, shieldTransf, hatTransf;

    [SerializeField]
    private Transform[] itemTransfs;

    private void Start() {
        equippedItems = new int[3] { GSystem.userData.itemIndices[0], GSystem.userData.itemIndices[1], GSystem.userData.itemIndices[2] };
        equippedSkin = GSystem.userData.itemIndices[3];
        if (equippedSkin > -1) {
            ChangeSkin(equippedSkin);
        }
        else {
            for (int i = 0; i < 3; i++)
                EquipItem(i, equippedItems[i]);
        }
    }

    public void EquipItem(int itemSlot, int itemIndex) {
        if (itemSlot > 2 || itemSlot < 0 || equippedSkin != -1)
            return;
        int n = itemTransfs[itemSlot].childCount;
        for (int i = n - 1; i >= 0; i--) {
            Destroy(itemTransfs[itemSlot].GetChild(i).gameObject);
        }
        GameObject itemObj;
        if (itemSlot == 0)
            itemObj = Instantiate(GlobalPrefab.Instance.weaponPrefabs[itemIndex], itemTransfs[0]);
        else if (itemSlot == 1)
            itemObj = Instantiate(GlobalPrefab.Instance.shieldPrefabs[itemIndex], itemTransfs[1]);
        else
            itemObj = Instantiate(GlobalPrefab.Instance.hatPrefabs[itemIndex], itemTransfs[2]);
    }

    public void ChangeSkin(int skinIndex) {
        Destroy(meshObj);
        meshObj = Instantiate(GlobalPrefab.Instance.skinPrefabs[skinIndex], transform);
        anim = meshObj.GetComponent<Animator>();
    }

}
