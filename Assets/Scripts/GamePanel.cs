using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    //tweak these as needed
    [SerializeField] int playingFieldHeight = 50; //height of the panel the game will play on, used to find spawn points
    [SerializeField] int playingFieldWidth = 100; //width of the panel the game will play on, used to find spawn points
    [SerializeField] int playingFieldDepth = 50; //depth of the panel the game will play on, used to find spawn points
    [SerializeField] int numOfRails = 3; //try to limit to like 5 max probably, CANNOT CURRENTLY BE CHANGED AT RUNTIME
    [SerializeField] float speedOfItems = 3f; //arbitary
    [SerializeField] float timeBetweenSpawn = 1f; //in seconds
    public SpawnableItem[] itemsOnRail = new SpawnableItem[6]; //set prefab for each item, easy way out for now will need to change later
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
        topLeft = center + new Vector3(-playingFieldWidth / 2f, playingFieldHeight / 2f, playingFieldDepth / 2f);
        spawnPointArray = new Vector3[numOfRails * 2]; //double the size to account for both sides

        //calculate the spawn points for the rails
        float spacing = (float)playingFieldHeight / numOfRails;

        //calculate the depth of each rail
        float zSpacing = (float)playingFieldDepth / numOfRails;

        //LEFT SPAWN POINTS
        //moving through adding the spawn points to list
        //have it always be centered
        for (int i = 0; i < numOfRails; i++)
        {
            //i love vector math
            float individualSeparation = ((numOfRails - 1) / 2f - i) * spacing;
            float individualZSeparation = ((numOfRails - 1) / 2f - i) * zSpacing;
            spawnPointArray[i] = center + new Vector3(-playingFieldWidth / 2f, individualSeparation, individualZSeparation);
        }

        //RIGHT SPAWN POINTS
        for (int i = 0; i < numOfRails; i++)
        {
            //i love vector math
            float individualSeparation = ((numOfRails - 1) / 2f - i) * spacing;
            float individualZSeparation = ((numOfRails - 1) / 2f - i) * zSpacing;
            spawnPointArray[i + numOfRails] = center + new Vector3(playingFieldWidth / 2f, individualSeparation, individualZSeparation);
        }

        // start spawn loop cause update is calling every frame
        StartCoroutine(SpawnItemsLoop());
    }

    void Update()
    {
        // Update logic can be added here if needed, currently not used
    }

    IEnumerator SpawnItemsLoop()
    {
        while (true)
        {
            //wait for the specified time before spawning the next item
            yield return new WaitForSeconds(timeBetweenSpawn);

            //randomly select a spawn point
            int randomSpawnIndex = Random.Range(0, numOfRails * 2);

            Vector3 spawnPoint = spawnPointArray[randomSpawnIndex];

            //change spawn based on weighted random chance
            GameObject itemToSpawn = GetWeightedRandomItem();

            //instantiate the item at the spawn point
            GameObject spawnedItem = Instantiate(itemToSpawn, spawnPoint, Quaternion.identity);

            //if the random index is greater than the number of rails, it will be on the right side
            if (randomSpawnIndex >= numOfRails)
            {
                spawnedItem.GetComponent<Item>().SetSpawnOrientation(false);
            }
            else
            {
                spawnedItem.GetComponent<Item>().SetSpawnOrientation(true);
            }

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
        Gizmos.DrawWireCube(transform.position, new Vector3(playingFieldWidth, playingFieldHeight, playingFieldDepth));

        //need to rewrite this cause this works before start is called
        Vector3 center = transform.position;
        Vector3 topLeft = center + new Vector3(-playingFieldWidth / 2f, playingFieldHeight / 2f, playingFieldDepth / 2f);


        //draw the gizmo for the spawn points
        Gizmos.color = new Color(0f, 1f, 0f);
        Gizmos.DrawSphere(topLeft, 1f); //draws gizmo for top left, nothing spawns there

        //blue for spawn points, only works on run time will give errors otherwise, comment out when it gets annoying
        /*
        Gizmos.color = new Color(0f, 0f, 1f);
        for (int i = 0; i < numOfRails * 2; i++) //times 2 for left and right spawn points
        {
            Gizmos.DrawSphere(spawnPointArray[i], 1f);
        }*/
    }

    //add together all the random spawn chances and return a random item based on the weights
    private GameObject GetWeightedRandomItem()
    {
        //calculate the total weight of all items
        float totalWeight = 0f;
        foreach (var item in itemsOnRail)
        {
            totalWeight += item.spawnChance;
        }

        //generate a random number between 0 and totalWeight
        float randomValue = Random.Range(0f, totalWeight);

        //iterate through the items and return the one that matches the random value
        float cumulativeWeight = 0f;
        foreach (var item in itemsOnRail)
        {
            cumulativeWeight += item.spawnChance;
            if (randomValue <= cumulativeWeight)
            {
                return item.itemPrefab;
            }
        }

        return itemsOnRail[itemsOnRail.Length - 1].itemPrefab; // Fallback will return the last item if no match found
    }

    public void setDificultyVariables(float speedOfItems, float timeBetweenSpawn)
    {
        this.speedOfItems = speedOfItems;
        this.timeBetweenSpawn = timeBetweenSpawn;
    }
}

// Struct to hold item prefab and its spawn chance
[System.Serializable]
public struct SpawnableItem
{
    public GameObject itemPrefab; // Prefab of the item to spawn
    [Range(0f, 1f)]
    public float spawnChance; // Chance of this item spawning, 0-1 range
}
