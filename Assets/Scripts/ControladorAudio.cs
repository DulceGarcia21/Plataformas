using UnityEngine;
using UnityEngine.UI;

public class ControladorAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip musicaFondo;
    public Slider sliderVolumen;
    public Button botonMute;
    public Sprite spriteConSonido;
    public Sprite spriteMuteado;

    private bool estaMuteado = false;
    private float volumenAnterior = 1f;

    void Start()
    {
        if (audioSource != null)
        {
            if (musicaFondo != null)
            {
                audioSource.clip = musicaFondo;
                audioSource.loop = true;
                audioSource.Play();
            }

            if (sliderVolumen != null)
            {
                sliderVolumen.value = audioSource.volume;
                sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
            }

            if (botonMute != null)
            {
                botonMute.onClick.AddListener(AlternarMute);
                ActualizarSpriteBoton();
            }
        }
    }

    public void CambiarVolumen(float valor)
    {
        if (!estaMuteado)
        {
            audioSource.volume = valor;
        }
    }

    public void AlternarMute()
    {
        estaMuteado = !estaMuteado;

        if (estaMuteado)
        {
            volumenAnterior = audioSource.volume;
            audioSource.volume = 0f;
        }
        else
        {
            audioSource.volume = volumenAnterior;
            if (sliderVolumen != null)
            {
                sliderVolumen.value = volumenAnterior;
            }
        }

        ActualizarSpriteBoton();
    }

    void ActualizarSpriteBoton()
    {
        if (botonMute != null)
        {
            Image imagenBoton = botonMute.GetComponent<Image>();
            if (imagenBoton != null)
            {
                imagenBoton.sprite = estaMuteado ? spriteMuteado : spriteConSonido;
            }
        }
    }
}