using UnityEngine;
using UnityEngine.UI;

public class ControladorMusica : MonoBehaviour
{
    public AudioSource musicaNivel;
    public AudioClip cancionNivel;
    public Slider sliderVolumen;
    
    public Button botonSilenciar;
    public Sprite spriteActivado;
    public Sprite spriteSilenciado;
    
    private bool estaSilenciado = false;
    private bool estaPausado = false;

    void Start()
    {
        if (musicaNivel != null && cancionNivel != null)
        {
            musicaNivel.clip = cancionNivel;
        }

        float volumenGuardado = PlayerPrefs.GetFloat("VolumenMusica", 1f);
        
        if (sliderVolumen != null)
        {
            sliderVolumen.value = volumenGuardado;
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }

        if (musicaNivel != null)
        {
            musicaNivel.volume = volumenGuardado;
            if (!musicaNivel.isPlaying)
            {
                musicaNivel.loop = true;
                musicaNivel.Play();
            }
        }
    }

    public void CambiarVolumen(float valor)
    {
        if (musicaNivel != null)
        {
            musicaNivel.volume = valor;
        }
        PlayerPrefs.SetFloat("VolumenMusica", valor);
        PlayerPrefs.Save();
    }

    public void AlternarSilencio()
    {
        estaSilenciado = !estaSilenciado;

        if (musicaNivel != null)
        {
            musicaNivel.mute = estaSilenciado;
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
        if (musicaNivel != null)
        {
            estaPausado = !estaPausado;

            if (estaPausado)
            {
                musicaNivel.Pause();
            }
            else
            {
                musicaNivel.UnPause();
            }
        }
    }
}
