using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Tower towerToUpgrade; // 需要升级的防御塔

    public void UpgradeTower()
    {
        if (towerToUpgrade != null)
        {
            towerToUpgrade.Upgrade(); // 调用升级方法
        }
    }
}