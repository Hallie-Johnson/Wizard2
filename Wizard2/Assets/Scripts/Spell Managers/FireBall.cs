using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float lifetime = 5f;  // Duration before the fireball is destroyed
    private float knockbackForce = 20f;   // Force to push the player backwards

    void Start()
    {
        // Destroy the fireball after 5 seconds if it doesn't collide with anything
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit something");
        // Check if the object collided with has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit player");
            // Get the player's Rigidbody component
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                // Calculate the knockback direction (away from the fireball)
                Vector3 knockbackDirection = collision.transform.position - transform.position;
                knockbackDirection.y = 0;  // Keep the force horizontal (no vertical force)
                knockbackDirection.Normalize();

                // Apply force to the player to push them backwards
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }
        } else if (collision.gameObject.CompareTag("Rictusempra"))
        {
            return;
        }

        // Destroy the fireball after collision
        Destroy(gameObject);
    }
}
