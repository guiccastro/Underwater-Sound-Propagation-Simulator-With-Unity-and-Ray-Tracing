using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
public class ReadFile : MonoBehaviour
{
    //Canvas
    //public InputField fileName;

    //File variables
    [Header("File content variables")]
    public int ncols;
    public int nrows;
    public float xllcenter;
    public float yllcenter;
    public float cellsize;
    public double nodata_value;
    public float depthMax = 0.0f;

    //Factors
    //public Toggle csvFile;
    //public Toggle ascFile;
    [Header("UI Objects Inputs")]
    public Toggle normalResolution;
    public InputField sizeFactorIF;
    public InputField heightFactorIF;
    public InputField depthFactorIF;
    public InputField resolutionFactorIF;
    public Toggle invertHeightsDepthsInputField;

    [Header("Inputs")]
    public float sizeFactor;
    public float depthFactor;
    float heightFactor;
    float resolutionFactor;

    [Header("Code variables")]
    int currentNRow;
    int currentNCol;
    int rowOffset;
    int colOffset;

    [Header("Mesh variables")]
    //Mesh variables
    float[][] matrix;
    Mesh mesh;
    Vector3[] vertices;
    List<int> subemesh1 = new List<int>(); //Water mesh
    List<int> subemesh2 = new List<int>(); //Land mesh
    //int[] triangles;

    [Header("Objects")]
    [SerializeField] WaterLayerGenerator waterLayerGenerator;
    public GameObject positionAnchor;
    public GameObject instacePA = null;

    //public GameObject waterLayerObject;
    //public GameObject sphere;
    //public GameObject source;
    //public InputField positionX;
    //public InputField positionY;
    //public InputField positionDepth;
    //public Text currentDepthText;
    //public bool invertHeightAndDepth = false;
    //public float slider = 1.0f;

    [Header("File Informations UI Objects")]
    [SerializeField] Text fileNameText;
    [SerializeField] Text colText;
    [SerializeField] Text rowText;
    [SerializeField] Text cellsizeText;
    [SerializeField] Text noDataValueText;
    [SerializeField] Text depthMaxText;

    [Header("File viarabels")]
    public string path;
    StreamReader sr;
    string fileContents;
    string[] lines;

    [Header("Source and listener pointers")]
    [SerializeField] GameObject sourceObject;
    [SerializeField] GameObject listenersObject;

    [Header("Message Box Object")]
    [SerializeField] GameObject messageBoxPrefab;
    [SerializeField] GameObject canvas;

    /*
    public void SetNewPositionSource()
    {
        source.transform.localPosition = new Vector3(float.Parse(positionX.text) / cellsize, float.Parse(positionDepth.text) * depthFactor, float.Parse(positionY.text) / cellsize);
    }
    public void CreateSource()
    {
        source.transform.parent = instacePA.transform;
        source.transform.localPosition = new Vector3(0, 0, 0);
    }
    */

    public void SetPositionAnchor()
    {
        //instacePA = Instantiate(positionAnchor, new Vector3(currentNRow - 1, 0, 0), positionAnchor.transform.rotation);
        instacePA = Instantiate(positionAnchor, new Vector3(vertices[(currentNRow-1)*currentNCol].x, 0.0f, 0.0f), positionAnchor.transform.rotation);

        sourceObject.transform.SetParent(instacePA.transform);
        sourceObject.transform.localPosition = Vector3.zero;
        sourceObject.transform.localRotation = Quaternion.identity;

        listenersObject.transform.SetParent(instacePA.transform);
        listenersObject.transform.localPosition = Vector3.zero;
        listenersObject.transform.localRotation = Quaternion.identity;
    }

    public void CreateScene()
    {
        //CreateSceneCSV();

        //Create scene
        sizeFactor = float.Parse(sizeFactorIF.text.Replace('.', ','));
        heightFactor = float.Parse(heightFactorIF.text.Replace('.', ','));
        depthFactor = float.Parse(depthFactorIF.text.Replace('.', ','));
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        CreateShape();
        UpdateMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        waterLayerGenerator.SetXY((currentNCol - 1) * sizeFactor, (currentNRow - 1) * sizeFactor);

        if (instacePA != null)
        {
            Destroy(instacePA);    
        }

        SetPositionAnchor();


        /*
        if (ascFile.isOn)
        {
            CreateSceneASC();
        }
        */
    }

