using UnityEngine;

public class SingletonCheckPoint
{
    private Vector3 mPosition;
    private static SingletonCheckPoint mSingletonCheckPoint;

    private SingletonCheckPoint()
    {
        mPosition = new Vector3(-8, 0, 7);
    }

    public static SingletonCheckPoint getInstance()
    {
        if (mSingletonCheckPoint == null)
            mSingletonCheckPoint = new SingletonCheckPoint();
        return mSingletonCheckPoint;
    }

    public Vector3 getPosition()
    {
        return mPosition;
    }

    public void setPosition(Vector3 vector3)
    {
        this.mPosition = vector3;
    }
}