using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorMove : MonoBehaviour
{
    bool isopen = false;
    [SerializeField]
    bool needKey = false;

    // Start is called before the first frame update

    private void OnMouseDown()
    {
        if (!needKey)
        {
            OpenDoor();
        }
        else
        {
            if(JSGameMode.instance.ActionObj[4].activeSelf)
            {
                OpenDoor();
            }
            else
            {
                StartCoroutine(JSGameMode.instance.SetGuideText("열쇠가 필요합니다"));
            }
        }
    }

    private void OpenDoor()
    {
        Vector3 rotAxis;
        rotAxis = transform.position;
        rotAxis.x -= 1.2f;
        if (!isopen)
        {
            transform.Rotate(new Vector3(0, 90, 0));
            isopen = true;
        }
        else
        {
            transform.Rotate(new Vector3(0, -90, 0));
            isopen = false;
        }
    }
}
