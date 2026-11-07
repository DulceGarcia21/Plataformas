using UnityEngine;

public class Canon : MonoBehaviour
{
    [Header("Configuración Cañón")]
    [SerializeField] private GameObject prefabBala;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float tiempoEntreDisparos = 3f;
    [SerializeField] private float distanciaDeteccion = 12f;
    [SerializeField] private int direccionDisparo = -1; // -1 Izquierda, 1 Derecha

    private float temporizador;
    private Transform jugador;

    private void Start()
    {
        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null) jugador = objJugador.transform;

        temporizador = tiempoEntreDisparos;
    }

    private void Update()
    {
        if (jugador == null) return;

        float distanciaX = Mathf.Abs(jugador.position.x - transform.position.x);
        float distanciaY = Mathf.Abs(jugador.position.y - transform.position.y);


        bool jugadorEnFrente = false;
        if (direccionDisparo == 1 && jugador.position.x > transform.position.x)
        {
            jugadorEnFrente = true; 
        }
        else if (direccionDisparo == -1 && jugador.position.x < transform.position.x)
        {
            jugadorEnFrente = true; 
        }

        if (jugadorEnFrente && distanciaX < distanciaDeteccion && distanciaY < 5f)
        {
            temporizador -= Time.deltaTime;
            if (temporizador <= 0f)
            {
                Disparar();
                temporizador = tiempoEntreDisparos;
            }
        }
    }

    private void Disparar()
    {
        if (prefabBala != null && puntoDisparo != null)
        {
            GameObject nuevaBala = Instantiate(prefabBala, puntoDisparo.position, Quaternion.identity);

            EnemigoBala scriptBala = nuevaBala.GetComponent<EnemigoBala>();
            if (scriptBala != null)
            {
       
                scriptBala.direccion = direccionDisparo;

      
                SpriteRenderer sr = nuevaBala.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
    
                    sr.flipX = (direccionDisparo == 1);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccion);
    }
}