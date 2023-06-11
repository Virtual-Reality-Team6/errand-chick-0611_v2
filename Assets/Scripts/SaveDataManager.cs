using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    public string name; // �÷��̾� �̸�
    public int[] rewardScores; // ���� ȹ�濩�ο� �ְ�����, 0: ����x, 1~3:�ְ�����
}

public class SaveDataManager : MonoBehaviour
{
    public static int rewardLength = 16; // ������ ������ �� ����, Resources\Reward Prefabs�� �׸��

    public static SaveDataManager instance; // �̱��� ����

    public PlayerData nowPlayer = new PlayerData();

    public string path; // ���� ������
    public string filename = "Savedata.json"; // ���� �̸�

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
        // Json���� ��ȯ
        string data = JsonUtility.ToJson(nowPlayer, true);

        // ��ȯ�� ������ Ȯ��
        Debug.Log(data);

        // �ܺο� ���Ϸ� ����
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
