using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [SerializeField] private float velocidad = 8.0f;
    public int direccion = 1;
    private SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Start() {
        // Si va a la izquierda (2), volteamos el sprite horizontalmente
        if (direccion == 2 && spriteRenderer != null) {
            spriteRenderer.flipX = true;
        }
    }

    void FixedUpdate() {
        if (direccion == 1) { // Derecha
            transform.position += new Vector3(1, 0, 0) * Time.deltaTime * velocidad;
        } else if (direccion == 2) { // Izquierda
            transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * velocidad;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "limites") {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "enemigo") {
            //collision.transform.GetComponent<Enemigo>().TomarDano(1);
            Destroy(this.gameObject);
        }
    }
}
