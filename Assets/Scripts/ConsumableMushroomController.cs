using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableMushroomController : MonoBehaviour
{
    private Rigidbody2D consumableMushroom;
    private int moveRight;
    float speed = 3.0f;
    private Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Mushroom started");
        consumableMushroom = GetComponent<Rigidbody2D>();
        RandomiseDirection();
        ComputeVelocity();
    }

    void RandomiseDirection(){
        int[] numbers = new int[] { -1, 1 };
        int randomIndex = Random.Range(0, numbers.Length);
        moveRight = numbers[randomIndex];
    }
     
    void ComputeVelocity(){
        velocity = new Vector2(moveRight * speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        MoveMushroom();
    }

    void MoveMushroom(){
        // Debug.Log("Mushroom move");
        consumableMushroom.MovePosition(consumableMushroom.position + velocity * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")){
            // Debug.Log("Consume mushroom");
            velocity = new Vector2(0, 0);
        }

        if (col.gameObject.CompareTag("Wall")){
            // Debug.Log("Collide, change direction.");
            
            moveRight *= -1;
            ComputeVelocity();
            MoveMushroom();
        }
    }
}
