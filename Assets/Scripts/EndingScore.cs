using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndingScore : MonoBehaviour
{
    public bool is_mainq_clear; // ����Ʈ ���࿩��
    public int subq_count;

    public GameObject failPanel;
    public GameObject scorePanel;
    public GameObject star1; // ���� ���� �ð�ȭ
    public GameObject star2;
    public GameObject star3;
    public Text failGuidText;
    public Text scoreGuidText;

    public GameObject endPanel;
    public GameObject manager;

    public Vector3 prefabPos; // ���� ������Ʈ�� ���Ӿ��� ��Ÿ���� ��ġ(�ӽ�)

    // ���� ���� ����
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

    public void getReward() // ���� Ŭ���� �� "ó������" ��ư�� ������ ������ �����ְ� Ÿ��Ʋ��
    {
        endPanel.SetActive(false);
        showReward(renum);
        Invoke("goTitle", 10f);
    }

    public int calcScore()
    {
        int score = 0;
        switch (subq_count) // ��������Ʈ 0~2��: 1��, 3~4:2�� 5:3��
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
        // ���� Ŭ���� ���� �Ǵ�
        if (!is_mainq_clear) // Ŭ���� ����
        {
            // Ŭ������� ������ Ȱ��ȭ
            failPanel.SetActive(true);
            //�ɺθ� �Ϸ� ���� �� or �ƴϿ�, ������ ���� 0~5��
            failGuidText.text = "�ɺθ� �Ϸ� : " + (is_mainq_clear ? "��" : "�ƴϿ�") + "\n������ ���� : " + subq_count;
        }
        else
        {
            // ������ Ȱ��ȭ
            scorePanel.SetActive(true);
            scoreGuidText.text = "�ɺθ� �Ϸ� : " + (is_mainq_clear ? "��" : "�ƴϿ�") + "\n������ ���� : " + subq_count;

            // ���� ���
            int score = calcScore();// ���� ����, �� 1��~3��

            // ���� �̹����� �����ֱ�
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

            // ���� ��� ����
            if (score > SaveDataManager.instance.nowPlayer.rewardScores[renum])
            { // �ش� ������ �ְ��� ������Ʈ
                SaveDataManager.instance.nowPlayer.rewardScores[renum] = score;
            }
            SaveDataManager.instance.SaveData(); // ������ ����
        }
    }

        // Update is called once per frame
    void Update()
    {

    }
}
