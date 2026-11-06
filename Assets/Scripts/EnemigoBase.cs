using UnityEngine;

public class EnemigoBase : MonoBehaviour
{
    [Header("Estadísticas del Enemigo")]
    [SerializeField] protected int vidaMax = 1;
    protected int vidaActual;

    [SerializeField] protected int danioContacto = 1;
    [SerializeField] protected int puntosAlMorir = 100;

    protected virtual void Start()
    {
        vidaActual = vidaMax;
    }

    public virtual void TomarDanio(int cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log($"<color=red>[DAÑO ENEMIGO]</color> {gameObject.name} recibió daño. Vida restante: {vidaActual}");

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    protected virtual void Morir()
    {
        // Otorga los puntos al ScoreManager
        if (ScoreManager.Instancia != null)
        {
            ScoreManager.Instancia.SumarPuntos(puntosAlMorir);

            SaludPlayer salud = FindFirstObjectByType<SaludPlayer>();
            if (salud != null) salud.ActualizarHUD();
        }

        Destroy(gameObject);
    }

    // Daño al jugador al tocarlo
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SaludPlayer saludPlayer = collision.gameObject.GetComponent<SaludPlayer>();
            if (saludPlayer != null)
            {
                saludPlayer.RecibirDano(danioContacto);
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaludPlayer saludPlayer = collision.GetComponent<SaludPlayer>();
            if (saludPlayer != null)
            {
                saludPlayer.RecibirDano(danioContacto);
            }
        }
    }
}