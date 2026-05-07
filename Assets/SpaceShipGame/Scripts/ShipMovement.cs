using UnityEngine;

/// <summary>
/// Misión 01 — Mover la Nave
/// Plano XY (lateral 2.5D). Requiere Rigidbody con Gravity = 0 y Freeze Z rotation.
/// Añade este script a la nave del jugador.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 8f;

    [Header("Límites de pantalla (unidades mundo)")]
    public float xMin = -8f;
    public float xMax =  8f;
    public float yMin = -4f;
    public float yMax =  4f;

    [Header("Bonus: inclinación al subir/bajar")]
    public float tiltAmount  = 20f;   // grados máximos de rotación en Z
    public float tiltSpeed   = 5f;    // velocidad de interpolación

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationY;
    }

    void FixedUpdate()
    {
        // a) Input
        float h = Input.GetAxis("Horizontal"); // A/D
        float v = Input.GetAxis("Vertical");   // W/S

        // b) Mover
        Vector3 move = new Vector3(h, v, 0f) * speed * Time.fixedDeltaTime;
        Vector3 newPos = rb.position + move;

        // c) Limitar a pantalla
        newPos.x = Mathf.Clamp(newPos.x, xMin, xMax);
        newPos.y = Mathf.Clamp(newPos.y, yMin, yMax);

        rb.MovePosition(newPos);
    }

    void Update()
    {
        // BONUS: rota en Z según movimiento vertical
        float v = Input.GetAxis("Vertical");
        float targetTilt = -v * tiltAmount;
        float currentZ   = transform.eulerAngles.z > 180f
                           ? transform.eulerAngles.z - 360f
                           : transform.eulerAngles.z;
        float newZ = Mathf.Lerp(currentZ, targetTilt, Time.deltaTime * tiltSpeed);
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            newZ
        );
    }
}
