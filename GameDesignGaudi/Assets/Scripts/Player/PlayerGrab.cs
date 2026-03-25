using UnityEngine;

/// <summary>
/// Permite al jugador agarrar objetos con una tecla.
/// Ideal para colocar en la Cámara Principal (Main Camera) del jugador.
/// </summary>
public class PlayerGrab : MonoBehaviour
{
    [Header("Opciones de Agarre")]
    [Tooltip("Distancia máxima a la que el raycast detectará el objeto.")]
    [SerializeField] private float distanciaAgarre = 3f;
    [Tooltip("Distancia a la que se mantendrá flotando el objeto frente a la cámara.")]
    [SerializeField] private float distanciaSujecion = 2f;
    [Tooltip("Capa (Layer) de los objetos que se pueden coger (ej. interactables o grabbable).")]
    [SerializeField] private LayerMask layerGrabbable;
    [Tooltip("Tecla utilizada para coger y soltar el objeto.")]
    [SerializeField] private KeyCode teclaAgarre = KeyCode.F;

    private GameObject objetoAgarrado;
    private Rigidbody rbObjetoAgarrado;
    private bool eraKinematic;
    private Transform padreOriginal;

    private void Update()
    {
        
        if (Input.GetKeyDown(teclaAgarre))
        {
            
            if (objetoAgarrado == null)
            {
                IntentarAgarrar();
            }
           
            else
            {
                SoltarObjeto();
            }
        }

        
        if (objetoAgarrado != null)
        {
            ActualizarPosicionObjeto();
        }
    }

    private void IntentarAgarrar()
    {
    
        Ray rayo = new Ray(transform.position, transform.forward);
        RaycastHit impacto;

        
        if (Physics.Raycast(rayo, out impacto, distanciaAgarre, layerGrabbable))
        {
            
            objetoAgarrado = impacto.collider.gameObject;
            rbObjetoAgarrado = objetoAgarrado.GetComponent<Rigidbody>();

            
            if (rbObjetoAgarrado != null)
            {
                eraKinematic = rbObjetoAgarrado.isKinematic;
                rbObjetoAgarrado.isKinematic = true; 
            }

            
            padreOriginal = objetoAgarrado.transform.parent;
            
            
            objetoAgarrado.transform.SetParent(this.transform);
        }
    }

    private void ActualizarPosicionObjeto()
    {
        
        objetoAgarrado.transform.localPosition = Vector3.forward * distanciaSujecion;
    }

    private void SoltarObjeto()
    {
        
        objetoAgarrado.transform.SetParent(padreOriginal);

        
        if (rbObjetoAgarrado != null)
        {
            rbObjetoAgarrado.isKinematic = eraKinematic;
        }

        // Limpiar nuestras variables
        objetoAgarrado = null;
        rbObjetoAgarrado = null;
    }
}
