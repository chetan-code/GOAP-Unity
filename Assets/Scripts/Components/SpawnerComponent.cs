using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{
    [SerializeField] private GameObject patientPrefab;
    [SerializeField] private int spawnCount;
    [SerializeField] private int spawnDelay;

    private int spawned;
    private void Start()
    {
        StartCoroutine(ProcessSpawn());
    }


    private IEnumerator ProcessSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        Spawn();
        if (spawned >= spawnCount)
        {
            yield return null; ;
        }
        else
        {
            StartCoroutine(ProcessSpawn());
        }
    }


    private void Spawn()
    {
        GameObject patient = Instantiate(patientPrefab);
        patient.name = patientPrefab.name + " " + spawned.ToString();
        spawned++;
    }



}
