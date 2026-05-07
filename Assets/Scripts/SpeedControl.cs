using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OrbitaPlaneta.multiplicadorGlobal /= 2;
            Debug.Log("Velocidad: x" + OrbitaPlaneta.multiplicadorGlobal);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            OrbitaPlaneta.multiplicadorGlobal = 1.0f;
            Debug.Log("Velocidad: x" + OrbitaPlaneta.multiplicadorGlobal);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OrbitaPlaneta.multiplicadorGlobal *= 2;
            Debug.Log("Velocidad: x" + OrbitaPlaneta.multiplicadorGlobal);
        }
    }
}