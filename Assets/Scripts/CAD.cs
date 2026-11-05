using UnityEngine;

public class CAD : MonoBehaviour
{
    [SerializeField] private GameObject proyectil;
    public float tiempoEntreAtaques;
    public float tiempoSigAtaque;
    private Animator anim;
    public Transform puntoEmision;

    public static bool disparando = false;
    public int dirDisparo = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(tiempoSigAtaque < 0.05f && tiempoEntreAtaques >0){
            disparando = false;
        }
        if(tiempoSigAtaque > 0){
            tiempoSigAtaque -= Time.deltaTime;
        }
        if(Input.GetButtonDown("Fire2") && tiempoSigAtaque <= 0){
            disparando = true;
            activaCapa("Atacar");
            Dispara();
            tiempoSigAtaque = tiempoEntreAtaques;
        }
    }

    private void Dispara() {
        if (MovimientoPlayer.dirAtaque == 1) { //Derecha
            anim.SetTrigger("disparaDer");
        }
        else if (MovimientoPlayer.dirAtaque == 2) { //Izquierda
            anim.SetTrigger("disparaIzq");
        }
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoEmision.position, 0.25f);
    }

    private void EmiteProyectil(){
        // Instanciamos el proyectil y guardamos su referencia en una variable local 'nuevoProyectil'
        GameObject nuevoProyectil = Instantiate(proyectil, puntoEmision.position, transform.rotation);
        
        // Le asignamos su propia dirección individual al proyectil recién creado
        Proyectil scriptProyectil = nuevoProyectil.GetComponent<Proyectil>();
        if (scriptProyectil != null) {
            scriptProyectil.direccion = MovimientoPlayer.dirAtaque;
        }
    }

    private void activaCapa(string nombre){
        for(int i=0;i<anim.layerCount; i++){
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(anim.GetLayerIndex(nombre),1);
    }
}
