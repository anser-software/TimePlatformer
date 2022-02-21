using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeState : MonoBehaviour
{
    
    public static TimeState instance { get; private set; }

    public float globalTimeScale { get; private set; }

    [SerializeField]
    private float fastForwardFactor, timeChangeRate;

    private float targetTimeScale = 1F;

    private bool playing = true;

    private void Awake()
    {
        instance = this;
    }

    public void Play() 
    {
        playing = !playing;

        targetTimeScale = playing ? 1F : 0F;
    }

    public void Fast() 
    {
        targetTimeScale = fastForwardFactor;
    }

    public void Reverse()
    {
        targetTimeScale = -1F;
    }

    private void Update()
    {
        globalTimeScale = Mathf.MoveTowards(globalTimeScale, targetTimeScale, Time.deltaTime * timeChangeRate);
    }

}
