using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    public void RegresarAInicio()
    {
        Time.timeScale = 1f; // Asegura que el tiempo corra con normalidad
        SceneManager.LoadScene("Inicio");
    }
}