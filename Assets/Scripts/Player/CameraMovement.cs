using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject LocalPlayerObj;
    [SerializeField]
    private GameObject CameraArm;
    [SerializeField]
    private float sensitivity = 30f;

    private float mx = 0;
    private float my = 0;

    private bool isESC = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isESC)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");
            mx += mouseX * sensitivity * Time.deltaTime;
            my += mouseY * sensitivity * Time.deltaTime;

            if (PlayerManager.instance.PlayerPerson == 3)
            {
                my = Mathf.Clamp(my, -7, 35);
                CameraArm.transform.localPosition = new Vector3(2, 6, -5);
            }
            else
            {
                my = Mathf.Clamp(my, -90, 60);
                CameraArm.transform.localPosition = new Vector3(0, 5, 0.5f);
            }

            transform.position = CameraArm.transform.position;
            transform.rotation = CameraArm.transform.rotation;

            LocalPlayerObj.transform.eulerAngles = new Vector3(0, mx, 0); //rotate player to mouse x axis

            Vector3 cameraRot = CameraArm.transform.eulerAngles;
            cameraRot.x = my;
            CameraArm.transform.eulerAngles = cameraRot; //rotate camera arm to mouse y axis

            if (Input.GetKeyDown("["))
            {
                sensitivity -= 100f;
            }

            if (Input.GetKeyDown("]"))
            {
                sensitivity += 100f;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            isESC = !isESC;
            CameraArm.transform.parent.GetComponent<PlayerMovement>().enabled =
                CameraArm.transform.parent.GetComponent<PlayerMovement>().enabled == true ? false : true;
            Cursor.lockState = isESC ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }
}
