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

    //field for water hose usage
    private float hoseRate = 3f;

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

        //slow down time when aiming
        if (Input.GetButton("Aim"))
        {
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

        //check if the player can't spray water
        if(canSpray == false)
        {
            //decrement spray cooldown
            currentSprayCooldown -= 0.2f;
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

        //check if spray cooldown has been decremented to zero
        if (currentSprayCooldown < 0f)
        {
            //set canSpray to true
            canSpray = true;

            //reset cooldown float
            currentSprayCooldown = maxSprayCooldown;
        }

        //TODO: add water usage mechanics
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

    private void SprayWater()
    {
        //get mouse position in world space
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //spray water if there's enough water in the tank
        if(currentWater > hoseRate)
        {
            //spawn water drop
            GameObject droplet = Instantiate(waterDrop, gameObject.transform.position, Quaternion.identity);

            //get transform of droplet as a vector2
            Vector2 dropletPos = new Vector2(droplet.transform.position.x, droplet.transform.position.y);

            //apply force to the water drop in the direction of the mouse
            droplet.GetComponent<Rigidbody2D>().AddForce((mousePos - dropletPos) * sprayForce);

            //subtract 5 points from current water value
            currentWater -= hoseRate;
        }
    }
}
