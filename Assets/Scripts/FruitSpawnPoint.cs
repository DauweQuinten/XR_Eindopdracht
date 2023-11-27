using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawnPoint : MonoBehaviour
{
    public void SpawnFruit(GameObject fruitPrefab)
    {
        GameObject newFruit = Instantiate(fruitPrefab, transform.position, transform.rotation);
        newFruit.transform.parent = this.transform;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a red sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
