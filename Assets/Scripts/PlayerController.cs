using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour {
    private CharacterController _controller;
    private InputActions _inputActions;
    [SerializeField] private Vector3 _playerVelocity;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _playerSpeed;
    [SerializeField] private bool _startJump;
    [SerializeField] private int _currNumOfJumps;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravityValue;

    private void OnEnable() {
        _inputActions = new InputActions();
        _inputActions.Player.Enable();
        _inputActions.Player.Movement.performed += MovementOnperformed;
        _inputActions.Player.Jump.performed += JumpOnperformed;
        _currNumOfJumps = 0;
    }

    private void OnDisable() {
        _inputActions.Player.Movement.performed -= MovementOnperformed;
        _inputActions.Player.Jump.performed -= JumpOnperformed;
    }

    private void MovementOnperformed(InputAction.CallbackContext context) {
        
    }
    
    private void JumpOnperformed(InputAction.CallbackContext context) { _startJump = true; _currNumOfJumps += 1; }

    void Start() {
        _controller = GetComponent<CharacterController>();
        DoNullChecks();
    }
    
    void Update() {
        Vector2 moveDirection = _inputActions.Player.Movement.ReadValue<Vector2>();
        float moveX = moveDirection.x;
        
        _isGrounded = _controller.isGrounded;
        
        if (_isGrounded) { _currNumOfJumps = 0;}
        if (_isGrounded && _playerVelocity.y < 0) { _playerVelocity.y = 0; }
        
        MovePlayer(new Vector3(moveX, 0f, 0f));
    }

    private void MovePlayer(Vector3 moveDirection) {
        _controller.Move(moveDirection * Time.deltaTime * _playerSpeed);
        
        if (_startJump && _isGrounded) { //first jump
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        } else if (_startJump && !_isGrounded && _currNumOfJumps < 2) { //double jump
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -2.5f * _gravityValue);
        }
        
        if (_startJump) { _startJump = false; }

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }
    
    private void DoNullChecks() {
        if (_playerSpeed <= 0) { _playerSpeed = 1; Debug.Log("PlayerController::DoNullChecks() _playerSpeed <= 0! Set to 1."); }
        if (_jumpHeight <= 0) { _jumpHeight = 1; Debug.Log("PlayerController::DoNullChecks() _jumpHeight <= 0! Set to 1."); }
        if (_gravityValue == 0) { _gravityValue = -100; Debug.Log("PlayerController::DoNullChecks() _gravityValue <= 0! Set to -100."); }
    }
}
