using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorNiveles : MonoBehaviour
{
    public Button[] botonesNiveles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int nivelDesbloqueado = PlayerPrefs.GetInt("NivelDesbloqueado", 1);
        
        for (int i = 0; i < botonesNiveles.Length; i++){
            if (i + 1 <= nivelDesbloqueado){
                botonesNiveles[i].interactable = true; // Habilitado
            }else{
                botonesNiveles[i].interactable = false; // Deshabilitado (bloqueado)
            }
        }
    }

    public void CargarNivel(string nombreEscena){
        SceneManager.LoadScene(nombreEscena);
    }

    public static void DesbloquearSiguienteNivel(int nivelACargar)
    {
        int nivelDesbloqueadoActual = PlayerPrefs.GetInt("NivelDesbloqueado", 1);
        
        if (nivelACargar > nivelDesbloqueadoActual)
        {
            PlayerPrefs.SetInt("NivelDesbloqueado", nivelACargar);
            PlayerPrefs.Save();
        }
    }
    public void BorrarProgreso()
    {
        PlayerPrefs.DeleteKey("NivelDesbloqueado");
        Debug.Log("Progreso reiniciado");
        Start(); // Actualiza los botones en tiempo real
    }
}
