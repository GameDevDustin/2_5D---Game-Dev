using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int _numOfCoins;
    [SerializeField] private UI_Manager _uiManager;

    private void Start() { DoNullChecks(); }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Collectable") && other.name.ToLower().StartsWith("coin")) {
            Coins coinScript = other.GetComponent<Coins>();

            _numOfCoins += coinScript.GetNumOfCoins();
            _uiManager.UpdateCoinCount(_numOfCoins);
            coinScript.HideCollectable();
        }
    }

    private void DoNullChecks() {
        if (_uiManager == null) { Debug.LogError("PlayerInventory::DoNullChecks() _uiManager is NULL!");}
    }
}
