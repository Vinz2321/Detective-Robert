using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform spawnPoint;
    public int npcCount =5;
    public float spawnInterval = 2f;
    public int spawnedCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnNPCS());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator spawnNPCS ()
    {
        while (spawnedCount < npcCount)
        {
            Instantiate(npcPrefab,spawnPoint.position,Quaternion.identity);

            spawnedCount++;

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
