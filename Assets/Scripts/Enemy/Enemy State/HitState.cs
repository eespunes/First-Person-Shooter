using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitState : MonoBehaviour, IEnemyState
{
    private IEnemyState mState;
    private DroneEnemy mDroneEnemy;
    private float timer;

    public HitState(DroneEnemy droneEnemy)
    {
        init(droneEnemy);
    }
    public IEnemyState init(DroneEnemy droneEnemy)
    {
        mDroneEnemy = droneEnemy;
        mDroneEnemy.mNavMeshAgent.isStopped = true;
        mDroneEnemy.changeLightsToWhite();
        mDroneEnemy.mAudioSource.loop = false;
        mDroneEnemy.mAudioSource.clip = null;
        return this;
    }

    public void UpdateState()
    {
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            
            mState = mDroneEnemy.getLastState();
            if(mState.GetType().ToString()=="PatrolState")
                mDroneEnemy.SetState(new AlertState(mDroneEnemy));
            else
                mDroneEnemy.SetState(mState.init(mDroneEnemy));
        }
    }
}