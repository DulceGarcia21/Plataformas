using UnityEngine;

public class EnemigoBala : EnemigoBase
{
    [Header("Configuración Bala")]
    public float velocidad = 6f;
    public int direccion = -1; // -1 Izquierda, 1 Derecha
    public float tiempoDeVida = 5f; // Para que se destruya si no choca con nada

    protected override void Start()
    {
        base.Start(); // Inicializa la vida de EnemigoBase
        Destroy(gameObject, tiempoDeVida); // Limpieza automática

        // Voltea el sprite si dispara hacia la derecha
        if (direccion == 1 && GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void Update()
    {
        // Movimiento constante en línea recta
        transform.Translate(new Vector3(direccion, 0, 0) * velocidad * Time.deltaTime);
    }

    // AQUI ESTÁ LA MAGIA: Sobrescribimos el daño a distancia
    public override void TomarDanioDistancia(int cantidad)
    {
        // Al dejar esto vacío, la bala ignora por completo el daño de tu Proyectil.
        // Tu proyectil chocará y se destruirá, pero la bala seguirá intacta.
        Debug.Log("<color=cyan>[BALA]</color> Inmune al ataque a distancia.");
    }

    // (Opcional) Si quieres que la bala desaparezca al golpear una pared
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Lógica original de hacer daño al jugador

        if (collision.CompareTag("Suelo") || collision.CompareTag("limites"))
        {
            Destroy(gameObject);
        }
    }
}