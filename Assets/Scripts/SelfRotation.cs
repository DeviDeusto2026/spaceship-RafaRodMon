using UnityEngine;

public class RotacionPropia : MonoBehaviour
{
    public TipoRotacion planeta;
    public float velocidadBaseRotacion = 50f;

    private float inclinacionAxial;
    private float multiplicadorRotacion;
    private bool sentidoHorario = true;

    public enum TipoRotacion { Mercurio, Venus, Tierra, Marte, Jupiter, Saturno, Urano, Neptuno }

    void Start()
    {
        ConfigurarPlaneta();

        // Aplicamos la inclinación axial al inicio para que rote sobre el eje correcto
        transform.rotation = Quaternion.Euler(0, 0, inclinacionAxial);
    }

    void ConfigurarPlaneta()
    {
        switch (planeta)
        {
            case TipoRotacion.Mercurio:
                multiplicadorRotacion = 0.017f;
                inclinacionAxial = 0.03f;
                break;
            case TipoRotacion.Venus:
                multiplicadorRotacion = 0.004f;
                inclinacionAxial = 177.3f;
                sentidoHorario = false;
                break;
            case TipoRotacion.Tierra:
                multiplicadorRotacion = 1.0f;
                inclinacionAxial = 23.5f;
                break;
            case TipoRotacion.Marte:
                multiplicadorRotacion = 0.97f;
                inclinacionAxial = 25.2f;
                break;
            case TipoRotacion.Jupiter:
                multiplicadorRotacion = 2.44f;
                inclinacionAxial = 3.1f;
                break;
            case TipoRotacion.Saturno:
                multiplicadorRotacion = 2.25f;
                inclinacionAxial = 26.7f;
                break;
            case TipoRotacion.Urano:
                multiplicadorRotacion = 1.39f;
                inclinacionAxial = 97.8f;
                sentidoHorario = false;
                break;
            case TipoRotacion.Neptuno:
                multiplicadorRotacion = 1.49f;
                inclinacionAxial = 28.3f;
                break;
        }
    }

    void Update()
    {
        float direccion = sentidoHorario ? -1f : 1f;
        float velocidadFinal = velocidadBaseRotacion * multiplicadorRotacion;

        transform.Rotate(Vector3.up, velocidadFinal * direccion * Time.deltaTime, Space.Self);
    }
}