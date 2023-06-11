using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField]
    GameObject ui;
    Camera cam;
    Rigidbody rigid;
    [SerializeField]
    float speed;

    float cameraRotY;
    float cameraRotX = 90;




    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        cam = Camera.main;
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {

        Move(new Vector3(
            -Input.GetAxisRaw("Horizontal"), 0,
            -Input.GetAxisRaw("Vertical")
            ));


        LookRotate(new Vector2(
            -Input.GetAxisRaw("Mouse Y"),
            Input.GetAxisRaw("Mouse X")));

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 10);

        if(Physics.Raycast(ray, out RaycastHit hit, 5, 1 << LayerMask.NameToLayer("Food")))
        {
            ui.SetActive(true);

            ui.transform.position = cam.WorldToScreenPoint(hit.transform.position);


            if(Input.GetKeyDown(KeyCode.F))
            {
                Destroy(hit.transform.gameObject);
            }
        }
        else
        {
            ui.SetActive(false);
        }

    }
    public void LookRotate(Vector3 axis)
    {
        cameraRotY += axis.x;
        cam.transform.localRotation = Quaternion.Euler(new Vector3(cameraRotY, 0, 0));
        cameraRotX += axis.y;
        transform.rotation = Quaternion.Euler(new Vector3(0, cameraRotX, 0));
    }
    public void Move(Vector3 axis)
    {
        axis *= speed;

        Vector3 vec = transform.rotation * axis;
        rigid.velocity = new Vector3(vec.x, rigid.velocity.y, vec.z);
    }
}
