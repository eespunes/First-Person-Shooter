using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShieldItem : Item
{
    public int mShieldPoints;
     public override void UseItem()
     {
         if (mGameController.mPlayerController.GetShield() <
             mGameController.mPlayerController.getMaxShield())
         {
             mGameController.mPlayerController.AddShield(mShieldPoints);
             DestroyItem();
         }
     }
 }