using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
public class Platform : MonoBehaviour
{

    [SerializeField]
    private PathCreator pathCreator;

    [SerializeField]
    private AnimationCurve curve;

    [SerializeField]
    private float speed;

    [SerializeField, Range(0F, 1F)]
    private float offset;

    private float pathProgress, currentTime;

    private void Start()
    {
        currentTime = offset;
    }

    private void Update()
    {
        currentTime += Time.deltaTime * speed * TimeState.instance.globalTimeScale;

        pathProgress = curve.Evaluate(1F - 2F * Mathf.Abs(currentTime - (float)System.Math.Truncate(currentTime) - 0.5F));

        transform.position = pathCreator.path.GetPointAtTime(pathProgress, EndOfPathInstruction.Stop);
    }

}