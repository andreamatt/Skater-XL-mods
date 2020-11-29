using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

[CreateAssetMenu(fileName = "MaterialSoundTagCollection", menuName = "SkaterXL/MaterialSoundTagCollection", order = 0)]
public class MaterialSoundTagCollection : ScriptableObject
{
	public List<Material> materials;
	public List<SurfaceTag> tags;
		
	public int GetSurfaceTagByMaterial(Material mat)
	{
		for (int i = 0; i < materials.Count; ++i)
		{
			if (materials.ElementAt(i) == mat)
			{
				return (int)tags.ElementAt(i);
			}
		}
		return 0;
	}

	public void Cleanse()
	{
        var results = materials.Select((item, i) => new { index = i, item = item }).Where(p => p.item == null).Select(item => item.index);
		
        foreach (int idx in results)
        {
            materials.RemoveAt(idx);
            tags.RemoveAt(idx);
        }
	}

	public void AddItem(Material mat, SurfaceTag tag) {
        materials.Add(mat);
        tags.Add(tag);
	}

	public void UpdateItem(int idx, Material mat, SurfaceTag tag)
	{
        materials[idx] = mat;
        tags[idx] = tag;
	}

	public void RemoveItem(MaterialTagItem item)
	{
		materials.Remove(item.mat);
		tags.Remove((SurfaceTag)item.tag);
	}
}
