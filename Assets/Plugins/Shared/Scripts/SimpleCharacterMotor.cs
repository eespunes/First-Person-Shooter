using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterMotor : MonoBehaviour
{
    public CursorLockMode cursorLockMode = CursorLockMode.Locked;
    public bool cursorVisible = false;
    [Header("Movement")]
    public float walkSpeed = 2;
    public float runSpeed = 4;
    public float gravity = 9.8f;
    [Space]
    [Header("Look")]
    public Transform cameraPivot;
    public float lookSpeed = 45;
    public bool invertY = true;
    [Space]
    [Header("Smoothing")]
    public float movementAcceleration = 1;

    CharacterController _controller;
    Vector3 _movement, _finalMovement;
    float _speed;
    Quaternion _targetRotation, _targetPivotRotation;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = cursorLockMode;
        Cursor.visible = cursorVisible;
        _targetRotation = _targetPivotRotation = Quaternion.identity;
    }

    void Update()
    {
        UpdateTranslation();
        UpdateLookRotation();
    }

    void UpdateLookRotation()
    {
        var x = Input.GetAxis("Mouse Y");
        var y = Input.GetAxis("Mouse X");

        x *= invertY ? -1 : 1;
        _targetRotation = transform.localRotation * Quaternion.AngleAxis(y * lookSpeed * Time.deltaTime, Vector3.up);
        _targetPivotRotation = cameraPivot.localRotation * Quaternion.AngleAxis(x * lookSpeed * Time.deltaTime, Vector3.right);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, _targetRotation, Time.deltaTime * 15);
        cameraPivot.localRotation = Quaternion.Slerp(cameraPivot.localRotation, _targetPivotRotation, Time.deltaTime * 15);
    }

    void UpdateTranslation()
    {
        if (_controller.isGrounded)
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            var run = Input.GetKey(KeyCode.LeftShift);

            var translation = new Vector3(x, 0, z);
            _speed = run ? runSpeed : walkSpeed;
            _movement = transform.TransformDirection(translation * _speed);
        }
        else
        {
            _movement.y -= gravity * Time.deltaTime;
        }
        _finalMovement = Vector3.Lerp(_finalMovement, _movement, Time.deltaTime * movementAcceleration);
        _controller.Move(_finalMovement * Time.deltaTime);
    }
}
