using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeState : MonoBehaviour
{
    
    public static TimeState instance { get; private set; }

    public float globalTimeScale { get; private set; }

    [SerializeField]
    private float fastForwardFactor, timeChangeRate;

    private float targetTimeScale;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Play();
    }

    public void Play() 
    {
        targetTimeScale = 1F;
    }

    public void Pause()
    {
        targetTimeScale = 0F;
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
