using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAim : MonoBehaviour
{
    public Transform PlayerTransform;
    public PlayerMovement playerMovementScript;
    public Rigidbody rb;
    private Vector3 lookDir = Vector3.zero;
    private Vector3 old_mouse_position = Vector3.zero;
    public float coolDownTime = 1f, CoolDownTimer = 0;
    public float lerpTime = 10f;
    public Vector3 moveDir;

    void Start(){
        rb = gameObject.GetComponent<Rigidbody>();
        playerMovementScript = gameObject.GetComponent<PlayerMovement>();
    }
    void Update()
    {  
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit cameraRayHit;
        if (Physics.Raycast(cameraRay, out cameraRayHit)){
            Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
            moveDir = (targetPosition-transform.position).normalized;
        }

    
        if(Input.GetMouseButton(1)){
            //PlayerTransform.Rotate(new Vector3(0, Vector3.SignedAngle(PlayerTransform.forward, lookDir, new Vector3(0,1,0)), 0));
            lookDir = Vector3.Lerp(lookDir, moveDir, lerpTime * Time.deltaTime);
            PlayerTransform.forward = lookDir;
        }else{
            Vector3 moveDir = playerMovementScript.moveDir;
            if(moveDir != Vector3.zero) lookDir = Vector3.Lerp(lookDir, moveDir, lerpTime * Time.deltaTime);
            PlayerTransform.forward = lookDir;
        }
    }

}

