using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartUI : MonoBehaviour
{

    [SerializeField]
    private Text levelCounter;

    private void Start()
    {
        levelCounter.text = string.Format(levelCounter.text, GameManager.instance.currentLevel);
    }

}
