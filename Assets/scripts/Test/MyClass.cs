using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WyrmTale;
using Colibry;


public class MyClass: ColibrySerializeHelperCustomClass
{
	public GameObject gameObject;
	
	private int[] _intArray;
	
	public int[] intArray
    {
        get { return _intArray; }
        set { _intArray = value; }
    }
	
	//unfortinatele due to language you must provide empty constructor
	public MyClass(){}
	
	// constructor will create an 'empty' game object
	public MyClass(string name, Vector3 position, Vector3 localScale,Quaternion rotation,int[]intarray)
	{
		this.gameObject = new GameObject(name);
		this.gameObject.transform.position = position;
		this.gameObject.transform.localScale = localScale;
		this.gameObject.transform.rotation = rotation;
		this.intArray=intarray;
	}
	
	// serialize this class to JSON
	public JSON ToJSON()
	{
		GameObject g = this.gameObject;
		JSON js = new JSON();
		if (g!=null)
		{
			JSON jsTransform = new JSON();
			js["name"] = g.name;
			js["transform"] = jsTransform;
			jsTransform["position"] = (JSON)g.transform.position;
			jsTransform["localScale"] = (JSON)g.transform.localScale;
			jsTransform["rotation"] = (JSON)g.transform.rotation;
			js["intArray"] = this.intArray;
		}
		return js;
	}
	
	// JSON to class conversion
	public ColibrySerializeHelperCustomClass JSONtoMyClass(JSON root)
	{
		checked
		{
			JSON jsTransform = root.ToJSON("transform");
			MyClass rezult=new MyClass(
			root.ToString("name"),
			(Vector3)jsTransform.ToJSON("position"),
			(Vector3)jsTransform.ToJSON("localScale"),
			(Quaternion)jsTransform.ToJSON("rotation"),
			root.ToArray<int>("intArray"));			
			return rezult;
		}
	}
	
	// convert a JSON array to a MyClass Array
	public ColibrySerializeHelperCustomClass[] Array(JSON[] array)
	{
		List<MyClass> tc = new List<MyClass>();
		for (int i=0; i<array.Length; i++)
		{
			tc.Add((JSONtoMyClass(array[i]) as MyClass));
		}
		return tc.ToArray();
	}

}
