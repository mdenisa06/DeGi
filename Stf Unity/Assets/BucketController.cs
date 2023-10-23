using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketController : MonoBehaviour
{
    public GameObject waterDropPrefab;
    public Transform spawnPoint;

    // Update is called once per frame
    void Update()
    {
        // Check for player tap
        if (Input.GetMouseButtonDown(0))
        {
            HandleTap();
        }
    }

    void HandleTap()
    {
        // Instantiate a water drop at the spawn point
        Instantiate(waterDropPrefab, spawnPoint.position, Quaternion.identity);
    }
}
