using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerScript : MonoBehaviour {

    public GameObject mainPlayer;
    int moveSpeed = 2;
    public int energy = 0;
    float terrainX, terrainZ;
    float enemyPosX, enemyPosZ;
    float enemyColorValue;
    float offset = 0.5f;
    Renderer enemyRenderer, groundRenderer;
    Rigidbody rigidbody;
    float playerPosX, playerPosZ;

    // Use this for initialization
    void Start ()
    {
        rigidbody = GetComponent<Rigidbody>();
        enemyRenderer = this.GetComponent<Renderer>();
        groundRenderer = GameObject.Find("Ground").GetComponent<Renderer>();
        terrainX = groundRenderer.bounds.size.x / 2;
        terrainZ = groundRenderer.bounds.size.z / 2;
        rigidbody.velocity = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Chase();
        ToridWorld();
        Color();
	}

    //Sets the color of the enemy object
    void Color()
    {
        /*Calculates the color value by subtracting it to its max value
         in order to get the opposite color since the higher value would make the color whiter
         subtracting by 100, because material.color takes a float between 0 and 1
         */
        enemyColorValue = (100 - energy) / 100f;
        enemyRenderer.material.color = new Color(0, 0, enemyColorValue, 1); //sets the new color
    }

    //Rotates towards the player and moves forward in order to close the distance towards him
    void Chase()
    {
        Quaternion rotation = Quaternion.LookRotation(mainPlayer.transform.position - transform.position); //finds how much it needs to rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 25); //rotates
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); //moves forward
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
            transform.position = new Vector3((terrainX - offset) * -1, transform.position.y, transform.position.z);
        }

        if ((playerPosX) < (terrainX * -1))
        {
            transform.position = new Vector3(terrainX - offset, transform.position.y, transform.position.z);
        }

        if (playerPosZ > terrainZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, (terrainZ - offset) * -1);
        }

        if ((playerPosZ) < (terrainZ * -1))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, terrainZ - offset);
        }
    }
}