    /*
    public void CreateSceneASC()
    {
        //Layer setup
        waterLayerGenerator = waterLayerObject.GetComponent<WaterLayerGenerator>();

        //Read file
        var sr = new StreamReader(Application.dataPath + "/" + fileName.text + ".asc");
        var fileContents = sr.ReadToEnd();
        sr.Close();
        var lines = fileContents.Split("\n"[0]);

        //Setup file variables
        ncols = int.Parse(lines[0].Split(' ')[1]);
        nrows = int.Parse(lines[1].Split(' ')[1]);
        xllcenter = float.Parse(lines[2].Split(' ')[1].Replace('.', ','));
        yllcenter = float.Parse(lines[3].Split(' ')[1].Replace('.', ','));
        cellsize = float.Parse(lines[4].Split(' ')[1].Replace('.', ','));
        nodata_value = int.Parse(lines[5].Split(' ')[1]);

        sizeFactor = float.Parse(sizeFactorIF.text.Replace('.', ','));
        heightFactor = float.Parse(heightFactorIF.text.Replace('.', ','));
        depthFactor = float.Parse(depthFactorIF.text.Replace('.', ','));
        resolutionFactor = float.Parse(resolutionFactorIF.text.Replace('.', ','));

        if (normalResolution.isOn)
        {
            matrix = new float[nrows][]; //Initiate first dimension of matrix

            for (int i = 0; i < nrows; i++)
            {
                matrix[i] = new float[ncols]; //Initiate second dimension of matrix

                var aux = lines[i + 6].Split(' '); //Get values of height and depth

                for (int j = 0; j < ncols; j++)
                {
                    matrix[i][j] = float.Parse(aux[j]); //Save values in the matrix
                }
            }

            currentNRow = nrows;
            currentNCol = ncols;

        }
        else //Create matrix for a different resolution from the file
        {
            //Calculate the new resolution
            currentNRow = (int)((float)nrows * resolutionFactor);
            currentNCol = (int)((float)ncols * resolutionFactor);

            //Calculate the offset
            rowOffset = (int)(nrows / currentNRow);
            colOffset = (int)(ncols / currentNCol);

            matrix = new float[currentNRow][]; //Initiate first dimension of matrix

            for (int i = 0; i < currentNRow; i++)
            {
                matrix[i] = new float[currentNCol];//Initiate second dimension of matrix

                var aux = lines[(i * rowOffset) + 6].Split(' '); //Get values of height and depth

                for (int j = 0; j < currentNCol; j++)
                {
                    matrix[i][j] = float.Parse(aux[j * colOffset]); //Save values in the matrix
                }

            }
        }

        //Create scene
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        CreateShape();
        UpdateMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        waterLayerGenerator.SetXY((currentNCol - 1) * sizeFactor, (currentNRow - 1) * sizeFactor);
    }
    */

    public void ReadCSVFile()
    {
        if (path.EndsWith(".csv"))
        {
            sr = new StreamReader(path);
            fileContents = sr.ReadToEnd();
            sr.Close();
            lines = fileContents.Split("\n"[0]);

            //Setup file variables
            ncols = int.Parse(lines[0].Substring(lines[0].LastIndexOf(' ')));
            nrows = int.Parse(lines[1].Substring(lines[1].LastIndexOf(' ')));
            xllcenter = float.Parse(lines[2].Substring(lines[2].LastIndexOf(' ')).Replace('.', ','));
            yllcenter = float.Parse(lines[3].Substring(lines[3].LastIndexOf(' ')).Replace('.', ','));
            cellsize = float.Parse(lines[4].Substring(lines[4].LastIndexOf(' ')).Replace('.', ','));
            nodata_value = double.Parse(lines[5].Substring(lines[5].LastIndexOf(' ')));

            matrix = new float[nrows][]; //Initiate first dimension of matrix

            for (int i = 0; i < nrows; i++)
            {
                matrix[i] = new float[ncols]; //Initiate second dimension of matrix

                var aux = lines[i + 6].Split(' '); //Get values of height and depth

                for (int j = 0; j < ncols; j++)
                {
                    //Debug.Log("i:" + i + " j:" + j);

                    float currentDepth = float.Parse(aux[j].Replace('.', ','));

                    if (currentDepth < depthMax)
                    {
                        depthMax = currentDepth;
                    }
                    matrix[i][j] = currentDepth; //Save values in the matrix
                }
            }

            currentNRow = nrows;
            currentNCol = ncols;

            fileNameText.text = "File name: " + path.Substring(path.LastIndexOf('\\') + 1);
            colText.text = "Columns: " + ncols;
            rowText.text = "Rows: " + nrows;
            cellsizeText.text = "Cell size: " + cellsize;
            noDataValueText.text = "No data value: " + nodata_value;
            depthMaxText.text = "Depth max: " + depthMax;
        }
        else {
            //print("Not a .csv file.");
            Instantiate(messageBoxPrefab, canvas.transform);
            messageBoxPrefab.GetComponent<MessageBox>().SetMessage("Not a .csv file.");
        }
        

    }

