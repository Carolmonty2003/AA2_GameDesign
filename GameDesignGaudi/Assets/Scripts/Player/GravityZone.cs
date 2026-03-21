using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class GravityZone : MonoBehaviour
{
    public Vector3 gravityDirection = Vector3.down;
    public float gravityForce = -20f;

    private List<Rigidbody> objectsInZone = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.SetGravityDirection(gravityDirection, gravityForce);
            }
        }

        
        if (other.CompareTag("GravityObject"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && !objectsInZone.Contains(rb))
            {
                rb.useGravity = false; 
                objectsInZone.Add(rb);
            }
        }
    }

/* Como no se que quedaria mejor, te dejo las dos opciones
DESCOMENTA ESTO SI QUIERES QUE AL SALIR DE LA ZONA VUELVA LA GRAVEDAD NORMAL DE UNITY
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GravityObject"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && objectsInZone.Contains(rb))
            {
                rb.useGravity = true;
                objectsInZone.Remove(rb);
            }
        }
    }

    */

    private void FixedUpdate()
    {
        //gravedad a todos los objetos dentro de la zona
        Vector3 customGravity = gravityDirection.normalized * Mathf.Abs(gravityForce);

        for (int i = objectsInZone.Count - 1; i >= 0; i--)
        {
            if (objectsInZone[i] == null)
            {
                objectsInZone.RemoveAt(i);
                continue;
            }
            objectsInZone[i].AddForce(customGravity, ForceMode.Acceleration);
        }
    }
}
