using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUIController : MonoBehaviour {

    public Text lastText;
    public Text bestText;

    public Toggle KejiSnakeToggle;
    public Toggle YellowSnakeToggle;
    public Toggle borderToggle;
    public Toggle noBorderToggle;

    public Button gameStartButton;

    private void Awake()
    {
        GetScoreAndLength();
    }

    // Use this for initialization
    void Start () {

        KejiSnakeToggle.onValueChanged.AddListener(KejiSnakeToggleEvent);
        YellowSnakeToggle.onValueChanged.AddListener(YellowSnakeToggleEvent);
        borderToggle.onValueChanged.AddListener(BorderModeToggleEvent);
        noBorderToggle.onValueChanged.AddListener(NoBorderModeToggleEvent);

        gameStartButton.onClick.AddListener(GoToGame);


        GetLastGameSnakeSkin();
        GetLastGameMode();
    }


    private void GetScoreAndLength() {

        lastText.text = "上次：长度" + PlayerPrefs.GetInt("LastLength", 0) +
            "，分数" + PlayerPrefs.GetInt("LastScore", 0);

        bestText.text = "最好：长度" + PlayerPrefs.GetInt("BestLength", 0) +
            "，分数" + PlayerPrefs.GetInt("BestScore", 0);
    }

    private void GetLastGameSnakeSkin() {
        if (PlayerPrefs.GetString("SnakeHead", "sh01").Equals("sh01"))
        {
            KejiSnakeToggleEvent(true);
        }
        else {
            YellowSnakeToggleEvent(true);
        }
    }

    private void GetLastGameMode() {
        if (PlayerPrefs.GetInt("Border", 1) == 1)
        {
            BorderModeToggleEvent(true);
        }
        else {
            NoBorderModeToggleEvent(true);
        }
    }

    private void KejiSnakeToggleEvent(bool isOn) {
        if (isOn == true) {

            KejiSnakeToggle.isOn = true;

            PlayerPrefs.SetString("SnakeHead", "sh01");
            PlayerPrefs.SetString("SnakeBody01", "sb0101");
            PlayerPrefs.SetString("SnakeBody02", "sb0102");
        }
    }

    private void YellowSnakeToggleEvent(bool isOn)
    {
        if (isOn == true)
        {
            YellowSnakeToggle.isOn = true;

            PlayerPrefs.SetString("SnakeHead", "sh02");
            PlayerPrefs.SetString("SnakeBody01", "sb0201");
            PlayerPrefs.SetString("SnakeBody02", "sb0202");
        }
    }

    private void BorderModeToggleEvent(bool isOn)
    {
        if (isOn == true)
        {
            borderToggle.isOn = true;

            PlayerPrefs.SetInt("Border", 1);
        }
    }

    private void NoBorderModeToggleEvent(bool isOn)
    {
        if (isOn == true)
        {
            noBorderToggle.isOn = true;

            PlayerPrefs.SetInt("Border", 0);
        }
    }


    private void GoToGame() {

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
