using UnityEngine;

/// <summary>
/// Hace que si este GameObject rota en el eje Y, OTRO GameObject asignado rote la misma cantidad en el eje X.
/// </summary>
public class SyncRotationZX : MonoBehaviour
{
    [Tooltip("El GameObject que va a recibir la rotación en su eje X. Si lo dejas vacío, rotará este mismo objeto.")]
    public Transform objetoDestino;

    [Tooltip("Si es true, la rotación ocurrirá en el espacio global del mundo. Si es false, en el espacio local.")]
    public bool espacioMundo = false;

    private float previousY;

    void Start()
    {
        previousY = GetYAngle();
        
        // Si no asignas ningún objeto externo en el Inspector, se afecta a sí mismo
        if (objetoDestino == null)
        {
            objetoDestino = transform; 
        }
    }

    void Update()
    {
        // Leemos la rotación Y del objeto que tiene este script (el "origen")
        float currentY = GetYAngle();
        
        // Calculamos cuánto ha cambiado la Y desde el último frame
        float deltaY = Mathf.DeltaAngle(previousY, currentY);
        
        if (Mathf.Abs(deltaY) > 0.001f)
        {
            // Aplicamos esa diferencia al X del objeto de destino
            if (espacioMundo)
            {
                objetoDestino.Rotate(deltaY, 0, 0, Space.World);
            }
            else
            {
                objetoDestino.Rotate(deltaY, 0, 0, Space.Self);
            }

            previousY = currentY;
        }
    }

    private float GetYAngle()
    {
        return espacioMundo ? transform.eulerAngles.y : transform.localEulerAngles.y;
    }
}
