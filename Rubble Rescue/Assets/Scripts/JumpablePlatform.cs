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

        //check if player is above or below the platform
        if (PlayerManager.instance.footPos.y > transform.position.y)
        {
            Debug.Log("platform enabled");
            //enable collider on this object
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            //disable collider if player is below the platform
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
