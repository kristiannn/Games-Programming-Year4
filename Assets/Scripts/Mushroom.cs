using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour {

    GameObject ground;
    Renderer groundRenderer;
    float terrainX, terrainZ;
    public int mushroomCount = 10;

    // Use this for initialization
    void Start () {
        ground = GameObject.Find("Ground");
        groundRenderer = ground.GetComponent<Renderer>();
        terrainX = groundRenderer.bounds.size.x / 2;
        terrainZ = groundRenderer.bounds.size.z / 2;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /* Uses the CreateMushroom script in order to create a preset amount of mushrooms
     * Can be called outside the script
     * It's used from the SQLite script, when there is no information found in the database
     */
    public ArrayList CreateMushrooms()
    {
        ArrayList mushroomsCreated = new ArrayList(mushroomCount);

        for (int i = 0; i < mushroomCount; i++)
        {
            CreateMushroom(i);
        }

        return mushroomsCreated;
    }

    /* Method is used when random variables for the mushrooms need to be assigned
     * Creates a single mushroom and cannot be used outside the particular script
     */
    private GameObject CreateMushroom(int id)
    {
        GameObject mushroomCreated = GameObject.CreatePrimitive(PrimitiveType.Cube);

        mushroomCreated.transform.position = new Vector3(Random.Range(-terrainX, terrainX), 0.5f, Random.Range(-terrainZ, terrainZ));

        Color randomColor = new Color(Random.value, Random.value, Random.value, 1);
        mushroomCreated.GetComponent<Renderer>().material.color = randomColor;

        mushroomCreated.name = "mushroom" + id;
        mushroomCreated.tag = "Mushroom";
        mushroomCreated.AddComponent<Rigidbody>();
        mushroomCreated.AddComponent<MushroomBehavior>();

        return mushroomCreated;
    }

    /* Method is public because it will be accessed within the SQLite script
     * It takes in the id, positions, age and deathage as arguments and 
     * uses those values in order to set up the mushroom
     * Those variables will be gathered by reading a table in a database, 
     * storing them and reusing them to call the method and create a mushroom
     */
    public GameObject CreateDBMushroom(int id, float posX, float posY, float posZ, float age, float deathAge)
    {
        GameObject mushroom = GameObject.CreatePrimitive(PrimitiveType.Cube);

        mushroom.transform.position = new Vector3(posX, posY, posZ);

        Color randomColor = new Color(Random.value, Random.value, Random.value, 1);
        mushroom.GetComponent<Renderer>().material.color = randomColor;
        mushroom.name = "mushroom" + id;
        mushroom.tag = "Mushroom";
        mushroom.AddComponent<Rigidbody>();
        MushroomBehavior mushBeh = mushroom.AddComponent<MushroomBehavior>();
        mushBeh.currentAge = age;
        mushBeh.deathAge = deathAge;


        return mushroom;
    }
}
