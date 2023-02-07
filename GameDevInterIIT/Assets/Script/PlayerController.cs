using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform followTarget, foot;
    public Rigidbody rb;
    public Vector3 input = Vector3.zero;
    public Vector3 moveDir = Vector3.zero;

    // public Transform playerCamera;
    // public float turnCalmTime = 0.1f;
    // public float turnCalmVelocity;

    public float moveSpeed = 10f;
    public float rotateSpeed = 5f;
    public float jumpSpeed = 10f;
    public Vector3 followTargetRotation;
    public float minAngle = -60f, maxAngle = 85f;
    public bool isGrounded = true, jump = false, sprinting = false, wasJumping = false;
    public float footOverLapSphereRadius = 0.1f;
    public float lerpConstant = 10f;
    public float sprintMultiplier = 1.5f;
    Animator playerAnimator;

    LayerMask mask;
    void Start()
    {
        followTargetRotation = Vector3.zero;
        rb = gameObject.GetComponent<Rigidbody>();
        mask = LayerMask.GetMask("Map");
        playerAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.z = Input.GetAxisRaw("Vertical");
        input.Normalize();
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        followTargetRotation.y += mouseX * rotateSpeed * Time.deltaTime;
        followTargetRotation.x -= mouseY * rotateSpeed * Time.deltaTime;    
        followTargetRotation.x = Mathf.Clamp(followTargetRotation.x, minAngle, maxAngle);

        if(Input.GetKeyDown("space") && isGrounded){
            jump = true;
        }

        if(Input.GetKeyDown("left shift") && isGrounded){
            sprinting = true;
        }

        if(Input.GetKeyUp("left shift")){
            sprinting = false;
        }

        
        // if(input.magnitude >= 0.1f){
        //     float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
        //     float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
        //     transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //     // cC.Move(input * moveSpeed * Time.deltaTime);
        // }
    }

    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(foot.position, footOverLapSphereRadius, mask);

        followTarget.eulerAngles = followTargetRotation;
        moveDir = (followTarget.forward * input.z + followTarget.right * input.x);
        moveDir.y = 0;
        moveDir.Normalize();

        if(moveDir != Vector3.zero){
            Vector3 targetRotation = new Vector3(moveDir.x, transform.forward.y, moveDir.z);
            Quaternion followTargetRotation = followTarget.rotation;
            transform.forward = Vector3.Lerp(transform.forward, targetRotation, lerpConstant * Time.fixedDeltaTime);
            followTarget.rotation = followTargetRotation;
        }

        Vector3 vel = moveDir * moveSpeed * Time.fixedDeltaTime * (sprinting ? sprintMultiplier : 1);

        if(isGrounded && jump){
            wasJumping = true;
            vel.y = jumpSpeed;
            jump = false;
        }else vel.y = rb.velocity.y;

        if(vel.y > 0){
            if(!wasJumping){
                vel.y *= -1;
            }
        }

        rb.velocity = vel;
        if (wasJumping){
            playerAnimator.SetBool("IsRunning", false);
            playerAnimator.SetBool("IsWalkingForward", false);
            playerAnimator.SetBool("IsJumping", true);
        }
        else
        {
            playerAnimator.SetBool("IsJumping", false);
        }
        if (sprinting && input.magnitude >= 0.1f)
        {
            playerAnimator.SetBool("IsRunning", true);
            playerAnimator.SetBool("IsWalkingForward", false);
            Debug.Log("running");
        }
        else
        {
            playerAnimator.SetBool("IsRunning", false);
            if (input.magnitude >= 0.1f)
            {
                playerAnimator.SetBool("IsWalkingForward", true);

            }
            else
            {
                playerAnimator.SetBool("IsWalkingForward", false);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Map")){
            wasJumping = false;
        }
    }

}
