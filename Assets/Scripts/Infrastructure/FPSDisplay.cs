using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
	private float deltaTime = 0.0f;
    private GUIStyle style;
    private Rect rect, rect2;
    private float msec;
    private float fps;
    private string text, text2;
    private int w,h;

    void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

	void OnGUI()
	{
		w = Screen.width;
        h = Screen.height;

		style = new GUIStyle();

		rect = new Rect(0, 0, 10f, h * 2 / 100);
        rect2 = new Rect(w-200f, 0, 100f, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
		msec = deltaTime * 1000.0f;
		fps = 1.0f / deltaTime;
		text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label(rect, text, style);
	}
}