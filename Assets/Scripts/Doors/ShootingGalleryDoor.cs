using System;
using UnityEngine;

public class ShootingGalleryDoor : MonoBehaviour
{
    public int mScoreToOpen;
    private AudioSource mAudioSource;

    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
    }

    public int getScoreToOpen()
    {
        return mScoreToOpen;
    }

    public void openDoor()
    {
        mAudioSource.Play();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}