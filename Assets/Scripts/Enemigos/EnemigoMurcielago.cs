using UnityEngine;

public class EnemigoMurcielago : EnemigoBase
{
    [Header("Configuración Murciélago")]
    [SerializeField] private float distanciaDeteccion = 7f;
    [SerializeField] private float velocidadCaida = 5f;
    [SerializeField] private float velocidadVuelo = 4f;
    [SerializeField] private float fuerzaReboteJugador = 12f;

    private Transform jugador;
    private Vector3 posicionObjetivo;

    private enum Estado { Colgado, Bajando, Volando }
    private Estado estadoActual = Estado.Colgado;

    protected override void Start()
    {
        base.Start(); // Llama al Start de EnemigoBase para inicializar la vida

        // Busca automáticamente al jugador por su etiqueta
        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null)
        {
            jugador = objJugador.transform;
        }
    }

    private void Update()
    {
        if (jugador == null) return;

        switch (estadoActual)
        {
            case Estado.Colgado:
                // Verifica qué tan cerca está el jugador (X e Y)
                float distanciaX = Mathf.Abs(jugador.position.x - transform.position.x);
                float distanciaY = Mathf.Abs(jugador.position.y - transform.position.y);

                // Si el jugador entra en el rango, fija su altura y se lanza
                if (distanciaX < distanciaDeteccion && distanciaY < distanciaDeteccion)
                {
                    estadoActual = Estado.Bajando;
                    posicionObjetivo = new Vector3(transform.position.x, jugador.position.y, transform.position.z);
                }
                break;

            case Estado.Bajando:
                // Desciende hasta la altura en la que estaba el jugador
                transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, velocidadCaida * Time.deltaTime);

                if (Mathf.Abs(transform.position.y - posicionObjetivo.y) < 0.1f)
                {
                    estadoActual = Estado.Volando;
                }
                break;

            case Estado.Volando:
                // Avanza de derecha a izquierda infinitamente (atraviesa paredes al usar Translate)
                transform.Translate(Vector3.left * velocidadVuelo * Time.deltaTime);
                break;
        }
    }

    // Sobrescribimos la colisión para permitir que el jugador lo aplaste
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rbJugador = collision.gameObject.GetComponent<Rigidbody2D>();

            // Calculamos si el jugador viene cayendo y está por encima del centro del enemigo
            bool vieneDesdeArriba = collision.transform.position.y > transform.position.y + 0.3f;
            bool estaCayendo = rbJugador != null && rbJugador.linearVelocity.y <= 0;

            if (vieneDesdeArriba && estaCayendo)
            {
                // El jugador lo pisó: Aplicamos rebote al jugador e instakill al enemigo
                rbJugador.linearVelocity = new Vector2(rbJugador.linearVelocity.x, fuerzaReboteJugador);
                TomarDanio(vidaMax); // Llamamos al método base para que otorgue puntos y muera
            }
            else
            {
                // Impacto horizontal o desde abajo: El enemigo daña al jugador (Lógica de EnemigoBase)
                base.OnCollisionEnter2D(collision);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja el radio de detección en el editor
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccion);
    }
}
