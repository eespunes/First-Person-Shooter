using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AmmoItem : Item
{
    [FormerlySerializedAs("m_Ammo")] public int mAmmo;
    public  override void UseItem()
    {
        Weapon lWeapon = mGameController.mPlayer.GetComponent<Weapon>();
        if (lWeapon.mCurrentMaxAmmo <
            lWeapon.mMaxAmmo)
        {
            lWeapon.AddAmmo(mAmmo);
            DestroyItem();
        }
    }
}
