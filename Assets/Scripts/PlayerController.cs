using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0,20)]
    public float speed = 10f;
    [Range(0,20)]
    public float rotationSpeed = 10f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private float horizontalMove;
    private float verticalMove;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // On key down get move direction, move player and set look direction
        if(Input.anyKey)
        {
            horizontalMove = Input.GetAxis("Horizontal");
            verticalMove = Input.GetAxis("Vertical");
            
            moveDirection = new Vector3(horizontalMove, 0.0f, verticalMove);
            moveDirection *= speed;

            controller.Move(moveDirection * Time.deltaTime);

            if(moveDirection != Vector3.zero){
                // Creates a rotation with the target forward and up directions
                Quaternion rotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);
                // Lerp rotation, from current rotation to target rotation with set speed
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
