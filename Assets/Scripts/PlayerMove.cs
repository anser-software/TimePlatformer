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
    private Vector3 jumpForce;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float platformInFrontCheckDistanceDown, platformInFrontCheckDistanceForward, 
        groundCheckDistance, minJumpDistance, jumpForceDelay, enableFallAnimAirborneTime, jumpDecceleration, gravityStrength;

    [SerializeField]
    private LayerMask ground;

    [SerializeField]
    private Animator animator;

    private Rigidbody rb;

    private bool grounded = true;

    private float jumpTimer, fallTimer, airborneTimer;

    private Vector3 currentJumpForce = Vector3.zero;

    private float currentGravity;

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

            airborneTimer = 0F;
        }
        else
        {
            grounded = false;

            airborneTimer += Time.deltaTime;

            currentGravity += Time.deltaTime * 9.81F * gravityStrength;

            if (jumpTimer <= 0F && fallTimer <= 0F && airborneTimer > enableFallAnimAirborneTime)
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
        var nextPlatformRayOrigin = transform.position + Vector3.right;

        if (Physics.Raycast(nextPlatformRayOrigin, Vector3.down, platformInFrontCheckDistanceDown, ground.value))
        {
            //rb.AddForce(Vector3.right, ForceMode.Force);
            rb.velocity = Vector3.right * speed + Vector3.down * currentGravity + currentJumpForce;
        }

        var jumpRayOrigin = transform.position + Vector3.right * platformInFrontCheckDistanceForward;

        if (Physics.Raycast(jumpRayOrigin, Vector3.down, out RaycastHit hit, platformInFrontCheckDistanceDown))
        {
            if ((hit.point - jumpRayOrigin).sqrMagnitude < minJumpDistance * minJumpDistance)
            {
                Jump();
            }
        }

        currentJumpForce = Vector3.Lerp(currentJumpForce, Vector3.zero, Time.deltaTime * jumpDecceleration);
        //rb.velocity = rb.velocity.normalized * speed;
    }

    private void Jump()
    {
        if (!grounded || jumpTimer > 0F)
            return;

        jumpTimer = 0.6F;

        DOTween.Sequence().SetDelay(jumpForceDelay).OnComplete(() => currentJumpForce = jumpForce);//rb.AddForce(jumpForce, ForceMode.Impulse));

        animator.SetTrigger("Jump");
    }

    private void OnCollisionStay(Collision collision)
    {
        if(Vector3.Dot(collision.contacts[0].normal, Vector3.up) > 0.5F)
            currentGravity = 0.2F;
    }

}
