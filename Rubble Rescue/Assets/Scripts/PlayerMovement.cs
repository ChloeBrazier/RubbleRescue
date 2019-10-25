using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Eddie Brazier
 *Rubble Rescue
 *October 20 2019
 */

public class PlayerMovement : PlayerPhysics
{
    
    //fields for max movespeed and jump takeoff speed
    public float moveSpeed = 7;
    public float jumpStartSpeed = 7;

    // Start is called before the first frame update
    void Start()
    {
        //initialize player position
        gameObject.transform.position = Vector2.zero;
    }

    /// <summary>
    /// calculates player's velocity during the current frame
    /// </summary>
    protected override void ComputeVelocity()
    {
        //create new vector for movement and set it to zero by default
        Vector2 move = Vector2.zero;

        //get horizontal movement input
        move.x = Input.GetAxis("Horizontal");

        //debug GetAxis for better understanding of what it does
        Debug.Log("move.x is " + move.x);

        //allow the player to jump if they are on the ground
        if(Input.GetButtonDown("Jump") && isGrounded == true)
        {
            //start the player's jump
            velocity.y = jumpStartSpeed;
        }
        else if(Input.GetButtonUp("Jump"))
        {
            //stop the jump if the jump key/button is released
            if(velocity.y > 0)
            {
                //reduce upward velocity when the player lets go of the jump button
                velocity.y *= 0.5f;
            }
        }

        //move the player horizontally based on max movespeed
        targetVelocity = move * moveSpeed;
    }
}
