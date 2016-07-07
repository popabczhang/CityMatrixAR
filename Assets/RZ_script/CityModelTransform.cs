using UnityEngine;
using System.Collections;

public class CityModelTransform : MonoBehaviour {
	
	private Vector3 vector;
	private string transformMode;
	private string axis;
	private float stepSize;

	void Start () {
		vector = new Vector3 (0.0f, 0.0f, 0.0f);
		transformMode = "translate";
		axis = "x";
		stepSize = 0.0f;
	}

	void Update () {
		
	}

	public void TranslateMode () {
		transformMode = "translate";
	}

	public void RotateMode () {
		transformMode = "rotate";
	}

	public void ScaleMode () {
		transformMode = "scale";
	}

	public void axisX () {
		axis = "x";
	}

	public void axisY () {
		axis = "y";
	}

	public void axisZ () {
		axis = "z";
	}

	public void stepSizeTen () {
		stepSize = 10.0f;
	}

	public void stepSizeOne () {
		stepSize = 1.0f;
	}

	public void stepSizePointOne () {
		stepSize = 0.1f;
	}

	public void stepSizePointZeroOne () {
		stepSize = 0.01f;
	}

	public void stepSizePointZeroZeroOne () {
		stepSize = 0.001f;
	}
	
	public void Plus () {
		if (transformMode == "scale") {
			vector = new Vector3 (stepSize, stepSize, stepSize);
			transform.localScale += vector;
		} else if (transformMode == "rotate") {
			if (axis == "x") {
				transform.RotateAround (transform.localPosition, new Vector3(1.0f,0.0f,0.0f), stepSize);
			} else if (axis == "y") {
				transform.RotateAround (transform.localPosition, new Vector3(0.0f,1.0f,0.0f), stepSize);
			} else if (axis == "z") {
				transform.RotateAround (transform.localPosition, new Vector3(0.0f,0.0f,1.0f), stepSize);
			}
//			if (axis == "x") {
//				vector = new Vector3 (stepSize, 0.0f, 0.0f);
//			} else if (axis == "y") {
//				vector = new Vector3 (0.0f, stepSize, 0.0f);
//			} else if (axis == "z") {
//				vector = new Vector3 (0.0f, 0.0f, stepSize);
//			}
//			transform.Rotate (vector);
		} else if (transformMode == "translate") {
			if (axis == "x") {
				vector = new Vector3 (stepSize, 0.0f, 0.0f);
			} else if (axis == "y") {
				vector = new Vector3 (0.0f, stepSize, 0.0f);
			} else if (axis == "z") {
				vector = new Vector3 (0.0f, 0.0f, stepSize);
			}
			transform.Translate (vector);
		}
	}


	public void Minus () {
		if (transformMode == "scale") {
			vector = new Vector3 (0.1F, 0, 0);
			transform.localScale -= vector;
		} else if (transformMode == "rotate") {
			if (axis == "x") {
				transform.RotateAround (transform.localPosition, new Vector3(1.0f,0.0f,0.0f), -stepSize);
			} else if (axis == "y") {
				transform.RotateAround (transform.localPosition, new Vector3(0.0f,1.0f,0.0f), -stepSize);
			} else if (axis == "z") {
				transform.RotateAround (transform.localPosition, new Vector3(0.0f,0.0f,1.0f), -stepSize);
			}
//			if (axis == "x") {
//				vector = new Vector3 (stepSize, 0.0f, 0.0f);
//			} else if (axis == "y") {
//				vector = new Vector3 (0.0f, stepSize, 0.0f);
//			} else if (axis == "z") {
//				vector = new Vector3 (0.0f, 0.0f, stepSize);
//			}
//			transform.Rotate (- vector);
		} else if (transformMode == "translate") {
			if (axis == "x") {
				vector = new Vector3 (stepSize, 0.0f, 0.0f);
			} else if (axis == "y") {
				vector = new Vector3 (0.0f, stepSize, 0.0f);
			} else if (axis == "z") {
				vector = new Vector3 (0.0f, 0.0f, stepSize);
			}
			transform.Translate (-vector);
		}
	}

}
