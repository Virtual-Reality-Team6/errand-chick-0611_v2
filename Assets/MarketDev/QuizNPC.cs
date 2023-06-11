using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class QuizNPC : Interactable
{

    CanvasHandler canvasHandler;
    Player player;

    public int foodCount;

    public int quizSync = 0;

    public void Start()
    {
        foodCount = GameManager.Instance.itemCount;
        canvasHandler = FindObjectOfType<CanvasHandler>();
        player = FindObjectOfType<Player>();
    }
    public override void Interact()
    {
        base.Interact();

        foreach (var item in player.iscompletedErrand)
        {
            if (!item) return;
        }

        player.isMove = false;


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CanvasHandler.Instance.quizPanel.SetActive(true);

        OpenQuiz1();
    }
    public void OpenQuiz1()
    {

        foreach (var item in canvasHandler.answerbuttons)
        {
            item.gameObject.SetActive(true);
        }
        canvasHandler.nextButton.gameObject.SetActive(false);

        canvasHandler.quizText.text = "가져온 물건의 개수는?";

        var rand = UnityEngine.Random.Range(0, 2);

        canvasHandler.answerbuttons[rand].onClick.RemoveAllListeners();
        canvasHandler.answerbuttons[rand].onClick.AddListener(NextSync);

        canvasHandler.answerText[rand].text = foodCount + "개";

        var randNum = UnityEngine.Random.Range(-2, 3);

        randNum += (randNum == 0) ? 1 : 0;

        canvasHandler.answerbuttons[1 - rand].onClick.RemoveAllListeners();
        canvasHandler.answerbuttons[1 - rand].onClick.AddListener(FailedSync);
        canvasHandler.answerText[1 - rand].text = foodCount + randNum + "개";
    }
    public void OpenQuiz2()
    {
        foreach (var item in canvasHandler.answerbuttons)
        {
            item.gameObject.SetActive(true);
        }
        canvasHandler.nextButton.gameObject.SetActive(false);



        canvasHandler.quizText.text = "받아야 하는 거스름 돈은?";

        var rand = UnityEngine.Random.Range(0, 2);

        canvasHandler.answerbuttons[rand].onClick.RemoveAllListeners();
        canvasHandler.answerbuttons[rand].onClick.AddListener(NextSync);

        canvasHandler.answerText[rand].text = (10 - foodCount) + "원";

        var randNum = UnityEngine.Random.Range(0, 3);

        randNum += (randNum == 0) ? 1 : 0;

        canvasHandler.answerbuttons[1 - rand].onClick.RemoveAllListeners();
        canvasHandler.answerbuttons[1 - rand].onClick.AddListener(FailedSync);
        canvasHandler.answerText[1 - rand].text = (10 - foodCount) + randNum + "원";
    }

    public void NextSync()
    {

        foreach (var item in canvasHandler.answerbuttons)
        {
            item.gameObject.SetActive(false);
        }
        canvasHandler.nextButton.gameObject.SetActive(true);
        canvasHandler.quizText.text = "정답입니다!";
        canvasHandler.nextText.text = "계속하기";
        if (quizSync == 0)
        {
            quizSync++;
            canvasHandler.nextButton.onClick.RemoveAllListeners();
            canvasHandler.nextButton.onClick.AddListener(OpenQuiz2);
        }
        else
        {
            quizSync++;
            canvasHandler.nextButton.onClick.RemoveAllListeners();
            canvasHandler.nextButton.onClick.AddListener(ClosePanel);
            canvasHandler.nextButton.onClick.AddListener(() => GameManager.Instance.endShopping = true);
            canvasHandler.nextButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        }
    }
    public void ClosePanel()
    {
        CanvasHandler.Instance.quizPanel.SetActive(false);
    }
    public void FailedSync()
    {
        foreach (var item in canvasHandler.answerbuttons)
        {
            item.gameObject.SetActive(false);
        }
        canvasHandler.nextButton.gameObject.SetActive(true);
        if (quizSync == 0)
        {
            canvasHandler.quizText.text = "다시 시도해 보세요";
            canvasHandler.nextText.text = "다시하기";
            canvasHandler.nextButton.onClick.RemoveAllListeners();
            canvasHandler.nextButton.onClick.AddListener(OpenQuiz1);
        }
        else
        {

            canvasHandler.quizText.text = "다시 시도해 보세요";
            canvasHandler.nextText.text = "다시하기";
            canvasHandler.nextButton.onClick.RemoveAllListeners();
            canvasHandler.nextButton.onClick.AddListener(OpenQuiz2);

        }
    }

}