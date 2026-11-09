using UnityEngine;
using UnityEngine.UI;

public class EfectoSonidoBoton : MonoBehaviour
{
    public AudioClip sonidoClick;
    
    private Button boton;
    private AudioSource fuenteAudio;

    void Start()
    {
        boton = GetComponent<Button>();
        
        fuenteAudio = GetComponent<AudioSource>();
        if (fuenteAudio == null)
        {
            fuenteAudio = gameObject.AddComponent<AudioSource>();
        }
        
        fuenteAudio.playOnAwake = false;
        
        if (boton != null)
        {
            boton.onClick.AddListener(ReproducirSonido);
        }
    }

    void ReproducirSonido()
    {
        if (sonidoClick != null && fuenteAudio != null)
        {
            fuenteAudio.PlayOneShot(sonidoClick);
        }
    }
}
