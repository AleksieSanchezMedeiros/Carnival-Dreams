using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    //tweak these as needed
    [SerializeField] int playingFieldHeight = 50; //height of the panel the game will play on, used to find spawn points
    [SerializeField] int playingFieldWidth = 100; //width of the panel the game will play on, used to find spawn points
    [SerializeField] int numOfRails = 3; //try to limit to like 5 max probably, CANNOT CURRENTLY BE CHANGED AT RUNTIME
    [SerializeField] float speedOfItems = 3f; //arbitary
    [SerializeField] float timeBetweenSpawn = 1f; //in seconds
    public GameObject[] itemsOnRail = new GameObject[6]; //set prefab for each item, easy way out for now will need to change later
                                                         //0 is enemy
                                                         //1 is astronaut
                                                         //2 is obstacle
                                                         //3 is power up reload
                                                         //4 is power up wobble
                                                         //5 is power up invincible

    //references to positions in square
    private Vector3 center;

    // top left = center + left (-right) and up, no z
    private Vector3 topLeft;
    private Vector3[] spawnPointArray;


    //currently in start since it doesnt need to be updated every frame
    //but will need to be updated if we want to change the playing field size dynamically
    //or if we want to change the number of rails dynamically
    void Start()
    {
        //calculate rail spawn point
        center = transform.position;
        topLeft = center + new Vector3(-playingFieldWidth / 2f, playingFieldHeight / 2f, 0f);
        spawnPointArray = new Vector3[numOfRails];

        //moving through adding the spawn points to list
        //have it always be centered
        float spacing = (float)playingFieldHeight / numOfRails;
        for (int i = 0; i < numOfRails; i++)
        {
            //i love vector math
            float individualSeparation = ((numOfRails - 1) / 2f - i) * spacing;
            spawnPointArray[i] = center + new Vector3(-playingFieldWidth / 2f, individualSeparation, 0f);
        }
        
        // start spawn loop cause update is calling every frame
        StartCoroutine(SpawnItemsLoop());
    }

    void Update()
    {
        //items will move faster based on the speed
        //make clone of items every blah seconds, change their speed value to set amount
        //StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItemsLoop()
    {
        while (true)
        {
            //wait for the specified time before spawning the next item
            yield return new WaitForSeconds(timeBetweenSpawn);

            //randomly select a spawn point
            int randomSpawnIndex = Random.Range(0, numOfRails);
            Vector3 spawnPoint = spawnPointArray[randomSpawnIndex];

            //EDIT HERE TO DETERMINE CHANCE OF ITEM SPAWNING
            //for now just randomly select an item to spawn
            int randomItemIndex = Random.Range(0, itemsOnRail.Length);
            GameObject itemToSpawn = itemsOnRail[randomItemIndex];

            //instantiate the item at the spawn point
            GameObject spawnedItem = Instantiate(itemToSpawn, spawnPoint, Quaternion.identity);

            //set the speed of the item (assuming it has a script that handles movement)
            Item itemMovement = spawnedItem.GetComponent<Item>();
            if (itemMovement != null)
            {
                itemMovement.SetSpeed(speedOfItems);
            }
        }
    }

    //for debugging
    private void OnDrawGizmosSelected()
    {
        //draw the gizmo for the playing field
        Gizmos.color = new Color(1f, 0f, 0f);
        Gizmos.DrawWireCube(transform.position, new Vector3(playingFieldWidth, playingFieldHeight, 0.01f));

        //need to rewrite this cause this works before start is called
        Vector3 center = transform.position;
        Vector3 topLeft = center + new Vector3(-playingFieldWidth / 2f, playingFieldHeight / 2f, 0f);


        //draw the gizmo for the spawn points
        Gizmos.color = new Color(0f, 1f, 0f);
        Gizmos.DrawSphere(topLeft, 1f); //draws gizmo for top left, nothing spawns there

        //blue for spawn points, only works on run time will give errors otherwise, comment out when it gets annoying
        /*
        Gizmos.color = new Color(0f, 0f, 1f);
        for (int i = 0; i < numOfRails; i++)
        {
            Gizmos.DrawSphere(spawnPointArray[i], 1f);
        }*/
    }
}
