using UnityEngine;
using UnityEngine.SceneManagement;

public class Victoria : MonoBehaviour
{
    public GameObject panelVictoria;

    public int numeroNivelActual = 1; 
    public string nombreEscenaSiguiente = "Nivel2"; 
    public string nombreEscenaMenu = "MenuNiveles";

    private void Start(){
        if (panelVictoria != null){
            panelVictoria.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            GanarNivel();
        }
    }

    void GanarNivel()
    {
        // Desbloquea el siguiente nivel basandose en el numero configurado aquí
        int siguienteNivelADesbloquear = numeroNivelActual + 1;
        int nivelGuardado = PlayerPrefs.GetInt("NivelDesbloqueado", 1);

        if (siguienteNivelADesbloquear > nivelGuardado){
            PlayerPrefs.SetInt("NivelDesbloqueado", siguienteNivelADesbloquear);
            PlayerPrefs.Save();
        }

        if (panelVictoria != null){
            panelVictoria.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void IrAlMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }

    public void IrAlSiguienteNivel(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaSiguiente);
    }
}
