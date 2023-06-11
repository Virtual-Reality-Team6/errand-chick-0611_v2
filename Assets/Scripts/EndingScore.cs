using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndingScore : MonoBehaviour
{
    public bool is_mainq_clear; // 퀘스트 진행여부
    public int subq_count;

    public GameObject failPanel;
    public GameObject scorePanel;
    public GameObject star1; // 게임 점수 시각화
    public GameObject star2;
    public GameObject star3;
    public Text failGuidText;
    public Text scoreGuidText;

    public GameObject endPanel;
    public GameObject manager;

    public Vector3 prefabPos; // 보상 오브젝트가 게임씬에 나타나는 위치(임시)

    // 랜덤 보상 결정
    int renum;

    public void goTitle()
    {
        endPanel.SetActive(false);
        failPanel.SetActive(false);
        scorePanel.SetActive(false);

        manager.GetComponent<GameManager>().GoTitle();
    }

    public void showReward(int reward_number)
    {
        Instantiate(Resources.Load("Reward Prefabs/" + reward_number.ToString()), prefabPos, Quaternion.identity);

    }

    public void getReward() // 게임 클리어 시 "처음으로" 버튼을 누르면 보상을 보여주고 타이틀로
    {
        endPanel.SetActive(false);
        showReward(renum);
        Invoke("goTitle", 10f);
    }

    public int calcScore()
    {
        int score = 0;
        switch (subq_count) // 서브퀘스트 0~2개: 1점, 3~4:2점 5:3점
        {
            case 0:
            case 1:
            case 2: score = 1; break;
            case 3:
            case 4: score = 2; break;
            case 5: score = 3; break;
        }
        return score;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 게임 클리어 여부 판단
        if (!is_mainq_clear) // 클리어 실패
        {
            // 클리어실패 윈도우 활성화
            failPanel.SetActive(true);
            //심부름 완료 여부 예 or 아니오, 스탬프 개수 0~5개
            failGuidText.text = "심부름 완료 : " + (is_mainq_clear ? "예" : "아니요") + "\n스탬프 개수 : " + subq_count;
        }
        else
        {
            // 윈도우 활성화
            scorePanel.SetActive(true);
            scoreGuidText.text = "심부름 완료 : " + (is_mainq_clear ? "예" : "아니요") + "\n스탬프 개수 : " + subq_count;

            // 점수 계산
            int score = calcScore();// 게임 점수, 별 1개~3개

            // 점수 이미지로 보여주기
            if (score == 1)
            {
                star1.gameObject.SetActive(true);
            }
            else if (score == 2)
            {
                star1.gameObject.SetActive(true);
                star2.gameObject.SetActive(true);
            }
            else if (score == 3)
            {
                star1.gameObject.SetActive(true);
                star2.gameObject.SetActive(true);
                star3.gameObject.SetActive(true);
            }

            renum = Random.Range(0, SaveDataManager.rewardLength);

            // 게임 결과 저장
            if (score > SaveDataManager.instance.nowPlayer.rewardScores[renum])
            { // 해당 보상의 최고점 업데이트
                SaveDataManager.instance.nowPlayer.rewardScores[renum] = score;
            }
            SaveDataManager.instance.SaveData(); // 데이터 저장
        }
    }

        // Update is called once per frame
    void Update()
    {

    }
}
