using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SnakeHead : MonoBehaviour {

    public float velocity;
    public int step;
    public FoodMaker foodMaker;
    public MainUIController mainUIController;

    public GameObject snakeBodyPrefab;
    public Sprite[] snakeBodySprites = new Sprite[2];
    public Transform sankeBodyParent;

    public GameObject dieEffect;

    public AudioClip eatAudioClip;
    public AudioClip dieAudioClip;


    // Use this for initialization
    void Start () {

        GetSnakeSkin();

        x = 0;
        y = step;
        InvokeRepeating("Move", 0, velocity);

        isDie = false;  

    }
	
	// Update is called once per frame
	void Update () {

        Direction();
        SpeedBySpace();
    }


    private void Move() {

        headPos = transform.localPosition;
        transform.localPosition = new Vector3(headPos.x+x, headPos.y+y, headPos.z);

        // 蛇身移动：第一种方式 （适合单色蛇身）
        // 最后的身体挪到蛇头位置，实现移动
        //if (snakeBodyList.Count > 0) {
        //    snakeBodyList.Last().localPosition = headPos;
        //    snakeBodyList.Insert(0, snakeBodyList.Last());
        //    snakeBodyList.RemoveAt(snakeBodyList.Count -1);
        //}

        // 蛇身移动：第二种方式
        // 蛇身的位置依次往前面挪动，实现移动
        if (snakeBodyList.Count > 0)
        {
            // 把倒数第二个的蛇身位置给倒数第一个的蛇身，依次类推
            for (int i = snakeBodyList.Count -2; i>=0; i--) {
                snakeBodyList[i + 1].localPosition = snakeBodyList[i].localPosition;
            }
            // 第一个蛇身时蛇头的位置
            snakeBodyList[0].localPosition = headPos;
        }
    }

    private void Direction() {

        // 游戏不暂停的情况下
        if (mainUIController.isGamePause == false && isDie == false) {

            // 按下 A 键，且当前不是向右走，就向左走
            if (Input.GetKey(KeyCode.A) && (x != step))
            {
                // 转向后，旋转蛇头方向
                transform.localRotation = Quaternion.Euler(0, 0, 90);

                x = -step;
                y = 0;
            }

            if (Input.GetKey(KeyCode.D) && (x != -step))
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);

                x = step;
                y = 0;
            }

            if (Input.GetKey(KeyCode.W) && (y != -step))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);

                x = 0;
                y = step;
            }

            if (Input.GetKey(KeyCode.S) && (y != step))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 180);

                x = 0;
                y = -step;
            }

        }
        
    }

    private void SpeedBySpace() {

        // 游戏不暂停的情况下
        if (mainUIController.isGamePause == false && isDie == false)
        {

            // 按下空格键实现加速运动（即频繁调用 Move 次数）
            if (Input.GetKeyDown(KeyCode.Space))
            {

                CancelInvoke("Move");
                InvokeRepeating("Move", 0, velocity - 0.2f);

            }

            if (Input.GetKeyUp(KeyCode.Space))
            {

                CancelInvoke("Move");
                InvokeRepeating("Move", 0, velocity);

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 吃掉食物，并且重新生成食物，而且还有20%的几率生成奖励 Reward
        if (collision.gameObject.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
            mainUIController.UpdateScoreAndLength();

            // 20% 的几率生成 Reward
            foodMaker.MakeFoods((Random.Range(0,100) < 20 ? true : false));

            // 吃掉食物生成蛇身
            SnakeBodySpawn();
        }
        else if (collision.gameObject.CompareTag("Reward"))
        {
            Destroy(collision.gameObject);
            mainUIController.UpdateScoreAndLength(Random.Range(20, 200));

            // 吃掉食物生成蛇身
            SnakeBodySpawn();
        }
        else if (collision.gameObject.CompareTag("SnakeBody"))
        {
            Die();
        }
        else
        {
            // 有边界撞墙直接死亡
            if (mainUIController.isBorder == true) {
                Die();
            }
            else {

                switch (collision.gameObject.name)
                {
                    case "Up":
                        transform.localPosition = new Vector3(transform.localPosition.x,
                            -transform.localPosition.y + 30, transform.localPosition.z);
                        break;
                    case "Down":
                        transform.localPosition = new Vector3(transform.localPosition.x,
                            -transform.localPosition.y - 30, transform.localPosition.z);
                        break;

                    case "Left":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 120,
                            transform.localPosition.y, transform.localPosition.z);
                        break;

                    case "Right":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 180,
                            transform.localPosition.y, transform.localPosition.z);
                        break;

                }

            }

            
        }
    }

    private void SnakeBodySpawn() {

        // 吃掉的声音
        AudioSource.PlayClipAtPoint(eatAudioClip, Vector3.zero);

        // 三目计算依次赋予不同身体 Sprite
        int snakeBodyIndex = (snakeBodyList.Count % 2) == 0 ? 0 : 1;
        // 生成的时候，先把他放到屏幕外
        GameObject body = Instantiate(snakeBodyPrefab, new Vector3(2000, 2000, -2000), Quaternion.identity);
        body.GetComponent<Image>().sprite = snakeBodySprites[snakeBodyIndex];
        body.transform.SetParent(sankeBodyParent, false);
        snakeBodyList.Add(body.transform);
    }

    private void Die() {

        // 死亡的声音
        AudioSource.PlayClipAtPoint(dieAudioClip, Vector3.zero);

        CancelInvoke("Move");

        Instantiate(dieEffect);
        isDie = true;

        // 刷新保存游戏得分和长度
        UpdateScoreAndLengthRecoder();

        // 重新开始游戏
        StartCoroutine(RestartGame());
    }

    private void UpdateScoreAndLengthRecoder() {

        PlayerPrefs.SetInt("LastScore", mainUIController.score);
        PlayerPrefs.SetInt("LastLength", mainUIController.length);

        if (PlayerPrefs.GetInt("BestScore") < mainUIController.score)
        {
            PlayerPrefs.SetInt("BestScore", mainUIController.score);
        }
        if (PlayerPrefs.GetInt("BestLength") < mainUIController.length)
        {
            PlayerPrefs.SetInt("BestLength", mainUIController.length);
        }
    }

    IEnumerator RestartGame() {

        yield return new WaitForSeconds(2.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }


    private void GetSnakeSkin() {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("SnakePart/"+PlayerPrefs.GetString("SnakeHead", "sh02"));
        snakeBodySprites[0] = Resources.Load<Sprite>("SnakePart/"+PlayerPrefs.GetString("SnakeBody01", "sb0201"));
        snakeBodySprites[1] = Resources.Load<Sprite>("SnakePart/"+PlayerPrefs.GetString("SnakeBody02", "sb0202"));
    }

    private int x;
    private int y;
    private Vector3 headPos;
    private bool isDie;

    private List<Transform> snakeBodyList = new List<Transform>();
}
