using UnityEngine;
using System.Collections;

public class VisitanteController : MonoBehaviour
{
    public Transform player;
    public float tiempoVisible = 4f;
    public float tiempoOculto = 6f;

    void Start()
    {
        SetVisible(false);
        StartCoroutine(CicloVisitante());
    }

    IEnumerator CicloVisitante()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoOculto);
            SetVisible(true);

            float t = 0f;
            while (t < tiempoVisible)
            {
                // Siempre mira al player mientras es visible
                if (player != null)
                {
                    Vector3 dir = player.position - transform.position;
                    dir.y = 0f;
                    if (dir != Vector3.zero)
                        transform.rotation = Quaternion.LookRotation(dir);
                }
                t += Time.deltaTime;
                yield return null;
            }

            SetVisible(false);
        }
    }

    public void Desactivar()
    {
        StopAllCoroutines();
        SetVisible(false);
    }

    void SetVisible(bool visible)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = visible;
    }
}