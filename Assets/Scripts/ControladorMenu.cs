using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    public GameObject panelOpciones;
    //public ControladorSonido controladorSonido;

    void Start()
    {
        panelOpciones.SetActive(false);
    }

    /*public void IrAIntro(){
        SceneManager.LoadScene("Intro");
    }*/

    public void IrACreditos(){
        SceneManager.LoadScene("Creditos");
    }

    public void AbreOpciones(){
        panelOpciones.SetActive(true);
    }

    public void CierraOpciones(){
        panelOpciones.SetActive(false);
    }

    public void SalirJuego(){
        Application.Quit();
    }
}
