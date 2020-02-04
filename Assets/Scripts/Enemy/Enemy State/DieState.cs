using System.Collections;
using UnityEngine;

public class DieState : MonoBehaviour, IEnemyState
{
    private Material mMaterial;
    private DroneEnemy mDroneEnemy;
    private float timer;
    private float mDeadPositionY;
    private static readonly int Cutoff = Shader.PropertyToID("_Cutoff");

    public DieState(DroneEnemy droneEnemy)
    {
        init(droneEnemy);
    }

    public IEnemyState init(DroneEnemy droneEnemy)
    {
        mDroneEnemy = droneEnemy;
        mDroneEnemy.transform.GetChild(0).rotation=new Quaternion(0,0,0,0);
        mDroneEnemy.disableAllLights();
        mDroneEnemy.mNavMeshAgent.isStopped = true;
        mDroneEnemy.mLifeBar.gameObject.SetActive(false);
        mDroneEnemy.mAudioSource.loop = false;
        mDroneEnemy.mAudioSource.clip = null;
        mDeadPositionY = mDroneEnemy.transform.GetChild(0).position.y - 3.25f;
        return this;
    }

    public void UpdateState()
    {
        mDroneEnemy.transform.GetChild(0).position = Vector3.MoveTowards(mDroneEnemy.transform.GetChild(0).position,
            new Vector3(mDroneEnemy.transform.GetChild(0).position.x,
                mDeadPositionY, mDroneEnemy.transform.GetChild(0).position.z),
            2.5f * Time.deltaTime);
        if (mDroneEnemy.transform.GetChild(0).position.y <= mDeadPositionY)
            timer += Time.deltaTime;
        if (timer > 2)
        {
            if (mDroneEnemy.mMaterial.GetFloat(Cutoff) >= 1)
            {
                GameObject go = Instantiate(getRandomItem(), mDroneEnemy.transform.GetChild(0));
                go.transform.parent = null;
                Destroy(mDroneEnemy.gameObject);
            }
            else
                mDroneEnemy.mMaterial.SetFloat(Cutoff, mDroneEnemy.mMaterial.GetFloat(Cutoff) + 0.01f);
        }
    }

    private GameObject getRandomItem()
    {
        float lRandom = Random.Range(0.0f, 1.0f);
        if (lRandom < 0.333f)
            return mDroneEnemy.mItems[0];
        if (lRandom < 0.666f)
            return mDroneEnemy.mItems[1];
        return mDroneEnemy.mItems[2];
    }
}