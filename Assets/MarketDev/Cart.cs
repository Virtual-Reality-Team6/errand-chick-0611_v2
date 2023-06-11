using UnityEngine;

public class Cart : MonoBehaviour
{
    FollowCamera followCam;
    Transform player;
    [SerializeField]
    Transform itemPos;
    public void Start()
    {
        followCam = FindObjectOfType<FollowCamera>();
        player = FindObjectOfType<Player>().transform;
    }
    public void Update()
    {
        var rot = Quaternion.Euler(0, followCam.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(0, followCam.transform.eulerAngles.y - 90, 0);
        transform.position = player.transform.position + rot * (followCam.offset + new Vector3(0, -2f, 1));
    }
    public void AddItem(GameObject obj)
    {
        var prefabTF = Instantiate(obj).transform;
        Destroy(prefabTF.GetComponent<Item>());

        prefabTF.localScale /= 2;
        prefabTF.gameObject.layer = 2;
        prefabTF.SetParent(transform);
        prefabTF.position = itemPos.position;

        prefabTF.gameObject.AddComponent<Rigidbody>();
    }


}