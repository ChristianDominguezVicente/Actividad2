using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float factorGravedad;
    [SerializeField] private float alturaDeSalto;
    [SerializeField] private Transform camara;
    [SerializeField] private InputManagerSO inputManager;

    [Header("Detección suelo")]
    [SerializeField] private Transform pies;
    [SerializeField] private float radioDeteccion;
    [SerializeField] private LayerMask queEsSuelo;

    private CharacterController controller;
    private Vector3 direccionMovimiento;
    private Vector3 direccionInput;
    private Vector3 velocidadVertical;

    private void OnEnable()
    {
        inputManager.OnSaltar += Saltar;
        inputManager.OnMover += Mover;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        AplicarMovimiento();

        ActualizarMovimiento();

        ManejarVelocidadVertical();
    }

    // Solo se va a ejecutar cuando se actualice el input de movimiento
    private void Mover(Vector2 ctx)
    {
        direccionInput = new Vector3(ctx.x, 0, ctx.y);
    }

    private void Saltar()
    {
        if (EstoyEnSuelo())
        {
            velocidadVertical.y = Mathf.Sqrt(-2 * factorGravedad * alturaDeSalto);
        }
    }

    private void AplicarMovimiento()
    {
        direccionMovimiento = camara.forward * direccionInput.z + camara.right * direccionInput.x;
        direccionMovimiento.y = 0;
        controller.Move(direccionMovimiento * velocidadMovimiento * Time.deltaTime);
    }

    private void ManejarVelocidadVertical()
    {
        //Si hemos aterrizado..
        if (EstoyEnSuelo() && velocidadVertical.y < 0)
        {
            //Entonces, reseteo mi velocidad vertical.
            velocidadVertical.y = 0;
        }
        AplicarGravedad();
    }

    private void ActualizarMovimiento()
    {
        if (direccionMovimiento.sqrMagnitude > 0)
        {
            RotarHaciaDestino();
        }
    }

    private void AplicarGravedad()
    {
        velocidadVertical.y += factorGravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }

    private bool EstoyEnSuelo()
    {
        return Physics.CheckSphere(pies.position, radioDeteccion, queEsSuelo);
    }

    private void RotarHaciaDestino()
    {
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
        transform.rotation = rotacionObjetivo;
    }
}
