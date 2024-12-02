using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongifyFix : MonoBehaviour
{

    public bool enableSpongifyMovement;
    private float Speed = 6f;
    private float turnSmoothVelocity;
    private Vector3 smoothVelocity;
    private Vector3 movementInput;
    private Vector3 movementVelocity;
    private Rigidbody PlayerBody;

    public ThirdPersonMovement thirdPersonMovement; // Reference to the ThirdPersonMovement script
    public bool movement;

    void Start()
    {
        enableSpongifyMovement = false; 
        
        if (thirdPersonMovement != null)
        {
            // Access the "movement" boolean from ThirdPersonMovement
            bool isMoving = thirdPersonMovement.movement;
        }

        PlayerBody = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (thirdPersonMovement != null)
        {
            // Access the "movement" boolean from ThirdPersonMovement
            movement = thirdPersonMovement.movement;
        }


        if (movement)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            movementInput = (transform.forward * vertical + transform.right * horizontal).normalized;

            if (enableSpongifyMovement)
            {
                Vector3 targetMovement = (transform.forward * vertical + transform.right * horizontal) * Speed;
                movementVelocity = Vector3.SmoothDamp(movementVelocity, targetMovement, ref smoothVelocity, 0.1f);
                PlayerBody.MovePosition(PlayerBody.position + movementVelocity * Time.fixedDeltaTime);
            }
            else
            {
                movementVelocity = movementInput * Speed;
                Vector3 newVelocity = new Vector3(movementVelocity.x, PlayerBody.velocity.y, movementVelocity.z);
                PlayerBody.velocity = newVelocity;
            }
        } else
        {
            return;
        }
        


    }

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.CompareTag("Ground") || hit.gameObject.CompareTag("SpongifyTarget"))
        {
            enableSpongifyMovement = false;
        } else if (hit.gameObject.CompareTag("Spongify"))
        {
            enableSpongifyMovement = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

}
