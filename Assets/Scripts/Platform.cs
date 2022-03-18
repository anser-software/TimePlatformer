using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
public class Platform : MonoBehaviour
{

    [SerializeField]
    private PathCreator pathCreator;

    [SerializeField]
    protected AnimationCurve curve;

    [SerializeField]
    protected bool loop;

    [SerializeField]
    protected float speed, correctPosThreshold;

    [SerializeField]
    protected Material correctMat;

    [SerializeField]
    protected MeshRenderer meshRenderer;

    [SerializeField, Range(0F, 1F)]
    protected float offset;

    protected float pathProgress, currentTime;

    protected Vector3 correctPosition;

    protected Material defaultMat;

    private void Start()
    {
        currentTime = offset;

        correctPosition = transform.position;

        defaultMat = meshRenderer.sharedMaterial;
    }

    private void Update()
    {
        Move();

        SetMaterial();
    }

    public virtual void CheckCorrectPos()
    {
        GameManager.instance.AddScore((transform.position - correctPosition).magnitude);
    }

    private void Move()
    {
        currentTime += Time.deltaTime * speed * TimeState.instance.globalTimeScale;

        if (loop)
        {
            pathProgress = curve.Evaluate(currentTime - Mathf.Floor(currentTime));
        }
        else
        {
            pathProgress = curve.Evaluate(1F - 2F * Mathf.Abs(currentTime - Mathf.Floor(currentTime) - 0.5F));
        }

        transform.position = pathCreator.path.GetPointAtTime(pathProgress, EndOfPathInstruction.Stop);
    }

    private void SetMaterial()
    {
        if ((transform.position - correctPosition).sqrMagnitude < correctPosThreshold * correctPosThreshold)
        {
            meshRenderer.sharedMaterial = correctMat;
        }
        else
        {
            meshRenderer.sharedMaterial = defaultMat;
        }
    }
}