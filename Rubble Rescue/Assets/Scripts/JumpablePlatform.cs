using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Eddie Brazier
 *Rubble Rescue
 *November 9th 2019
 */

public class JumpablePlatform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: MAKE THIS WORK
    }

    //activates when a collision occurs with the object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO: replace tag usage
        //check if colliding with the player
        if (collision.gameObject.tag == "Player")
        {
            //check if player's y velocity is positive
            if (PlayerMovement.instance.velocity.y >= 0)
            {
                //disable this object's collider
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    //activates after a collision with this object
    private void OnCollisionExit2D(Collision2D collision)
    {
        //re-enable this object's collider when a collision is over
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
