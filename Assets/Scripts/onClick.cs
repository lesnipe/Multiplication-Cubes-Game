using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onClick : MonoBehaviour
{
    private bool answering = false;
    private bool destroyed = true;
    private bool showAnswer = false;
    private bool pressedOnce = false;
    private float timer = 0f;
    private float timerEnd = 3.5f;
    private float animationSpeed = 5.0f;
    private int gridSize;
    private int cubesClickedSum = 0;
    private int correctAnswersSum = 0;
    private int lastCorrectAnswer;
    private int points = 0;
    private string lastUserAnswer;
    private GameObject allCubes;
    private GameObject lastClickedBlock = null;
    private GameObject inputFieldCanvas;
    private GameObject stringInputField;
    private GameObject successMessage;
    private GameObject wrongMessage;
    private TextMesh pointsText;
    private Vector3 frontPosition = new Vector3 (0, 0, -10);
    private Vector3 frontDownPosition = new Vector3(0, -1, -10);
    private Vector3 frontUpPosition = new Vector3(0, 2, -10);
    private Vector3 hidePosition = new Vector3(0, 0, -25);

    // Start is called before the first frame update
    void Start()
    {
        pointsText = GameObject.Find("Points").GetComponent<TextMesh>();
        inputFieldCanvas = GameObject.Find("Answer Input");
        stringInputField = GameObject.Find("Read Input");
        successMessage = GameObject.Find("Success Message");
        wrongMessage = GameObject.Find("Wrong Message");
        gridSize = GameObject.Find("All Cubes").GetComponent<createCubes>().getGridSize();
    }

    // Update is called once per frame
    void Update()
    {
        OnClick();
        TriggerAnsweringState();
        DestroyCube();

        // Change state to not answering
        if (destroyed)
        {
            answering = false;
        }
    }

    void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If clicked on any cube
            if (Physics.Raycast(ray, out hit, 100) && !answering)
            {
                lastCorrectAnswer = hit.transform.gameObject.GetComponent<answerCube>().result;
                cubesClickedSum++;

                // Save last clicked block
                lastClickedBlock = hit.transform.parent.gameObject;

                // Change state to answering & not destroyed
                destroyed = false;
                answering = true;   
            }
        }
    }

    private void TriggerAnsweringState()
    {
        // User answering actions
        if (answering)
        {
            // Move cube to frontPosition
            lastClickedBlock.transform.position = frontPosition;
            lastClickedBlock.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.white);

            // Move input field to frontDownPosition
            inputFieldCanvas.transform.position = frontDownPosition;

            // User entered input (1 time only)
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !pressedOnce )
            {
                pressedOnce = true;
                string[] inputArray = stringInputField.GetComponent<readInput>().input.ToArray();
                lastUserAnswer = inputArray[inputArray.Length - 1];

                // Validation according to user's input && correct answer
                if (lastCorrectAnswer.ToString() == lastUserAnswer)
                {
                    // Show successMessage
                    successMessage.transform.position = frontUpPosition;
                    points++;
                    correctAnswersSum++;
                    pointsText.text = "Points: " + points;
                }
                else
                {
                    // Show wrongMessage
                    wrongMessage.transform.position = frontUpPosition;
                    points -= 1;
                    pointsText.text = "Points: " + points;
                    
                } 

                // Change state to show answer
                showAnswer = true;
            }
        }
    }

    private void DestroyCube()
    {
        // Show answer state lasts for ~ 3sec
        if (showAnswer && timer <= timerEnd)
        {
            timer += Time.deltaTime;

            // Rotate cube to show answer
            if (timer <= 1.0f)
            {
                lastClickedBlock.transform.Rotate(Vector3.up * Time.deltaTime * 540);
            }

            // Text animation
            if (timer <= timerEnd && timer > 1.0f)
            {
                wrongMessage.transform.position -= transform.right * animationSpeed * Time.deltaTime;
                successMessage.transform.position += transform.right * animationSpeed *  Time.deltaTime;
            }

            // Destory cube on last tick - Hide stuff - change states accordingly - reset timer
            if (timer >= timerEnd - 0.01f)
            {
                Destroy(lastClickedBlock);
                inputFieldCanvas.transform.position = hidePosition;
                successMessage.transform.position = hidePosition;
                wrongMessage.transform.position = hidePosition;
                destroyed = true;
                showAnswer = false;
                pressedOnce = false;
                timer = 0f;

                // Check if game over
                if (points < 0)
                {
                    Debug.Log("GAME OVER!\n");
                    Debug.Break();
                }

                // Check if won the game (If clicked on all cubes)
                if (cubesClickedSum == (gridSize * gridSize))
                {
                    float correctAnswersPersentage = correctAnswersSum*100f / (gridSize * gridSize);
                    Debug.Log("Congratulations, you won the game!!! " +
                        "\nTotal Points: " + points + 
                        ",  Total Correct Answers Rate: " + correctAnswersPersentage + '%' );
                    Debug.Break();
                }
            }
        }
    }
}
