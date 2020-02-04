using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackState : MonoBehaviour, IEnemyState
{
    private DroneEnemy mDroneEnemy;
    private float mShootMax = 2.0f;
    private float timer;
    private float mShootingAccuracy = 0.85f;

    public AttackState(DroneEnemy droneEnemy)
    {
        init(droneEnemy);
    }

    public IEnemyState init(DroneEnemy droneEnemy)
    {
        this.mDroneEnemy = droneEnemy;
        mDroneEnemy.mNavMeshAgent.isStopped = true;
        mDroneEnemy.mNavMeshAgent.destination = mDroneEnemy.transform.position;
        mDroneEnemy.changeLightsToRed();
        mDroneEnemy.deactivateSpotLight();
        mDroneEnemy.mAudioSource.loop = false;
        mDroneEnemy.mAudioSource.clip = mDroneEnemy.mShootSound;
        return this;
    }

    public void UpdateState()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            Shoot();
            timer = 0;
        }

        mDroneEnemy.findPlayer();

        if (mDroneEnemy.GetSqrDistanceXZToPosition(mDroneEnemy.mGameController.mPlayerController.transform.position) >
            mShootMax)
        {
            mDroneEnemy.SetState(new ChaseState(mDroneEnemy));
        }
    }

    public void Shoot()
    {
        Ray lCameraRay = new Ray(mDroneEnemy.mGun.transform.position, mDroneEnemy.mGun.transform.forward);
        RaycastHit lRaycastHit;

        if (Random.Range(0.0f, 1.0f) < mShootingAccuracy)
            if (Physics.Raycast(lCameraRay, out lRaycastHit, 200.0f, mDroneEnemy.mShootLayerMask.value))
            {
                switch (lRaycastHit.collider.tag)
                {
                    case "Player":
                        mDroneEnemy.mGameController.mPlayerController.Hit(50);
                        break;
                }
            }
        
        mDroneEnemy.mAudioSource.Play();
        CreateShootWeaponParticles(mDroneEnemy.mGun.transform.position);
    }

    void CreateShootWeaponParticles(Vector3 position)
    {
        Instantiate(mDroneEnemy.mParticlesEffect, position, Quaternion.identity, mDroneEnemy.mGun.transform);
    }
}