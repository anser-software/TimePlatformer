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
    private float speed, minRunVelocity;

    [SerializeField]
    private float platformInFrontCheckDistanceDown, platformInFrontCheckDistanceForward, 
        groundCheckDistance, nextPlatformCheckDistFwd, minJumpDistance, jumpForceDelay, enableFallAnimAirborneTime, jumpDecceleration, gravityStrength;

    [SerializeField]
    private LayerMask ground;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private int maxPositionSamples;

    private Rigidbody rb;

    private bool grounded = true;

    private float jumpTimer;

    private Vector3 currentJumpForce = Vector3.zero;

    private float currentGravity;

    private List<Vector3> playerPositions = new List<Vector3>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        FinishController.instance.OnBeginFinish += () =>
        {
            nextPlatformCheckDistFwd = 1F;
        };
    }

    private void Update()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);

        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance, ground.value))
        {
            grounded = true;

            onMovingPlatform = hit.collider.transform.GetComponent<Platform>() != null;
        }
        else
        {
            grounded = false;

            currentGravity += Time.deltaTime * 9.81F * gravityStrength;
        }


        if (jumpTimer > 0F)
            jumpTimer -= Time.deltaTime;

        if (!grounded && TimeState.instance.globalTimeScale < 0 && playerPositions.Count > 0)
        {
            transform.position = Vector3.Lerp(transform.position, playerPositions[playerPositions.Count - 1], Time.deltaTime * 10F);
        }
    }

    private void FixedUpdate()
    {

        if (!grounded && TimeState.instance.globalTimeScale < 0 && playerPositions.Count > 0) 
        {
            playerPositions.RemoveAt(playerPositions.Count - 1);
        }
        else
        {
            playerPositions.Add(transform.position);

            if (playerPositions.Count > maxPositionSamples)
            {
                playerPositions.RemoveAt(0);
            }

            var nextPlatformRayOrigin = transform.position + Vector3.right * nextPlatformCheckDistFwd;

            if (Physics.Raycast(nextPlatformRayOrigin, Vector3.down, platformInFrontCheckDistanceDown, ground.value))
            {
                //rb.AddForce(Vector3.right, ForceMode.Force);
                rb.velocity = Vector3.right * speed + Vector3.down * currentGravity + currentJumpForce;
            } else
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.down * currentGravity + currentJumpForce, Time.deltaTime * 5F);
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
    }

    private void Jump()
    {
        if (!grounded || jumpTimer > 0F)
            return;

        jumpTimer = 1F;

        DOTween.Sequence().SetDelay(jumpForceDelay).OnComplete(() => currentJumpForce = jumpForce);//rb.AddForce(jumpForce, ForceMode.Impulse));

        animator.SetTrigger("Jump");
    }

    private void OnCollisionStay(Collision collision)
    {
        if(Vector3.Dot(collision.contacts[0].normal, Vector3.up) > 0.5F)
            currentGravity = 0.2F;
    }

}
