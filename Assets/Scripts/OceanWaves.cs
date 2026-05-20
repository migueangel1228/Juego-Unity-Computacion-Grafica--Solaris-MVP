using UnityEngine;

public class OceanWaves : MonoBehaviour
{
    public float waveHeight = 0.3f;
    public float waveFrequency = 1.2f;
    public float waveSpeed = 1.5f;
    public float colorPulseSpeed = 0.8f;

    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;
    private Material mat;
    private Color colorA = new Color(0.1f, 0.18f, 0.29f); // azul oscuro
    private Color colorB = new Color(0.35f, 0.15f, 0.05f); // naranja oscuro

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        float t = Time.time;

        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 v = originalVertices[i];
            v.y = Mathf.Sin((v.x * waveFrequency) + (t * waveSpeed)) * waveHeight
                + Mathf.Sin((v.z * waveFrequency * 0.8f) + (t * waveSpeed * 1.2f)) * waveHeight * 0.6f;
            displacedVertices[i] = v;
        }

        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals();

        // Pulso de color entre azul y naranja
        float pulse = (Mathf.Sin(t * colorPulseSpeed) + 1f) / 2f;
        mat.color = Color.Lerp(colorA, colorB, pulse);
    }
}