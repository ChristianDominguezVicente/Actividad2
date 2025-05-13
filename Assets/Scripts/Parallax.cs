using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Vector3 direccion;
    [SerializeField] private float anchoImagen;

    private Vector3 posicionInicial;
    // Start is called before the first frame update
    void Start()
    {
        posicionInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Resto: Cuanto me queda de recorrido para alcanzar un nuevo ciclo.
        float resto = (velocidad * Time.time) % anchoImagen;

        // Mi posición se va refrescando desde la inicial SUMANDO tanto como resto me quede
        // en la dirección deseada.
        transform.position = posicionInicial + resto * direccion;
    }
}
