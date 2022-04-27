using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    private Rigidbody2D aiRigidbody;
    private float timer = 0.0f;
    private int saveValueCount = 0;
    private int randomCount = 0;
    private int maxRandomRange = 0;
    public bool isModelTrained = false;
    public int modelIteration = 0;
    private int counter = 2;
    [SerializeField] private float speed;
    [SerializeField] private float boundY;

    [SerializeField] private KeyCode moveUp;
    [SerializeField] private KeyCode moveDown;

    private Vector2 velocity = new Vector2();

    private ML.NeuralNetwork neuralNetwork;
    private List<List<float>> inputValues;
    private List<List<float>> targetValues;
    private List<List<float>> currentInputValues;
    private List<float> outputValues;

    private List<int> topology;
    private TrainingData _TrainingData;

    private Rigidbody2D _BallRigidBody;

    private List<float> valueSet;

    // Start is called before the first frame update
    void Start()
    {
        aiRigidbody = GetComponent<Rigidbody2D>();
        _BallRigidBody = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();

        _TrainingData = new TrainingData();
        topology = new List<int>
        {
            5, 8, 2
        };

        neuralNetwork = new ML.NeuralNetwork(topology);
        inputValues = new List<List<float>>();
        inputValues = TrainingData.ReadInputValues(GameManager.GetInstance().inputValueFileName);
        targetValues = new List<List<float>>();
        targetValues = TrainingData.ReadTargetValues(GameManager.GetInstance().targetValueFileName);

        outputValues = new List<float> { new float(), new float(), new float() };
        maxRandomRange = TrainingData.ReadInputValues(GameManager.GetInstance().inputValueFileName).Count - 1;

        valueSet = new List<float>
        {
            aiRigidbody.position.y, _BallRigidBody.position.x, _BallRigidBody.position.y,
            _BallRigidBody.velocity.x, _BallRigidBody.velocity.y
        };

        currentInputValues = new List<List<float>>
        {
            valueSet
        };

        if (isModelTrained)
            StartCoroutine(TrainModel());
    }

    IEnumerator TrainModel()
    {
        for (int i = 0; i < modelIteration; ++i)
        {
            randomCount = Random.Range(0, maxRandomRange);
            neuralNetwork.FeedForward(inputValues[randomCount]);
            outputValues = neuralNetwork.GetResults();
            neuralNetwork.BackPropogate(targetValues[randomCount]);
        }
        Debug.Log("Model Trained!");

        yield return null;
    }

    void MoveUp()
    {
        Vector2 upDir = new Vector2(0.0f, 1.0f);
        velocity = upDir.normalized * speed;
        aiRigidbody.position += velocity * Time.fixedDeltaTime;
        Debug.Log("Moving Up");
    }

    void MoveDown()
    {
        Vector2 downDir = new Vector2(0.0f, -1.0f);
        velocity = downDir.normalized * speed;
        aiRigidbody.position += velocity * Time.fixedDeltaTime;
        Debug.Log("Moving Down");
    }


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (!isModelTrained)
        {
            Vector2 input = new Vector2();
            if (Input.GetKey(moveUp))
                input = new Vector2(0.0f, 1.0f);
            else if (Input.GetKey(moveDown))
                input = new Vector2(0.0f, -1.0f);
            else
                input = new Vector2(0.0f, 0.0f);

            if (timer > 0.5f)
            {
                timer = 0.0f;
                _TrainingData.SaveInputValues(saveValueCount);

                if (Input.GetKey(moveUp))
                    _TrainingData.SaveTargetValues(1.0f, 0.0f, saveValueCount);
                else
                    _TrainingData.SaveTargetValues(0.0f, 1.0f, saveValueCount);

                saveValueCount += 1;
            }

            velocity = input.normalized * speed;
        }
        else
        {
            //if(timer > 0.5f)
            //{
            //    timer = 0.0f;
            //    _TrainingData.SaveInputValues(saveValueCount);

            //    if (outputValues[0] > outputValues[1])
            //        _TrainingData.SaveTargetValues(1.0f, 0.0f, saveValueCount);
            //    else
            //        _TrainingData.SaveTargetValues(0.0f, 1.0f, saveValueCount);

            //    saveValueCount += 1;
            //}
            if (timer > 0.5f)
            {
                neuralNetwork.FeedForward(GetValues());
                outputValues = neuralNetwork.GetResults();
            }
        }

    }

    private void FixedUpdate()
    {
        var pos = aiRigidbody.position;
        if (pos.y > boundY)
            pos.y = boundY;
        else if (pos.y < -boundY)
            pos.y = (-boundY);
        aiRigidbody.position = pos;

        if (isModelTrained)
        {
            if (outputValues[0] > outputValues[1])
                MoveUp();
            else
                MoveDown();
        }
        else
            aiRigidbody.MovePosition(aiRigidbody.position + velocity * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        if (GameManager.GetInstance().matchCount == counter)
        { 
            StartCoroutine(TrainModel());
            counter += 2;
        }
    }

    List<float> GetValues()
    {
        if (timer > 0.5f)
        {
            currentInputValues[0][0] = aiRigidbody.position.y;
            currentInputValues[0][1] = _BallRigidBody.position.x;
            currentInputValues[0][2] = _BallRigidBody.position.y;
            currentInputValues[0][3] = _BallRigidBody.velocity.x;
            currentInputValues[0][4] = _BallRigidBody.velocity.y;
            timer = 0.0f;
        }
        return currentInputValues[0];

    }
}
