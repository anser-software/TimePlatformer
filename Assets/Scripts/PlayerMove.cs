using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance { get; private set; }

    public bool onMovingPlatform { get; private set; }

    [SerializeField]
    private Vector3 movement;

    [SerializeField]
    private float floorCheckDistance;

    private Rigidbody rb;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position + movement.normalized, Vector3.down, floorCheckDistance))
        {
            rb.AddForce(movement, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.parent = collision.collider.transform;

        onMovingPlatform = collision.collider.transform.GetComponent<Platform>() != null;     
    }

}
