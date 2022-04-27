using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    [SerializeField] private float speed;
    [SerializeField] private float boundY;

    [SerializeField] private KeyCode moveUp;
    [SerializeField] private KeyCode moveDown;

    private GameObject _Ball;

    private Vector2 velocity = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        _Ball = GameObject.FindGameObjectWithTag("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2();
        //if (Input.GetKey(moveUp))
        //    input = new Vector2(0.0f, 1.0f);
        //else if (Input.GetKey(moveDown))
        //    input = new Vector2(0.0f, -1.0f);

        if (_Ball.GetComponent<Rigidbody2D>().position.y > playerRigidBody.position.y)
            input = new Vector2(0.0f, 1.0f);
        else if (_Ball.GetComponent<Rigidbody2D>().position.y < playerRigidBody.position.y)
            input = new Vector2(0.0f, -1.0f);

        velocity = input.normalized * speed;
    }

    private void FixedUpdate()
    {
        var pos = playerRigidBody.position;
        if (pos.y > boundY)
            pos.y = boundY;
        else if (pos.y < -boundY)
            pos.y = (-boundY);
        playerRigidBody.position = pos;

        playerRigidBody.MovePosition(playerRigidBody.position + velocity * Time.fixedDeltaTime);
    }
}
