using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float sensitivity = 200f;

    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

