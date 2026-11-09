using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class controlmusicaMenu : MonoBehaviour
{
    public GameObject panelOpciones; // Arrastra aquí tu panel de opciones
    public AudioSource musicaMenu;
    public AudioClip cancionMenu;
    public Slider sliderVolumen;
    
    public Button botonSilenciar;
    public Sprite spriteActivado;
    public Sprite spriteSilenciado;
    
    public string nombreEscenaInicio = "MenuPrincipal";
    
    private bool estaSilenciado = false;
    private bool estaPausado = false;

    void Start()
    {
        if (musicaMenu != null && cancionMenu != null)
        {
            musicaMenu.clip = cancionMenu;
        }

        float volumenGuardado = PlayerPrefs.GetFloat("VolumenMusica", 1f);
        
        if (sliderVolumen != null)
        {
            sliderVolumen.value = volumenGuardado;
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }

        if (musicaMenu != null)
        {
            musicaMenu.volume = volumenGuardado;
            if (!musicaMenu.isPlaying)
            {
                musicaMenu.loop = true;
                musicaMenu.Play();
            }
        }

        // Aseguramos que el panel inicie cerrado
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }
    }

    public void AbrirOpciones()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(true);
        }
    }

    public void CerrarOpciones()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }
    }

    public void RegresarAlInicio()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaInicio);
    }

    public void CambiarVolumen(float valor)
    {
        if (musicaMenu != null)
        {
            musicaMenu.volume = valor;
        }
        PlayerPrefs.SetFloat("VolumenMusica", valor);
        PlayerPrefs.Save();
    }

    public void AlternarSilencio()
    {
        estaSilenciado = !estaSilenciado;

        if (musicaMenu != null)
        {
            musicaMenu.mute = estaSilenciado;
        }

        if (botonSilenciar != null)
        {
            Image imagenBoton = botonSilenciar.GetComponent<Image>();
            if (imagenBoton != null)
            {
                if (estaSilenciado)
                {
                    imagenBoton.sprite = spriteSilenciado;
                }
                else
                {
                    imagenBoton.sprite = spriteActivado;
                }
            }
        }
    }

    public void PausarReanudarMusica()
    {
        if (musicaMenu != null)
        {
            estaPausado = !estaPausado;

            if (estaPausado)
            {
                musicaMenu.Pause();
            }
            else
            {
                musicaMenu.UnPause();
            }
        }
    }
}
