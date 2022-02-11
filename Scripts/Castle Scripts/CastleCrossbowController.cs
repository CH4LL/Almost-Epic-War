using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CastleCrossbowController : MonoBehaviour
{
    [SerializeField] private GameObject castleCrossbowBolt = default;
    [SerializeField] private GameManager gameManager = default;
    const float rotationSpeed = 45.0f;
    float rotation;
    readonly float boltDelay = 0.03f;
    bool boltSpawnTimer = true;

    // Crossbow rotation boundaries
    float rotationBoundUp = 350;
    float rotationBoundDown = 210;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Crossbow movement
        rotation = Time.deltaTime * rotationSpeed * Input.GetAxis("Vertical");
        transform.Rotate(Vector3.forward,rotation);

        // Crossbow boundaries
        if (transform.rotation.eulerAngles.z > rotationBoundUp)
            transform.Rotate(Vector3.forward, -rotation);
        if (transform.rotation.eulerAngles.z < rotationBoundDown)
            transform.Rotate(Vector3.forward, -rotation);

        // bolt spawning
        if(Input.GetKey(KeyCode.Space) == true && boltSpawnTimer)
        {
            StartCoroutine(SpawnBolt());
        }
    }
    
    IEnumerator SpawnBolt()
    {
        float boltDelayCounter = 0;
        boltSpawnTimer = false;
        for(int i=0;i<1+gameManager.castleCrossbowLevel;i++)
        {
            Instantiate(castleCrossbowBolt,transform.position,transform.rotation);
            yield return new WaitForSeconds(boltDelay);
            boltDelayCounter += boltDelay;
        }
        yield return new WaitForSeconds(1-boltDelayCounter);
        boltSpawnTimer = true;
        
    }
}
