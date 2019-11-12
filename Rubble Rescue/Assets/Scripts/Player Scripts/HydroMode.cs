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

    //fields for water spray speed and spray bool
    private float maxSprayCooldown = 1f;
    private float currentSprayCooldown = 1f;
    private bool canSpray = true;
    private float sprayForce = 100f;

    //fields for water ability usage
    private float hoseRate = 3f;
    private float jumpRate = 30f;

    //field for water prefab (testing)
    [SerializeField]
    private GameObject waterDrop;

    // Start is called before the first frame update
    void Start()
    {

    }

    // OnEnable is called when this script is enabled
    private void OnEnable()
    {
        //reset water tank to zero
        //currentWater = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //make sure that max water is no greater than 100 percent
        if(currentWater > maxWater)
        {
            currentWater = maxWater;
        }

        //check if the player is airborne and presses the ability button
        if(PlayerMovement.instance.isGrounded == false && Input.GetButtonDown("Ability"))
        {
            //move the player upward and decrease current water by 25 percent
            if(UseWater(jumpRate))
            {
                PlayerMovement.instance.MoveUpward(30f);
            }

            //TODO: animation event that spawns water when the player does a hydro jump
        }

        //slow down time when aiming
        if (Input.GetButton("Aim") && PlayerMovement.instance.isGrounded == true)
        {
            //TODO: make arm move when aiming

            //spray water in the direction of the mouse when the ability button is pressed
            if (Input.GetButton("Ability"))
            {
                //spray water or decrease spray speed float
                if(canSpray == true)
                {
                    //spray water
                    SprayWater();

                    //start spray cooldown
                    canSpray = false;
                }
            }

            //gradually slow down time to desired speed
            /*
            Time.timeScale *= 0.9f;
            if(Time.timeScale <= 0.3f)
            {
                Time.timeScale = 0.3f;
            }
            */
        }

        //speed time back up to full speed
        /*
        if(!Input.GetButton("Aim") && Time.timeScale < 1f)
        {
            //gradually speed time back up to real time
            Time.timeScale *= 1.1f;
            if (Time.timeScale >= 1f)
            {
                Time.timeScale = 1f;
            }
        }
        */

        //check if the player can't spray water
        if (canSpray == false)
        {
            //decrement spray cooldown
            currentSprayCooldown -= 0.2f;
        }
        
        //check if spray cooldown has been decremented to zero
        if (currentSprayCooldown < 0f)
        {
            //set canSpray to true
            canSpray = true;

            //reset cooldown float
            currentSprayCooldown = maxSprayCooldown;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //make sure the player is in hydro mode before filling the tank
        if(this.isActiveAndEnabled)
        {
            //TODO: AVOID TAG USAGE
            //fill water tank if the player is touching water
            if (other.gameObject.tag == "Water" && currentWater < maxWater)
            {
                currentWater += waterFillSpeed * Time.deltaTime;
            }
        }
       
    }

    private void OnGUI()
    {
        //display water tank percentage to the player
        GUI.color = Color.blue;
        GUI.skin.label.fontSize = 20;
        GUI.Label(new Rect(20, 20, 400, 200), "Water: " + string.Format("{0: 0.0}%", currentWater));
    }

    private void SprayWater()
    {
        //get mouse position in world space
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //spray water if there's enough water in the tank
        if(UseWater(hoseRate))
        {
            //spawn water drop
            GameObject droplet = Instantiate(waterDrop, gameObject.transform.position, Quaternion.identity);

            //get transform of droplet as a vector2
            Vector2 dropletPos = new Vector2(droplet.transform.position.x, droplet.transform.position.y);

            //apply force to the water drop in the direction of the mouse
            droplet.GetComponent<Rigidbody2D>().AddForce((mousePos - dropletPos) * sprayForce);
        }
    }

    /// <summary>
    /// method that depletes the water tank when a water move is used
    /// </summary>
    /// <param name="usageRate">the amount of water to subtract from current water</param>
    /// <returns>if there's enough water in the tank</returns>
    private bool UseWater(float usageRate)
    {
        //check if current water is greater than usage rate for the water move
        if(currentWater > usageRate)
        {
            //subtract the usage rate amount from current water
            currentWater -= usageRate;

            //return true
            return true;
        }

        //return false if the above is false
        return false;
    }
}
