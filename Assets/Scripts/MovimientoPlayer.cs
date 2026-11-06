using UnityEngine;

public class MovimientoPlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;

    private Vector2 dirMov;
    [SerializeField] private float velMov, velSalto;
    private float movX = 0;
    private int numSaltos;
    private bool estaSaltando = false;

    public static int dirIdle = 1;
    public static int dirAtaque = 0;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform puntoSuelo; // Asigna aquí el objeto vacío "PuntoSuelo"
    [SerializeField] private float radioSuelo = 0.2f;
    [SerializeField] private LayerMask capaSuelo;  // Selecciona aquí la capa "Suelo"

    private bool estaEnSuelo;

    void Start()
    {
        anim.SetBool("saltando", false);
        anim.SetBool("saltandoIzq", false);
        numSaltos = 0;
    }

    void FixedUpdate()
    {
        Movimiento();
    }

    void Update()
    {
        // 1. Detección física instantánea de alta precisión
        estaEnSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, capaSuelo);

        // 2. Si toca el suelo y está cayendo o quieto, se reinician los saltos
        if (estaEnSuelo && rb.linearVelocity.y <= 0.1f)
        {
            numSaltos = 0;
            estaSaltando = false;
            anim.SetBool("saltando", false);
            anim.SetBool("saltandoIzq", false);
        }

        // 3. Control del salto doble
        if (Input.GetButtonDown("Jump"))
        {
            if (numSaltos < 2)
            {
                numSaltos++;
                estaSaltando = true;

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, velSalto);

                if (dirIdle == 1)
                {
                    anim.SetBool("saltando", true);
                    anim.SetBool("saltandoIzq", false);
                }
                else if (dirIdle == -1)
                {
                    anim.SetBool("saltandoIzq", true);
                    anim.SetBool("saltando", false);
                }
            }
        }
    }

    private void Movimiento()
    {
        movX = Input.GetAxisRaw("Horizontal");
        dirMov = new Vector2(movX, 0).normalized;
        rb.linearVelocity = new Vector2(dirMov.x * velMov, rb.linearVelocity.y);

        if (movX != 0)
        {
            AnimacionesPlayer(dirMov.x);
            if (movX > 0)
            {
                dirIdle = 1;
                dirAtaque = 1;
            }
            else
            {
                dirIdle = -1;
                dirAtaque = 2;
            }
        }
        else
        {
            anim.SetBool("corriendo", false);
            if (dirIdle == 1)
            {
                AnimacionesPlayer(0);
            }
            else if (dirIdle == -1)
            {
                AnimacionesPlayer(-10);
            }
        }

        ActualizaCapa();
    }

    private void ActualizaCapa()
    {
        if (CCC.atacando || CAD.disparando)
        {
            activaCapa("Atacar");
        }
        else if (estaSaltando)
        {
            activaCapa("Saltar");
        }
        else if (movX != 0 && estaEnSuelo)
        {
            activaCapa("Caminar");
        }
        else
        {
            activaCapa("Base Layer");
        }
    }

    private void activaCapa(string nombre)
    {
        int index = anim.GetLayerIndex(nombre);
        if (index == -1) return;

        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(index, 1);
    }

    private void AnimacionesPlayer(float n)
    {
        anim.SetFloat("movX", n);
    }

    // Dibuja un círculo rojo en la escena para calibrar el detector
    private void OnDrawGizmos()
    {
        if (puntoSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
        }
    }
}