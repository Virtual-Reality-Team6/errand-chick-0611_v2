using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ShowGallery : MonoBehaviour
{
    // Gallery scene ���� �Լ�
    int rewardLen = SaveDataManager.rewardLength;

    public Text statusText;

    // ������Ʈ ��ġ ��ġ �Ķ���͵�
    public float radius;
    public float height;
    public float starheight;
    public GameObject player;

    Vector3 convertAngleToVec(float deg, float h)
    {
        var rad = deg * Mathf.Deg2Rad;
        return new Vector3(-Mathf.Cos(rad) * radius, h, Mathf.Sin(rad) * radius);
    }

    // game scene���� ���ư��� �Լ�, "ó������" ��ư Ŭ�� �� ȣ��
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

        SaveDataManager.instance.LoadData(); // ������ �ε�, �ӽ��̸� ��ĥ ���� ���� ������ ����

        for (int i = 0; i < rewardLen; i++) // ��� ����
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
                // ���� ���� ���������� ǥ��
                Instantiate(Resources.Load("Reward Prefabs/" + "Locked"), rewardPos, rewardRotY);

            }
            else
            {
                // ���� ���������� ǥ��
                Instantiate(Resources.Load("Reward Prefabs/" + i.ToString()), rewardPos, rewardRotY);
                // ������ ���� ������ ���� ǥ��
                Instantiate(Resources.Load("Reward Prefabs/Star" + score.ToString()), rewardPos + new Vector3(0, starheight, 0), rewardRotY);

                totalRewards++;
                totalScore += score;
            }
        }

        statusText.text = "�غ� : " + totalRewards + " / " + rewardLen + "       ���� : " + totalScore + " / " + rewardLen * 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
