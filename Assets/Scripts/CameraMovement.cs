using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public int movSpeed = 30;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move();        
    }

    private void move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.position -= transform.right * Time.deltaTime * movSpeed;
        }

        else if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.position += transform.forward * Time.deltaTime * movSpeed;
        }

        else if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.position -= transform.forward * Time.deltaTime * movSpeed;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.position += transform.right * Time.deltaTime * movSpeed;
        }

        else if (Input.GetKey(KeyCode.Space))
        {
            gameObject.transform.position += Vector3.up * Time.deltaTime * movSpeed;
        }

        else if (Input.GetKey(KeyCode.LeftShift))
        {
            gameObject.transform.position += Vector3.down * Time.deltaTime * movSpeed;
        }
    }

}



