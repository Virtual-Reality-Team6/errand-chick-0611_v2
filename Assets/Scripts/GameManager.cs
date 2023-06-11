using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class GameManager : Singleton<GameManager>
{
    //게임 매니저가 필요한 변수 세팅
    public GameObject startCam;
    public GameObject gameCam;
    public Player player;

    public Button startButton;
    public Button ContinueButton;
    public bool hasPlaylog;
    public Button GalleryButton;
    public GameObject namePanel;
    public InputField newPlayerName;

    public string[] errandLists;
    public GameObject[] ErrandListObjects;
    public Text[] errandTexts;
    public Image checkFrontAImg;
    public Image checkFrontBImg;
    public Image checkFrontCImg;
    public Image checkFrontDImg;
    public Text listMamaText;

    public GameObject stampA;
    public GameObject stampB;
    public GameObject stampC;
    public GameObject stampD;
    public GameObject stampE;
    
    public float playTime;
    private float playTimeLimit = 10 * 60;
    public RectTransform timeBar;

    public bool IsTimerRunning;
    public GameObject TimeOutPanel;
    public Text TimeOutText;
    public Text menuTimeText;

    public GameObject startPanel;
    public GameObject gamePanel;
    public Text playTimeText;
    public Text playerCoinText;

    public GameObject endPanel;

    public Dictionary<ItemType, int> itemPool = new Dictionary<ItemType, int>();

    public int itemCount;

    public bool endShopping = false;

    public int selectionCount = 4;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        Debug.Log("awake");
        //checkSaveFile();
        checkPlayLog();
        makeErrandList();
    }

    void checkSaveFile()
    {
        // 세이브파일이 이미 있는지 체크
        if (File.Exists(SaveDataManager.instance.path + SaveDataManager.instance.filename))
        {
            hasPlaylog = true;
        }
    }

    void checkPlayLog()
    {
        if(hasPlaylog){
            Text startButtonText = startButton.GetComponentInChildren<Text>();
            startButtonText.text = "새 게임 시작";
            ContinueButton.gameObject.SetActive(true);
            GalleryButton.gameObject.SetActive(true);
        }
        
    }

    int[] generateRandomNumbers(){
        int[] selectedIndices = {-1, -1, -1, -1};
        bool[] boolMap = new bool[18];

        // 배열을 false로 초기화
        for (int i = 0; i < boolMap.Length; i++)
        {
            boolMap[i] = false;
        }
        
        for(int i=0; i<selectedIndices.Length; i++){
            int randomNum = Random.Range(0, errandLists.Length);
            if(!boolMap[randomNum]){
                selectedIndices[i] = randomNum;
                boolMap[randomNum] = true;
            }
            else i--;
        }
        return selectedIndices;
    }

    void makeErrandList()
    {
        itemCount = 0;
        int[] selectedIndices = generateRandomNumbers();

        for(int i=0; i<ErrandListObjects.Length; i++){
            GameObject AErrandList = ErrandListObjects[i];
            Image[] childImages = AErrandList.GetComponentsInChildren<Image>();

            int randomIndex = selectedIndices[i];
            Debug.Log(randomIndex);
            for (int j = 0; j < childImages.Length; j++){
                childImages[j].gameObject.SetActive(false);
            }
            childImages[randomIndex].gameObject.SetActive(true);

            int buyQuantity = Random.Range(1, 4);
            if(randomIndex >= 14) buyQuantity = 1;

            errandTexts[i].text = errandLists[randomIndex] + " " + buyQuantity.ToString() + " 개";

            itemPool.Add((ItemType)selectedIndices[i], buyQuantity);
            itemCount += buyQuantity;
            Debug.Log((ItemType)selectedIndices[i] + "," + buyQuantity);
        }
    }
    
    public void GameStart()
    {
        var camHandler = FindObjectOfType<CameraHandler>();

        player.isMove = true;

        startCam = camHandler.startCam;
        gameCam = camHandler.gameCam;
        startCam.SetActive(false);
        gameCam.SetActive(true);

        startPanel.SetActive(false);
        gamePanel.SetActive(true);

        IsTimerRunning = true;
    }

    public void NewGame()
    {
        namePanel.gameObject.SetActive(true); // 플레이어 닉네임 입력 UI를 활성화
    }

    public void NewGameStart()
    {
        SaveDataManager.instance.nowPlayer.name = newPlayerName.text;
        SaveDataManager.instance.nowPlayer.rewardScores = new int[SaveDataManager.rewardLength]; // 정수형은 0으로 초기화
        SaveDataManager.instance.SaveData();

        namePanel.SetActive(false);

        GameStart();
    }

    public void ContinueGame()
    {
        SaveDataManager.instance.LoadData(); // 데이터 로드
        GameStart();
    }

    public void GoGallery()
    {
        SaveDataManager.instance.LoadData(); // 데이터 로드
        SceneManager.LoadScene("Gallery");
    }

    public void GameEnd() // 플레이어가 집에 도착 또는 타임아웃
    {
        gameCam.SetActive(false);
        startCam.SetActive(true);

        gamePanel.SetActive(false);
        TimeOutPanel.SetActive(false);
        endPanel.SetActive(true);

        playTime = 1; // update 함수에서 타임오버 창 뜨지 않도록
        IsTimerRunning = false;
    }

    public void GoTitle() // 게임을 그만두고 타이틀 화면으로
    {
        gameCam.SetActive(false);
        startCam.SetActive(true);

        gamePanel.SetActive(false);
        endPanel.SetActive(false);
        startPanel.SetActive(true);

        IsTimerRunning = false;

        // ***진행상황 초기화...***
        playTime = playTimeLimit;
    }

    public void QuitGame()
    {
        Application.Quit(); // 게임 종료
    }


    void Update()
    {
        if (playTime > 0F)
        {
            if (IsTimerRunning) playTime -= Time.deltaTime;
        }
        else
        {
            // time over
            // 타임오버 창 보여주기
            gamePanel.SetActive(false);
            TimeOutPanel.SetActive(true);
            // 게임 실패여야 하므로 진행상황과 관계없이 심부름 실패로 bool변수 업데이트
            Invoke("GameEnd", 5f); // 5초 경과 후 엔딩 scene으로 이동

        }
    }

    void LateUpdate()
    {
        playerCoinText.text = string.Format("{0:n0}", player.coin);

        int min = (int)(playTime / 60);
        int second = (int)(playTime % 60);
        playTimeText.text = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);

        checkFrontAImg.color = new Color(1,1,1, player.iscompletedErrand[0] ? 1:0);
        checkFrontBImg.color = new Color(1,1,1, player.iscompletedErrand[1] ? 1:0); 
        checkFrontCImg.color = new Color(1,1,1, player.iscompletedErrand[2] ? 1:0); 
        checkFrontDImg.color = new Color(1,1,1, player.iscompletedErrand[3] ? 1:0);
        listMamaText.text = SaveDataManager.instance.nowPlayer.name + ", 5시까지 꼭 돌아와야 해!";


        timeBar.localScale = new Vector3((playTime)/playTimeLimit, 1, 1);

        stampA.SetActive(player.iscompletedStamp[0]);
        stampB.SetActive(player.iscompletedStamp[1]);
        stampC.SetActive(player.iscompletedStamp[2]);
        stampD.SetActive(player.iscompletedStamp[3]);
        stampE.SetActive(player.iscompletedStamp[4]);

        menuTimeText.text = playTimeText.text;
        TimeOutText.text = SaveDataManager.instance.nowPlayer.name + ", 어디 있니?\n이만 집으로 돌아오렴~";
    }
}
