using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = default;
    private Vector2 movement;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.right * Time.deltaTime * speed * Input.GetAxis("Horizontal");
        transform.Translate(movement);
        if (transform.position.x < -30)
            transform.Translate(-30 - transform.position.x, 0, 0);
        if (transform.position.x > 30)
            transform.Translate(30 - transform.position.x, 0, 0);
    }
}
