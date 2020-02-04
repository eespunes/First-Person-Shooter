using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public int mMouseShootButton;

    public int mMousePointButton;

    public KeyCode mEnableDisableToAutomaticButton = KeyCode.V;

    [Header("Ammo")] public int mCurrentAmmoCount;

    public int mStartAmmo;
    public int mMaxAmmo;


    public int mCurrentMaxAmmo;

    public float mZoom;
    private float mReturnZoom;
    private bool mAutomaticShoot;

    public GameController mGameController;


    public LayerMask mShootLayerMask;

    public GameObject mShootHitParticles;

    public GameObject mDestroyObjects;

    public GameObject mWeaponDummy;

    public GameObject mWeaponParticlesEffect;

    public Animation mWeaponAnimator;

    public Text mCurrentAmmoText;

    public Text mCurrentMaxAmmoText;

    public GameObject mBulletHoleDecal;
    public GameObject mTargetExplosion;

    private AudioSource mAudioSource;
    public AudioClip mShootSound;
    public AudioClip mReloadSound;

    void Start()
    {
        mCurrentAmmoCount = mStartAmmo;
        mCurrentMaxAmmo = mMaxAmmo;
        mCurrentAmmoText.text = mCurrentAmmoCount.ToString();
        mCurrentMaxAmmoText.text = mCurrentMaxAmmo.ToString();
        mAudioSource = GetComponent<AudioSource>();
        mReturnZoom = Camera.main.fieldOfView;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(mMouseShootButton))
            if (mAutomaticShoot)
                InvokeRepeating("AutomaticShoot", 0, 0.1f);
            else
                TryShoot();
        if (mAutomaticShoot)
            if (Input.GetMouseButtonUp(mMouseShootButton))
                CancelInvoke();
        if (Input.GetKeyDown(mEnableDisableToAutomaticButton))
        {
            mAutomaticShoot = !mAutomaticShoot;
            if (mAutomaticShoot)
            {
                mCurrentAmmoText.color = Color.blue;
                mCurrentMaxAmmoText.color = Color.blue;
            }
            else
            {
                mCurrentAmmoText.color = Color.red;
                mCurrentMaxAmmoText.color = Color.red;
            }
        }

        if (Input.GetMouseButtonDown(mMousePointButton))
            Camera.main.fieldOfView = mZoom;

        if (Input.GetMouseButtonUp(mMousePointButton))
            Camera.main.fieldOfView = mReturnZoom;
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CanReload())
            {
                Reload();
                mWeaponAnimator.CrossFade("weapon_reload");
            }
        }

        if (!mWeaponAnimator.isPlaying)
            mWeaponAnimator.CrossFade("weapon_idle");
    }

    public void AutomaticShoot()
    {
        TryShoot();
    }

    private void TryShoot()
    {
        if (CanShoot())
        {
            Shoot();
            mWeaponAnimator.Play("weapon_shoot");
        }
        else
            mWeaponAnimator.Play("weapon_no_shoot");
    }

    void Shoot()
    {
        Ray lCameraRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit lRaycastHit;

        if (Physics.Raycast(lCameraRay, out lRaycastHit, 200.0f, mShootLayerMask.value))
        {
            switch (lRaycastHit.collider.tag)
            {
                case "Enemy":
                    HitCollider lHitCollider = lRaycastHit.collider.GetComponent<HitCollider>();
                    if (lHitCollider.m_HitColliderType == HitCollider.THitColliderType.HEAD)
                        lHitCollider.m_DroneEnemy.Hit(100.0f);
                    else if (lHitCollider.m_HitColliderType == HitCollider.THitColliderType.HELIX)
                        lHitCollider.m_DroneEnemy.Hit(25.0f);
                    else
                        lHitCollider.m_DroneEnemy.Hit(0.0f);
                    break;
                case "Target":
                    mGameController.addScore(50);
                    GameObject go = Instantiate(mTargetExplosion, lRaycastHit.collider.transform.parent);
                    go.transform.parent = null;
                    StartCoroutine(DestroyAfterTime(go, 2f));
                    Destroy(lRaycastHit.collider.transform.parent.gameObject);
                    break;
                case "Destroyable":
                    StartCoroutine(DestroyAfterTime(lRaycastHit.collider.gameObject, 0.5f));
                    break;
            }

            mAudioSource.clip = mShootSound;
            mAudioSource.Play();
            CreateShootHitParticles(lRaycastHit.point, lRaycastHit.normal, lRaycastHit.collider.transform);
        }

        CreateShootWeaponParticles(mWeaponDummy.transform.position);
        mCurrentAmmoCount--;
        mCurrentAmmoText.text = mCurrentAmmoCount.ToString();
    }

    IEnumerator DestroyAfterTime(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }


    void CreateShootHitParticles(Vector3 position, Vector3 normal, Transform parent)
    {
        Instantiate(mShootHitParticles, position, Quaternion.LookRotation(normal), mDestroyObjects.transform);
        SingletonDecal.getInstance().createDecal(mBulletHoleDecal, position, normal, parent);
    }

    void CreateShootWeaponParticles(Vector3 position)
    {
        Instantiate(mWeaponParticlesEffect, position, Quaternion.identity, mWeaponDummy.transform);
    }

    bool CanShoot()
    {
        return mCurrentAmmoCount > 0;
    }

    void Reload()
    {
        mAudioSource.clip = mReloadSound;
        mAudioSource.Play();
        int lTryReload = mStartAmmo - mCurrentAmmoCount;
        int lToReload = Mathf.Min(lTryReload, mCurrentMaxAmmo);
        mCurrentAmmoCount += lToReload;
        mCurrentMaxAmmo -= lToReload;
        mCurrentMaxAmmoText.text = mCurrentMaxAmmo.ToString();
        mCurrentAmmoText.text = mCurrentAmmoCount.ToString();
    }

    bool CanReload()
    {
        return mCurrentMaxAmmo != 0 && mCurrentAmmoCount != mStartAmmo;
    }

    public void AddAmmo(int ammo)
    {
        if (mCurrentMaxAmmo + ammo > mMaxAmmo)
            mCurrentMaxAmmo = mMaxAmmo;
        else
            mCurrentMaxAmmo += ammo;
        mCurrentMaxAmmoText.text = mCurrentMaxAmmo.ToString();
    }
}