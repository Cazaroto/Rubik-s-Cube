/* Author: Danilo Cazaroto
 * Date: 10-26-2019
 * This class manages the actions of the Title Screen.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject menuCanvas;
    public Toggle showTimerToggle;
    public Text timeElapsedText;
    public Text timerText;
    public Button undoButton;

    [Header("Cube")]
    public CubeManager cubeManager;

    float time, minutes, seconds;
    bool timerOn;


    // Start is called before the first frame update
    void Start()
    {
        //Set default value to the timer variable.
        timerOn = true;
        time = minutes = seconds = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Enable - Disable interactability of undo butotn.
        undoButton.interactable = cubeManager.canRotate;

        //Accumulate time
        time += Time.deltaTime;

        //Transform time value into a minutes.
        minutes = Mathf.Floor(time / 60);
        //Transform time value into a seconds.
        seconds = Mathf.Round(time % 60);

        //Treatments to show the right value from minutes and seconds.
        if (seconds > 59)
            seconds = 59;

        if (minutes < 0)
        {
            minutes = 0;
            seconds = 0;
        }

        if (seconds < 10)
            timerText.text = string.Format("{0}:0{1}", minutes, seconds);
        else
            timerText.text = string.Format("{0}:{1}", minutes, seconds);

        //Debug.Log($"Minutes: {minutes} - Seconds: {seconds}");
    }

    #region Private Methods

    #endregion

    #region Public Methods

    /// <summary>
    /// Opens the menu window.
    /// </summary>
    public void MenuButtonClicked()
    {
        Debug.Log("Menu button clicked.");

        menuCanvas.SetActive(true);
    }

    /// <summary>
    /// Closes the menu window.
    /// </summary>
    public void CloseButtonClicked()
    {
        Debug.Log("Close button clicked.");

        menuCanvas.SetActive(false);
    }

    /// <summary>
    /// Show/Hide the timer.
    /// </summary>
    public void ShowHideTimer(bool isOn)
    {
        Debug.Log("Show timer toggled.");

        timeElapsedText.gameObject.SetActive(isOn);
        timerText.gameObject.SetActive(isOn);
    }

    /// <summary>
    /// Restart the game.
    /// </summary>
    public void RestartButtonClicked()
    {
        Debug.Log("Restart button clicked.");
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Go back to the title Scene.
    /// </summary>
    public void TitleButtonClicked()
    {
        Debug.Log("Title button clicked.");
        SceneManager.LoadScene("Title");
    }

    #endregion
}
