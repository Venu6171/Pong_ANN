using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D ballRigidBody;
    [SerializeField] private Vector2 randomXForce;
    [SerializeField] private Vector2 randomYForce;

    [SerializeField] private float speed;
    [SerializeField] private float speedChange;
    private PhysicsMaterial2D bounciness;
    private bool increasedOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        ballRigidBody = GetComponent<Rigidbody2D>();
        bounciness = GetComponent<PhysicsMaterial2D>();
        Invoke("GoBall", 2);
    }

    void GoBall()
    {
        float rand = Random.Range(0, 3);

        float forceX = Random.Range(randomXForce.x, randomXForce.y);
        float forceY = Random.Range(randomYForce.x, randomYForce.y);

        switch (rand)
        {
            case 0:
                ballRigidBody.AddForce(new Vector2(forceX, forceY) * speed);
                break;
            case 1:
                ballRigidBody.AddForce(new Vector2(-forceX, forceY) * speed);
                break;
            case 2:
                ballRigidBody.AddForce(new Vector2(forceX, -forceY) * speed);
                break;
            case 3:
                ballRigidBody.AddForce(new Vector2(-forceX, -forceY) * speed);
                break;
        }
    }

    void ResetBall()
    {
        ballRigidBody.velocity = Vector2.zero;
        ballRigidBody.position = Vector2.zero;
        increasedOnce = false;
        speed = 1.0f;
    }

    void RestartGame()
    {
        ResetBall();
        Invoke("GoBall", 1);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player") || coll.collider.CompareTag("AI"))
        {
            speed = speedChange;
            Vector2 vel = new Vector2();
            vel.x = ballRigidBody.velocity.x;
            vel.y = (ballRigidBody.velocity.y / 2) + (coll.collider.attachedRigidbody.velocity.y / 3);
            
            if (!increasedOnce)
            {
                ballRigidBody.velocity = vel * speed;
                increasedOnce = true;
            }
            else
                ballRigidBody.velocity = vel;

        }
    }

    public void AddForce(Vector2 force)
    {
        ballRigidBody.AddForce(force);
    }
}
