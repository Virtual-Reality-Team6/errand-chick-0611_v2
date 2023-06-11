using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public RectTransform uiGroup;
    public GameObject gameCam;
    public GameObject interactionCam;

    public int interactionIndex;
    public string[] talkData;
    
    public Image talkImg;
    public Text talkText;

    Player enterPlayer;
    public float rotationSpeed = 5f;

    public void Enter(Player player)
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.down * 1000;
        RotateTowardsPlayer();
        interactionCam.SetActive(true);
        gameCam.SetActive(false);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 targetDirection = enterPlayer.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = targetRotation;
    }

    public void Exit()
    {
        uiGroup.anchoredPosition = Vector3.zero;
        interactionCam.SetActive(false);
        gameCam.SetActive(true);
    }

    public void CorrectAnswerTalk()
    {
        enterPlayer.iscompletedStamp[interactionIndex] = true;
        Exit();
        DisplayNPCTalk(0);
        enterPlayer.Stamp();
    }

    public void UncorrectAnswerTalk()
    {
        Exit();
        DisplayNPCTalk(1);
    }

    void DisplayNPCTalk(int talkDataIndex)
    {
        talkText.text = talkData[talkDataIndex];
        int spaceCount = CountSpaces(talkData[talkDataIndex]);
        int nonSpaceCount = talkData[talkDataIndex].Length - spaceCount;

        Debug.Log("Space Count: " + spaceCount);
        Debug.Log("Non-Space Count: " + nonSpaceCount);

        float widthInPixels = nonSpaceCount * 60 + spaceCount * 9;
        talkImg.rectTransform.sizeDelta = new Vector2(widthInPixels, talkImg.rectTransform.sizeDelta.y);
        talkImg.gameObject.SetActive(true);
    }

    private int CountSpaces(string text)
    {
        int count = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == ' ')
            {
                count++;
            }
        }
        return count;
    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
