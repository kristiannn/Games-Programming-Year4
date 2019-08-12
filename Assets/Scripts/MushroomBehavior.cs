using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBehavior : MonoBehaviour {

    public float currentAge = 0;
    int maxDeathAge = 100;
    public float deathAge;
    private Rigidbody rigidbody;
    public bool poisonous;
    public bool dead;
    Renderer mushroomRenderer;
    float offset = 0.5f;
    float terrainX, terrainZ;
    float playerPosX, playerPosZ;
    Renderer groundRenderer;


    // Use this for initialization
    void Start ()
    {
        //currentAge = Random.value * maxDeathAge;
        rigidbody = GetComponent<Rigidbody>();
        mushroomRenderer = this.GetComponent<Renderer>();
        deathAge = Random.Range (0,maxDeathAge);
        groundRenderer = GameObject.Find("Ground").GetComponent<Renderer>();
        terrainX = groundRenderer.bounds.size.x / 2;
        terrainZ = groundRenderer.bounds.size.z / 2;
        dead = false;
        rigidbody.velocity = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        currentAge += Time.deltaTime * 2.5f;
        if (currentAge >= deathAge)
        {
            //Should not be able to collect mushroom
            Death();
        }

        if (currentAge > deathAge / 2)
        {
            //Mushroom becomes poisonous
            poisonous = true;
            mushroomRenderer.material.color = new Color(0, 0, 0, 1);
        }
        ToridWorld();
    }

    void Death()
    {
        dead = true;
        if (transform.localScale.x > 0.5f)
        {
            rigidbody.useGravity = false;
            rigidbody.velocity = transform.up * Time.deltaTime * transform.localScale.x * 25;
            float decreaseBy = Time.deltaTime * 0.2f;
            transform.localScale -= new Vector3(decreaseBy, decreaseBy, decreaseBy);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
