using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RadarChartCtrl : MonoBehaviour {

	[Range(0, 1f)]
	public float[] values;
	float[] prev = new float[0];

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
			if (!values.SequenceEqual(prev)) {
				prev = (float[]) values.Clone();
				foreach(Transform c in this.transform) {
					UnityEngine.UI.Extensions.RadarPolygon child = c.GetComponent<UnityEngine.UI.Extensions.RadarPolygon>();
					UpdateChild(child);
				}
			}

			values[3] = (float) (values[3] + 0.1) % 1;

	}

	void OnValidate () {
		if (!values.SequenceEqual(prev)) {
			prev = (float[]) values.Clone();
			foreach(Transform c in this.transform) {
				UnityEngine.UI.Extensions.RadarPolygon child = c.GetComponent<UnityEngine.UI.Extensions.RadarPolygon>();
				UpdateChild(child);
				child.Update();
			}
		}
	}

	void UpdateChild(UnityEngine.UI.Extensions.RadarPolygon child) {
		child.value = (float[]) values.Clone();

	}
}
