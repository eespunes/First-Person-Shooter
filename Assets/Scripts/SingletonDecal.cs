using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SingletonDecal
{
    private List<GameObject> mDecals;
    private static SingletonDecal mSingletonDecal;

    private SingletonDecal()
    {
        mDecals = new List<GameObject>();
    }

    public static SingletonDecal getInstance()
    {
        if (mSingletonDecal == null)
        {
            mSingletonDecal = new SingletonDecal();
        }

        return mSingletonDecal;
    }

    public void createDecal(GameObject decal, Vector3 position, Vector3 normal, Transform parent)
    {
        if (mDecals.Count == 25)
        {
            GameObject go = mDecals[0];
            mDecals.Remove(go);
            GameObject.Destroy(go);
        }
        
        mDecals.Add(GameObject.Instantiate(decal, position, Quaternion.LookRotation(normal), parent));
    }
}