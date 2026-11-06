using System.Collections;
using UnityEngine;
using TMPro; // <-- 1. Asegúrate de incluir esta librería al inicio

public class SaludPlayer : MonoBehaviour
{
    [Header("Configuración de Vidas y Salud")]
    [SerializeField] private int maxSaludPorVida = 3;
    private int saludActual;

    [SerializeField] private int maxVidasTotales = 5;
    private int vidasActuales;

    [Header("UI Provisional")]
    [SerializeField] private TextMeshProUGUI textoHUD; // <-- 2. Cambia 'Text' por 'TextMeshProUGUI'

    [Header("Invulnerabilidad (i-Frames)")]
    [SerializeField] private float tiempoInvulnerable = 1.5f;
    [SerializeField] private float frecuenciaParpadeo = 0.1f;
    private bool esInvulnerable = false;

    private SpriteRenderer spriteRenderer;
    private Vector3 posicionInicial;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        posicionInicial = transform.position;

        vidasActuales = maxVidasTotales;
        ReiniciarSalud();

        // Diagnóstico en la Consola
        if (textoHUD == null)
        {
            Debug.LogError("<b>[ERROR UI]</b> La casilla 'Texto HUD' en el Player está VACÍA. Arrastra el objeto UI_HUD.");
        }
        else
        {
            Debug.Log("<b>[UI OK]</b> El texto del HUD está conectado correctamente.");
        }
    }

    void Update()
    {
        // PRUEBAS EN EL EDITOR:
        if (Input.GetKeyDown(KeyCode.K)) // Presiona 'K' para restar 1 de daño
        {
            RecibirDano(1);
        }
        if (Input.GetKeyDown(KeyCode.P)) // Presiona 'P' para sumar 100 puntos
        {
            if (ScoreManager.Instancia != null)
            {
                ScoreManager.Instancia.SumarPuntos(100);
                ActualizarHUD();
            }
        }
    }

    public void RecibirDano(int cantidad)
    {
        if (esInvulnerable) return;

        saludActual -= cantidad;
        ActualizarHUD();
        Debug.Log($"<color=orange>[DAÑO RECIBIDO]</color> Salud actual: {saludActual}/{maxSaludPorVida} | Vidas restantes: {vidasActuales}");

        if (saludActual <= 0)
        {
            PerderVida();
        }
        else
        {
            StartCoroutine(RutinaInvulnerabilidad());
        }
    }

    private void PerderVida()
    {
        vidasActuales--;
        ActualizarHUD();

        if (vidasActuales > 0)
        {
            Debug.Log($"<color=red>[VIDA PERDIDA]</color> Te quedan {vidasActuales} vidas. Reapareciendo...");
            transform.position = posicionInicial;
            ReiniciarSalud();
            StartCoroutine(RutinaInvulnerabilidad());
        }
        else
        {
            Debug.Log("<color=red><b>[GAME OVER]</b></color> Has agotado las 5 vidas.");
        }
    }

    private void ReiniciarSalud()
    {
        saludActual = maxSaludPorVida;
        ActualizarHUD();
    }

    public void ActualizarHUD()
    {
        if (textoHUD != null)
        {
            int puntos = ScoreManager.Instancia != null ? ScoreManager.Instancia.GetPuntos() : 0;

            string corazones = new string('♥', saludActual) + new string('♡', maxSaludPorVida - saludActual);

            textoHUD.text = $"VIDAS: x{vidasActuales}\nSALUD: {corazones} ({saludActual}/{maxSaludPorVida})\nPUNTOS: {puntos:D6}";
        }
    }

    private IEnumerator RutinaInvulnerabilidad()
    {
        esInvulnerable = true;

        float tiempoTranscurrido = 0f;
        Color colorOriginal = spriteRenderer.color;

        while (tiempoTranscurrido < tiempoInvulnerable)
        {
            spriteRenderer.color = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, 0.2f);
            yield return new WaitForSeconds(frecuenciaParpadeo);

            spriteRenderer.color = colorOriginal;
            yield return new WaitForSeconds(frecuenciaParpadeo);

            tiempoTranscurrido += frecuenciaParpadeo * 2;
        }

        spriteRenderer.color = colorOriginal;
        esInvulnerable = false;
    }

    public int GetSaludActual() => saludActual;
    public int GetVidasActuales() => vidasActuales;
}