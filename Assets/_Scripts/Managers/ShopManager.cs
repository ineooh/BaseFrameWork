using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : SingletonMono<ShopManager> {

    private int shopIndex;
    private int[] shopPageIndices;

    [SerializeField]
    private int[] itemPageCounts;

    [SerializeField]
    private Transform shopsTransf;
    [SerializeField]
    private Transform btnsTransf;
    [SerializeField]
    private Transform hightlightWeapon, highlightShield, highlightHat, highlightSkin;

    [SerializeField]
    private Transform[] itemGrids, itemSelected;

    [SerializeField]
    private Sprite[] weaponSprites, shieldSprites, hatSprites, skinSprites;

    private void Start() {
        Debug.Log(GSystem.userData.itemIndices[0]);
        Transform itemPage;
        shopPageIndices = new int[4];
        for (int i = 0; i < 4; i++) {
            shopPageIndices[i] = GSystem.userData.itemIndices[i] / 12;
            itemGrids[i].GetChild(shopPageIndices[i]).gameObject.SetActive(true);
            itemPage = itemGrids[i].GetChild(shopPageIndices[i]);
            itemPage.gameObject.SetActive(true);
            if (GSystem.userData.itemIndices[i] != -1) {
                itemSelected[i].gameObject.SetActive(true);
                itemSelected[i].SetParent(itemPage.GetChild(GSystem.userData.itemIndices[i] % 12));
                itemSelected[i].localPosition = Vector2.zero;
            }
        }

        // mark selected items
        for (int i = 0; i < GSystem.userData.itemsProgress[0].Length; i++) {
            if (GSystem.userData.itemsProgress[0][i] == 1) {
                itemGrids[0].GetChild(i / 12).GetChild(i % 12).GetComponent<Image>().sprite = weaponSprites[i];
            }
        }
        for (int i = 0; i < GSystem.userData.itemsProgress[1].Length; i++) {
            if (GSystem.userData.itemsProgress[1][i] == 1) {
                itemGrids[1].GetChild(i / 12).GetChild(i % 12).GetComponent<Image>().sprite = shieldSprites[i];
            }
        }
        for (int i = 0; i < GSystem.userData.itemsProgress[2].Length; i++) {
            if (GSystem.userData.itemsProgress[2][i] == 1) {
                itemGrids[2].GetChild(i / 12).GetChild(i % 12).GetComponent<Image>().sprite = hatSprites[i];
            }
        }
        for (int i = 0; i < GSystem.userData.itemsProgress[3].Length; i++) {
            if (GSystem.userData.itemsProgress[3][i] == 1) {
                itemGrids[3].GetChild(i / 12).GetChild(i % 12).GetComponent<Image>().sprite = skinSprites[0];
            }
        }
    }

    public void ChangeShop(int newShopIndex) {
        shopsTransf.GetChild(shopIndex).gameObject.SetActive(false);
        Transform btnTransf = btnsTransf.GetChild(shopIndex);
        btnTransf.localPosition = new Vector2(btnTransf.localPosition.x, 0);
        btnTransf.GetChild(0).gameObject.SetActive(true);
        btnTransf.GetChild(1).gameObject.SetActive(false);


        shopIndex = newShopIndex;
        shopsTransf.GetChild(shopIndex).gameObject.SetActive(true);
        btnTransf = btnsTransf.GetChild(shopIndex);
        btnTransf.localPosition = new Vector2(btnTransf.localPosition.x, 35);
        btnTransf.GetChild(0).gameObject.SetActive(false);
        btnTransf.GetChild(1).gameObject.SetActive(true);
    }

    public void PrevPage() {
        itemGrids[shopIndex].GetChild(shopPageIndices[shopIndex]).gameObject.SetActive(false);
        shopPageIndices[shopIndex]--;
        if (shopPageIndices[shopIndex] < 0)
            shopPageIndices[shopIndex] = 0;
        itemGrids[shopIndex].GetChild(shopPageIndices[shopIndex]).gameObject.SetActive(true);
    }

    public void NextPage() {
        itemGrids[shopIndex].GetChild(shopPageIndices[shopIndex]).gameObject.SetActive(false);
        shopPageIndices[shopIndex]++;
        if (shopPageIndices[shopIndex] > itemPageCounts[shopIndex] - 1)
            shopPageIndices[shopIndex] = itemPageCounts[shopIndex] - 1;
        itemGrids[shopIndex].GetChild(shopPageIndices[shopIndex]).gameObject.SetActive(true);
    }

    public void SelectWeapon(int weaponIndex) {
        SelectItem(0, weaponIndex);
    }

    public void SelectShield(int shieldIndex) {
        SelectItem(1, shieldIndex);
    }

    public void SelectHat(int hatIndex) {
        SelectItem(2, hatIndex);
    }

    public void SelectSkin(int skinIndex) {
        SelectItem(3, skinIndex);
    }

    public void SelectItem(int shopIndex, int itemIndex) {
        if (GSystem.userData.itemsProgress[shopIndex][itemIndex] != 1)
            return;
        GSystem.userData.itemIndices[shopIndex] = itemIndex;
        GSystem.SaveUserData();
        itemSelected[shopIndex].gameObject.SetActive(true);
        itemSelected[shopIndex].SetParent(itemGrids[shopIndex].GetChild(shopPageIndices[shopIndex]).GetChild(itemIndex % 12));
        itemSelected[shopIndex].localPosition = Vector2.zero;

        if (shopIndex == 3)
            ShopModel.Instance.ChangeSkin(itemIndex);
        else
            ShopModel.Instance.EquipItem(shopIndex, itemIndex);
    }

}
