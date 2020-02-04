using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public String scene;

    private void Start()
    {
        if (!SceneManager.GetActiveScene().name.Equals("Outside"))
            Cursor.lockState = CursorLockMode.None;
    }

    public void changeScene()
    {
        SceneManager.LoadScene(scene);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            changeScene();
            SingletonCheckPoint.getInstance().setPosition(new Vector3(-8, 0, 7));
        }
    }
}