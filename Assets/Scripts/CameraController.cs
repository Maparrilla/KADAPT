using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 5F;
    public float sensitivityY = 5F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    public enum KEYBOARD_INPUT : int
    {
        RIGHT = KeyCode.D,
        LEFT = KeyCode.A,
        FORWARD = KeyCode.W,
        BACK = KeyCode.S,
        UP = KeyCode.E,
        DOWN = KeyCode.Q
    }

    public int moveSpeed;
    public int scrollSpeed;

    public float sensitivity;

    private Camera myCam;
    private Transform g_Camera;

    // Use this for initialization
    void Start()
    {
        moveSpeed = 6;
        scrollSpeed = 10;

        sensitivity = 0.02f;

        myCam = GetComponent<Camera>();
        g_Camera = myCam.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        /*Vector3 vp = myCam.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, myCam.nearClipPlane));
        vp.x -= 0.5f;
        vp.y -= 0.5f;
        vp.x *= sensitivity;
        vp.y *= sensitivity;
        vp.x += 0.5f;
        vp.y += 0.5f;
        Vector3 sp = myCam.ViewportToScreenPoint(vp);

        Vector3 v = myCam.ScreenToWorldPoint(sp);
        transform.LookAt(v, Vector3.up);*/


        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }

        if (Input.GetKey(KeyCode.Space))
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;

        foreach (KEYBOARD_INPUT val in System.Enum.GetValues(typeof(KEYBOARD_INPUT)))
        {
            if (Input.GetKey((KeyCode)val))
            {
                switch (val)
                {
                    case KEYBOARD_INPUT.FORWARD:
                        g_Camera.Translate(0, 0, 1 * moveSpeed * Time.deltaTime, Space.Self);
                        break;
                    case KEYBOARD_INPUT.BACK:
                        g_Camera.Translate(0, 0, -1 * moveSpeed * Time.deltaTime, Space.Self);
                        break;
                    case KEYBOARD_INPUT.LEFT:
                        g_Camera.Translate(-1 * moveSpeed * Time.deltaTime, 0, 0, Space.Self);
                        break;
                    case KEYBOARD_INPUT.RIGHT:
                        g_Camera.Translate(1 * moveSpeed * Time.deltaTime, 0, 0, Space.Self);
                        break;
                    case KEYBOARD_INPUT.UP:
                        g_Camera.Translate(0, 1 * moveSpeed * Time.deltaTime, 0, Space.Self);
                        break;
                    case KEYBOARD_INPUT.DOWN:
                        g_Camera.Translate(0, -1 * moveSpeed * Time.deltaTime, 0, Space.Self);
                        break;
                }
            }
        }
    }
}
