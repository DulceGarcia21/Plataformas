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

    public virtual void TomarDanioDistancia(int cantidad)
    {
        // Por defecto, hace exactamente lo mismo que el daño normal
        TomarDanio(cantidad);
    }

    protected virtual void Morir()
    {

        if (ScoreManager.Instancia != null)
        {
            ScoreManager.Instancia.SumarPuntos(puntosAlMorir);

            SaludPlayer salud = FindFirstObjectByType<SaludPlayer>();
            if (salud != null) salud.ActualizarHUD();
        }

        Destroy(gameObject);
    }

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