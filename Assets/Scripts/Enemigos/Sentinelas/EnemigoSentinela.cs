using UnityEngine;

public class EnemigoCentinela : EnemigoBase
{
    [Header("Configuración de Patrullaje")]
    [SerializeField] protected float distanciaPatrulla = 5f;
    [SerializeField] protected float velocidadMovimiento = 2f;

    [Header("Configuración de Persecución y Combate")]
    [SerializeField] protected float rangoVision = 6f; // Cuándo te detecta
    [SerializeField] protected float rangoAtaque = 1.5f; // Cuándo se detiene a golpear
    [SerializeField] protected float velocidadPersecucion = 3.5f; // Puede correr más rápido al seguirte
    [SerializeField] protected float tiempoEntreAtaques = 1.5f;
    [SerializeField] protected int danioCuerpoACuerpo = 1;

    protected Vector3 posicionInicial;
    protected bool moviendoADerecha = true;
    protected bool persiguiendo = false; // <-- El interruptor que lo vuelve implacable

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Transform jugador;
    protected float temporizadorAtaque;

    protected override void Start()
    {
        base.Start();
        posicionInicial = transform.position;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null) jugador = objJugador.transform;

        if (moviendoADerecha)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    protected virtual void Update()
    {
        temporizadorAtaque -= Time.deltaTime;

        // Si el jugador desaparece, deja de perseguir
        if (jugador == null) return;

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        // 1. DETECCIÓN: Si entra en su visión, enciende la persecución para siempre
        if (!persiguiendo && distanciaAlJugador <= rangoVision)
        {
            persiguiendo = true;
        }

        // 2. ACCIÓN: Decide si patrulla o te caza
        if (persiguiendo)
        {
            PerseguirYAtacar(distanciaAlJugador);
        }
        else
        {
            Patrullar();
        }
    }

    protected void PerseguirYAtacar(float distanciaAlJugador)
    {
        // Voltea a mirarte sin importar dónde estés
        if (jugador.position.x > transform.position.x && !moviendoADerecha) Girar();
        else if (jugador.position.x < transform.position.x && moviendoADerecha) Girar();

        // Si estás lejos, corre hacia ti
        if (distanciaAlJugador > rangoAtaque)
        {
            float direccion = moviendoADerecha ? 1 : -1;
            rb.linearVelocity = new Vector2(direccion * velocidadPersecucion, rb.linearVelocity.y);

            // Le enviamos la velocidad al Animator para que active la animación Run
            if (anim != null) anim.SetFloat("velocidadX", Mathf.Abs(rb.linearVelocity.x));
        }
        // Si ya está pegado a ti, se detiene y ataca
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if (anim != null) anim.SetFloat("velocidadX", 0);

            if (temporizadorAtaque <= 0f)
            {
                Atacar();
                temporizadorAtaque = tiempoEntreAtaques;
            }
        }
    }

    protected void Patrullar()
    {
        float limiteIzquierdo = posicionInicial.x - distanciaPatrulla;
        float limiteDerecho = posicionInicial.x + distanciaPatrulla;

        if (moviendoADerecha)
        {
            rb.linearVelocity = new Vector2(velocidadMovimiento, rb.linearVelocity.y);
            if (transform.position.x >= limiteDerecho) Girar();
        }
        else
        {
            rb.linearVelocity = new Vector2(-velocidadMovimiento, rb.linearVelocity.y);
            if (transform.position.x <= limiteIzquierdo) Girar();
        }

        if (anim != null)
        {
            anim.SetFloat("velocidadX", Mathf.Abs(rb.linearVelocity.x));
        }
    }

    protected void Girar()
    {
        moviendoADerecha = !moviendoADerecha;
        Vector3 escalaLocal = transform.localScale;
        escalaLocal.x *= -1;
        transform.localScale = escalaLocal;
    }

    protected virtual void Atacar()
    {
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }

        if (jugador != null)
        {
            SaludPlayer salud = jugador.GetComponent<SaludPlayer>();
            if (salud != null)
            {
                salud.RecibirDano(danioCuerpoACuerpo);
            }
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoVision);


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }
}