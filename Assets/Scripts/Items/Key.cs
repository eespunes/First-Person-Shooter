using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    public override void UseItem()
    {
        mGameController.mPlayerController.setKey();
        DestroyItem();
    }
}
