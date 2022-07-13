using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour {
    [SerializeField] private TMP_Text _txtCoinCount;

    private void Start() { DoNullChecks(); }
    public void UpdateCoinCount(int numOfCoins) { _txtCoinCount.text = numOfCoins.ToString(); }
    private void DoNullChecks() {
        if (_txtCoinCount == null) { Debug.LogError("UI_Manager::DoNullChecks() _txtCoinCount is NULL!");}
    }
}
