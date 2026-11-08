using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPausa : MonoBehaviour
{
    public string nombreEscenaMenu = "MenuNiveles";

    public void AbrirOpciones()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CerrarOpciones()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void RegresarAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}
