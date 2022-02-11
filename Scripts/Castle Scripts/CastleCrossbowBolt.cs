using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using UnityEngine;

public class CastleCrossbowBolt : MonoBehaviour
{
    private Rigidbody2D boltRb;
    private GameObject castleCrossbow;
    private GameManager gameManager;
    private UnitController enemyHit;
    public float damage;
    private const float speed = 140.0f;
    private bool isDying = false;

    // Start is called before the first frame update
    void Start()
    {
        // asigning GameObject and component variales
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        castleCrossbow = GameObject.Find("Castle Crossbow");
        boltRb = GetComponent<Rigidbody2D>();

        // Amount of damage dealt by the bolt based on the level of CastleCrossbow
        damage = gameManager.castleCrossbowLevel * 2 + 10;

        // Random disparity in the angle of the bolts
        Vector3 boltDisparity = new Vector3(0, Random.Range(0.03f, 0.09f), 0);

        // Initial force of the bolt, constant across all levels
        boltRb.AddForce((castleCrossbow.transform.up + boltDisparity) * speed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDying == false && transform.position.y < -8)
        {
            StartCoroutine(boltDeathAnimation());
        }
    }

    // logic invoked when the projectile hits the ground
    IEnumerator boltDeathAnimation()
    {
        isDying = true;
        boltRb.velocity = Vector3.zero;
        boltRb.isKinematic = true;
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("Collision occured");
        if(col.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("Collision with enemy occured");
            enemyHit = col.gameObject.GetComponent<UnitController>();
            enemyHit.ReceiveDamage(damage);
            Destroy(gameObject);
        }
    }

}
