using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = System.Action;
public class FinishController : MonoBehaviour
{
    
    public static FinishController instance { get; private set; }

    public int currentMultiplier { get { return currentPlatformCount > 0 ? multiplierPerPlatform[currentPlatformCount - 1] : 0; } }

    public Action OnBeginFinish;

    [SerializeField]
    private GameObject[] platforms;

    [SerializeField]
    private Vector3[] halfExtentsOvelapCheckPerPlatform;

    [SerializeField]
    private Transform initialPlacement;

    [SerializeField]
    private float targetZ, zRange, correctThreshold, platformMoveSpeed, xStepPerPlatform;

    [SerializeField]
    private int[] multiplierPerPlatform;

    [SerializeField]
    private AnimationCurve platformMovementCurve;

    [SerializeField]
    private bool snap;

    [SerializeField]
    private int snapStep;

    private GameObject currentPlatform = null;

    private bool active;

    private int currentPlatformCount = 0;

    private int currentPlatformIndex;

    private float currentOffset;

    private void Awake()
    {
        instance = this;
    }

    public void Begin()
    {
        OnBeginFinish?.Invoke();

        active = true;

        NewPlatform();
    }

    private void Update()
    {
        if (!active || currentPlatform == null)
            return;

        var currentPlatformPos = currentPlatform.transform.position;

        var time = Time.time * platformMoveSpeed;

        var pathProgress = platformMovementCurve.Evaluate(1F - 2F * Mathf.Abs(time + currentOffset - Mathf.Floor(time + currentOffset) - 0.5F));

        var actualZ = Mathf.Lerp(-zRange, zRange, pathProgress);

        currentPlatformPos.z = targetZ + (snap ? Mathf.Round(actualZ / snapStep) * snapStep : actualZ);

        currentPlatform.transform.position = currentPlatformPos;

        if(Input.GetMouseButtonDown(0))
        {
            if (CheckCorrectness() == false)
                return;

            if(currentPlatformCount < multiplierPerPlatform.Length)
            {
                NewPlatform();
            } else
            {
                if (currentPlatform != null)
                {
                    currentPlatform.layer = LayerMask.NameToLayer("Default");
                    currentPlatform = null;
                }

                GameManager.instance.Win();
            }
        }
    }

    private void NewPlatform()
    {
        if(currentPlatform != null)
        {
            currentPlatform.layer = LayerMask.NameToLayer("Default");
        }

        currentPlatformIndex = Random.Range(0, platforms.Length);

        currentPlatform = Instantiate(platforms[currentPlatformIndex], initialPlacement.position + Vector3.right * xStepPerPlatform * currentPlatformCount, Quaternion.identity);

        currentOffset = Random.Range(0F, 1F);

        currentPlatformCount++;
    }

    private bool CheckCorrectness()
    {
        if(currentPlatformCount == 1 || Physics.OverlapBox(currentPlatform.transform.position, halfExtentsOvelapCheckPerPlatform[currentPlatformIndex], Quaternion.identity).Length > 1)
        {
            Debug.Log("CORRECT");
            return true;
        } else
        {
            active = false;
            GameManager.instance.Win();

            return false;
        }
    }

}
