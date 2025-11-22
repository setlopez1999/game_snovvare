using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 200.0f;
    private Animator anima;
    public float x, y;

    public Rigidbody rb;
    public float fuerzaDeSalto = 8;
    public bool puedoSaltar;

    // Start is called before the first frame update
    void Start()
    {
        anima = GetComponent<Animator>();
        puedoSaltar = false;
    }

    // FixedUpdate: Usado para cálculos físicos
    private void FixedUpdate()
    {
        // Calcula el movimiento
        Vector3 movimiento = transform.forward * y * velocidadMovimiento * Time.deltaTime;
        Vector3 rotacion = new Vector3(0, x * velocidadRotacion * Time.deltaTime, 0);

        // Usa Rigidbody para mover y rotar el personaje
        rb.MovePosition(rb.position + movimiento);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotacion));
    }

    // Update: Controla las entradas del jugador
    void Update()
    {
        // Obtiene las entradas del jugador
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        // Actualiza los valores del Animator
        anima.SetFloat("EjeX", x);
        anima.SetFloat("EjeY", y);

        // Control del salto
        if (puedoSaltar)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anima.SetBool("Salte", true);
                rb.AddForce(new Vector3(0, fuerzaDeSalto, 0), ForceMode.Impulse);
            }
            anima.SetBool("TocoSuelo", true);
        }
        else
        {
            estoyCayendo();
        }
    }

    // Método para gestionar la animación de caída
    public void estoyCayendo()
    {
        anima.SetBool("TocoSuelo", false);
        anima.SetBool("Salte", false);
    }
}
