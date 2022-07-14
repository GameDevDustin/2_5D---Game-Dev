using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Vector3 _playerStartingPosition;


    private void Start() {
        DoNullChecks();
    }

    public Vector3 GetPlayerStartPosition() { return _playerStartingPosition; }

   private void DoNullChecks() {
       if (_playerStartingPosition == Vector3.zero) { Debug.Log("LevelManager::DoNullChecks() _playerStartingPosition = Vector3.zero!"); }
   }
}
