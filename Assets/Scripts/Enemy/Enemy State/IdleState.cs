using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IdleState : MonoBehaviour, IEnemyState
{
    private DroneEnemy mDroneEnemy;

    public IdleState(DroneEnemy droneEnemy)
    {
        init(droneEnemy);
    }

    public IEnemyState init(DroneEnemy droneEnemy)
    {
        mDroneEnemy = droneEnemy;
        mDroneEnemy.mNavMeshAgent.isStopped = true;
        mDroneEnemy.changeLightsToBlue();
        mDroneEnemy.deactivateSpotLight();
        mDroneEnemy.mAudioSource.loop = false;
        mDroneEnemy.mAudioSource.clip = null;
        return this;
    }

    public void UpdateState()
    {
        if (mDroneEnemy.GetSqrDistanceXZToPosition(mDroneEnemy.mGameController.mPlayerController.transform.position) <
            mDroneEnemy.mMaxDistanceToPatrol)
            mDroneEnemy.SetState(new PatrolState(mDroneEnemy));
    }
}