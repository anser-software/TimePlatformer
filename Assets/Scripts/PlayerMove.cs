using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance { get; private set; }

    public bool onMovingPlatform { get; private set; }

    [SerializeField]
    private Vector3 movement, jumpForce;

    [SerializeField]
    private float platformInFrontCheckDistanceDown, platformInFrontCheckDistanceForward, groundCheckDistance, minJumpDistance, jumpForceDelay;

    [SerializeField]
    private LayerMask ground;

    [SerializeField]
    private Animator animator;

    private Rigidbody rb;

    private bool grounded = true;

    private float jumpTimer, fallTimer;

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

            //transform.parent = hit.collider.transform;

            onMovingPlatform = hit.collider.transform.GetComponent<Platform>() != null;

            animator.SetTrigger("Run");
        }
        else
        {
            grounded = false;


            if(jumpTimer <= 0F && fallTimer <= 0F)
            {
                Debug.Log("Not grounded; Fall");

                animator.SetTrigger("Fall");
                fallTimer = 0.8F;
            }
        }

        if (jumpTimer > 0F)
            jumpTimer -= Time.deltaTime;

        if (fallTimer > 0F)
            fallTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        var nextPlatformRayOrigin = transform.position + movement.normalized;

        if (Physics.Raycast(nextPlatformRayOrigin, Vector3.down, platformInFrontCheckDistanceDown))
        {
            rb.AddForce(movement, ForceMode.Force);
        }

        var jumpRayOrigin = transform.position + movement.normalized * platformInFrontCheckDistanceForward;

        if (Physics.Raycast(jumpRayOrigin, Vector3.down, out RaycastHit hit, platformInFrontCheckDistanceDown))
        {
            if ((hit.point - jumpRayOrigin).sqrMagnitude < minJumpDistance * minJumpDistance)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        if (!grounded || jumpTimer > 0F)
            return;

        jumpTimer = 0.6F;

        DOTween.Sequence().SetDelay(jumpForceDelay).OnComplete(() => rb.AddForce(jumpForce, ForceMode.Impulse));

        animator.SetTrigger("Jump");
    }


}
