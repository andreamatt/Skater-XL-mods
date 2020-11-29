using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


public enum SurfaceTag
{
	None = 0,
	Concrete = 1,
	Brick = 2,
	Tarmac = 3,
	Wood = 4,
	Grass = 5
}

[Serializable]
public class TagCollection
{
	public List<MaterialTagItem> tagItems = new List<MaterialTagItem>();

	public int GetSurfaceTagByMaterial(string mat)
	{
		MaterialTagItem item = tagItems.Where(x => x.name == mat).FirstOrDefault();
		return item != null ? 0 : item.tag;
	}

	public void LogContents()
	{
		Debug.Log($"{tagItems.Count}");
		foreach (MaterialTagItem i in tagItems)
		{
			Debug.Log($"{i.name} {i.tag}");
		}
	}
}

[Serializable]
public class MaterialTagItem
{
	public string name;
	public int tag;
	public Material mat;

	public MaterialTagItem(string m, SurfaceTag t)
	{
		this.name = m;
		this.tag = (int)t;
		this.mat = null;
	}

	public MaterialTagItem(Material m, SurfaceTag t)
	{
		this.mat = m;
		this.name = mat != null ? m.name : "";
		this.tag = (int)t;
	}
}
