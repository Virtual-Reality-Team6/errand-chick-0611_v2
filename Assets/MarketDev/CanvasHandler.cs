using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHandler : Singleton<CanvasHandler>
{

    [SerializeField]
    public GameObject selectUIButton;

    [SerializeField]
    public GameObject crossUIText;

    [SerializeField]
    public GameObject quizPanel;

    [SerializeField]
    public Text[] answerText = new Text[2];

    [SerializeField]
    public Button[] answerbuttons = new Button[2];

    [SerializeField]
    public Button nextButton;

    [SerializeField]
    public Text nextText;

    public Text quizText;


}
