using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createCubes : MonoBehaviour
{
    // Variables
    private int gridSize = 10;
    private int offset = 6;
    public GameObject prefabCube;

    void Start()
    {
        CreateCubes();
    }

    public int getGridSize()
    {
        return gridSize;
    }

    // Dynamically creates the grid of cubes
    public void CreateCubes()
    {
        for (int r = 1; r <= gridSize; r++)
        {
            Color color = Random.ColorHSV(0f, 1f, 0.3f, 0.5f, 0.5f, .7f);

            for (int c = 1; c <= gridSize; c++)
            {
                // Instansiate prefab (Cube&Text)
                GameObject currentCubeAndText = (GameObject)Instantiate(
                    prefabCube,
                    new Vector3(r - offset, -c + offset, -offset),
                    Quaternion.identity,
                    GameObject.Find("All Cubes").transform);

                // Get children of prefab
                GameObject currCube = currentCubeAndText.transform.GetChild(0).gameObject;
                GameObject currCanvasFront = currentCubeAndText.transform.GetChild(1).gameObject;
                GameObject currCanvasBack = currentCubeAndText.transform.GetChild(2).gameObject;
                GameObject currTextFront = currCanvasFront.transform.GetChild(0).gameObject;
                GameObject currTextBack = currCanvasBack.transform.GetChild(0).gameObject;

                // Rename children accordingly
                currentCubeAndText.transform.name = "Block [" + r + "][" + c + "]";
                currentCubeAndText.transform.GetChild(0).name = "Cube";

                // Setting data as needed
                //Texts
                currTextFront.GetComponent<TMPro.TextMeshProUGUI>().text = r + " X " + c;
                currTextBack.GetComponent<TMPro.TextMeshProUGUI>().text = "Answer: " + r * c;
                //Cube
                currCube.GetComponent<answerCube>().result = r * c;
                currCube.GetComponent<Renderer>().material.SetColor("_Color", color);
            }
        }
    }

}
