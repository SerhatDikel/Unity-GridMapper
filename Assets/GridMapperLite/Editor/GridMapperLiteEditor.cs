using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridMapperLite))]
public class GridMapperLiteEditor : Editor
{
    float width;
    int[,] grid;
    string layoutHelpString = "";
    Color layoutHelpColor;

    SerializedProperty row;
    SerializedProperty column; 
    SerializedProperty gridArray;

    GUIStyle gridButtonStyle;
    bool initialized;

    private void OnEnable()
    {
        row = serializedObject.FindProperty(nameof(GridMapperLite.row));
        column = serializedObject.FindProperty(nameof(GridMapperLite.column));
        gridArray = serializedObject.FindProperty(nameof(GridMapperLite.gridArray));

        int[] tempArray = new int[gridArray.arraySize];
        for (int i = 0; i < gridArray.arraySize; i++) { tempArray[i] = gridArray.GetArrayElementAtIndex(i).intValue; }

        grid = GridMapperLite.Grid(row.intValue, column.intValue, tempArray);


    }

    public override void OnInspectorGUI()
    {
        if (!initialized)
        {
            gridButtonStyle = new GUIStyle(GUI.skin.button);
            gridButtonStyle.margin = new RectOffset(0, 0, 0, 0);
            gridButtonStyle.padding = new RectOffset(0, 0, 0, 0);
            initialized = true;
        }
        serializedObject.Update();

        //width = EditorGUIUtility.currentViewWidth - 30f;
  
        EditorGUILayout.LabelField("Grid Dimensions", EditorStyles.boldLabel);

        //TOP BOX
        EditorGUILayout.BeginVertical("box");

        //ADJUSTMENTS / BUTTONS
        EditorGUILayout.BeginHorizontal();

        //SLIDERS
        EditorGUILayout.BeginVertical();

        //Rows
        EditorGUILayout.BeginHorizontal();


        EditorGUILayout.LabelField("Row:", GUILayout.MaxWidth(50));
        row.intValue = EditorGUILayout.IntSlider(row.intValue, 1, 64);
        EditorGUILayout.EndHorizontal();

        //Columns
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Column:", GUILayout.MaxWidth(50));
        column.intValue = EditorGUILayout.IntSlider(column.intValue, 1, 64);
        EditorGUILayout.EndHorizontal();


        if (grid.GetLength(0) != row.intValue || grid.GetLength(1) != column.intValue)
        {
            int[,] gridBackup = grid;

            int rows = grid.GetLength(0) < row.intValue ? grid.GetLength(0) : row.intValue;
            int columns = grid.GetLength(1) < column.intValue ? grid.GetLength(1) : column.intValue;

            grid = new int[row.intValue, column.intValue];
            gridArray.arraySize = row.intValue * column.intValue;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    grid[i, j] = gridBackup[i, j];

                    gridArray.GetArrayElementAtIndex(j * row.intValue + i).intValue = grid[i, j];
                }
            }

        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        //ADJUSTMENTS END

        EditorGUILayout.EndVertical();
        //TOP BOX END
 

        //GRID LAYOUT
        EditorGUILayout.LabelField("Layout", EditorStyles.boldLabel);

        GUI.backgroundColor = layoutHelpColor;
        EditorGUILayout.HelpBox(layoutHelpString, MessageType.None);

        width = GUILayoutUtility.GetLastRect().width;

        GUI.backgroundColor = Color.white;
        EditorGUILayout.BeginVertical("box");


          
        for (int i = 0; i < row.intValue; i++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int j = 0; j < column.intValue; j++)
            {

                Color buttonColor = Color.white;
                string buttonString = "";
                string buttonToolTip = "";

                switch (grid[i, j])
                {
                    case 0: buttonColor = Color.gray; buttonString = "⊡"; break;
                    case 1: buttonColor = Color.green; buttonString = "⊞"; break;
                    case 2: buttonColor = Color.blue; buttonString = "⊟"; break;
                    case 3: buttonColor = Color.red; buttonString = "⊠"; break;

                    case 4: grid[i, j] = 0; break;
                }

                GUI.backgroundColor = buttonColor;
                if (GUILayout.Button(buttonString, gridButtonStyle, GUILayout.MinWidth(width / column.intValue)))
                {
                    grid[i, j]++;
                    switch (grid[i, j])
                    {
                        case 0: layoutHelpColor = Color.gray; buttonToolTip = "Empty"; break;
                        case 1: layoutHelpColor = Color.green; buttonToolTip = "Open"; break;
                        case 2: layoutHelpColor = Color.blue; buttonToolTip = "Obstacle"; break;
                        case 3: layoutHelpColor = Color.red; buttonToolTip = "Blocked"; break;
                        case 4: layoutHelpColor = Color.gray; buttonToolTip = "Empty"; break;
                    }
                    layoutHelpString = buttonToolTip;
                    layoutHelpString += "(" + (i + 1) + "/" + (j + 1) + ")";
                    gridArray.GetArrayElementAtIndex(j * row.intValue + i).intValue = grid[i, j];

                }
             
            }
        
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        //GRID LAYOUT END

        serializedObject.ApplyModifiedProperties();

    }
    
   
}

