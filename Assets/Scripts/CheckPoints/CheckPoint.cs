using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CheckPoint : MonoBehaviour
{

    private AudioSource mAudioSource;
    private Light mLight;
    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        mLight = transform.GetChild(0).GetComponent<Light>();
        if(SingletonCheckPoint.getInstance().getPosition().Equals(transform.position))
            mLight.color=Color.green;
    }

    private void ActivateCheckPoint()
    {
        mLight.color=Color.green;
        SingletonCheckPoint.getInstance().setPosition(transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCheckPoint();
            mAudioSource.Play();
        }
    }
}
