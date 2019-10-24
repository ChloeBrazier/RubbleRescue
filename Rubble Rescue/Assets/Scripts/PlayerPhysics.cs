using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Eddie Brazier
 *Rubble Rescue
 *October 23 2019
 */

public class PlayerPhysics : MonoBehaviour
{
    //field for the scale of gravity
    [SerializeField]
    private float gravityScale;

    //field for minimum ground normal for collision checks
    [SerializeField]
    private float minGroundNormalY;

    //protected fields to check if the player is grounded
    protected bool isGrounded;
    protected Vector2 groundNormal;

    //protected fields for player movement
    protected Rigidbody2D playerBody;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer;
    protected List<RaycastHit2D> bufferList;

    //protected fields for collision checks
    protected const float MIN_MOVE_DISTANCE = 0.001f;
    protected const float COLLISION_RADIUS = 0.01f;

    private void OnEnable()
    {
        //get player's rigidbody
        playerBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //initialize gravity with a value of 1
        gravityScale = 1f;

        //initialize ground normals
        minGroundNormalY = 0.65f;

        //set contact filter to ignore triggers
        contactFilter.useTriggers = false;

        //set contact filter to use the project's physics2D settings for collision checking
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;

        //initialize hit buffer array and buffer list
        hitBuffer = new RaycastHit2D[16];
        bufferList = new List<RaycastHit2D>(16);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //simulate gravity by moving the player down
        velocity += gravityScale * Physics2D.gravity * Time.deltaTime;

        //set isGrounded to false before checking for collisions
        isGrounded = false;

        //change the player's position based on velocity
        Vector2 position = velocity * Time.deltaTime;

        //calculate downward movement
        Vector2 move = Vector2.up * position.y;

        //apply movement to player rigidbody
        Movement(move, true);
    }

    /// <summary>
    /// method that applies custom physics to the player's rigidbody
    /// </summary>
    /// <param name="move"> the vector2 that represents the direction of movement </param>
    /// <param name="verticalMovement"></param>
    void Movement(Vector2 move, bool verticalMovement)
    {
        //save the magnitude of the distance of player movement for collision checking
        float collisionDistance = move.magnitude;

        //only check for collisions if the magnitude of the movement vector is greater than the min distance
        if(collisionDistance > MIN_MOVE_DISTANCE)
        {
            //clear hit buffer list for new collision checks
            bufferList.Clear();

            //store all casts and use a radius to prevent the player from getting stuck in other colliders
            int collisionCount = playerBody.Cast(move, contactFilter, hitBuffer, collisionDistance + COLLISION_RADIUS);

            //add valid collisions to the buffer list
            for(int i = 0; i < collisionCount; i++)
            {
                bufferList.Add(hitBuffer[i]);
            }

            for(int i = 0; i < bufferList.Count; i++)
            {
                //check the normal of the raycast hit
                Vector2 bufferNormal = bufferList[i].normal;

                //check if the player hits the ground and set grounded state accordingly
                if(bufferNormal.y > minGroundNormalY)
                {
                    isGrounded = true;

                    if(verticalMovement == true)
                    {
                        groundNormal = bufferNormal;
                        bufferNormal.x = 0;
                    }
                }

                //get the difference between the velocity and current normal to determine if veloicty must be decreased
                float projection = Vector2.Dot(velocity, bufferNormal);
                if(projection < 0)
                {
                    velocity -= projection * bufferNormal;
                }

                //modify collision distance if it's less than the new calculated distance
                float modifiedDistance = bufferList[i].distance - COLLISION_RADIUS;
                collisionDistance = modifiedDistance < collisionDistance ? modifiedDistance : collisionDistance;
            }
        }

        //apply movement vector to player position
        playerBody.position += move.normalized * collisionDistance;
    }
}
