using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    public float forwardSpeed = 12f;
    public float verticalSpeed = 8f;    

    public float yawSpeed = 90f;        

    public float linearDrag = 3f;
    public float angularDrag = 10f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = linearDrag;
        rb.angularDamping = angularDrag;
        // Evitar que la física voltee la nave
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void Update()
    {
        HandleRotation();
    }

    void HandleMovement()
    {
        float fwd = Input.GetAxis("Vertical");   // W = +1, S = -1
        float upDown = 0f;
        if (Input.GetKey(KeyCode.Space)) upDown = 1f;
        if (Input.GetKey(KeyCode.LeftShift)) upDown = -1f;

        Vector3 force = transform.forward * fwd * forwardSpeed
                      + transform.up * upDown * verticalSpeed;

        rb.AddForce(force, ForceMode.Acceleration);
    }

    void HandleRotation()
    {
        // A = -1 → gira izquierda, D = +1 → gira derecha, solo en Y
        float yaw = Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        transform.Rotate(0f, yaw, 0f, Space.World);
    }
}