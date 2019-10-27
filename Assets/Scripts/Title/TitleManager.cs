/* Author: Danilo Cazaroto
 * Date: 10-26-2019
 * This class manages the actions of the Title Screen.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("Cube Type Canvas Object")]
    public GameObject cubeTypeWindow;

    [Header("Cube Type Dropdown")]
    public Dropdown cubeTypeDropdown;

    [Header("Continue Button")]
    public Button continueBtn;

    //Awake is called before the first frame update
    void Awake()
    {
        cubeTypeDropdown.value = 1;
        cubeTypeDropdown.interactable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Private Methods

    #endregion

    #region Public Methods

    /// <summary>
    /// This method initiate a new game.
    /// </summary>
    public void NewGameClicked()
    {
        Debug.Log("New game clicked.");

        //Open the cube type window.
        cubeTypeWindow.SetActive(true);
    }

    /// <summary>
    /// This method continues the previous game, if there is one.
    /// </summary>
    public void ContinueGameClicked()
    {
        Debug.Log("Continue game clicked.");
    }

    /// <summary>
    /// This method closes the Cube Type Window.
    /// </summary>
    public void CloseCubeTypeWindowClicked()
    {
        Debug.Log("Close cube type window clicked.");

        cubeTypeWindow.SetActive(false);
    }

    /// <summary>
    /// This method call the game scene.
    /// </summary>
    public void StartClicked()
    {
        Debug.Log("Start button clicked.");

        SceneManager.LoadScene("Game");
    }

    #endregion
}
