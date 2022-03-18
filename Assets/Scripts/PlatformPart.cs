using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPart : MonoBehaviour
{

    [SerializeField]
    private int maxPosSamlples;

    private Rigidbody rb;

    private bool activated;

    private Stack<Vector3> positions = new Stack<Vector3>();

    private Stack<Quaternion> rotations = new Stack<Quaternion>();

    private Vector3 targetPos;

    private Quaternion targetRot;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Activate()
    {
        activated = true;
    }

    private void Update()
    {
        if (activated && TimeState.instance.globalTimeScale < 0F && positions.Count > 0)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10F);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 10F);
        }
    }

    private void FixedUpdate()
    {
        if(activated)
        {
            if(TimeState.instance.globalTimeScale > 0F && positions.Count < maxPosSamlples)
            {
                rb.isKinematic = false;

                positions.Push(transform.position);

                rotations.Push(transform.rotation);
            } else if(TimeState.instance.globalTimeScale < 0F && positions.Count > 0)
            {
                rb.isKinematic = true;

                targetPos = positions.Pop();

                targetRot = rotations.Pop();
            }
        }
    }

}
