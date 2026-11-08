using UnityEngine;

public class ZonaMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            SaludPlayer saludPlayer = collision.GetComponent<SaludPlayer>();
            if (saludPlayer != null)
            {
                saludPlayer.MuerteAlVacio();
            }
        }
    }
}