using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public Vector3 mFinalPosition;
    private bool mOpening;
    private FPSController mPlayer;
    private bool never;

    void Start()
    {
        mPlayer = GameObject.Find("Player").GetComponent<FPSController>();
        mOpening = true;
        never = true;
    }

    private void Update()
    {
        if (never)
            if (!mPlayer.hasKey())
                return;
        if (mOpening)
        {
            never = false;
            if (transform.GetChild(0).localPosition != mFinalPosition)
            {
                transform.GetChild(0).localPosition =
                    Vector3.MoveTowards(transform.GetChild(0).localPosition, mFinalPosition, 0.2f);
            }
        }
    }
}