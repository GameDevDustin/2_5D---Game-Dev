using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int _numOfLives;
    [SerializeField] private int _defaultNumOfLives;
    [SerializeField] private int _numOfCoins;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private UI_Manager _uiManager;
    [SerializeField] private LevelManager _levelManager;

    private void Start() {
        DoNullChecks();
        _uiManager.UpdateLivesCount(_numOfLives);
        _uiManager.UpdateCoinCount(_numOfCoins);
    }

    private void FixedUpdate() { if (transform.position.y < -50f) { OnDeath(); } }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Collectable") && other.name.ToLower().StartsWith("coin")) {
            Coins coinScript = other.GetComponent<Coins>();

            _numOfCoins += coinScript.GetNumOfCoins();
            _uiManager.UpdateCoinCount(_numOfCoins);
            coinScript.HideCollectable();
        }
    }

    private void OnDeath() {
        _numOfLives -= 1;
        _uiManager.UpdateLivesCount(_numOfLives);

        if (_numOfLives <= 0) { //Show Game Over
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            transform.position = _levelManager.GetPlayerStartPosition();
            _playerController.DisableMovement();
        }else { //Respawn player at start of level
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(RespawnPlayer());
        }
    }

    private IEnumerator RespawnPlayer() {
        _playerController.DisableMovement();
        transform.position = _levelManager.GetPlayerStartPosition();
        yield return new WaitForSeconds(3f);
        transform.GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        _playerController.EnableMovement();
    }

    private void DoNullChecks() {
        if (_playerController == null) { Debug.LogError("PlayerInventory::DoNullChecks() _playerController is NULL!"); }
        if (_uiManager == null) { Debug.LogError("PlayerInventory::DoNullChecks() _uiManager is NULL!"); }
        if (_levelManager == null) { Debug.LogError("PlayerInventory::DoNullChecks() _levelManager is NULL!"); }
        if (_defaultNumOfLives == 0) { _defaultNumOfLives = 1; Debug.LogError("_defaultNumOfLives = 0! Set to 1."); }
        if (_numOfLives == 0) { _numOfLives = _defaultNumOfLives; Debug.Log("PlayerInventory::DoNullChecks() _numOfLives = 0! Set to _defaultNumOfLives.");}
    }
}