    public void InvertHeightsAndDepths()
    {
        depthMax = 0.0f;

        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
            {
                float currentDepth = -matrix[i][j];

                if (currentDepth < depthMax)
                {
                    depthMax = currentDepth;
                }
                matrix[i][j] = currentDepth; //Save values in the matrix
            }
        }

        depthMaxText.text = "Depth max: " + depthMax;
    }

    public void Resize()
    {
        resolutionFactor = float.Parse(resolutionFactorIF.text.Replace('.', ','));

        //Calculate the new resolution
        currentNRow = (int)((float)nrows * resolutionFactor);
        currentNCol = (int)((float)ncols * resolutionFactor);

        //Calculate the offset
        rowOffset = (nrows / currentNRow);
        colOffset = (ncols / currentNCol);

        matrix = new float[currentNRow][]; //Initiate first dimension of matrix

        for (int i = 0; i < currentNRow; i++)
        {
            matrix[i] = new float[currentNCol];//Initiate second dimension of matrix

            var aux = lines[(i * rowOffset) + 6].Split(' '); //Get values of height and depth

            for (int j = 0; j < currentNCol; j++)
            {
                matrix[i][j] = float.Parse(aux[j * colOffset].Replace('.', ',')); //Save values in the matrix
            }
        }

        colText.text = "Columns: " + currentNCol;
        rowText.text = "Rows: " + currentNRow;
    }

    /*
    public void CreateSceneCSV()
    {
        //Layer setup
        waterLayerGenerator = waterLayerObject.GetComponent<WaterLayerGenerator>();

        //Read file
        //var sr = new StreamReader(Application.dataPath + "/" + fileName.text + ".csv");
        var sr = new StreamReader(path);
        var fileContents = sr.ReadToEnd();
        sr.Close();
        var lines = fileContents.Split("\n"[0]);

        //Setup file variables
        ncols = int.Parse(lines[0].Substring(lines[0].LastIndexOf(' ')));
        nrows = int.Parse(lines[1].Substring(lines[1].LastIndexOf(' ')));
        xllcenter = float.Parse(lines[2].Substring(lines[2].LastIndexOf(' ')).Replace('.', ','));
        yllcenter = float.Parse(lines[3].Substring(lines[3].LastIndexOf(' ')).Replace('.', ','));
        cellsize = float.Parse(lines[4].Substring(lines[4].LastIndexOf(' ')).Replace('.', ','));
        nodata_value = double.Parse(lines[5].Substring(lines[5].LastIndexOf(' ')));

        
        resolutionFactor = float.Parse(resolutionFactorIF.text.Replace('.', ','));

        if (normalResolution.isOn)
        {
            matrix = new float[nrows][]; //Initiate first dimension of matrix

            for (int i = 0; i < nrows; i++)
            {
                matrix[i] = new float[ncols]; //Initiate second dimension of matrix

                var aux = lines[i + 6].Split(' '); //Get values of height and depth

                for (int j = 0; j < ncols; j++)
                {
                    //Debug.Log("i:" + i + " j:" + j);
                    matrix[i][j] = float.Parse(aux[j].Replace('.', ',')); //Save values in the matrix
                }
            }

            currentNRow = nrows;
            currentNCol = ncols;

        }
        else //Create matrix for a different resolution from the file
        {
            //Calculate the new resolution
            currentNRow = (int)((float)nrows * resolutionFactor);
            currentNCol = (int)((float)ncols * resolutionFactor);

            //Calculate the offset
            rowOffset = (int)(nrows / currentNRow);
            colOffset = (int)(ncols / currentNCol);

            matrix = new float[currentNRow][]; //Initiate first dimension of matrix

            for (int i = 0; i < currentNRow; i++)
            {
                matrix[i] = new float[currentNCol];//Initiate second dimension of matrix

                var aux = lines[(i * rowOffset) + 6].Split(' '); //Get values of height and depth

                for (int j = 0; j < currentNCol; j++)
                {
                    matrix[i][j] = float.Parse(aux[j * colOffset].Replace('.', ',')); //Save values in the matrix
                }

            }
        }

        //Create scene
        sizeFactor = float.Parse(sizeFactorIF.text.Replace('.', ','));
        heightFactor = float.Parse(heightFactorIF.text.Replace('.', ','));
        depthFactor = float.Parse(depthFactorIF.text.Replace('.', ','));
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        CreateShape();
        UpdateMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        waterLayerGenerator.SetXY((currentNCol - 1) * sizeFactor, (currentNRow - 1) * sizeFactor);
        SetPositionAnchor();
    }
    */

    void CreateShape()
    {
        vertices = new Vector3[currentNRow * currentNCol]; //Initialize vertice
        int indexV = 0; //Indice for the vertice

        for (int i = 0; i < currentNRow; i++)
        {
            for (int j = 0; j < currentNCol; j++)
            {
                if (matrix[i][j] == nodata_value)
                {
                    vertices[indexV] = new Vector3(-j * sizeFactor, 0, i * sizeFactor); //Zero depth for no data value
                }
                else
                {
                    /*if (matrix[i][j] > 0.0f)
                    {
                        vertices[indexV] = new Vector3(-j * sizeFactor, matrix[i][j] * heightFactor * (float)heightResize, i * sizeFactor); //Vertice for land
                    }
                    else
                    {
                        vertices[indexV] = new Vector3(-j * sizeFactor, matrix[i][j] * depthFactor * (float)depthResize, i * sizeFactor); //Vertice for depth
                    }*/

                    //if (!invertHeightAndDepth)
                    //{
                        if (matrix[i][j] > 0.0f)
                        {
                            vertices[indexV] = new Vector3(i * sizeFactor, matrix[i][j] * heightFactor, j * sizeFactor); //Vertice for land
                        }
                        else
                        {
                            vertices[indexV] = new Vector3(i * sizeFactor, matrix[i][j] * depthFactor, j * sizeFactor); //Vertice for depth

                            /*
                            if (matrix[i][j] < depthMax)
                            {
                                depthMax = matrix[i][j];
                            }
                            */
                        }

                    /*}
                    else
                    {
                        if (matrix[i][j] > 0.0f)
                        {
                            vertices[indexV] = new Vector3(i * sizeFactor, -matrix[i][j] * heightFactor, j * sizeFactor); //Vertice for land
                        }
                        else
                        {
                            vertices[indexV] = new Vector3(i * sizeFactor, -matrix[i][j] * depthFactor, j * sizeFactor); //Vertice for depth

                            if (matrix[i][j] < depthMax)
                            {
                                depthMax = matrix[i][j];
                            }
                        }
                    }*/
                    

                    //Instantiate(sphere, new Vector3(i, matrix[i][j] * 0.1f, j), Quaternion.identity);

                }

                indexV++;
            }
        }

        //triangles = new int[((currentNCol-1)*(currentNRow-1)*2)*3];
        mesh.vertices = vertices; //Save vertices in the mesh
        mesh.subMeshCount = 2; //Set submesh to 2

        subemesh1 = new List<int>(); //Water mesh
        subemesh2 = new List<int>(); //Land mesh

        //int indexT = 0;
        indexV = 0;

        for (int i = 0; i < currentNRow - 1; i++)
        {
            for (int j = 0; j < currentNCol - 1; j++)
            {
                if (vertices[indexV].y < 0)
                {
                    subemesh1.Add(indexV);
                    subemesh1.Add(indexV + currentNCol + 1);
                    subemesh1.Add(indexV + currentNCol);
                    subemesh1.Add(indexV);
                    subemesh1.Add(indexV + 1);
                    subemesh1.Add(indexV + currentNCol + 1);
                }
                else
                {
                    subemesh2.Add(indexV);
                    subemesh2.Add(indexV + currentNCol + 1);
                    subemesh2.Add(indexV + currentNCol);
                    subemesh2.Add(indexV);
                    subemesh2.Add(indexV + 1);
                    subemesh2.Add(indexV + currentNCol + 1);
                }

                /*triangles[indexT] = indexV;
                indexT++;
                triangles[indexT] = indexV + currentNCol + 1;
                indexT++;
                triangles[indexT] = indexV + currentNCol;
                indexT++;

                triangles[indexT] = indexV;
                indexT++;
                triangles[indexT] = indexV + 1;
                indexT++;
                triangles[indexT] = indexV + currentNCol + 1;
                indexT++;*/

                indexV++;

                
            }
            indexV++;
        }
    }

    void UpdateMesh()
    {
        mesh.SetTriangles(subemesh1, 0);
        mesh.SetTriangles(subemesh2, 1);
        //mesh.vertices = vertices;
        //mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
