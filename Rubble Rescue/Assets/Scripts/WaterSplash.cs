using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Eddie Brazier
 *Rubble Rescue
 *October 29th 2019
 */

public class WaterSplash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //immediately destroy this object on a timer
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //activates when the gameobject enters a trigger state
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: add splashing animation when water hits a surface

        //TODO: replace find gameobject with tag use in water splash
        //destroy this object when it touches a surface
        if(collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
