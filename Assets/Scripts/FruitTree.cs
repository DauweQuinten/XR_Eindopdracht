using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FruitTree : MonoBehaviour
{
    FruitSpawnPoint[] fruitSpawnPoints;
    [SerializeField] GameObject fruitPrefab;
    [SerializeField] GameObject positionIndicator;


    private void Awake()
    {
        fruitSpawnPoints = transform.GetComponentsInChildren<FruitSpawnPoint>();
    }

    private void Start()
    {
        InvokeRepeating("CheckFruitPresence", 0f, 10f);
    }


    void CheckFruitPresence()
    {
        foreach (FruitSpawnPoint spawnpoint in fruitSpawnPoints)
        {
            if (spawnpoint.gameObject.transform.childCount == 0)
            {
                spawnpoint.SpawnFruit(fruitPrefab);
            }
        }
    }

    public void TogglePositionIndicator(bool state)
    {
        positionIndicator.SetActive(state);
    }
}
