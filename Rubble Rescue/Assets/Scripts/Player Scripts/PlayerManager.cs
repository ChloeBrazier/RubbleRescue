using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Eddie Brazier
 *Rubble Rescue
 *November 9th 2019
 */

public class PlayerManager : MonoBehaviour
{
    //make this class static
    public static PlayerManager instance;

    //field for player spawn gameobject
    [SerializeField]
    private GameObject playerSpawn;

    //field for player prefab
    public GameObject player;

    //TODO: create references to other player information (height, width, etc)
    public float playerHeight;
    public float playerWidth;
    public Vector3 footPos;

    //awake is called when the script is initialized, before anything else
    private void Awake()
    {
        //initialize static instace
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //spawn player at player spawnpoint
        player = Instantiate(player, playerSpawn.transform.position, Quaternion.identity);

        //initialize player width and height
        Vector3 playerMax = player.GetComponent<SpriteRenderer>().bounds.max;
        Vector3 playerMin = player.GetComponent<SpriteRenderer>().bounds.min;
        playerHeight = playerMax.x - playerMin.x;
        playerWidth = playerMax.y - playerMin.y;
    }

    // Update is called once per frame
    void Update()
    {
        //get position of player's feet
        footPos = player.transform.Find("feet").transform.position;
    }
}
