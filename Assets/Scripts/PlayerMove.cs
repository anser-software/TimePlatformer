using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance { get; private set; }

    public bool onMovingPlatform { get; private set; }

    [SerializeField]
    private Vector3 movement, jumpForce;

    [SerializeField]
    private float platformInFrontCheckDistance, groundCheckDistance, minJumpDistance;

    [SerializeField]
    private LayerMask ground;

    private Rigidbody rb;

    private bool grounded = true;

    private float jumpTimer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance, ground.value))
        {
            grounded = true;

            transform.parent = hit.collider.transform;

            onMovingPlatform = hit.collider.transform.GetComponent<Platform>() != null;
        } else
        {
            grounded = false;
        }

        if (jumpTimer > 0F)
            jumpTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        var rayOrigin = transform.position + movement.normalized;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, platformInFrontCheckDistance))
        {
            rb.AddForce(movement, ForceMode.Force);

            if ((hit.point - rayOrigin).sqrMagnitude < minJumpDistance * minJumpDistance)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        if (!grounded || jumpTimer > 0F)
            return;

        jumpTimer = 0.4F;

        rb.AddForce(jumpForce, ForceMode.Impulse);
    }


}
