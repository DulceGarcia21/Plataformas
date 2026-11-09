using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SaludPlayer : MonoBehaviour
{
    [Header("Configuración de Salud (Corazones)")]
    [SerializeField] private int maxCorazones = 3;
    private int corazonesActuales;

    [Header("UI y Game Over")]
    [SerializeField] private TextMeshProUGUI textoHUD;
    [SerializeField] private GameObject panelGameOver; 

    [Header("Invulnerabilidad (i-Frames)")]
    [SerializeField] private float tiempoInvulnerable = 1.5f;
    [SerializeField] private float frecuenciaParpadeo = 0.1f;
    private bool esInvulnerable = false;
    private bool estaMuerto = false;

    [Header("Efectos de Sonido")]
    [SerializeField] private AudioSource fuenteAudio;
    [SerializeField] private AudioClip sonidoDaño;
    [SerializeField] private AudioClip sonidoCurar;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        corazonesActuales = maxCorazones;

        if (fuenteAudio == null)
        {
            fuenteAudio = GetComponent<AudioSource>();
        }

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }

        ActualizarHUD();

        if (textoHUD == null)
        {
            Debug.LogError("<b>[ERROR UI]</b> La casilla 'Texto HUD' en el Player está VACÍA. Arrastra el objeto UI_HUD.");
        }
    }

    void Update()
    {
        if (estaMuerto) return;

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
        if (esInvulnerable || estaMuerto) return;

        corazonesActuales -= cantidad;
        ActualizarHUD();
        
        if (fuenteAudio != null && sonidoDaño != null)
        {
            fuenteAudio.PlayOneShot(sonidoDaño);
        }

        Debug.Log($"<color=orange>[DAÑO RECIBIDO]</color> Corazones actuales: {corazonesActuales}/{maxCorazones}");

        if (corazonesActuales <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(RutinaInvulnerabilidad());
        }
    }

    public void Curar(int cantidad)
    {
        if (estaMuerto) return;

        corazonesActuales += cantidad;

        if (corazonesActuales > maxCorazones)
        {
            corazonesActuales = maxCorazones;
        }

        ActualizarHUD();

        if (fuenteAudio != null && sonidoCurar != null)
        {
            fuenteAudio.PlayOneShot(sonidoCurar);
        }

        Debug.Log($"<color=green>[CURACIÓN]</color> Corazones actuales: {corazonesActuales}/{maxCorazones}");
    }

    private void GameOver()
    {
        estaMuerto = true;
        Debug.Log("<color=red><b>[GAME OVER]</b></color> Has perdido todos los corazones.");

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ActualizarHUD()
    {
        if (textoHUD != null)
        {
            int puntos = ScoreManager.Instancia != null ? ScoreManager.Instancia.GetPuntos() : 0;
            int saludVisual = Mathf.Max(0, corazonesActuales);

            string corazones = new string('♥', saludVisual) + new string('♡', maxCorazones - saludVisual);
            
            textoHUD.text = $"SALUD: {corazones} ({saludVisual}/{maxCorazones})\nPUNTOS: {puntos:D6}";
        }
    }

    public void MuerteAlVacio()
    {
        if (estaMuerto) return;

        esInvulnerable = false; 
        RecibirDano(3);         
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

    public int GetCorazonesActuales() => corazonesActuales;
}