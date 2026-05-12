using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    [Header("Velocidad de traslación")]
    public float forwardSpeed = 12f;
    public float strafeSpeed = 0f;    // pon > 0 si quieres movimiento lateral con A/D
    public float verticalSpeed = 8f;    // Q / E

    [Header("Velocidad de rotación")]
    public float yawSpeed = 90f;    // A/D  → girar izquierda/derecha
    public float pitchSpeed = 60f;    // Mouse Y → cabecear

    [Header("Física")]
    public float linearDrag = 3f;
    public float angularDrag = 6f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = linearDrag;
        rb.angularDamping = angularDrag;
    }

    void Update()
    {
        HandleRotation();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    // ── Traslación ────────────────────────────────────────────────────────
    void HandleMovement()
    {
        float fwd = Input.GetAxis("Vertical");       // W = +1, S = -1
        float upDown = 0f;
        if (Input.GetKey(KeyCode.E)) upDown = 1f;
        if (Input.GetKey(KeyCode.Q)) upDown = -1f;

        Vector3 force = transform.forward * fwd * forwardSpeed
                      + transform.up * upDown * verticalSpeed;

        rb.AddForce(force, ForceMode.Acceleration);
    }

    // ── Rotación ─────────────────────────────────────────────────────────
    void HandleRotation()
    {
        // A/D → yaw (girar sobre el eje Y local de la nave)
        float yaw = Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;

        // Mouse → pitch (cabecear sobre el eje X local)
        float pitch = -Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime;

        transform.Rotate(pitch, yaw, 0f, Space.Self);
    }
}
