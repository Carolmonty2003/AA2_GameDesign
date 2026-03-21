using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GravityZone : MonoBehaviour
{
    public Vector3 gravityDirection = Vector3.down;
    public float gravityForce = -20f;

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
    }
}
