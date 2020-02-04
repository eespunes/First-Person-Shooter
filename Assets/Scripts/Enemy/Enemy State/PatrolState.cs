using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.PlayerLoop;

public class PatrolState : MonoBehaviour, IEnemyState
{
    private DroneEnemy mDroneEnemy;
    private int mCurrentPatrolPositionId;

    public PatrolState(DroneEnemy droneEnemy)
    {
        init(droneEnemy);
    }

    public IEnemyState init(DroneEnemy droneEnemy)
    {
        mDroneEnemy = droneEnemy;
        mCurrentPatrolPositionId = GetClosestPatrolPositionId();
        mDroneEnemy.mNavMeshAgent.isStopped = false;
        mDroneEnemy.mNavMeshAgent.SetDestination(mDroneEnemy.mPatrolPositions[mCurrentPatrolPositionId].position);
        mDroneEnemy.changeLightsToBlue();
        mDroneEnemy.deactivateSpotLight();
        mDroneEnemy.mAudioSource.loop = false;
        mDroneEnemy.mAudioSource.clip = null;
        return this;
    }

    public void UpdateState()
    {
        if (!mDroneEnemy.mNavMeshAgent.hasPath &&
            mDroneEnemy.mNavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
            MoveToNextPatrolPosition();
        if (HearsPlayer())
            mDroneEnemy.SetState(new AlertState(mDroneEnemy));
        if (mDroneEnemy.GetSqrDistanceXZToPosition(mDroneEnemy.mGameController.mPlayerController.transform.position) >
            mDroneEnemy.mMaxDistanceToPatrol)
            mDroneEnemy.SetState(new IdleState(mDroneEnemy));
    }

    bool HearsPlayer()
    {
        return
            mDroneEnemy.GetSqrDistanceXZToPosition(mDroneEnemy.mGameController.mPlayerController.transform.position) <
            (mDroneEnemy.mMinDistanceToAlert * mDroneEnemy.mMinDistanceToAlert);
    }


    void MoveToNextPatrolPosition()
    {
        ++mCurrentPatrolPositionId;
        if (mCurrentPatrolPositionId >= mDroneEnemy.mPatrolPositions.Count)
            mCurrentPatrolPositionId = 0;
        mDroneEnemy.mNavMeshAgent.SetDestination(mDroneEnemy.mPatrolPositions[mCurrentPatrolPositionId].position);
    }

    private int GetClosestPatrolPositionId()
    {
        var lPosition = -1;
        var lCounter = 0;
        var lMinDistance = float.MaxValue;
        foreach (var lActualDistance in mDroneEnemy.mPatrolPositions.Select(position =>
            mDroneEnemy.GetSqrDistanceXZToPosition(position.position)))
        {
            if (lMinDistance > lActualDistance)
            {
                lPosition = lCounter;
                lMinDistance = lActualDistance;
            }

            lCounter++;
        }

        return lPosition;
    }
}