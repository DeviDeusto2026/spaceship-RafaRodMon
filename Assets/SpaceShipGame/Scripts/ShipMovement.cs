using UnityEngine;

/// <summary>
/// Movimiento de nave — estilo coche espacial.
///   W / S   → avanzar / retroceder
///   A / D   → rotar sobre el eje Y (izquierda / derecha)
///   Q / E   → subir / bajar
/// Requiere Rigidbody con useGravity = false.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    [Header("Traslación")]
    public float forwardSpeed = 12f;
    public float verticalSpeed = 8f;    // Q / E

    [Header("Rotación")]
    public float yawSpeed = 90f;        // A / D → giro en Y

    [Header("Física")]
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
        if (Input.GetKey(KeyCode.E)) upDown = 1f;
        if (Input.GetKey(KeyCode.Q)) upDown = -1f;

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