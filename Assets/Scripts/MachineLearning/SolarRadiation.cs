using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class SolarRadiation : MonoBehaviour {

    public TextAsset regressionFile;

    private double[,] coefficients;

	// Use this for initialization
	void Start () {
        coefficients = LoadCoefficientData();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Loads the coefficient data and returns the array of coefficients as they are in the file
    /// </summary>
    /// <param name="path">File to read coefficients from</param>
    /// <returns>Array of coefficients</returns>
    double[,] LoadCoefficientData()
    {
        StreamReader inp = new StreamReader(
            new MemoryStream(
                Encoding.UTF8.GetBytes(regressionFile.text ?? "")));
        double[,] output = new double[1225, 1225];

        int i = 0;
        while(!inp.EndOfStream)
        {
            string line = inp.ReadLine();
            string[] coeffs = line.Split(',');
            int j = 0;
            foreach(string coeff in coeffs)
            {
                double val = double.Parse(coeff, System.Globalization.NumberStyles.Float);
                output[i, j] = val;
                j++;
            }
            i++;
        }

        return output;
    }

    /// <summary>
    /// Runs the regression produced by the solar radiation ML algorithm on the input, as if the central building were demolished.
    /// </summary>
    /// <param name="heights">Heights of a 5x5 array of buildings.</param>
    /// <returns>2x2 Array of the new heights of each building cell of which there are 7x7 cells for each of 5x5 buildings.</returns>
    float[,] PredictCentralRemoval(float[,] heights)
    {
        if(heights.GetLength(0) != 5 || heights.GetLength(1) != 5)
        {
            throw new System.Exception("The solar radiation algorithm expects an array of building cells 35x35 (5x5 buildings of 7x7 cells)");
        }
        float[,] output = (float[,]) heights.Clone();

        for(int i = 0; i < coefficients.GetLength(0); i ++)
        {
            double delta = 0;
            for(int j = 0; j < coefficients.GetLength(1); j ++)
            {
                Pair<int, int> pos = GetCellWithIndex(j, heights.GetLength(0));
                delta += coefficients[i, j] * heights[pos.First, pos.Second];
            }
            Pair<int, int> changed = GetCellWithIndex(i, heights.GetLength(0));
            output[changed.First, changed.Second] += (float) delta;
        }

        return output;
    }

    /// <summary>
    /// Determines an iterative order over a square array and offers a cell for the index provided.
    /// </summary>
    /// <param name="index">The index of the cell retured</param>
    /// <param name="width">The width of the square array.</param>
    /// <returns>The cell (as a pair) referenced by the index.</returns>
    private Pair<int, int> GetCellWithIndex(int index, int width)
    {
        return new Pair<int, int>(index % width, index / width);
    }

    /// <summary>
    /// Class that can hold a pair of two objects.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    private class Pair<T1, T2>
    {
        public T1 First { get; private set; }
        public T2 Second { get; private set; }
        internal Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }
}
