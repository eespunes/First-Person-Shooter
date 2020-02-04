using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    private Vector3 mInitialPosition;
    public Vector3 mFinalPosition;
    private bool mOpening;
    private bool mClosing;

    private AudioSource mAudioSource;

    void Start()
    {
        mInitialPosition = transform.GetChild(0).localPosition;
        mAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (mOpening)
        {
            if (transform.GetChild(0).localPosition!= mFinalPosition)
            {
                transform.GetChild(0).localPosition =
                    Vector3.MoveTowards(transform.GetChild(0).localPosition, mFinalPosition, 0.2f);
            }
        }
        else if (mClosing)
        {
            if (transform.GetChild(0).localPosition != mInitialPosition)
            {
                transform.GetChild(0).localPosition =
                    Vector3.MoveTowards(transform.GetChild(0).localPosition, mInitialPosition, 0.2f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mOpening = true;
            mClosing = false;
            mAudioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mOpening = false;
            mClosing = true;
            mAudioSource.Play();
        }
    }
}