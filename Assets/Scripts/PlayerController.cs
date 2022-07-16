using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour {
    private CharacterController _controller;
    private InputActions _inputActions;
    [SerializeField] private Vector3 _playerVelocity;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private bool _startJump;
    [SerializeField] private int _currNumOfJumps;
    [SerializeField] private bool _canWallJump;
    [SerializeField] bool _isWallJumping;
    [SerializeField] private float _gravityValue;
    [SerializeField] private bool _movementDisabled;
    private Vector3 _wallCollisionNormal;


    private void OnEnable() {
        _inputActions = new InputActions();
        _inputActions.Player.Enable();
        _inputActions.Player.Movement.performed += MovementOnperformed;
        _inputActions.Player.Jump.performed += JumpOnperformed;
        _currNumOfJumps = 0;
        _movementDisabled = false;
        _canWallJump = false;
        _isWallJumping = false;
    }
    
    private void OnDisable() {
        _inputActions.Player.Movement.performed -= MovementOnperformed;
        _inputActions.Player.Jump.performed -= JumpOnperformed;
    }

    private void MovementOnperformed(InputAction.CallbackContext context) { }

    private void JumpOnperformed(InputAction.CallbackContext context) {
        if (_canWallJump) { _canWallJump = false; _isWallJumping = true; //wall jump
        } else { _startJump = true; _currNumOfJumps += 1; transform.SetParent(null); } //start normal jump
    }

    void Start() { _controller = GetComponent<CharacterController>(); DoNullChecks(); }
    
    void FixedUpdate() { DeterminePlayerVelocity(); }

    private void DeterminePlayerVelocity() {
        Vector3 moveDirection = _inputActions.Player.Movement.ReadValue<Vector2>();
        _isGrounded = _controller.isGrounded;

        if (_isGrounded) {
            _currNumOfJumps = 0;
            _playerVelocity.x = moveDirection.x;
            _playerVelocity.x *= _playerSpeed;
            _canWallJump = false;
            _isWallJumping = false;
        } else if (_isWallJumping) { _playerVelocity.x = _wallCollisionNormal.x * 4; } //horizontal bounce when wall jumping
        
        if (_isGrounded && _playerVelocity.y < 0) { _playerVelocity.y = 0; } //_playerVelocity.y should never be < 0
        
        if (_isWallJumping && !_canWallJump) { _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -1.25f * _gravityValue); //wall jumping
        } else if (_startJump && _isGrounded) { _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue); //first jump
        } else if (_startJump && !_isGrounded && _currNumOfJumps < 2) { _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -2.5f * _gravityValue); } //double jump
        
        if (_startJump) { _startJump = false; }
        if (!_isGrounded && _wallCollisionNormal.y == -1f) { _playerVelocity.y = -1.5f; } //bounce off ceilings
        if (!_isGrounded) { _playerVelocity.y += _gravityValue * Time.deltaTime; } //apply gravity
        if (!_movementDisabled) { MovePlayer(_playerVelocity); }
    }
    
    private void MovePlayer(Vector3 moveVelocity) { _controller.Move(moveVelocity * Time.deltaTime); }

    public void EnableMovement() { _movementDisabled = false;}
    public void DisableMovement() { _movementDisabled = true; }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.ToLower().StartsWith("moving")) { transform.parent = other.transform; }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag.ToLower().StartsWith("moving")) { transform.parent = null; }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody rigidBody = hit.collider.attachedRigidbody;
        
        if (!_isGrounded) { _wallCollisionNormal = hit.normal; }
        
        if (!_isGrounded && hit.transform.CompareTag("JumpableWall")) {
            _canWallJump = true;
            StartCoroutine(EndIsWallJumping());
            return;
        }

        if (rigidBody != null && !rigidBody.isKinematic && hit.normal.y == 0f) {
            Vector3 pushDirection = new Vector3(hit.moveDirection.x * (_playerVelocity.x / 2), 0, 0);
            
            rigidBody.velocity = pushDirection;
        }
    }

    private IEnumerator EndIsWallJumping() {
        yield return new WaitForSeconds(0.15f);
        _isWallJumping = false;
    }

    private void DoNullChecks() {
        if (_playerSpeed <= 0) { _playerSpeed = 1; Debug.Log("PlayerController::DoNullChecks() _playerSpeed <= 0! Set to 1."); }
        if (_jumpHeight <= 0) { _jumpHeight = 1; Debug.Log("PlayerController::DoNullChecks() _jumpHeight <= 0! Set to 1."); }
        if (_gravityValue == 0) { _gravityValue = -100; Debug.Log("PlayerController::DoNullChecks() _gravityValue <= 0! Set to -100."); }
    }
}
