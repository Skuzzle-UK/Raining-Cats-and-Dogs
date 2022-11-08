using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPointController : MonoBehaviour
{
    public GameObject[] spawnableTargets;
    [HideInInspector]
    public bool hasTargets = false;
    public List<GameObject> spawnedTargets = new List<GameObject>();
    [SerializeField]
    private List<SpawnPointController> interferingSpawnPoints = new List<SpawnPointController>();
    [SerializeField]
    private Vector2 initialSpawnVelocity;
    [SerializeField, Range(1, 500)]
    private float minSpawnDelaySeconds;
    [SerializeField, Range(1, 500)]
    private float maxSpawnDelaySeconds;
    public bool spawnLock = false;
    private bool timerRunning = false;


    private void Awake()
    {
        if (minSpawnDelaySeconds > maxSpawnDelaySeconds)
        {
            minSpawnDelaySeconds = maxSpawnDelaySeconds;
        }
    }

    private void FixedUpdate()
    {
        while (!timerRunning)
        {
            StartCoroutine(SpawnTimer());
        }
    }

    private void Update()
    {
        //Let others know there are no targets in queue
        if (spawnedTargets.Count == 0)
        {
            hasTargets = false;
        }
    }

    private IEnumerator SpawnTimer()
    {
        timerRunning = true;
        //Prepare target
        var targetIndex = 0;
        if (spawnableTargets.Length > 1)
        {
            targetIndex = (int)Math.Round((double)Random.Range(0, spawnableTargets.Length - 1));
        }
        GameObject target = spawnableTargets[targetIndex];

        //Delay before spawn
        float delay = Random.Range(minSpawnDelaySeconds, maxSpawnDelaySeconds);
        yield return new WaitForSeconds(delay);

        //Wait for other lock
        while (spawnLock)
        {
            yield return new WaitForSeconds(1);
        }

        var interferingTargets = interferingSpawnPoints.FirstOrDefault(x => x.hasTargets == true);
        var interferingLock = interferingSpawnPoints.FirstOrDefault(x => x.spawnLock == true);

        while (interferingLock || interferingTargets)
        {
            yield return new WaitForEndOfFrame();
            interferingTargets = interferingSpawnPoints.FirstOrDefault(x => x.hasTargets == true);
            interferingLock = interferingSpawnPoints.FirstOrDefault(x => x.spawnLock == true);
        }

        //get spawn lock, spawn and then release lock
        spawnLock = true;
        SpawnTarget(target);
        spawnLock = false;

        timerRunning = false;
    }

    private void SpawnTarget(GameObject t)
    {
        GameObject target = Instantiate(t, this.transform);
        spawnedTargets.Add(target);
        hasTargets = true;

        //Set start position
        target.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, target.transform.position.z);

        //Set initial velocity
        target.GetComponent<TargetController>().velocity = initialSpawnVelocity;
    }
}
