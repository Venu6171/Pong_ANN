using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounciness : MonoBehaviour
{
    [SerializeField] private float bounceStrength;

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var ball = collision.gameObject.GetComponent<BallController>();

        Vector2 normal = collision.GetContact(0).normal;
        ball.AddForce(-normal * bounceStrength);
    }
}
