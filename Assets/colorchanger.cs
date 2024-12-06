using System.Collections;
using UnityEngine;
using TMPro; // TextMeshPro namespace for UI
using UnityEngine.UI; // For handling the UI elements

public class Colorchanger : MonoBehaviour
{
    // References to objects and UI elements
    public GameObject cylinder;               // Reference to the cylinder
    public GameObject cube;                   // Reference to the cube
    public Material[] colorMaterials;         // Array of color materials          

    private Material correctCylinderColor;    // Correct color for cylinder
    private Material correctCubeColor;        // Correct color for cube
    private int score = 0;                    // Player's score
    private int round = 0;                    // Current round number

    private bool isSelectingColors = false;   // Flag to track if player is selecting colors
    private bool cylinderColorAssigned = false; // Track if cylinder color is assigned
    private bool cubeColorAssigned = false; // Track if cube color is assigned

    // Start is called before the first frame update
    void Start()
    {
        // Start the game
        StartCoroutine(ShowRandomColorForTime());
    }

    // Coroutine to show random colors for 3 seconds
    IEnumerator ShowRandomColorForTime()
    {
        // Randomly assign colors to objects
        correctCylinderColor = colorMaterials[Random.Range(0, colorMaterials.Length)];
        correctCubeColor = colorMaterials[Random.Range(0, colorMaterials.Length)];

        cylinder.GetComponent<Renderer>().material = correctCylinderColor;
        cube.GetComponent<Renderer>().material = correctCubeColor;

        // Show colors for 3 seconds
        yield return new WaitForSeconds(3);

        // Revert to white (default)
        cylinder.GetComponent<Renderer>().material.color = Color.white;
        cube.GetComponent<Renderer>().material.color = Color.white;

        // Allow player to start selecting colors
        isSelectingColors = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelectingColors)
        {
            if (Input.GetMouseButtonDown(0)) // Left-click to select colors
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject selectedObject = hit.collider.gameObject;

                    // Check if the player clicked on a color plane
                    if (selectedObject.CompareTag("ColorPlane"))
                    {
                        Material selectedMaterial = selectedObject.GetComponent<Renderer>().material;

                        // Assign the color to the correct object (cylinder or cube)
                        if (!cylinderColorAssigned)
                        {
                            AssignColorToObject(selectedMaterial, "Cylinder");
                            cylinderColorAssigned = true;
                        }
                        else if (!cubeColorAssigned)
                        {
                            AssignColorToObject(selectedMaterial, "Cube");
                            cubeColorAssigned = true;

                            // After both colors are assigned, show them for 3 seconds
                            StartCoroutine(ShowColorsForTime());
                        }
                    }
                }
            }
        }
    }

    // Function to assign selected color to the object
    void AssignColorToObject(Material selectedMaterial, string objectName)
    {
        if (objectName == "Cylinder")
        {
            cylinder.GetComponent<Renderer>().material = selectedMaterial;
        }
        else if (objectName == "Cube")
        {
            cube.GetComponent<Renderer>().material = selectedMaterial;
        }
    }

    // Show the colors for 3 seconds and check if they are correct
    IEnumerator ShowColorsForTime()
    {
        // Show the colors for 3 seconds
        yield return new WaitForSeconds(3);

        // Check if the colors are correct
        CheckCorrectColors();

        // Reset the scene and prepare for the next round
        round++;
        if (round >= 5)
        {
            EndGame();
        }
        else
        {
            ResetObjectsAndStart();
        }
    }

    // Function to compare the colors and update the score
    void CheckCorrectColors()
    {
        // Compare the color values of the materials
        bool isCylinderCorrect = cylinder.GetComponent<Renderer>().material.color == correctCylinderColor.color;
        bool isCubeCorrect = cube.GetComponent<Renderer>().material.color == correctCubeColor.color;

        if (isCylinderCorrect && isCubeCorrect)
        {
            score++;
            Debug.Log("Correct! Both objects have the correct colors.");
        }
        else
        {
            Debug.Log("Incorrect. Try again.");
        }

        // UpdateScoreText(); // Update the score display
    }

    // // Update the score text UI
    // void UpdateScoreText()
    // {
    //     if (scoreText != null)
    //     {
    //         scoreText.text = "Score: " + score + "/5"; // Update the UI with the current score
    //     }
    // }

    // Reset objects to white and start the next round
    void ResetObjectsAndStart()
    {
        // Reset to white and prepare for the next round
        cylinder.GetComponent<Renderer>().material.color = Color.white;
        cube.GetComponent<Renderer>().material.color = Color.white;

        // Reset flags for the next round
        cylinderColorAssigned = false;
        cubeColorAssigned = false;

        // Start the color change cycle again
        isSelectingColors = false;
        StartCoroutine(ShowRandomColorForTime());
    }

    // End the game and show final score and replay button
    void EndGame()
    {
        // Display the score and show the replay button
        Debug.Log("Game Over! Your score: " + score + "/5");
        // if (scoreText != null)
        // {
        //     scoreText.text = "Game Over! Your score: " + score + "/5"; // Display final score
        // }

        // replayCard.SetActive(true); // Show the Replay Panel when game ends
    }

}