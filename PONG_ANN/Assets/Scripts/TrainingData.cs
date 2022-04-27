using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TrainingData
{
    public List<List<string>> inputString = new List<List<string>>();
    public List<List<string>> targetString = new List<List<string>>();
    public List<string> inputValues;
    public List<string> targetValues;

    private GameObject _AIPaddle;
    private GameObject _Ball;

    public void SaveInputValues(int index)
    {
        _AIPaddle = GameObject.FindGameObjectWithTag("AI");
        _Ball = GameObject.FindGameObjectWithTag("Ball");

        var aiPaddle = _AIPaddle.GetComponent<Rigidbody2D>();
        var ball = _Ball.GetComponent<Rigidbody2D>();

        inputValues = new List<string> {
            aiPaddle.position.y + "," + ball.position.x + "," + ball.position.y + "," + ball.velocity.x + "," + ball.velocity.y
        };

        inputString.Add(inputValues);

        using StreamWriter input = File.AppendText(Application.streamingAssetsPath + "/" + GameManager.GetInstance().inputValueFileName);
        input.WriteLine(inputString[index][0]);
    }

    public static List<List<float>> ReadInputValues(string filePath)
    {
        List<List<float>> inputFloats = new List<List<float>>();

        if (filePath == null)
            return inputFloats;

        using StreamReader readInput = new StreamReader(Application.streamingAssetsPath + "/" + filePath);
        string data = readInput.ReadLine();
        while (data != null)
        {
            List<float> readInputFloats = new List<float>();
            string[] inputString = data.Split(',');

            for (int i = 0; i < inputString.Length; ++i)
                readInputFloats.Add(float.Parse(inputString[i]));

            inputFloats.Add(readInputFloats);

            data = readInput.ReadLine();
        }

        return inputFloats;
    }
    public void SaveTargetValues(float up, float down, int i)
    {
        targetValues = new List<string>
        {up + "," + down};

        targetString.Add(targetValues);

        using StreamWriter target = File.AppendText(Application.streamingAssetsPath + "/" + GameManager.GetInstance().targetValueFileName);
        target.WriteLine(targetString[i][0]);
    }

    public static List<List<float>> ReadTargetValues(string filePath)
    {
        List<List<float>> targetFloats = new List<List<float>>();

        if (filePath == null)
            return targetFloats;

        using StreamReader readInput = new StreamReader(Application.streamingAssetsPath + "/" + filePath);
        string data = readInput.ReadLine();
        while (data != null)
        {
            List<float> readTargetFloats = new List<float>();
            string[] inputString = data.Split(',');

            for (int i = 0; i < inputString.Length; ++i)
                readTargetFloats.Add(float.Parse(inputString[i]));

            targetFloats.Add(readTargetFloats);

            data = readInput.ReadLine();
        }

        return targetFloats;
    }
}
