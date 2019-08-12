using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerScript : MonoBehaviour {
    
    public string playerName;
    Rigidbody rigidBody;
    public int speed;
    public GameObject ground;
    Renderer groundRenderer;
    Renderer playerRenderer;
    float terrainX, terrainZ;
    float playerPosX, playerPosZ;
    public int energy;
    int minEnergy = 0;
    int maxEnergy = 100;
    float offset = 0.5f;
    float playerColorValue;
    EnemyPlayerScript enemyPlayerScript;

    // Use this for initialization
    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezePositionY;
        groundRenderer = ground.GetComponent<Renderer>();
        terrainX = groundRenderer.bounds.size.x / 2;
        terrainZ = groundRenderer.bounds.size.z / 2;
        playerRenderer = gameObject.GetComponent<Renderer>();
        enemyPlayerScript = GameObject.Find("EnemyPlayer").GetComponent<EnemyPlayerScript>();
        rigidBody.velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void FixedUpdate()
    {
        Movement();
        ToridWorld();
        Energy();
    }

    /* Energy stays between the minimum and maximum amount 
     Done by checking the current energy level as it could exceed the values and 
     keeping them locked to those values
         */
    private void Energy()
    {
        if (energy < minEnergy)
        {
            energy = minEnergy;
        }

        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }

        playerColorValue = (100 - energy) / 100f;
        playerRenderer.material.color = new Color(playerColorValue, playerColorValue, playerColorValue, 1);
    }

    /*Movement handling done via user input
      It takes in the axis input through unity's framework 
      creates a vector with it and adds force to the object using the said vector
     */
    private void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rigidBody.AddForce(movement * speed);
    }

    /*Checks the position of the player in accordance to the size of the terrain 
    When the player reaches the end of the terrain, he gets transformed
    to the other side of it by subtracting the exact same axis by minus 1
     */
    private void ToridWorld()
    {
        //Checks position and stores it into variables in order to check it just once and not many times
        playerPosX = transform.position.x;
        playerPosZ = transform.position.z;

        if (playerPosX > terrainX)
        {
            transform.position = new Vector3((terrainX - offset)* -1, transform.position.y, transform.position.z);
        }

        if ((playerPosX) < (terrainX * -1))
        {
            transform.position = new Vector3(terrainX - offset, transform.position.y, transform.position.z);
        }

        if (playerPosZ > terrainZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, (terrainZ - offset)* -1);
        }

        if ((playerPosZ) < (terrainZ * -1))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, terrainZ - offset);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        /* Checks collisions with a tag "Mushroom"
         * This method is being used because many mushrooms could be on screen 
         * and it would be easier and more efficient to have a tag for all of them
         */
        if (collision.gameObject.tag == "Mushroom")
        {
            /* Checks if the mushroom is dead since after it is, players shouldn't
             * be able to eat it even if it is still on the screen
             */
            if (!collision.gameObject.GetComponent<MushroomBehavior>().dead)
            {
                //Checks if the mushroom is poisonous and decreases energy by a random amount if it is or increases it if it's not
                if (collision.gameObject.GetComponent<MushroomBehavior>().poisonous)
                {
                    energy -= Random.Range(1, 20);
                }
                else
                {
                    energy += Random.Range(1, 20);
                }
                Destroy(collision.gameObject); //destroys the mushroom 
            }
        }

        /* Checks if the player has collided with the enemy 
         * transfers a random amount of energy from the player to the enemy
         * this is done by accessing the public energy variable of the enemy's script
         */
        if (collision.gameObject.name == "EnemyPlayer")
        {
            int randomNumber = Random.Range(1, 20);
            energy -= randomNumber;
            enemyPlayerScript.energy += randomNumber;
        }
    }
}
