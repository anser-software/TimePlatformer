using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform target, finishPlacement;

    [SerializeField]
    private float finishTransitionDuration;

    private Vector3 offset;

    private bool finishCam;

    private void Start()
    {
        OnEnable();

        FinishController.instance.OnBeginFinish += SetFinish;
    }

    private void OnEnable()
    {
        offset = transform.position - target.position;
    }

    private void SetFinish()
    {
        finishCam = true;

        transform.DOMove(finishPlacement.position, finishTransitionDuration).SetEase(Ease.InOutSine);
        transform.DORotateQuaternion(finishPlacement.rotation, finishTransitionDuration).SetEase(Ease.InOutSine);
    }

    private void Update()
    {
        if (finishCam)
            return;

        var targetPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
    }

}
