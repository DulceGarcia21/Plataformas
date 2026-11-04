using UnityEngine;

public class MovimientoPlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;

    private Vector2 dirMov;
    [SerializeField]private float velMov, velSalto;
    private float movX = 0;
    private int numSaltos;
    private bool estaSaltando = false;

    public static int dirIdle = 1;
    public static int dirAtaque = 0; // 0 -.Sin atacar, 1-. Ataque derecha, 2-. Ataque izq

    void Start(){
        anim.SetBool("saltando", false);
        anim.SetBool("saltandoIzq", false);
        numSaltos = 0;
    }

    void FixedUpdate(){
        Movimiento();
    }

    void Update()
    {
        if(Input.GetButtonDown("Jump")){
            estaSaltando = true;
            if(numSaltos <= 1){
                numSaltos++;
                ColisionSuelo.estaEnSuelo = false;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, velSalto);
                if(dirIdle == 1){  //drecha
                    anim.SetBool("saltando", true);
                }else if (dirIdle == -1){  //izquierda
                    anim.SetBool("saltandoIzq", true);
                }
            }
        } else {  
            anim.SetBool("saltando", false);
            anim.SetBool("saltandoIzq", false);
            if(ColisionSuelo.estaEnSuelo == true){
                numSaltos = 0;
            }
        }
    }

    private void Movimiento(){
        movX = Input.GetAxisRaw("Horizontal");
        dirMov = new Vector2(movX, 0).normalized;
        rb.linearVelocity = new Vector2(dirMov.x*velMov, rb.linearVelocity.y);
        if(movX != 0){
            AnimacionesPlayer(dirMov.x);
            if(movX >0){ //derecha
                dirIdle =1;
                dirAtaque =1;
            }else{  //Izquierda
                dirIdle =-1;
                dirAtaque =2;
            }
        }else{ //estatico
            anim.SetBool("corriendo", false);
            if(dirIdle == 1){
                AnimacionesPlayer(0);
            }else if (dirIdle == -1){
                AnimacionesPlayer(-10);
            }
        }
        anim.SetBool("saltando", false);
        ActualizaCapa();
    }

    private void ActualizaCapa(){
       if(CCC.atacando || CAD.disparando){//Si esta atacando cuerpo acuerpo o a distancia
            activaCapa("Atacar");
        }else if(movX !=0){//si esta caminando
            activaCapa("Caminar");
        }else if(estaSaltando){
            activaCapa("Saltar");
        }/*else if(){
            activaCapa("Daño");
        }*/
    }

    private void activaCapa(string nombre){
        for(int i=0;i<anim.layerCount; i++){
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(anim.GetLayerIndex(nombre),1);
    }

    private void AnimacionesPlayer(float n){
        anim.SetFloat("movX", n);
    }
}