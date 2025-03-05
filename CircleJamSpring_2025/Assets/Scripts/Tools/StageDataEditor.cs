using UnityEditor;
using UnityEngine;
using System.IO;

public class StageDataEditor : EditorWindow
{
    private StageData stageData = new StageData();
    private string savePath = "Assets/Resources/Json/StageData.json"; // JSON�̕ۑ���
    private Vector2 scrollPosition; // �X�N���[���p
    private int stageToRemove = -1; // �폜����X�e�[�W�̃C���f�b�N�X

    [MenuItem("Tools/Stage Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<StageDataEditor>("Stage Data Editor");
    }

    private void OnEnable()
    {
        LoadFromJson();
    }

    private void OnGUI()
    {
        GUILayout.Label("Stage Data Editor", EditorStyles.boldLabel);

        if (stageData == null || stageData.list_stage == null)
        {
            EditorGUILayout.HelpBox("StageData �����[�h����Ă��܂���B", MessageType.Warning);
            if (GUILayout.Button("Create New StageData"))
            {
                stageData = new StageData();
            }
            return;
        }

        // �X�N���[���J�n
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(400));

        // �X�e�[�W���X�g�̕ҏW
        GUILayout.Label("Edit Stages", EditorStyles.boldLabel);
        for (int i = 0; i < stageData.list_stage.Count; i++)
        {
            GUILayout.BeginVertical("box");

            // ���͕���
            stageData.list_stage[i].stage_id = EditorGUILayout.TextField("Stage ID", stageData.list_stage[i].stage_id);
            stageData.list_stage[i].stage_name = EditorGUILayout.TextField("Stage Name", stageData.list_stage[i].stage_name);
            stageData.list_stage[i].is_stage_clear = EditorGUILayout.Toggle("Stage Clear", stageData.list_stage[i].is_stage_clear);
            
            GUILayout.Label("Stage Position"); 
            var x = float.Parse(EditorGUILayout.TextField("    x", stageData.list_stage[i].position.x.ToString()));
            var y = float.Parse(EditorGUILayout.TextField("    y", stageData.list_stage[i].position.y.ToString()));
            var z = float.Parse(EditorGUILayout.TextField("    z", stageData.list_stage[i].position.z.ToString()));

            stageData.list_stage[i].position = new Vector3(x,y,z);

            if (GUILayout.Button("Remove Stage"))
            {
                stageToRemove = i; // �t���O�ō폜�Ώۂ��L�^
            }

            GUILayout.EndVertical(); // �����Ŋm���ɕ���
        }

        EditorGUILayout.EndScrollView(); // �X�N���[���I��

        // ���[�v��ɗv�f�폜�i���C�A�E�g����h�~�j
        if (stageToRemove != -1)
        {
            stageData.list_stage.RemoveAt(stageToRemove);
            stageToRemove = -1;
        }

        if (GUILayout.Button("Add Stage"))
        {
            stageData.list_stage.Add(new Stage());
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Save to JSON"))
        {
            SaveToJson();
        }
        if (GUILayout.Button("Load from JSON"))
        {
            LoadFromJson();
        }
    }

    private void SaveToJson()
    {
        string json = JsonUtility.ToJson(stageData, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"StageData saved to {savePath}");
        AssetDatabase.Refresh();
    }

    private void LoadFromJson()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            stageData = JsonUtility.FromJson<StageData>(json);
            Debug.Log("StageData loaded from JSON");
        }
        else
        {
            Debug.LogWarning("JSON file not found! Creating a new StageData.");
            stageData = new StageData();
        }
    }
}
