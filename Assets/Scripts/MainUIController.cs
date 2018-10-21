using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainUIController : MonoBehaviour {

    public Image BGImage;
    public Text ModeText;
    public Text ScoreText;
    public Text LengthText;

    public Image pauseImage;
    public Button pauseButton;
    public Sprite[] pausePlaySprites;

    public Button homeButton;

	// Use this for initialization
	void Start () {
        isGamePause = false;
        score = 0;
        length = 0;

        pauseButton.onClick.AddListener(GamePauseAndPlay);
        homeButton.onClick.AddListener(BackMainMenuScene);

        ShowHideBorder(PlayerPrefs.GetInt("Border", 0) == 0 ? false : true);

    }
	
	// Update is called once per frame
	void Update () {
        UpdateBGImageColor();

    }


    internal void UpdateScoreAndLength(int score = 10, int length = 1) {
        this.score += score;
        this.length += length;


        ScoreText.text = "得分：\n" + this.score;
        LengthText.text = "长度：\n" + this.length;

    }

    private void UpdateBGImageColor() {

        switch (this.score / 100) {

            case 1:
                ModeText.text = "阶段：" + 1;
                ColorUtility.TryParseHtmlString("#CCEEFFFF", out tempColor);
                BGImage.color = tempColor;
                break;

            case 2:
                ModeText.text = "阶段：" + 2;
                ColorUtility.TryParseHtmlString("#CC00FFFF", out tempColor);
                BGImage.color = tempColor;
                break;

            case 3:
            case 4:
            case 5:
                ModeText.text = "无尽模式";
                ColorUtility.TryParseHtmlString("#000000FF", out tempColor);
                BGImage.color = tempColor;
                break;
        }
    }

    private void GamePauseAndPlay() {

        isGamePause = !isGamePause;

        // 游戏暂停
        if (isGamePause == true)
        {

            Time.timeScale = 0;
            pauseImage.sprite = pausePlaySprites[1];
        }
        else {

            Time.timeScale = 1;
            pauseImage.sprite = pausePlaySprites[0];
        }

    }

    private void BackMainMenuScene() {

        // 返回主界面， timeScale 恢复 1
        if (isGamePause ==true) {
            Time.timeScale = 1;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void ShowHideBorder(bool isBorder) {

        this.isBorder = isBorder;

        foreach (Transform t in BGImage.gameObject.transform)
        {
            t.GetComponent<Image>().enabled = isBorder;
        }
    }

    internal bool isGamePause;
    internal int score;
    internal int length;
    internal bool isBorder;
    private Color tempColor;

    
    
}
