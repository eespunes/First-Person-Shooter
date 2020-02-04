using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FPSController : MonoBehaviour
{
    float mYaw;
    float mPitch;

    public float mYawRotationalSpeed = 360f;

    public float mPitchRotationalSpeed = 180f;
    public float mMinPitch = -80f;
    public float mMaxPitch = 50f;

    public Transform mPitchControllerTransform;

    private CharacterController mCharacterController;
    public float mSpeed = 10f;

    public KeyCode mLeftKeyCode = KeyCode.A;

    public KeyCode mRightKeyCode = KeyCode.D;

    public KeyCode mForwardKeyCode = KeyCode.W;

    public KeyCode mBackwardsKeyCode = KeyCode.S;

    float mVerticalSpeed = 0f;
    bool mOnGround = false;

    public KeyCode mRunKeyCode = KeyCode.LeftShift;
    public KeyCode mJumpKeyCode = KeyCode.Space;
    public float mFastSpeedMultiplier = 1.2f;
    public float mJumpSpeed = 10f;

    private const float mMaxLife = 200.0f;
    float mLife = mMaxLife;

    private const float mMaxShield = 100.0f;
    private float mShield = mMaxShield;

    public Text mLifeText;
    public Text mShieldText;
    private bool mKey;

    private GameController mGameController;
    private bool mIsInShootingGallery;

    public GameObject mBlood;

    private AudioSource mAudioSource;
    public AudioClip mHitSound;

    // Start is called before the first frame update
    void Awake()
    {
        mYaw = transform.rotation.eulerAngles.y;
        mPitch = mPitchControllerTransform.localRotation.eulerAngles.x;
        mCharacterController = GetComponent<CharacterController>();
        mGameController = GameObject.Find("GameController").GetComponent<GameController>();
        Cursor.lockState = CursorLockMode.Locked;
        mLifeText.text = mLife.ToString();
        mShieldText.text = mShield.ToString();
        mAudioSource=GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("Inside"))
        {
            Vector3 checkPointPosition = SingletonCheckPoint.getInstance().getPosition();
            transform.position = new Vector3(checkPointPosition.x, transform.position.y, checkPointPosition.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.F) && mIsInShootingGallery)
            mGameController.startShootingGallery();
    }

    private void Movement()
    {
        //Pitch
        float lMouseAxisY = -Input.GetAxis("Mouse Y");
        mPitch += lMouseAxisY * mPitchRotationalSpeed * Time.deltaTime;
        mPitch = Mathf.Clamp(mPitch, mMinPitch, mMaxPitch);
        mPitchControllerTransform.localRotation = Quaternion.Euler(mPitch, 0, 0);


        //Yaw
        float lMouseAxisX = Input.GetAxis("Mouse X");
        mYaw += lMouseAxisX * mYawRotationalSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, mYaw, 0);

        //Movement
        Vector3 lMovement = new Vector3(0, 0, 0);
        float lYawInRadians = mYaw * Mathf.Deg2Rad;
        float lYaw90InRadians = (mYaw + 90.0f) * Mathf.Deg2Rad;
        Vector3 lForward = new Vector3(Mathf.Sin(lYawInRadians), 0.0f, Mathf.Cos(lYawInRadians));
        Vector3 lRight = new Vector3(Mathf.Sin(lYaw90InRadians), 0.0f, Mathf.Cos(lYaw90InRadians));
        if (Input.GetKey(mForwardKeyCode))
            lMovement = lForward;
        else if (Input.GetKey(mBackwardsKeyCode))
            lMovement = -lForward;

        if (Input.GetKey(mRightKeyCode))
            lMovement += lRight;
        else if (Input.GetKey(mLeftKeyCode))
            lMovement -= lRight;

        lMovement.Normalize();
        lMovement = lMovement * Time.deltaTime * mSpeed;

        //Gravity
        mVerticalSpeed += Physics.gravity.y * Time.deltaTime;
        lMovement.y = mVerticalSpeed * Time.deltaTime;

        float lSpeedMultiplier = 1f;
        if (Input.GetKey(mRunKeyCode))
            lSpeedMultiplier = mFastSpeedMultiplier;

        lMovement *= Time.deltaTime * mSpeed * lSpeedMultiplier;

        CollisionFlags lCollisionFlags = mCharacterController.Move(lMovement);
        if ((lCollisionFlags & CollisionFlags.Below) != 0)
        {
            mOnGround = true;
            mVerticalSpeed = 0.0f;
        }
        else
            mOnGround = false;

        if ((lCollisionFlags & CollisionFlags.Above) != 0 && mVerticalSpeed > 0.0f)
            mVerticalSpeed = 0.0f;
        // Jump
        if (mOnGround && Input.GetKeyDown(mJumpKeyCode))
            mVerticalSpeed = mJumpSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Item":
                other.GetComponent<Item>().UseItem();
                break;
            case "DeadZone":
                SceneManager.LoadScene("GameOver");
                break;
            case "ShootingGallery":
                mIsInShootingGallery = true;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ShootingGallery")) mIsInShootingGallery = false;
    }

    public void Hit(float damage)
    {
        mAudioSource.clip = mHitSound;
        mAudioSource.Play();
        mBlood.SetActive(true);
        Invoke("stopBlood",0.25f);
        float lDamageShield = damage * 0.75f;
        float lDamageHealth = damage * 0.25f;
        if (mShield > 0)
        {
            if (mShield <= lDamageShield)
            {
                mShield -= lDamageShield;
                mLife -= lDamageHealth + mShield;
                mShield = 0;
            }
            else
            {
                mShield -= lDamageShield;
                mLife -= lDamageHealth;
            }
        }
        else
            mLife -= damage;

        mLifeText.text = mLife.ToString();
        mShieldText.text = mShield.ToString();
        if (mLife <= 0)
            SceneManager.LoadScene("GameOver");
    }

    public void stopBlood()
    {
        mBlood.SetActive(false);
    }

    public void AddLife(int lifePoints)
    {
        if (mLife + lifePoints > mMaxLife)
            mLife = mMaxLife;
        else
            mLife += lifePoints;
        mLifeText.text = mLife.ToString();
    }

    public float GetLife()
    {
        return mLife;
    }

    public float GetMaxLife()
    {
        return mMaxLife;
    }

    public void AddShield(int shieldPoints)
    {
        if (mShield + shieldPoints > mMaxShield)
            mShield = mMaxShield;
        else
            mShield += shieldPoints;
        mShieldText.text = mShield.ToString();
    }

    public float getMaxShield()
    {
        return mMaxShield;
    }

    public float GetShield()
    {
        return mShield;
    }

    public bool hasKey()
    {
        if (mKey)
        {
            mKey = false;
            return true;
        }

        return false;
    }

    public void setKey()
    {
        mKey = true;
    }
}