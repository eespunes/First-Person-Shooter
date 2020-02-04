using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    public GameObject mToDestroy;

    [HideInInspector]
    public GameObject mPlayer;
    [HideInInspector]
    public FPSController mPlayerController;
    public GameObject mKey;
    [HideInInspector]
    public GameObject mShootingGallery;

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GameObject.Find("Player");
        mPlayerController = mPlayer.GetComponent<FPSController>();
        mShootingGallery = GameObject.Find("Shooting Gallery");
    }

    // Update is called once per frame
    void Update()
    {
        if (mPlayer == null)
        {
            mPlayer = GameObject.Find("Player");
            mPlayerController = mPlayer.GetComponent<FPSController>();
        }
    }

    public void GetKey(Transform mKeyTransform)
    {
        Instantiate(mKey, mKeyTransform);
    }

    public void startShootingGallery()
    {
        mShootingGallery.GetComponent<ShootingGalleryController>().init();
    }

    public void addScore(int score)
    {
        mShootingGallery.GetComponent<ShootingGalleryController>().addScore(score);
    }}