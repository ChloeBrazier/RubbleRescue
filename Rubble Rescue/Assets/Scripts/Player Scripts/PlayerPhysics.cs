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

    //protected fields for movement input from outside classes
    protected Vector2 targetVelocity;

    //fields to check if the player is grounded
    public bool isGrounded;
    protected Vector2 groundNormal;

    //fields for player movement
    protected Rigidbody2D playerBody;
    public Vector2 velocity;
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

    private void Awake()
    {
        //initialize gravity with a value of 1
        gravityScale = 6f;

        //initialize ground normals
        minGroundNormalY = 0.65f;

        //initialize hit buffer array and buffer list
        hitBuffer = new RaycastHit2D[16];
        bufferList = new List<RaycastHit2D>(16);
    }

    // Start is called before the first frame update
    void Start()
    {
        //set contact filter to ignore triggers
        contactFilter.useTriggers = false;

        //set contact filter to use the project's physics2D settings for collision checking
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        //reset velocity before calculation
        targetVelocity = Vector2.zero;
        ComputeVelocity();   
    }

    /// <summary>
    /// overridden method used to calculate velocity
    /// </summary>
    protected virtual void ComputeVelocity()
    {

    }

    private void FixedUpdate()
    {
        //simulate gravity by moving the player down based on gravity scale
        velocity += gravityScale * Physics2D.gravity * Time.deltaTime;

        //move the player horizontally based on input
        velocity.x = targetVelocity.x;

        //set isGrounded to false before checking for collisions
        isGrounded = false;

        //change the player's position based on velocity
        Vector2 position = velocity * Time.deltaTime;

        //store movement along the ground (vector perpendicular to ground normal)
        Vector2 groundMovement = new Vector2(groundNormal.y, -groundNormal.x);

        //move player along the ground based on the ground movement vector
        Vector2 move = groundMovement * position.x;

        //apply x-axis movement to player rigidbody
        Movement(move, false);

        //calculate downward movement
        move = Vector2.up * position.y;

        //apply y-axis movement to player rigidbody
        Movement(move, true);
    }

    /// <summary>
    /// method that applies custom physics to the player's rigidbody
    /// </summary>
    /// <param name="move"> the vector2 that represents the direction of movement </param>
    /// <param name="verticalMovement"> bool to determine if movement is occuring on the y-axis </param>
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
