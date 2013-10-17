using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WyrmTale;
using Colibry;
using System.Reflection;


public class MyClass2: Entity
{
	public GameObject gameObject;
	
	private string _name;
	public string name
    {
        get { return _name; }
        set { _name = value; }
    }
	
	private Vector3 _position;
	public Vector3 position
    {
        get { return _position; }
        set { _position = value; }
    }
	
	private Vector3 _localScale;
	public Vector3 localScale
    {
        get { return _localScale; }
        set { _localScale = value; }
    }
	
	private Quaternion _rotation;
	public Quaternion rotation
    {
        get { return _rotation; }
        set { _rotation = value; }
    }
	
	private Quaternion _rotation2;
	public Quaternion rotation2
    {
        get { return _rotation2; }
        set { _rotation2 = value; }
    }
	
	private int[] _intarray;
	
	public int[] intarray
    {
        get { return _intarray; }
        set { _intarray = value; }
    }
	
	private ArrayList _XXXArrayList=new ArrayList();
	
	public ArrayList XXXArrayList
    {
        get { return _XXXArrayList; }
        set { _XXXArrayList = value; }
    }
	
	
	//unfortinatele due to language you must provide empty constructor
	public MyClass2(){this.gameObject = new GameObject(name);}
	
	// constructor will create an 'empty' game object
	public MyClass2(string name, Vector3 position, Vector3 localScale,Quaternion rotation,int[]intarray)
	{
		this.name=name;
		this.position=position;
		this.localScale=localScale;
		this.rotation=rotation;
		
		this.gameObject = new GameObject(name);
		this.gameObject.transform.position = position;
		this.gameObject.transform.localScale = localScale;
		this.gameObject.transform.rotation = rotation;
		this.intarray=intarray;
	}
	
	private void SynchronizeFromGameObject()
	{
		this.name=this.gameObject.name;
		this.position=this.gameObject.transform.position;
		this.localScale=this.gameObject.transform.localScale;
		this.rotation=this.gameObject.transform.rotation;
	}
	
	private void SynchronizeToGameObject()
	{
		this.gameObject.name=this.name;
		this.gameObject.transform.position=this.position;
		this.gameObject.transform.localScale=this.localScale;
		this.gameObject.transform.rotation=this.rotation;
	}
	
	// serialize this class to JSON
	public override JSON ToJSON()
	{
		JSON root=new JSON();
			
		PropertyInfo[] properties = this.GetType().GetProperties();
		
		foreach (var p in properties){
			if(p.Name=="id"){
				if(id==null){
					continue;
				}
			}
			ColibrySerializeHelper.getProperty(this,p,root,p.Name);
		}
		return root;
	} 
	
	// JSON to class conversion
	public override ColibrySerializeHelperCustomClass JSONtoMyClass(JSON root)
	{
		checked
		{
			PropertyInfo[] properties = this.GetType().GetProperties();
			foreach (PropertyInfo p in properties)
			{
			    string name = p.Name;
				if(root.fields.ContainsKey(name))
				{
					ColibrySerializeHelper.setProperty(this,p,root,name);
				}
			}				
			return this;
		}
	}
	
	// convert a JSON array to a MyClass Array
	public override ColibrySerializeHelperCustomClass[] Array(JSON[] array)
	{
		List<MyClass2> tc = new List<MyClass2>();
		for (int i=0; i<array.Length; i++)
		{
			tc.Add((new MyClass2().JSONtoMyClass(array[i]) as MyClass2));
		}
		return tc.ToArray();
	}

}
