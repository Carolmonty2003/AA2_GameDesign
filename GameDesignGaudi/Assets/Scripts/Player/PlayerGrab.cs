using System.Collections;
using UnityEngine;

/// <summary>
/// Permite al jugador agarrar objetos con una tecla.
/// Si el objeto tiene la layer "rotate", F lo rota 90° en Y en lugar de soltarlo.
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
    [Tooltip("Nombre de la layer que activa la rotación en lugar de soltar.")]
    [SerializeField] private LayerMask nombreLayerRotar;
    [Tooltip("Tecla utilizada para coger, soltar o rotar el objeto.")]
    [SerializeField] private KeyCode teclaAgarre = KeyCode.F;
    [Tooltip("Velocidad en grados por segundo a la que rota el objeto.")]
    [SerializeField] private float velocidadRotacion = 180f;

    private bool rotando = false;

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
                
                if (!IntentarRotar())
                {
                    
                    IntentarAgarrar();
                }
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

    private bool IntentarRotar()
    {
        Ray rayo = new Ray(transform.position, transform.forward);
        RaycastHit impacto;

        if (Physics.Raycast(rayo, out impacto, distanciaAgarre, nombreLayerRotar))
        {
            if (!rotando)
            {
                StartCoroutine(RotarSuavemente(impacto.collider.transform));
            }
            return true;
        }

        return false;
    }

    private IEnumerator RotarSuavemente(Transform objetivo)
    {
        rotando = true;

        float gradosRestantes = 90f;

        while (gradosRestantes > 0f)
        {
            float paso = velocidadRotacion * Time.deltaTime;
            paso = Mathf.Min(paso, gradosRestantes); // No pasarse de 90°
            objetivo.Rotate(Vector3.up, paso, Space.World);
            gradosRestantes -= paso;
            yield return null;
        }

        rotando = false;
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

    private void RotarObjeto()
    {
        
        objetoAgarrado.transform.Rotate(Vector3.up, 90f, Space.World);
    }

    private void SoltarObjeto()
    {
        objetoAgarrado.transform.SetParent(padreOriginal);

        if (rbObjetoAgarrado != null)
        {
            rbObjetoAgarrado.isKinematic = eraKinematic;
        }

        objetoAgarrado = null;
        rbObjetoAgarrado = null;
    }

    private bool EsLayerRotar(GameObject obj)
    {
        return (nombreLayerRotar.value & (1 << obj.layer)) != 0;
    }
}