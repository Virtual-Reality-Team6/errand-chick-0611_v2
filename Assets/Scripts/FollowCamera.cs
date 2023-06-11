using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraView
{
    First,
    Third,
}
public class FollowCamera : MonoBehaviour
{
    Camera cam;
    public Transform target;
    public Player player;
    public Vector3 offset;
    public CameraView cameraView = CameraView.Third;

    float cameraRotY;
    float cameraRotX;

    private void Start()
    {
        cam = GetComponent<Camera>();
        switch (cameraView)
        {
            case CameraView.First:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;

                CanvasHandler.Instance.crossUIText.SetActive(true);
                break;
            case CameraView.Third:

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                CanvasHandler.Instance.crossUIText.SetActive(false);
                break;
            default:
                break;
        }
        player = FindObjectOfType<Player>();
        target = player.transform;
    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 10);

        if (Physics.Raycast(ray, out RaycastHit hit, 4))
        {

            if (!hit.transform.TryGetComponent(out Interactable interactable)) return;

            CanvasHandler.Instance.selectUIButton.SetActive(true);

            CanvasHandler.Instance.selectUIButton.transform.position = cam.WorldToScreenPoint(hit.transform.position);


            if (Input.GetKeyDown(KeyCode.F))
            {
                interactable.Interact();
            }
        }
        else
        {
            CanvasHandler.Instance.selectUIButton.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        CameraMove();
    }
    void CameraMove()
    {
        switch (cameraView)
        {
            case CameraView.First:
                {

                    CameraThirdView(new Vector2(
                        -Input.GetAxisRaw("Mouse Y"),
                        Input.GetAxisRaw("Mouse X")));
                    player.rot = Quaternion.Euler(0, cameraRotX, 0); 
                }
                break;
            case CameraView.Third:
                player.rot = Quaternion.identity;
                transform.rotation = Quaternion.Euler(50, 0, 0);
                transform.position = target.position + offset;
                break;
            default:
                break;
        }
    }
    void CameraThirdView(Vector2 axis)
    {
        transform.position = target.position + offset;
        cameraRotY += axis.x;
        cameraRotX += axis.y;
        transform.rotation = Quaternion.Euler(new Vector3(cameraRotY, cameraRotX, 0));
    }
}
