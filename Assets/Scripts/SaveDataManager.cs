using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    public string name; // 플레이어 이름
    public int[] rewardScores; // 보상 획득여부와 최고점수, 0: 수집x, 1~3:최고점수
}

public class SaveDataManager : MonoBehaviour
{
    public static int rewardLength = 16; // 리워드 종류의 총 개수, Resources\Reward Prefabs의 항목수

    public static SaveDataManager instance; // 싱글톤 패턴

    public PlayerData nowPlayer = new PlayerData();

    public string path; // 파일 저장경로
    public string filename = "Savedata.json"; // 파일 이름

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);


        path = Application.dataPath + "/";
    }

    public void SaveData()
    {
        // Json으로 변환
        string data = JsonUtility.ToJson(nowPlayer, true);

        // 변환된 데이터 확인
        Debug.Log(data);

        // 외부에 파일로 저장
        File.WriteAllText(path + filename, data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + filename);
        nowPlayer = JsonUtility.FromJson<PlayerData>(data);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
