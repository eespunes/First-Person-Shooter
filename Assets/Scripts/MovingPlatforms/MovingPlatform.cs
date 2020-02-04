using UnityEngine;

public abstract class MovingPlatform : MonoBehaviour
{
    protected bool mMoving;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mMoving = true;
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mMoving = true;
            other.transform.parent = null;
        }
    }
}