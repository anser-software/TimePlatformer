using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlatformTriggered : Platform
{

    private bool activated;

    void Start()
    {
        currentTime = offset;

        correctPosition = transform.position = pathCreator.path.GetPointAtTime(0F, EndOfPathInstruction.Stop);

        defaultMat = meshRenderer.sharedMaterial;
    }

    public void Activate()
    {
        activated = true;

        Debug.Log("ACTIVATE");
    }

    private void Update()
    {
        if (activated)
            Move();
    }

    private void Move()
    {
        currentTime += Time.deltaTime * speed * TimeState.instance.globalTimeScale;

        pathProgress = curve.Evaluate(currentTime);

        transform.position = pathCreator.path.GetPointAtTime(pathProgress, EndOfPathInstruction.Stop);
    }
}
