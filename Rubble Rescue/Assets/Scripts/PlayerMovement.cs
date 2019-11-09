using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Eddie Brazier
 *Rubble Rescue
 *October 20 2019
 */

public class PlayerMovement : PlayerPhysics
{
    //make this class a static instance
    public static PlayerMovement instance;

    //fields for max movespeed and jump takeoff speed
    public float moveSpeed = 7;
    public float jumpStartSpeed = 7;

    //field to control the direction the player is facing with right as the default
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        //initialize static instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

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

        //check for player direction using getaxis and flip player accordingly
        if(move.x > 0 && facingRight != true || move.x < 0 && facingRight == true)
        {
            FlipPlayer();
        }

        //debug GetAxis for better understanding of what it does
        //Debug.Log("move.x is " + move.x);

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
                velocity.y *= 0.3f;
            }
        }

        //move the player horizontally based on max movespeed
        targetVelocity = move * moveSpeed;
    }

    /// <summary>
    /// method that flips the player in the direction they are moving towards
    /// </summary>
    private void FlipPlayer()
    {
        //set the right facing bool to the opposite of its current state
        facingRight = facingRight ? false : true;

        //get local scale and reverse it on the x axis
        Vector3 tempScale = gameObject.transform.localScale;
        tempScale.x *= -1;
        gameObject.transform.localScale = tempScale;
    }

    /// <summary>
    /// applies an upward force to the player
    /// </summary>
    public void MoveUpward(float upwardForce)
    {
        //apply the passed-in force to the player
        velocity.y += upwardForce;
    }
}
