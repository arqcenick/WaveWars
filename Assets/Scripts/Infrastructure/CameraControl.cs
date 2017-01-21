using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	// Use this for initialization

	public GameObject initObject;
    public static GameObject Player1;
    public static GameObject Winner;
    public static GameObject Player2;
    PlayerController playControl;

	private const float rotateSpeed = 1f;

	private float camSpeed, zoomSpeed, tempRotateSpeed;
	private bool rotateClock, rotateCounter;
    private float SelectedOrtographicSize;



    private float currentEuler;
    public bool DebugMode;
    

	void Start () {

		//transform.LookAt(initObject.transform.position);
		camSpeed = 10;
		zoomSpeed = 15;
		tempRotateSpeed = rotateSpeed;
        SelectedOrtographicSize = Camera.main.orthographicSize;
        rotateClock = false;
		rotateCounter = false;

    }

    // Update is called once per frame

    void Update() {


        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, SelectedOrtographicSize, 0.10f);
        if (DebugMode)
        {
            if (Input.anyKey)
            {

                if (SelectedOrtographicSize / Camera.main.orthographicSize < 1.001f && SelectedOrtographicSize / Camera.main.orthographicSize > 0.999f)
                    Camera.main.orthographicSize = SelectedOrtographicSize;
                //Camera movement
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate((Vector3.forward + Vector3.up) / Mathf.Sqrt(2) * camSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Translate(Vector3.left * camSpeed * Time.deltaTime);

                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    transform.Translate((Vector3.back + Vector3.down) / Mathf.Sqrt(2) * camSpeed * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    transform.Translate(Vector3.right * camSpeed * Time.deltaTime);

                }

                if (Input.GetKey(KeyCode.E))
                {

                    transform.RotateAround(transform.position + transform.forward * transform.position.y * Mathf.Sqrt(2f), Vector3.up, rotateSpeed);
                    //			rotateCounter = true;
                    //			currentEuler = transform.rotation.eulerAngles.y;
                    //			tempRotateSpeed = rotateSpeed;
                }

                if (Input.GetKey(KeyCode.Q))
                {

                    transform.RotateAround(transform.position + transform.forward * transform.position.y * Mathf.Sqrt(2f), Vector3.up, -rotateSpeed);

                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (SelectedOrtographicSize > 0.5f)
                {
                    if (zoomSpeed * Time.deltaTime > 0.2f)
                        SelectedOrtographicSize -= zoomSpeed * Time.deltaTime;

                }
                else
                {
                    SelectedOrtographicSize = 0.1f;
                }

            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (SelectedOrtographicSize < 9f)
                {
                    SelectedOrtographicSize += zoomSpeed * Time.deltaTime;

                }
            }
        }
        else if(!GameController.GameEnd)
        {
            //Kouhai
            
            
            Vector3 pos = new Vector3((Player1.transform.position.x + Player2.transform.position.x)/2 -10f,
                20f,  + (Player2.transform.position.z + Player1.transform.position.z)/2 -10f);
            transform.position = pos;

            Vector3 distVector = (Player1.transform.position - Player2.transform.position);
            float distance = Mathf.Sqrt(distVector.x * distVector.x + distVector.y * distVector.y);
            SelectedOrtographicSize = 5f + distance/3f;

        }
        else
        {
            Vector3 pos = new Vector3(Winner.transform.position.x - 10f,
                20f, Winner.transform.position.z- 10f);
            transform.position = pos;
            SelectedOrtographicSize = 5f;
        }
        camSpeed = Camera.main.orthographicSize * 10f / 4f;
    


        //		if (rotateCounter) {
        //
        //			if (transform.rotation.eulerAngles.y < (currentEuler + 90) % 360) {
        //
        //				transform.RotateAround (transform.position + transform.forward * 5 * Mathf.Sqrt (2f), Vector3.up, tempRotateSpeed);
        //				tempRotateSpeed = Mathf.Lerp (tempRotateSpeed, 0f, 0.008f);
        //				Debug.Log (tempRotateSpeed);
        //			}
        //
        //
        //		}




    }
		
}
