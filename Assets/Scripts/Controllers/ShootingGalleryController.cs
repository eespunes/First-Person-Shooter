using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingGalleryController : MonoBehaviour
{
    public GameObject mScoreTextGameObject;
    private Text mScoreText;
    private Text mMaxScoreText;
    private int mScore;
    private int mMaxScore;

    public GameObject mShootingGallery;

    public ShootingGalleryDoor mShootingGalleryDoor;
    public Transform mShootingGalleryPosition;

    // Start is called before the first frame update
    void Start()
    {
        mScoreText = mScoreTextGameObject.transform.GetChild(0).GetComponent<Text>();
        mMaxScoreText = mScoreTextGameObject.transform.GetChild(1).GetChild(0).GetComponent<Text>();
    }

    public void init()
    {
        if (mShootingGalleryPosition.childCount == 0)
        {
            mScoreTextGameObject.SetActive(true);
            mScoreText.text = mScore.ToString();
            mMaxScoreText.text = mMaxScore.ToString();
            Instantiate(mShootingGallery, mShootingGalleryPosition);
            Invoke("stopShootingGallery", 40);
        }
    }

    public void addScore(int score)
    {
        mScore += score;
        mScoreText.text = mScore.ToString();
    }

    public void stopShootingGallery()
    {
        if (mScore >= mShootingGalleryDoor.getScoreToOpen() && mMaxScore < mShootingGalleryDoor.getScoreToOpen())
        {
            mShootingGalleryDoor.openDoor();
        }

        mScoreTextGameObject.SetActive(false);
        if (mScore > mMaxScore)
            mMaxScore = mScore;
        mScore = 0;
        Destroy(transform.GetChild(1).GetChild(0).gameObject);
    }
}