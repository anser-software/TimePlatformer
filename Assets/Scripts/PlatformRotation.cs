using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotation : Platform
{

    [SerializeField]
    private Vector3 localRotationCenter, up;

    private Quaternion correctRotation;


    void Start()
    {
        currentTime = offset;

        correctPosition = transform.position;

        correctRotation = transform.rotation;

        defaultMat = meshRenderer.sharedMaterial;
    }

    void Update()
    {
        Rotate();

        SetMaterial();
    }

    private void Rotate()
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

        transform.position += transform.right * localRotationCenter.x + transform.up * localRotationCenter.y + transform.forward * localRotationCenter.z;

        transform.rotation = Quaternion.AngleAxis(pathProgress * 360F, up) * correctRotation;

        transform.position -= transform.right * localRotationCenter.x + transform.up * localRotationCenter.y + transform.forward * localRotationCenter.z;
    }

    public override void CheckCorrectPos()
    {
        var x = (transform.position - correctPosition).magnitude;
        var y = (transform.rotation.eulerAngles.normalized - correctRotation.eulerAngles.normalized).magnitude;
        GameManager.instance.AddScore(new Vector2(x, y).magnitude);
    }

    private void SetMaterial()
    {
        var x = (transform.position - correctPosition).magnitude;
        var y = (transform.rotation.eulerAngles.normalized - correctRotation.eulerAngles.normalized).magnitude;
        if (new Vector2(x, y).sqrMagnitude < correctPosThreshold * correctPosThreshold)
        {
            meshRenderer.sharedMaterial = correctMat;
        }
        else
        {
            meshRenderer.sharedMaterial = defaultMat;
        }
    }
}
