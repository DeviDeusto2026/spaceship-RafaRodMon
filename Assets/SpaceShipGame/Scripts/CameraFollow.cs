using UnityEngine;

/// <summary>
/// Cámara que sigue al jugador suavemente en XY.
/// Añade este script a la Main Camera.
/// Arrastra la nave del jugador en el campo "target".
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform target;

    [Header("Offset respecto al jugador")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Suavizado")]
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;

        // Mantener siempre Z fija (cámara ortogonal/perspectiva lateral)
        desiredPos.z = offset.z;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            smoothSpeed * Time.deltaTime
        );
    }
}
