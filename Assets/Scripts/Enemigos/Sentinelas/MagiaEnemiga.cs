using UnityEngine;

public class MagiaEnemiga : MonoBehaviour
{
    public float velocidad = 6f;
    public int danio = 1;
    [HideInInspector] public int direccion = 1; 

    void Start()
    {

        Destroy(gameObject, 3f);
    }

    void Update()
    {

        transform.Translate(Vector2.right * direccion * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaludPlayer salud = collision.GetComponent<SaludPlayer>();
            if (salud != null)
            {
                salud.RecibirDano(danio);
            }
            Destroy(gameObject);
        }
    }
}