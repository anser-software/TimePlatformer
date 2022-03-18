using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugTimeScale : MonoBehaviour
{

    [SerializeField]
    private Text timeScaleText;

    private void Update()
    {
        timeScaleText.text = TimeState.instance.globalTimeScale.ToString();
    }

}
