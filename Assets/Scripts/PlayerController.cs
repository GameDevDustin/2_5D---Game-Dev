using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private CharacterController _controller;
    private InputActions _inputActions;
    [SerializeField] private float _playerSpeed;

    private void OnEnable() {
        _inputActions = new InputActions();
        _inputActions.Player.Enable();
        _inputActions.Player.Movement.performed += MovementOnperformed;
    }

    private void OnDisable() {
        _inputActions.Player.Movement.performed -= MovementOnperformed;
    }

    void Start() {
        _controller = GetComponent<CharacterController>();
        DoNullChecks();
    }

    private void MovementOnperformed(InputAction.CallbackContext context) {
        
    }

    void Update() {
        Vector2 moveDirection = _inputActions.Player.Movement.ReadValue<Vector2>();
        float moveX = moveDirection.x;
        
        MovePlayer(new Vector3(moveX, 0f, 0f));
    }

    private void MovePlayer(Vector3 moveDirection) {
        transform.Translate(moveDirection * Time.deltaTime * _playerSpeed);
    }

    private void DoNullChecks() {
        if (_playerSpeed <= 0) { 
            _playerSpeed = 1; 
            Debug.Log("PlayerController::DoNullChecks() _playerSpeed <= 0! Set to 1.");
        }
    }
}
