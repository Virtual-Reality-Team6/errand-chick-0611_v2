using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string currentMapName;
    public float speed;
    public float jumpUp;
    float hAxis;
    float vAxis;

    bool shiftDown;
    bool spaceDown;
    bool interactionDown;
    bool memoDown;
    bool stampDown;
    bool menuDown;

    public int coin;
    public int score;

    bool isJump;
    bool isShop;
    public bool isInteraction;

    bool seeMemo;
    bool seeStamp;
    bool seeMenu = false;

    public bool[] iscompletedErrand;
    public bool[] iscompletedStamp;

    Vector3 moveVec;

    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;

    public RectTransform memoUIGroup;
    public RectTransform stampUIGroup;
    public RectTransform menuUIGroup;

    public Quaternion rot;
    public SceneLoader sceneLoader;

    public bool isMove = true;
    public bool hasKey;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        //PlayerPrefs.SetInt("MaxScore", 112500);
        Debug.Log("다시 깨어나는가");
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        FreezeRotation();
    }
    
    void Update()
    {
        if (!isMove) return; 


        GetInput();
        Move();
        Turn();
        Jump();
        Interaction();
        Memo();
        Stamp();
        Menu();
        
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        shiftDown = Input.GetButton("Run");
        spaceDown = Input.GetButton("Jump");
        interactionDown = Input.GetButtonDown("Interaction");
        memoDown = Input.GetButtonDown("Memo");
        stampDown = Input.GetButtonDown("Stamp");
        menuDown = Input.GetButtonDown("Menu");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position +=
             rot * moveVec * speed * (shiftDown ? 2f : 1f) * Time.deltaTime;

        anim.SetBool("Walk", moveVec != Vector3.zero);
        anim.SetBool("Run", shiftDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (spaceDown && !isJump && !isInteraction){
            rigid.AddForce(Vector3.up * jumpUp,  ForceMode.Impulse);
            anim.SetBool("Jump", true);
            anim.SetTrigger("Jump");
            isJump = true;
        }
    }

    void Interaction()
    {
        if(interactionDown && nearObject != null){
            if(nearObject.tag == "HelloNPC"){
                HelloNPC hellonNPC = nearObject.GetComponent<HelloNPC>();
                hellonNPC.Enter(this);
                isInteraction = true;
            }
            if(nearObject.tag == "BadNPC"){
                BadNPC badNPC = nearObject.GetComponent<BadNPC>();
                badNPC.Enter(this);
                isInteraction = true;
            }
            if(nearObject.tag == "SadNPC"){
                SadNPC sadNPC = nearObject.GetComponent<SadNPC>();
                sadNPC.Enter(this);
                isInteraction = true;
            }
        }
    }

    void Memo()
    {
        if(memoDown){
            if(seeMemo) {
                memoUIGroup.anchoredPosition = Vector3.down * 1000;
                seeMemo = false;
            }
            else {
                memoUIGroup.anchoredPosition = Vector3.zero;
                seeMemo = true;
            }
        }
    }

    public void Stamp()
    {
        if(stampDown){
            if(seeStamp) {
                stampUIGroup.anchoredPosition = Vector3.down * 1000;
                seeStamp = false;
            }
            else {
                stampUIGroup.anchoredPosition = new Vector3(718f, 0f, 0f);
                seeStamp = true;
            }
        }
    }

    public void Menu()
    {
        if(menuDown){
            if(seeMenu) {
                menuUIGroup.anchoredPosition = Vector3.left * 1500;
                seeMenu = false;
            }
            else {
                menuUIGroup.anchoredPosition = new Vector3(0f, 0f, 0f);
                seeMenu = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor"){
            isJump = false;
        }
        if(collision.gameObject.CompareTag("Portal")){
            GameObject sceneLoaderObject = GameObject.Find("Scene Loader");
            sceneLoader = sceneLoaderObject.GetComponent<SceneLoader>();
            if(sceneLoader == null){
                Debug.Log("sceneLoader is null");
            }
            else{
                sceneLoader.LoadScene();
            }
        }
    }

    void OnTriggerStay(Collider other){
        if(other.tag == "HelloNPC" || other.tag == "BadNPC" || other.tag == "SadNPC")
            nearObject = other.gameObject;
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "HelloNPC"){
            HelloNPC hellonNPC = nearObject.GetComponent<HelloNPC>();
            hellonNPC.Exit();
            isInteraction = false;
            nearObject = null;
        }
        if(other.tag == "BadNPC"){
            BadNPC badNPC = nearObject.GetComponent<BadNPC>();
            badNPC.Exit();
            isInteraction = false;
            nearObject = null;
        }
        if(other.tag == "SadNPC"){
            SadNPC sadNPC = nearObject.GetComponent<SadNPC>();
            sadNPC.Exit();
            isInteraction = false;
            nearObject = null;
        }
    }
}
