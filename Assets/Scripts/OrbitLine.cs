using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class OrbitLinePlaneta : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform centro;

    [Range(30, 200)] public int segmentos = 100;
    public float anchoLinea = 0.1f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (centro == null)
        {
            GameObject solGo = GameObject.FindWithTag("Sun");
            if (solGo != null) centro = solGo.transform;
        }

        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = anchoLinea;
        lineRenderer.endWidth = anchoLinea;

        
        lineRenderer.numCornerVertices = 5;
        lineRenderer.numCapVertices = 5;
    }

    void Update()
    {
        if (centro != null)
        {
            DibujaOrbita();
        }
    }

    void DibujaOrbita()
    {
        lineRenderer.positionCount = segmentos + 1;

        float radioActual = Vector3.Distance(transform.position, centro.position);

        float angulo = 0f;
        for (int i = 0; i <= segmentos; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angulo) * radioActual;
            float z = Mathf.Cos(Mathf.Deg2Rad * angulo) * radioActual;

            Vector3 punto = new Vector3(x, 0f, z) + centro.position;

            lineRenderer.SetPosition(i, punto);
            angulo += (360f / segmentos);
        }
    }
}