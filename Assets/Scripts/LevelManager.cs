using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Vector3 _playerStartPosition;
    [SerializeField] private Vector3 _playerFinishPosition;


    private void Start() { DoNullChecks(); }

    public Vector3 GetPlayerStartPosition() { return _playerStartPosition; }
    public Vector3 GetPlayerFinishPosition() { return _playerFinishPosition;}
    
    private void DoNullChecks() {
       if (_playerStartPosition == Vector3.zero) { Debug.Log("LevelManager::DoNullChecks() _playerStartPosition = Vector3.zero!"); }
       if (_playerFinishPosition == Vector3.zero) { Debug.Log("LevelManager::DoNullChecks _playerFinishPosition = Vector3.zero!"); }
   }
}
