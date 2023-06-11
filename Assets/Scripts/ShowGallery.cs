using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ShowGallery : MonoBehaviour
{
    // Gallery scene 관련 함수
    int rewardLen = SaveDataManager.rewardLength;

    public Text statusText;

    // 오브젝트 배치 위치 파라미터들
    public float radius;
    public float height;
    public float starheight;
    public GameObject player;

    Vector3 convertAngleToVec(float deg, float h)
    {
        var rad = deg * Mathf.Deg2Rad;
        return new Vector3(-Mathf.Cos(rad) * radius, h, Mathf.Sin(rad) * radius);
    }

    // game scene으로 돌아가는 함수, "처음으로" 버튼 클릭 시 호출
    public void goTitleScene()
    {
        SceneManager.LoadScene("Game");
    }

    // Start is called before the first frame update
    void Start()
    {
        int score;
        Quaternion rewardRotY;
        Vector3 rewardPos;
        int totalRewards = 0;
        int totalScore = 0;

        SaveDataManager.instance.LoadData(); // 데이터 로드, 임시이며 합칠 때는 게임 씬에서 동작

        for (int i = 0; i < rewardLen; i++) // 모든 보상
        {
            score = SaveDataManager.instance.nowPlayer.rewardScores[i];
            //score = 1;
            rewardRotY = Quaternion.Euler(0, 45 * i % 360, 0);
            rewardPos = convertAngleToVec(45 * i % 360, 0.1f + (i / 8) * height) + player.transform.position;
            Debug.Log(score);
            Debug.Log(rewardRotY);
            Debug.Log(rewardPos);


            if (score == 0)
            {
                // 얻지 못한 아이템으로 표시
                Instantiate(Resources.Load("Reward Prefabs/" + "Locked"), rewardPos, rewardRotY);

            }
            else
            {
                // 얻은 아이템으로 표시
                Instantiate(Resources.Load("Reward Prefabs/" + i.ToString()), rewardPos, rewardRotY);
                // 아이템 위에 점수로 별을 표시
                Instantiate(Resources.Load("Reward Prefabs/Star" + score.ToString()), rewardPos + new Vector3(0, starheight, 0), rewardRotY);

                totalRewards++;
                totalScore += score;
            }
        }

        statusText.text = "준비물 : " + totalRewards + " / " + rewardLen + "       점수 : " + totalScore + " / " + rewardLen * 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
