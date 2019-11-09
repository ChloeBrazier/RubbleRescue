using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Eddie Brazier
 *Rubble Rescue
 *October 24th 2019
 */

public class FormChange : MonoBehaviour
{
    //enum for player states
    enum SuitMode
    {
        Medic,
        Hydro,
        Buster
    }

    //field for current suit mode
    private SuitMode currentMode;

    //field for an array of player sprites
    [SerializeField]
    private Sprite[] formSprites;

    //fields for object's rigidbody and spriterenderer
    private SpriteRenderer playerRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //set suit mode to medic by default
        currentMode = SuitMode.Medic;

        //get player sprite renderer
        playerRenderer = gameObject.GetComponent<SpriteRenderer>();

        //initialize stats with form change method
        ChangeForm();
    }

    // Update is called once per frame
    void Update()
    {
        //change to medic mode
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentMode = SuitMode.Medic;
            ChangeForm();
        }
        //change to hydro mode
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentMode = SuitMode.Hydro;
            ChangeForm();
        }
        //change to buster mode
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentMode = SuitMode.Buster;
            ChangeForm();
        }
    }

    /// <summary>
    /// method that changes player forms by switching sprites and setting player stats
    /// </summary>
    private void ChangeForm()
    {
        //get the player's move speed and jump start speed
        float jumpStart = gameObject.GetComponent<PlayerMovement>().jumpStartSpeed;
        float moveSpeed = gameObject.GetComponent<PlayerMovement>().moveSpeed;

        //change stats based on current form
        switch (currentMode)
        {
            case SuitMode.Medic:

                //change movement stats
                moveSpeed = 7.0f;
                jumpStart = 23.0f;

                //disable hydro mode (and any other suit mode scripts) script
                gameObject.GetComponent<HydroMode>().enabled = false;

                //enable medic scripts

                //change player sprite to medic sprite
                playerRenderer.sprite = formSprites[0];
                break;

            case SuitMode.Hydro:

                //change movement stats
                moveSpeed = 8.0f;
                jumpStart = 20.0f;

                //enable hydro mode script
                gameObject.GetComponent<HydroMode>().enabled = true;

                //change player sprite to hydro  sprite
                playerRenderer.sprite = formSprites[1];
                break;

            case SuitMode.Buster:

                //change movement stats
                moveSpeed = 5.0f;
                jumpStart = 16.0f;

                //disable hydro mode (and any other suit mode scripts) script
                gameObject.GetComponent<HydroMode>().enabled = false;

                //enable buster scripts

                //change player sprite to buster sprite
                playerRenderer.sprite = formSprites[2];
                break;
        }

        //set stats now that forms have changed
        SetStats(moveSpeed, jumpStart);
    }

    /// <summary>
    /// method that changes player stats based on passed-in values
    /// </summary>
    /// <param name="moveSpeed"></param>
    /// <param name="jumpStart"></param>
    private void SetStats(float moveSpeed, float jumpStart)
    {
        PlayerMovement.instance.moveSpeed = moveSpeed;
        PlayerMovement.instance.jumpStartSpeed = jumpStart;
    }
}
