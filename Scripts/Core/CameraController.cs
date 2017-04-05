using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    static public CameraController cameraController;
    public GameObject followTarget;

    [SerializeField]
    float camSpeed;

    void Awake()
    {
        if(cameraController == null)
        {
            cameraController = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, followTarget.transform.position, Time.deltaTime * camSpeed);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
