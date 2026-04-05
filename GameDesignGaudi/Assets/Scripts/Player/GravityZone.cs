using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class GravityZone : MonoBehaviour
{
    [Header("Base Settings")]
    public Vector3 gravityDirection = Vector3.down;
    public float gravityForce = -20f;

    [Header("Rotation Settings")]
    public bool rotateGravity = false;
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 30f;

    [Header("Jump Modifier")]
    public bool modifyJump = false;
    public float jumpMultiplier = 1f;

    private float originalJumpHeight = -1f;

    private List<Rigidbody> objectsInZone = new List<Rigidbody>();
    private PlayerMovement playerInZone;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                playerInZone = player;
                player.SetGravityDirection(gravityDirection, gravityForce);

                // Guardar el jump original y aplicar el multiplicador si está activado
                originalJumpHeight = player.jumpHeight;
                if (modifyJump)
                {
                    player.jumpHeight = originalJumpHeight * jumpMultiplier;
                }
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                // Restore normal Unity gravity direction (down) and normal force
                player.SetGravityDirection(Vector3.down, player.normalGravity);
                Debug.Log("Salimos de la zona");

                // Restaurar siempre el jump original al salir
                if (originalJumpHeight >= 0f)
                {
                    player.jumpHeight = originalJumpHeight;
                    originalJumpHeight = -1f;
                }

                if (playerInZone == player)
                {
                    playerInZone = null;
                }
            }
        }

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

    private void Update()
    {
        if (rotateGravity)
        {
            // Rotate the gravity direction around the chosen axis
            gravityDirection = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, rotationAxis) * gravityDirection;
            
            // If the player is in the zone, update their gravity dynamically
            if (playerInZone != null)
            {
                playerInZone.SetGravityDirection(gravityDirection, gravityForce);
            }
        }
    }

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
