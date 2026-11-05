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
        // 1. Detectar si aterrizamos para reiniciar saltos y apagar la animación de salto
        if (ColisionSuelo.estaEnSuelo && rb.linearVelocity.y <= 0.1f)
        {
            numSaltos = 0;
            estaSaltando = false;
            anim.SetBool("saltando", false);
            anim.SetBool("saltandoIzq", false);
        }

        // 2. Lógica del salto y doble salto
        if (Input.GetButtonDown("Jump"))
        {
            if (numSaltos < 2)
            {
                numSaltos++;
                estaSaltando = true;

                // Reiniciar la velocidad vertical asegura que el doble salto tenga la misma altura
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
        { // Prioridad al salto sobre el caminar
            activaCapa("Saltar");
        }
        else if (movX != 0 && ColisionSuelo.estaEnSuelo)
        { // Solo caminar si está en el suelo
            activaCapa("Caminar");
        }
        else
        {
            // Regresar a la capa base (Idle) cuando no hace nada
            activaCapa("Base Layer");
        }
    }

    private void activaCapa(string nombre)
    {
        int index = anim.GetLayerIndex(nombre);
        if (index == -1) return; // Evita errores si la capa "Base Layer" tiene otro nombre

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
}