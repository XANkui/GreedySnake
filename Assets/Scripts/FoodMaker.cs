using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodMaker : MonoBehaviour {

    public GameObject foodPrefab;
    public Transform foodsParent;
    public Sprite[] foodSprites;

    public GameObject rewardPrefab;

    // Use this for initialization
    void Start () {

        // 刚开始不生成奖励
        MakeFoods(false);

    }

    internal void MakeFoods(bool isReward) {

        int foodSpriteIndex = Random.Range(0, foodSprites.Length);
        GameObject food = Instantiate(foodPrefab);
        food.transform.SetParent(foodsParent, false);
        food.GetComponent<Image>().sprite = foodSprites[foodSpriteIndex];

        int x = Random.Range(-xLimit + xOffset, xLimit);
        int y = Random.Range(-yLimit, yLimit);
        food.transform.localPosition = new Vector3(x * 30, y * 30, 0);

        // 生成奖励
        if (isReward == true) {

            GameObject reward = Instantiate(rewardPrefab);
            reward.transform.SetParent(foodsParent, false);

            x = Random.Range(-xLimit + xOffset, xLimit);
            y = Random.Range(-yLimit, yLimit);
            food.transform.localPosition = new Vector3(x * 30, y * 30, 0);

        }

    }

    private int xLimit = 11;
    private int yLimit = 9;
    private int xOffset = 4;
}
