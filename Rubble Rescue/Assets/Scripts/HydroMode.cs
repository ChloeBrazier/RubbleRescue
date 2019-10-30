using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Eddie Brazier
 *Rubble Rescue
 *October 29th 2019
 */

public class HydroMode : MonoBehaviour
{
    //field for max water tank amount (not constant for potential upgrades) and current water amount
    private float maxWater = 100.0f;
    private float currentWater;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // OnEnable is called when this script is enabled
    private void OnEnable()
    {
        //reset water tank to zero
        currentWater = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //make sure that max water is no greater than 100 percent
        if(currentWater > maxWater)
        {
            currentWater = maxWater;
        }

        //TODO: add water usage mechanics
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //fill water tank if the player is touching water
        if(other.gameObject.tag == "Water" && currentWater < maxWater)
        {
            currentWater += 10 * Time.deltaTime;
        }
    }

    private void OnGUI()
    {
        //display water tank percentage to the player
        GUI.color = Color.blue;
        GUI.skin.label.fontSize = 20;
        GUI.Label(new Rect(20, 20, 400, 200), "Water: " + string.Format("{0: 0.0}%", currentWater));
    }
}
