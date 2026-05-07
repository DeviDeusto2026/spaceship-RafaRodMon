using UnityEngine;

public class OrbitaPlaneta : MonoBehaviour
{
    public Transform sol; // El objeto central (Sol)
    public TipoPlaneta planeta;
    public Vector3 ejeRotacion = Vector3.up;

    private float velocidadOrbital;

    public static float multiplicadorGlobal = 1f;

    // Velocidades orbitales relativas (Tierra = 1)
    public enum TipoPlaneta
    {
        Mercurio,
        Venus,
        Tierra,
        Marte,
        Jupiter,
        Saturno,
        Urano,
        Neptuno
    }

    void Start()
    {
        float velocidadTierra = 20f;

        // Asignar velocidad según el planeta
        switch (planeta)
        {
            case TipoPlaneta.Mercurio:
                velocidadOrbital = velocidadTierra * 3.2f;
                break;
            case TipoPlaneta.Venus:
                velocidadOrbital = velocidadTierra * 2.34f;
                break;
            case TipoPlaneta.Tierra:
                velocidadOrbital = velocidadTierra;
                break;
            case TipoPlaneta.Marte:
                velocidadOrbital = velocidadTierra * 0.80f;
                break;
            case TipoPlaneta.Jupiter:
                velocidadOrbital = velocidadTierra * 0.43f;
                break;
            case TipoPlaneta.Saturno:
                velocidadOrbital = velocidadTierra * 0.32f;
                break;
            case TipoPlaneta.Urano:
                velocidadOrbital = velocidadTierra * 0.22f;
                break;
            case TipoPlaneta.Neptuno:
                velocidadOrbital = velocidadTierra * 0.18f;
                break;
        }
    }

    void Update()
    {
        float velocidadFinal = velocidadOrbital * multiplicadorGlobal;
        transform.RotateAround(sol.position, ejeRotacion, velocidadFinal * Time.deltaTime);
    }
}
