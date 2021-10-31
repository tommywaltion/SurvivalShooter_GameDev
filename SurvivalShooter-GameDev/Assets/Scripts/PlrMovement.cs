using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlrMovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform character;
    public Transform PlayerCam;

    public float speed = 12f;
    public float sprintSpeed = 1.5f;

    public float crouchSpeed = 2f;
    private Vector3 playerScale;
    private Vector3 crouchScale = new Vector3(0,0.5f,0);

    public float gravity = -9.81f;

    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool IsGrounded,crouching,sprinting;

    void Start() {
        playerScale = character.transform.localScale;
    }

    void Update()
    {
        //ground check
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //gravity check
        if(IsGrounded && velocity.y < 0){
            velocity.y = -2f;
        }
        //get movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //crouch
        crouching = Input.GetKey(KeyCode.LeftShift);
        //sprinting
        sprinting = Input.GetKey(KeyCode.LeftControl);
        //change size of player if crouching or not
        if(Input.GetKeyDown(KeyCode.LeftShift))
            StartCrouch();
        if(Input.GetKeyUp(KeyCode.LeftShift))
            StopCrouch();
        //getting the movement direction
        Vector3 move = transform.right * x + transform.forward * z;
        //moving the character
        if(crouching){ //if crouching, slow down
            controller.Move(move * speed * crouchSpeed * Time.deltaTime);
        }
        if(!crouching && sprinting){ //if springting
            controller.Move(move * speed * sprintSpeed * Time.deltaTime);
        }
        if(!crouching && !sprinting){ //if not crouching or sprinting
            controller.Move(move * speed * Time.deltaTime);
        }
        //Jump
        if(Input.GetAxis("Jump") == 1 && IsGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        //gravity 
        velocity.y += gravity * Time.deltaTime;
        //applied gravity
        controller.Move(velocity * Time.deltaTime);
    }

    private void StartCrouch(){ //change the player size to half if crouching and move down to make it looks like crouching
        character.transform.localScale = playerScale - crouchScale;
        character.transform.position = character.transform.position - crouchScale;
        PlayerCam.transform.position = new Vector3(PlayerCam.transform.position.x, PlayerCam.transform.position.y - 0.8f, PlayerCam.transform.position.z);
    }

    private void StopCrouch(){ //change player size to normal and move camera up to where it was
        character.transform.localScale = playerScale;
        character.transform.position = character.transform.position + crouchScale;
        PlayerCam.transform.position = new Vector3(PlayerCam.transform.position.x, PlayerCam.transform.position.y + 0.8f, PlayerCam.transform.position.z);
    }
}
