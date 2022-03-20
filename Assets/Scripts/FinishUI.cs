using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishUI : MonoBehaviour
{

    [SerializeField]
    private GameObject briefParent, loseScreen, winScreen;

    [SerializeField]
    private Text score, multiplier;

    private string scoreString, multiplierString;

    private bool activated;

    private void Start()
    {
        scoreString = score.text;

        multiplierString = multiplier.text;

        GameManager.instance.OnCompleted += isWin =>
        {
            briefParent.SetActive(true);
            activated = true;

            winScreen.SetActive(isWin);
            loseScreen.SetActive(!isWin);
        };
    }

    private void Update()
    {
        if (!activated)
            return;

        score.text = string.Format(scoreString, (GameManager.instance.score * FinishController.instance.currentMultiplier).ToString());

        multiplier.text = string.Format(multiplierString, (FinishController.instance.currentMultiplier).ToString());
    }

}
