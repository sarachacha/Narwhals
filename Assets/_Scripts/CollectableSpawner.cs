using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [Header("Prefabs and Components")]
    public List<GameObject> collectablesToSpawn = new List<GameObject>();

    public bool isRandomized;

    [Header("Timer Settings")]
    public bool isTimer;

    public float timeToSpawn;

    public float timeToDespawn;

    public float timerOffset;

    private float currentTimeToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        currentTimeToSpawn = timerOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if(isTimer)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        if(currentTimeToSpawn > 0)
        {
            currentTimeToSpawn -= Time.deltaTime;
        }
        else
        {
            SpawnObject();
            currentTimeToSpawn = timeToSpawn;
        }
    }

    public void SpawnObject()
    {
        int index = isRandomized ? Random.Range(0, collectablesToSpawn.Count) : 0;
        if(collectablesToSpawn.Count > 0)
        {
            GameObject clone = Instantiate(collectablesToSpawn[index], transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            //Destroy(clone, timeToDespawn);
        }
    }
}
