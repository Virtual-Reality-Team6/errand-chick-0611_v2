using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelloNPC : MonoBehaviour
{
    public RectTransform helloUiGroup;
    public GameObject gameCam;
    public GameObject interactionCam;

    public int interactionIndex;
    public string[] talkData;
    public string[] questionData;
    public string[] answerData;
    public int currentQuestionIndex;
    
    public Image outsideTalkImg;
    public Text outsideTalkText;

    public Image QuestionImg;
    public Text QuestionText;
    public Button correctAnswerButton;
    public Text correctButtonText;
    public Button wrongAnswerButton;
    public Text wrongButtonText;

    Player enterPlayer;
    public float rotationSpeed = 5f;

    public void Enter(Player player)
    {
        enterPlayer = player;
        if(enterPlayer.iscompletedStamp[interactionIndex]){
            Exit();
        }
        else{
            outsideTalkImg.gameObject.SetActive(false);
            helloUiGroup.anchoredPosition = Vector3.down * 1000;
            RotateTowardsPlayer();
            interactionCam.SetActive(true);
            gameCam.SetActive(false);
            currentQuestionIndex = 0;
            setUI(currentQuestionIndex);
        }
    }
    
    private void RotateTowardsPlayer()
    {
        Vector3 targetDirection = enterPlayer.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = targetRotation;
    }

    private void setUI(int index)
    {
        QuestionText.text = questionData[index];
        correctButtonText.text = answerData[index*2];
        wrongButtonText.text = answerData[index*2+1];

        QuestionImg.rectTransform.sizeDelta = new Vector2(CalculateImgWidth(QuestionText.text), QuestionImg.rectTransform.sizeDelta.y);

        RectTransform correctButtonRect = correctAnswerButton.GetComponent<RectTransform>();
        correctButtonRect.sizeDelta = new Vector2(CalculateImgWidth(correctButtonText.text), correctButtonRect.sizeDelta.y);
        
        RectTransform wrongButtonRect = wrongAnswerButton.GetComponent<RectTransform>();
        wrongButtonRect.sizeDelta = new Vector2(CalculateImgWidth(wrongButtonText.text), wrongButtonRect.sizeDelta.y);
    }

    public void Exit()
    {
        helloUiGroup.anchoredPosition = Vector3.zero;
        interactionCam.SetActive(false);
        gameCam.SetActive(true);
        if(enterPlayer != null && enterPlayer.isInteraction) {
            StartCoroutine(ShowImageForDuration(1f));
        }
    }

    private IEnumerator ShowImageForDuration(float duration)
    {
        outsideTalkImg.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        outsideTalkImg.gameObject.SetActive(false);
    }

    public void CorrectAnswerTalk()
    {
        if(currentQuestionIndex + 1 == questionData.Length){
            enterPlayer.iscompletedStamp[interactionIndex] = true;
            DisplayNPCTalk(0);
            Exit();
            enterPlayer.Stamp();
        }
        else{
            setUI(++currentQuestionIndex);
        }
    }

    public void UncorrectAnswerTalk()
    {
        DisplayNPCTalk(1);
        Exit();
    }

    void DisplayNPCTalk(int talkDataIndex)
    {
        outsideTalkText.text = talkData[talkDataIndex];
        float widthInPixels = CalculateImgWidth(outsideTalkText.text);
        outsideTalkImg.rectTransform.sizeDelta = new Vector2(widthInPixels, outsideTalkImg.rectTransform.sizeDelta.y);
    }

    private float CalculateImgWidth(string talkText)
    {
        int spaceCount = CountSpaces(talkText);
        int nonSpaceCount = talkText.Length - spaceCount;

        float widthInPixels = nonSpaceCount * 65 + spaceCount * 9;
        return widthInPixels;
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
}
