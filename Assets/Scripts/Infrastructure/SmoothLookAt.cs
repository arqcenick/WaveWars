using UnityEngine;
using System.Collections;

public class SmoothLookAt : MonoBehaviour
{
    public GameObject target;
    public float size = 10;
    public float scrollSpeed = 30;

    Vector3 pos;
    private Camera cam;

    void Start()
    {
        target = GameController.Player1;
        this.cam = (Camera)this.gameObject.GetComponent("Camera");
        this.cam.orthographic = true;
        this.cam.transform.rotation = Quaternion.Euler(30, 45, 0);

        pos = target.transform.position;
    }

    void Update()
    {
        this.cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

        float distance = 30;

        //transform.position = target.transform.position + new Vector3(-distance, distance, -distance);

        transform.position = Vector3.Lerp(transform.position, target.transform.position + new Vector3(-distance, distance, -distance), 0.5f * Time.deltaTime);
        this.cam.transform.LookAt(target.transform);
    }

    void OnGUI()
    {
        //    GUI.Label(new Rect(10, 100, 200, 50), "" + target.transform.position.x + ", " + target.transform.position.y + ", " + target.transform.position.z);
        //    GUI.Label(new Rect(10, 130, 200, 50), "" + cam.transform.position.x + ", " + cam.transform.position.y + ", " + cam.transform.position.z);
    }
}