using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AlertState : MonoBehaviour,IEnemyState
{
    private float mConeAngle;
    private float mAngle;
    private float mAngleCounter;
    private DroneEnemy mDroneEnemy;

    public AlertState(DroneEnemy droneEnemy)
    {
        init(droneEnemy);
    }
    
    public IEnemyState init(DroneEnemy droneEnemy)
    {
        mDroneEnemy = droneEnemy;
        mConeAngle = 60.0f;
        mAngle = 1;
        mDroneEnemy.mNavMeshAgent.isStopped = true;
        mDroneEnemy.changeLightsToYellow();
        mDroneEnemy.activateSpotLight();
        mDroneEnemy.mAudioSource.loop = true;
        mDroneEnemy.mAudioSource.clip = mDroneEnemy.mAlertSound;
        mDroneEnemy.mAudioSource.Play();
        return this;
    }

    public void UpdateState()
    {
        if (SeesPlayer())
        {
            mDroneEnemy.mAudioSource.Stop();
            mDroneEnemy.SetState(new ChaseState(mDroneEnemy));
        }
        else if (mAngleCounter >= 360)
        {
            mDroneEnemy.mAudioSource.Stop();
            mDroneEnemy.SetState(new PatrolState(mDroneEnemy));
        }
        else
        {
            mAngleCounter++;
            mDroneEnemy.transform.Rotate(0, mAngle, 0, Space.Self);
        }
    }

    bool SeesPlayer()
    {
        Vector3
            lDirection = (mDroneEnemy.mGameController.mPlayerController.transform.position + Vector3.up * 0.9f
                         ) - mDroneEnemy.transform.position;
        Ray lRay = new Ray(mDroneEnemy.transform.position, lDirection);
        float lDistance = lDirection.magnitude;
        lDirection /= lDistance;
        bool lCollides = Physics.Raycast(lRay, lDistance, mDroneEnemy.mCollisionLayerMask.value);
        float lDotAngle = Vector3.Dot(lDirection, mDroneEnemy.transform.forward);
        Debug.DrawRay(mDroneEnemy.transform.position, lDirection * lDistance, lCollides ? Color.red : Color.yellow);
        return !lCollides && lDotAngle > Mathf.Cos(mConeAngle * 0.5f * Mathf.Deg2Rad);
    }
}