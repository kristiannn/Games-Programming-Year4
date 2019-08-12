using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SQLite : MonoBehaviour {

    string charName;
    MainPlayerScript playerScript;
    EnemyPlayerScript enemyScript;
    Mushroom mushroomScript;
    int mushroomCount = 0;
    IDbCommand dbcmd; //a global variable so script can be easily separated into methods


    //For loading from the database
    public float dbPosXPlayer = 0, dbPosYPlayer = 0, dbPosZPlayer = 0, dbPosXEnemy = 0, dbPosYEnemy = 0, dbPosZEnemy = 0;
    public int dbEnergyPlayer = 0, dbEnergyEnemy = 0;

    // Use this for initialization
    void Start ()
    {
        //Get the player, enemy and mushroom scripts in order to use them later
        playerScript = gameObject.GetComponent<MainPlayerScript>();
        charName = playerScript.playerName;
        enemyScript = GameObject.Find("EnemyPlayer").GetComponent<EnemyPlayerScript>();
        mushroomScript = gameObject.GetComponent<Mushroom>();


        //A string which holds the path to the database, which will be called in a function later on
        string conn = "URI=file:" + Application.dataPath + "/kristiyan_nenov_cw.db";

        IDbConnection dbconn; //creates a db connection entity
        dbconn = (IDbConnection)new SqliteConnection(conn); //opens the sqlite connection through the connection entity
        dbconn.Open(); //Open connection to the database.

        dbcmd = dbconn.CreateCommand(); //Uses the create command functionality, which enables script-like queries to the database



        PlayerTable();
        EnemyTable();
        CountMushrooms();
        SpawnMushrooms();
        

        //Closes everything and makes sure all values are set to null
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }

    //Player Settings
    void PlayerTable()
    {
        /* Stores the command in a string
         * The command selects the values from the player table which are for the character name selected inside the PlayerMainScript
         */
        string sqlQueryPlayer = "SELECT posX, posY, posZ, energy FROM PlayerTable WHERE id = '" + charName + "';";
        dbcmd.CommandText = sqlQueryPlayer;
        IDataReader readerPlayer = dbcmd.ExecuteReader();

        //Reads through all the results 
        while (readerPlayer.Read())
        {
            //sets the results into variables
            dbPosXPlayer = readerPlayer.GetFloat(0); 
            dbPosYPlayer = readerPlayer.GetFloat(1);
            dbPosZPlayer = readerPlayer.GetFloat(2);
            dbEnergyPlayer = readerPlayer.GetInt32(3);

            /*Uses those variables to access the position of the player through the player script
             * Changes the position of the player to the ones from the database
             * Displays the values derived from the database into the console
             */
            playerScript.gameObject.transform.position = new Vector3(dbPosXPlayer, dbPosYPlayer, dbPosZPlayer);
            playerScript.energy = dbEnergyPlayer;
            Debug.Log("Player Name: " + charName + " X = " + dbPosXPlayer + "  Y = " + dbPosYPlayer + "  Z = " + dbPosZPlayer + "  energy = " + dbEnergyPlayer);
        }

        //closes the reader and makes sure it's null (debugging purposes)
        readerPlayer.Close();
        readerPlayer = null;

    }

    //ENEMY SETTINGS
    void EnemyTable()
    {
        /* Stores the command in a string
        * The command selects the values from the enemy table which are for the character name selected inside the EnemyMainScript
        */
        string sqlQueryEnemy = "SELECT posX, posY, posZ, energy FROM EnemyTable WHERE id = '" + charName + "';";
        dbcmd.CommandText = sqlQueryEnemy;
        IDataReader readerEnemy = dbcmd.ExecuteReader();

        while (readerEnemy.Read())
        {
            //sets the results into variables
            dbPosXEnemy = readerEnemy.GetFloat(0);
            dbPosYEnemy = readerEnemy.GetFloat(1);
            dbPosZEnemy = readerEnemy.GetFloat(2);
            dbEnergyEnemy = readerEnemy.GetInt32(3);


            /*Uses those variables to access the position of the enemy through the enemy script
            * Changes the position of the enemy to the ones from the database
            * Displays the values derived from the database into the console
            */
            enemyScript.gameObject.transform.position = new Vector3(dbPosXEnemy, dbPosYEnemy, dbPosZEnemy);
            enemyScript.energy = dbEnergyEnemy;
            Debug.Log("Enemy Name: " + charName + " X = " + dbPosXEnemy + "  Y = " + dbPosYEnemy + "  Z = " + dbPosZEnemy + "  energy = " + dbEnergyEnemy);
        }

        //closes the reader and makes sure it's null (debugging purposes)
        readerEnemy.Close();
        readerEnemy = null;
    }

    //COUNT MUSHROOMS
    void CountMushrooms()
    {
        /* Stores the command in a string
        * The command selects the mushID value from the mushrooms table
        */
        string sqlQueryMushroomCount = "SELECT mushID FROM MushroomsTable WHERE id = '" + charName + "';";

        dbcmd.CommandText = sqlQueryMushroomCount;
        IDataReader readerMushroomCount = dbcmd.ExecuteReader();

        //Goes through all the results and increases a counter 
        while (readerMushroomCount.Read())
        {
            mushroomCount++;
            //Debug.Log(mushroomCount);
        }

        //closes the reader and makes sure it's null (debugging purposes)
        readerMushroomCount.Close();
        readerMushroomCount = null;
    }

    //SPAWN MUSHROOMS
    void SpawnMushrooms()
    {
        // If any mushrooms are found in the database
        if (mushroomCount > 0)
        {
            /* Stores the command in a string
            * The command selects the values from the player table which are for the character name selected inside the PlayerMainScript
            */
            string sqlQueryMushrooms = "SELECT mushID, posX, posY, posZ, currentAge, deathAge FROM MushroomsTable WHERE id = '" + charName + "';";

            dbcmd.CommandText = sqlQueryMushrooms;
            IDataReader readerMushrooms = dbcmd.ExecuteReader();

            while (readerMushrooms.Read())
            {
                //sets the results into variables
                int mushroomID = readerMushrooms.GetInt32(0);
                float mushroomPosX = readerMushrooms.GetFloat(1);
                float mushroomPosY = readerMushrooms.GetFloat(2);
                float mushroomPosZ = readerMushrooms.GetFloat(3);
                float mushroomAge = readerMushrooms.GetFloat(4);
                float mushroomDeathAge = readerMushrooms.GetFloat(5);

                /* Uses the CreateDBMushroom script, which is described inside the mushroom script 
                 * in order to spawn the mushrooms loaded from the database
                 * Also displays their information into the console
                 */
                mushroomScript.CreateDBMushroom(mushroomID, mushroomPosX, mushroomPosY, mushroomPosZ, mushroomAge, mushroomDeathAge);
                Debug.Log("Mushroom Created. ID: " + mushroomID + " x = " + mushroomPosX + " y = " +mushroomPosY + " z = "
                    + mushroomPosZ + " Age: " + mushroomAge + " Death Age: " + mushroomDeathAge);
            }

            //closes the reader and makes sure it's null (debugging purposes)
            readerMushrooms.Close();
            readerMushrooms = null;
        }

        //Spawns random mushrooms
        else
        {
            mushroomScript.CreateMushrooms();
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
