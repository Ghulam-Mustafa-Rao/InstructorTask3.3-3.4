using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpwanManager : MonoBehaviour
{
    GameObject spawnedObject;
    public GameObject stone;
    public GameObject rockHolder;
    public float xBounds;
    int lifeIncrement = 0;

    // Start is called before the first frame update
    void Start()
    {
        //spwan ball after every 5sec
        StartCoroutine(spwanRocksCo());

    }

    // Update is called once per frame
    void Update()
    {
        //spwan rocks if all balls/rocks are destroyed
        if (rockHolder.transform.childCount == 0)
        {
            SpwanRocks();
        }
        
    }


    void SpwanRocks()
    {
        int life = 4;
        //Spwan rocks/balls according to waveno
        for (int i = 0; i < GameManager.gameManager.waveNo; i++)
        {
            //limit spwaned balls to 3 to not to overcrowed gameplay
            if (i == 3)
            {
                break;
            }

            //create ball/rock
            spawnedObject = Instantiate(stone, new Vector3(Random.Range(-xBounds, xBounds + 1), rockHolder.gameObject.transform.position.y, 80), Quaternion.identity);
            spawnedObject.transform.SetParent(rockHolder.transform);
            spawnedObject.GetComponent<Rocks>().spwanManager = this;
            spawnedObject.GetComponent<Rocks>().life = life;
            spawnedObject.GetComponent<Rocks>().initialLife = life;


        }
        GameManager.gameManager.waveNo++;
    }

    IEnumerator spwanRocksCo()
    {
        int life = 4;
        while (GameManager.gameManager.playerLifes>0)
        {
            yield return new WaitForSeconds(5f);

            //if game is in pause state then dont spwan rock/ball
            if (GameManager.gameManager.pauseAllballs)
                continue;

            spawnedObject = Instantiate(stone, new Vector3(Random.Range(-xBounds, xBounds + 1), rockHolder.gameObject.transform.position.y, 80), Quaternion.identity);
            spawnedObject.transform.SetParent(rockHolder.transform);
            spawnedObject.GetComponent<Rocks>().spwanManager = this;
            spawnedObject.GetComponent<Rocks>().life = life;
            spawnedObject.GetComponent<Rocks>().initialLife = life;
        }
    }

}
