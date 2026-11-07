using UnityEngine;

public class CCC : MonoBehaviour
{
    public Transform controladorGolpe;
    public float radioGolpe;
    public int danioGolpe = 1;
    public float tiempoEntreAtaques;
    public float tiempoSigAtaque;
    private Animator anim;

    public static bool atacando = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
       
        float posX = Mathf.Abs(controladorGolpe.localPosition.x);
        if (MovimientoPlayer.dirAtaque == 1) 
        {
            controladorGolpe.localPosition = new Vector2(posX, controladorGolpe.localPosition.y);
        }
        else if (MovimientoPlayer.dirAtaque == 2)
        {
            controladorGolpe.localPosition = new Vector2(-posX, controladorGolpe.localPosition.y);
        }

        if (tiempoSigAtaque < 0.05f && tiempoEntreAtaques > 0)
        {
            atacando = false;
        }
        if (tiempoSigAtaque > 0)
        {
            tiempoSigAtaque -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Fire1") && tiempoSigAtaque <= 0)
        {
            atacando = true;
            activaCapa("Atacar");
            Golpe();
            tiempoSigAtaque = tiempoEntreAtaques;
        }
    }

    private void Golpe()
    {
        if (MovimientoPlayer.dirAtaque == 1)
        { 
            anim.SetTrigger("ataqueDer");
        }
        else if (MovimientoPlayer.dirAtaque == 2)
        {   
            anim.SetTrigger("ataqueIzq");
        }
    }

    private void VerificaGolpe()
    {
        Collider2D[] objs = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D colisionador in objs)
        {
            if (colisionador.CompareTag("enemigo"))
            {
                EnemigoBase enemigo = colisionador.GetComponent<EnemigoBase>();
                if (enemigo != null)
                {
                    enemigo.TomarDanio(danioGolpe);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }

    private void activaCapa(string nombre)
    {
        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(anim.GetLayerIndex(nombre), 1);
    }
}