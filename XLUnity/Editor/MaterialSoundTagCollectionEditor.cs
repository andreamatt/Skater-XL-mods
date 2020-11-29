using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MaterialSoundTagCollection))]
public class MaterialSoundTagCollectionEditor : Editor
{
	MaterialSoundTagCollection soundTagCollection;

    void OnEnable() {
		soundTagCollection = (MaterialSoundTagCollection)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        // Collection Label
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"{target.name}");
        if (GUILayout.Button("Clean Collection")) {
			soundTagCollection.Cleanse();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20);

        if (GUILayout.Button("+ Add Item")) {
			soundTagCollection.AddItem(null, SurfaceTag.None);
        }
        EditorGUILayout.Space(20);

        // List<MaterialTagItem> collectionItems = soundTagCollection.ItemTags;
		List<Material> materials = soundTagCollection.materials;
		List<SurfaceTag> tags = soundTagCollection.tags;
        List<MaterialTagItem> itemsForDelete = new List<MaterialTagItem>();

        // Parameter settings
        for (int i=0; i<materials.Count; ++i) {

            EditorGUILayout.BeginHorizontal();
            Material mat = (Material)EditorGUILayout.ObjectField(materials[i], typeof(Material), false);
            SurfaceTag tag = (SurfaceTag)EditorGUILayout.EnumPopup(tags[i]);
            soundTagCollection.UpdateItem(i, mat, tag);
            if (GUILayout.Button("x")) {
                itemsForDelete.Add(new MaterialTagItem(mat, tag));
            }
            EditorGUILayout.EndHorizontal();
        }

        // Purge items after iterative process
        foreach(MaterialTagItem entry in itemsForDelete) {
			soundTagCollection.RemoveItem(entry);
        }

		EditorGUILayout.EndVertical();
    }
}
