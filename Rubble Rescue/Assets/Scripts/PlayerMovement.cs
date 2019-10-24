using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Eddie Brazier
 *Rubble Rescue
 *October 20 2019
 */

public class PlayerMovement : MonoBehaviour
{
    //enum for player states
    enum SuitMode
    {
        Medic,
        Hydro,
        Buster
    }

    //potential vectors for movement
    /*
    private Vector3 position;
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 acceleration;
    */

    //fields for movespeed and jumpheight
    private float moveSpeed;
    private float jumpHeight;

    //fields for object's rigidbody and spriterenderer
    private Rigidbody2D playerBody;
    private SpriteRenderer playerRenderer;

    //field for current suit mode
    private SuitMode currentMode;

    //field for an array of player sprites
    [SerializeField]
    private Sprite[] formSprites;

    // Start is called before the first frame update
    void Start()
    {
        //initialize player position
        gameObject.transform.position = Vector2.zero;

        //get player rigidbody and sprite renderer
        playerBody = gameObject.GetComponent<Rigidbody2D>();
        playerRenderer = gameObject.GetComponent<SpriteRenderer>();

        //set suit mode to medic by default
        currentMode = SuitMode.Medic;

        //initialize stats with form change method
        FormChange();
    }

    // Update is called once per frame
    void Update()
    {
        //single-press movement

        //face left
        if(Input.GetKeyDown(KeyCode.A))
        {
            playerRenderer.flipX = true;
        }
        //face right
        if(Input.GetKeyDown(KeyCode.D))
        {
            playerRenderer.flipX = false;
        }
        //jump
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerBody.MovePosition(gameObject.transform.position + Vector3.up * jumpHeight * Time.deltaTime);
            Debug.Log("jump");
            //must prevent infinite jumping later
        }


        //continuous movement

        //move left
        if(Input.GetKey(KeyCode.A))
        {
            playerBody.MovePosition(gameObject.transform.position + Vector3.left * moveSpeed * Time.deltaTime);
        }
        //move right
        if(Input.GetKey(KeyCode.D))
        {
            playerBody.MovePosition(gameObject.transform.position + Vector3.right * moveSpeed * Time.deltaTime);
        }


        //form changes

        //change to medic mode
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentMode = SuitMode.Medic;
            FormChange();
        }
        //change to hydro mode
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentMode = SuitMode.Hydro;
            FormChange();
        }
        //change to buster mode
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentMode = SuitMode.Buster;
            FormChange();
        }
    }

    /// <summary>
    /// method that changes player forms by switching sprites and setting player stats
    /// </summary>
    private void FormChange()
    {
        //change stats based on current form
        switch (currentMode)
        {
            case SuitMode.Medic:

                //change movement stats
                moveSpeed = 10.0f;
                jumpHeight = 100f;

                //change player sprite to medic sprite
                playerRenderer.sprite = formSprites[0];
                break;

            case SuitMode.Hydro:

                //change movement stats
                moveSpeed = 9.0f;
                jumpHeight = 70f;

                //change player sprite to hydro sprite
                playerRenderer.sprite = formSprites[1];
                break;

            case SuitMode.Buster:

                //change movement stats
                moveSpeed = 5.0f;
                jumpHeight = 40f;

                //change player sprite to buster sprite
                playerRenderer.sprite = formSprites[2];
                break;
        }
    }
}
