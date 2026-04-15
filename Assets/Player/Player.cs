using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField]
    private float _health;
    [SerializeField]
    private TMP_Text _healthText;
    [SerializeField]
    private Transform _respawnPoint;

    private Rigidbody _rigidbody;
    private Coroutine _powerupCoroutine;
    private Transform _cameraTransform;
    private bool _isPowerUpActive = false;

    public void PickPowerUp()
    {
        if (_powerupCoroutine != null) StopCoroutine(_powerupCoroutine);
        _powerupCoroutine = StartCoroutine(StartPowerUp());
    }

    public void Dead()
    {
        _health -= 1;
        if (_health > 0)
        {
            if (_respawnPoint != null)
                transform.position = _respawnPoint.position;
            else
                Debug.LogWarning("Player: _respawnPoint is not assigned!", this);
        }
        else
        {
            _health = 0;
            Debug.Log("Lose");
        }
        UpdateUI();
    }

    private IEnumerator StartPowerUp()
    {
        _isPowerUpActive = true;
        OnPowerUpStart?.Invoke(); 
        yield return new WaitForSeconds(_powerupDuration);
        _isPowerUpActive = false;
        OnPowerUpStop?.Invoke();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Debug.Assert(_respawnPoint != null, "Player: _respawnPoint is not assigned!", this);
        Debug.Assert(_healthText != null, "Player: _healthText is not assigned!", this);

        if (Camera.main != null)
            _cameraTransform = Camera.main.transform;

        UpdateUI();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (_isPowerUpActive)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().Dead();
            }
        }
    }

    private void UpdateUI()
    {
    _healthText.text = "Health: " + _health;
    }

}