using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour {
    [SerializeField] private TMP_Text _txtCoinCount;
    [SerializeField] private TMP_Text _txtLivesCount;

    private void Start() { DoNullChecks(); }
    public void UpdateCoinCount(int numOfCoins) { _txtCoinCount.text = numOfCoins.ToString(); }
    public void UpdateLivesCount(int numOfLives) { _txtLivesCount.text = numOfLives.ToString(); }
    private void DoNullChecks() {
        if (_txtCoinCount == null) { Debug.LogError("UI_Manager::DoNullChecks() _txtCoinCount is NULL!");}
    }
}
