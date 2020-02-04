using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class DroneEnemy : MonoBehaviour
{
    public NavMeshAgent mNavMeshAgent;
    public IEnemyState mEnemyState;
    public List<Transform> mPatrolPositions;
    [HideInInspector] public GameController mGameController;
    public float mMinDistanceToAlert = 2.5f;
    public LayerMask mCollisionLayerMask;
    public float mMaxDistanceToPatrol = 10.0f;
    const float mMaxLife = 100.0f;
    float mLife = mMaxLife;

    public List<GameObject> mItems;

    public Light mLeftLight;
    public Light mRightLight;
    public Light mCenterLight;
    private IEnemyState mLastState;
    public LayerMask mShootLayerMask;
    public GameObject mGun;
    public Slider mLifeBar;
    public GameObject mParticlesEffect;
    [HideInInspector] public AudioSource mAudioSource;
    public AudioClip mShootSound;
    public AudioClip mAlertSound;
    public Material mMaterial;
    private List<MeshRenderer> listOfChildren;
    public RectTransform mLifeBarCanvasRectTransform;
    public RectTransform mLifeBarAnchor;
    public float mLifeBarOffsetY;
    public float mLifeBarScaleOffset;
    private static readonly int Cutoff = Shader.PropertyToID("_Cutoff");

    void Start()
    {
        mGameController = GameObject.Find("GameController").GetComponent<GameController>();
        mAudioSource = GetComponent<AudioSource>();
        mEnemyState = new IdleState(this);

        mMaterial.SetFloat(Cutoff, 0);
        changeMaterialAllChildsRecursive(gameObject);
    }

    private void changeMaterialAllChildsRecursive(GameObject obj)
    {
        if (null == obj)
            return;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
                continue;
            //child.gameobject contains the current child you can do whatever you want like add it to an array
            if (child.GetComponent<MeshRenderer>() != null)
                child.GetComponent<MeshRenderer>().material = mMaterial;
            changeMaterialAllChildsRecursive(child.gameObject);
        }
    }

    void Update()
    {
        mEnemyState.UpdateState();
        updateLifeBar();
    }

    public void SetState(IEnemyState enemyState)
    {
        mLastState = mEnemyState;
        this.mEnemyState = enemyState;
    }

    public float GetSqrDistanceXZToPosition(Vector3 transformPosition)
    {
        var lPosition = transform.position;
        return Mathf.Sqrt(Vector2.Distance(new Vector2(transformPosition.x, transformPosition.z),
            new Vector2(lPosition.x, lPosition.z)));
    }

    public void changeLightsToBlue()
    {
        mLeftLight.color = Color.blue;
        mRightLight.color = Color.blue;
        mCenterLight.color = Color.blue;
    }

    public void changeLightsToRed()
    {
        mLeftLight.color = Color.red;
        mRightLight.color = Color.red;
        mCenterLight.color = Color.red;
    }

    public void changeLightsToYellow()
    {
        mLeftLight.color = Color.yellow;
        mRightLight.color = Color.yellow;
        mCenterLight.color = Color.yellow;
    }

    public void changeLightsToWhite()
    {
        mLeftLight.color = Color.white;
        mRightLight.color = Color.white;
        mCenterLight.color = Color.white;
    }

    public void activateSpotLight()
    {
        mCenterLight.gameObject.SetActive(true);
    }

    public void deactivateSpotLight()
    {
        mCenterLight.gameObject.SetActive(false);
    }

    public void Hit(float f)
    {
        mLife -= f;
        mLifeBar.value = mLife / mMaxLife;
        if (mLife > 0)
        {
            if (f > 0 || mEnemyState.GetType().ToString().Equals("PatrolState"))
            {
                SetState(new HitState(this));
            }
        }
        else if (mLife <= 0)
            SetState(new DieState(this));
    }

    private void updateLifeBar()
    {
        mLifeBarCanvasRectTransform.gameObject.SetActive(isRendering());
            Vector3 lViewportPoint =
                Camera.main.WorldToViewportPoint(transform.position + Vector3.up * mLifeBarOffsetY);
            mLifeBarAnchor.anchoredPosition = new Vector3(
                lViewportPoint.x * mLifeBarCanvasRectTransform.sizeDelta.x, -(1.0f - lViewportPoint.y) *
                                                                            mLifeBarCanvasRectTransform.sizeDelta
                                                                                .y, 0.0f);
            float lDistance;
            lDistance = 1 / GetSqrDistanceXZToPosition(mGameController.mPlayerController.transform.position);
            mLifeBarAnchor.localScale=new Vector3(mLifeBarScaleOffset*lDistance,mLifeBarScaleOffset*lDistance,mLifeBarScaleOffset*lDistance);
    }

    private bool isRendering()
    {
        var position = transform.GetChild(0).position;
        var position1 = Camera.main.transform.position;
        Ray lCameraRay = new Ray(position1,
            new Vector3(position.x - position1.x,
                position.y - position1.y,
                position.z - position1.z));
        RaycastHit lRaycastHit;
        Debug.DrawRay(position1,
            new Vector3(position.x - position1.x,
                position.y - position1.y,
                position.z - position1.z),
            Color.yellow);
        if (Physics.Raycast(lCameraRay, out lRaycastHit, 200.0f, mShootLayerMask.value))
        {
            return lRaycastHit.collider.CompareTag("Enemy");
        }

        return false;
    }

    public IEnemyState getLastState()
    {
        return mLastState;
    }

    public void disableAllLights()
    {
        mLeftLight.gameObject.SetActive(false);
        mRightLight.gameObject.SetActive(false);
        mCenterLight.gameObject.SetActive(false);
    }

    public void findPlayer()
    {
        Vector3 targetPostition = new Vector3(mGameController.mPlayer.transform.GetChild(0).position.x,
            mGameController.mPlayer.transform.GetChild(0).position.y + 2,
            mGameController.mPlayer.transform.GetChild(0).position.z);
        transform.GetChild(0).LookAt(targetPostition);
    }
}