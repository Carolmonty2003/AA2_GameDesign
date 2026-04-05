using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    bool[] checkPointsDone = new bool[3];

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] MouseLook mouse;
    [SerializeField] PlayerGrab grab;
    [SerializeField] Camera camera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckPointToTick(int checkPointDone)
    {
        if (checkPointDone >= 0 && checkPointDone < checkPointsDone.Length)
        {
            checkPointsDone[checkPointDone] = true;

            if (AllCheckPointsDone())
            {
                playerMovement.enabled = false;
                StartCoroutine(GameOverCamera());
            }
        }
    }

    bool AllCheckPointsDone()
    {
        for (int i = 0; i < checkPointsDone.Length; i++)
        {
            if (!checkPointsDone[i])
                return false;
        }

        return true;
    }

    IEnumerator GameOverCamera()
    {
        yield return new WaitForSeconds(.1f);
        float duration = 1.5f; // duración total
        float time = 0f;

        Transform camTransform = camera.transform;

        Vector3 startPos = camTransform.position;
        Vector3 endPos = startPos + camTransform.forward * 1f; // avanzar 1 metro

        Quaternion startRot = camTransform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, 180f, 0f); // girar 180º en Y

        while (time < duration)
        {
            float t = time / duration;

            // interpolación suave
            camTransform.position = Vector3.Lerp(startPos, endPos, t);
            camTransform.rotation = Quaternion.Slerp(startRot, endRot, t);

            time += Time.deltaTime;
            yield return null;
        }

        // asegurar valores finales exactos
        float waitTimer = 0f;
        while (waitTimer < 2f)
        {
            camTransform.position = endPos;
            camTransform.rotation = endRot;
            waitTimer += Time.deltaTime;
            yield return null;
        }

        //Change Scene

        SceneManager.LoadScene("GameOver");
    }


}
