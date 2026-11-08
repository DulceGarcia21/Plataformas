using System.Collections;
using UnityEngine;
using TMPro;

public class SaludPlayer : MonoBehaviour
{
    [Header("Configuración de Vidas y Salud")]
    [SerializeField] private int maxSaludPorVida = 3;
    private int saludActual;

    [SerializeField] private int maxVidasTotales = 5;
    private int vidasActuales;

    [Header("UI Provisional")]
    [SerializeField] private TextMeshProUGUI textoHUD;

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
        if (Input.GetKeyDown(KeyCode.K))
        {
            RecibirDano(1);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (ScoreManager.Instancia != null)
            {
                ScoreManager.Instancia.SumarPuntos(100);
                ActualizarHUD();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("trampa"))
        {
            RecibirDano(1);
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


            int saludVisual = Mathf.Max(0, saludActual);

            string corazones = new string('♥', saludVisual) + new string('♡', maxSaludPorVida - saludVisual);

            textoHUD.text = $"VIDAS: x{vidasActuales}\nSALUD: {corazones} ({saludVisual}/{maxSaludPorVida})\nPUNTOS: {puntos:D6}";
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