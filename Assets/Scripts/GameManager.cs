using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject cohete;
    public GameObject visitante;
    public Camera mainCamera;
    public GameObject player;

    [Header("Cinemática")]
    public float velocidadDespegue = 6f;
    public float duracionCinemática = 8f;
    public Vector3 offsetCamaraCohete = new Vector3(5f, 3f, -10f);

    private bool finActivado = false;
    private VisitanteController visitanteCtrl;

    void Start()
    {
        visitanteCtrl = visitante != null ? visitante.GetComponent<VisitanteController>() : null;
    }

    public void ActivarFinal()
    {
        if (finActivado) return;
        finActivado = true;
        StartCoroutine(SecuenciaFinal());
    }

    IEnumerator SecuenciaFinal()
    {
        // 1 — Desactivar visitante y controles del player
        if (visitanteCtrl != null) visitanteCtrl.Desactivar();
        player.GetComponent<PlayerController>().enabled = false;

        // 2 — Desactivar CameraFollow para tomar control manual
        CameraFollow cf = mainCamera.GetComponent<CameraFollow>();
        if (cf != null) cf.enabled = false;

        // 3 — Pequeña pausa dramática antes de que desaparezca el player
        yield return new WaitForSeconds(0.8f);

        // 4 — Astronauta desaparece
        player.SetActive(false);

        // 5 — Pausa para que el jugador note que el player se fue
        yield return new WaitForSeconds(0.5f);

        // 6 — Cinemática: cámara sigue al cohete subiendo
        float tiempo = 0f;
        while (tiempo < duracionCinemática)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionCinemática;

            // Cohete sube cada vez más rápido
            cohete.transform.position += Vector3.up * velocidadDespegue * (1f + t * 3f) * Time.deltaTime;

            // Cámara sigue al cohete con offset lateral
            Vector3 targetCamPos = cohete.transform.position + offsetCamaraCohete;
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetCamPos,
                Time.deltaTime * 3f
            );

            // Cámara siempre mira al cohete
            mainCamera.transform.LookAt(cohete.transform.position);

            yield return null;
        }

        // 7 — Alejar cámara para ver el planeta completo
        yield return StartCoroutine(AlejarCamara());
    }

    IEnumerator AlejarCamara()
    {
        float tiempo = 0f;
        float duracion = 4f;
        Vector3 posInicial = mainCamera.transform.position;
        Vector3 posFinal = posInicial + new Vector3(0f, 40f, -60f);

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            mainCamera.transform.position = Vector3.Lerp(posInicial, posFinal, t);
            // Seguir mirando hacia donde estaba el cohete
            mainCamera.transform.LookAt(posInicial + Vector3.up * 20f);

            yield return null;
        }
    }
}