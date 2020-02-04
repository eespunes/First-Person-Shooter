using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaseState : MonoBehaviour, IEnemyState
{
    private float mMinDistanceToAttack = 2.0f;
    private float mMaxDistanceToAttack = 5.0f;
    private DroneEnemy mDroneEnemy;
    
    public ChaseState(DroneEnemy droneEnemy)
    {
        init(droneEnemy);
    }

    public IEnemyState init(DroneEnemy droneEnemy)
    {
        mDroneEnemy = droneEnemy;
        mDroneEnemy.mNavMeshAgent.isStopped = false;
        mDroneEnemy.changeLightsToRed();
        mDroneEnemy.activateSpotLight();
        mDroneEnemy.mAudioSource.loop = false;
        mDroneEnemy.mAudioSource.clip = null;
        return this;
    }
    public void UpdateState()
    {
        mDroneEnemy.findPlayer();
        mDroneEnemy.mNavMeshAgent.SetDestination(mDroneEnemy.mGameController.mPlayerController.transform.position);
        float lDistanceToPlayer =
            mDroneEnemy.GetSqrDistanceXZToPosition(mDroneEnemy.mGameController.mPlayerController.transform.position);
        if(lDistanceToPlayer<=mMinDistanceToAttack)
            mDroneEnemy.SetState(new AttackState(mDroneEnemy));
        else if(lDistanceToPlayer>mMaxDistanceToAttack)
            mDroneEnemy.SetState(new PatrolState(mDroneEnemy));
    }
    
}