using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Action OnPowerUpStart;
    public Action OnPowerUpStop;

    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _rotationSpeed = 10f;
    [SerializeField]
    private float _powerupDuration;

    private Rigidbody _rigidbody;
    private Coroutine _powerupCoroutine;
    private Transform _cameraTransform;

    public void PickPowerUp()
    {
        Debug.Log("1. Player: Fungsi PickPowerUp terpanggil!"); 
        if (_powerupCoroutine != null) StopCoroutine(_powerupCoroutine);
        _powerupCoroutine = StartCoroutine(StartPowerUp());
    }

    private IEnumerator StartPowerUp()
    {
        Debug.Log("2. Player: Mengirim sinyal OnPowerUpStart...");
        OnPowerUpStart?.Invoke(); 
        yield return new WaitForSeconds(_powerupDuration);
        Debug.Log("3. Player: Mengirim sinyal OnPowerUpStop...");
        OnPowerUpStop?.Invoke();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        if (Camera.main != null) 
        {
            _cameraTransform = Camera.main.transform;
        }
        
        HideAndLockCursor();
    }

    private void HideAndLockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (_cameraTransform == null) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            _rigidbody.linearVelocity = new Vector3(moveDirection.x * _speed, _rigidbody.linearVelocity.y, moveDirection.z * _speed);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
        else
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
        }
    }
}