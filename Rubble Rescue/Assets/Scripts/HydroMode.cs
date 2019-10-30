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

    //field for the speed at which water fills
    private float waterFillSpeed = 20f;

    //field for player movement script
    private PlayerPhysics playerPhysics;

    // Start is called before the first frame update
    void Start()
    {
        //initialize player movement script
        playerPhysics = gameObject.GetComponent<PlayerPhysics>();
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

        //slow down time when aiming
        if(Input.GetButton("Aim"))
        {
            Debug.Log("Aiming");

            //gradually slow down time to desired speed
            Time.timeScale *= 0.9f;
            if(Time.timeScale <= 0.3f)
            {
                Time.timeScale = 0.3f;
            }

            Debug.Log("time scale is " + Time.timeScale);
        }

        //speed time back up
        if(Input.GetButtonUp("Aim"))
        {
            //TODO: gradually speed time back up to real time
            Time.timeScale = 1f;
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        //fill water tank if the player is touching water
        if(other.gameObject.tag == "Water" && currentWater < maxWater)
        {
            currentWater += waterFillSpeed * Time.deltaTime;
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
