using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform target;

    private Vector3 offset;

    private void Start()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        if (PlayerMove.instance.onMovingPlatform)
            return;

        var targetPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
    }

    private void Update()
    {
        if (!PlayerMove.instance.onMovingPlatform)
            return;

        var targetPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
    }

}
