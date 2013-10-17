using UnityEngine;
using System.Collections;
using WyrmTale;
using System.Reflection;
using System.Collections.Generic;

namespace Colibry {
	public class ColibrySerializeHelper {
		
		public static void getProperty(object obj,PropertyInfo p,JSON root,string name)
		{
		    var value = p.GetValue(obj, null);
			if (value is ColibrySerializeHelperCustomClass)
			{
				//custom class
				root[name]=(value as ColibrySerializeHelperCustomClass).ToJSON();
			}else
			if(value is ColibrySerializeHelperCustomClass[])
			{
				//custom class array
				List<JSON> jsonarray = new List<JSON>();
				for (int i=0; i<(value as ColibrySerializeHelperCustomClass[]).Length; i++){
  					jsonarray.Add((value as ColibrySerializeHelperCustomClass[])[i].ToJSON());
				}					
				root[name]=jsonarray.ToArray();
				
			}else
			{
				//usual type
				if(p.PropertyType==typeof(Vector3))
				{
					root[name]=(JSON)(Vector3)value;
				}
				else if(p.PropertyType==typeof(Vector4))
				{
					root[name]=(JSON)(Vector4)value;
				}
				else if(p.PropertyType==typeof(Vector2))
				{
					root[name]=(JSON)(Vector2)value;
				}
				else if(p.PropertyType==typeof(Quaternion))
				{
					root[name]=(JSON)(Quaternion)value;
				}
				else if(p.PropertyType==typeof(Color))
				{
					root[name]=(JSON)(Color)value;
				}
				else if(p.PropertyType==typeof(Color32))
				{
					root[name]=(JSON)(Color32)value;
				}
				else if(p.PropertyType==typeof(Rect))
				{
					root[name]=(JSON)(Rect)value;
				}
				else
				{
					root[name]=value;
				}
			
			}			
		}
		
		public static void setProperty(object obj,PropertyInfo p,JSON root,string name)
		{
			try{	
				//Debug.Log (name);
				
				//arrays
				if(p.PropertyType==typeof(int[]))
				{
					p.SetValue(obj,root.ToArray<int>(name),null);return;
				}
				
				if(p.PropertyType==typeof(float[]))
				{
					p.SetValue(obj,root.ToArray<float>(name),null);return;
				}
				
				if(p.PropertyType==typeof(bool[]))
				{
					p.SetValue(obj,root.ToArray<bool>(name),null);return;
				}
				
				if(p.PropertyType==typeof(string[]))
				{
					p.SetValue(obj,root.ToArray<string>(name),null);return;
				}
				if(p.PropertyType==typeof(object[]))
				{
					p.SetValue(obj,root.ToArray<object>(name),null);return;
				}
				
				//ArrayList
				if(p.PropertyType==typeof(ArrayList))
				{
					setPropertyArrayList(obj,p,root,name);return;
				}
				
				//copy to new dictionary
				/*JSON newroot=new JSON();
				string serialized=root.serialized;
				string obg=serialized.Clone().ToString();
				newroot.serialized=obg;
				///	
				if(ReferenceEquals(newroot[name],root[name]))
				{
					Debug.Log ("newroot[\"somename\"]==root[name]");
				}
				
				if(newroot.ToString(name)=="undefined")
				{
					//nothing to serialize
					p.SetValue(obj,null,null);
					return;
				}*/
				
				//simple types
				if(p.PropertyType==typeof(int))
				{
					p.SetValue(obj,root.ToInt(name),null);return;
				}
				if(p.PropertyType==typeof(float))
				{
					p.SetValue(obj,root.ToFloat(name),null);return;
				}
				if(p.PropertyType==typeof(bool))
				{
					p.SetValue(obj,root.ToBoolean(name),null);return;
				}
				if(p.PropertyType==typeof(string))
				{
					p.SetValue(obj,root.ToString(name),null);return;
				}
				if(p.PropertyType==typeof(object))
				{
					p.SetValue(obj,root.ToObject(name),null);return;
				}
				
				
				//extra types
				if(p.PropertyType==typeof(Vector3))
				{
					p.SetValue(obj,(Vector3)(root.ToJSON(name)),null);return;
				}
				
				if(p.PropertyType==typeof(Vector4))
				{
					p.SetValue(obj,(Vector4)(root.ToJSON(name)),null);return;
				}
				
				if(p.PropertyType==typeof(Vector2))
				{
					p.SetValue(obj,(Vector2)(root.ToJSON(name)),null);return;
				}
				
				if(p.PropertyType==typeof(Quaternion))
				{
					p.SetValue(obj,(Quaternion)(root.ToJSON(name)),null);return;
				}
				
				if(p.PropertyType==typeof(Color))
				{
					p.SetValue(obj,(Color)(root.ToJSON(name)),null);return;
				}
				
				if(p.PropertyType==typeof(Color32))
				{
					p.SetValue(obj,(Color32)(root.ToJSON(name)),null);return;
				}
				
				if(p.PropertyType==typeof(Rect))
				{
					p.SetValue(obj,(Rect)(root.ToJSON(name)),null);return;
				}
				
				
				if(root.ToString(name)=="")
				{
					//nothing to serialize
					p.SetValue(obj,null,null);return;
				}
				//Dictionary
				if(p.PropertyType==typeof(Dictionary<string,string>))
				{
					setPropertyDictionaryString(obj,p,root,name);return;
				}
				
				if(p.PropertyType==typeof(Dictionary<string,int>))
				{
					setPropertyDictionaryInt(obj,p,root,name);return;
				}
				
				if(p.PropertyType==typeof(Dictionary<string,float>))
				{
					setPropertyDictionaryFloat(obj,p,root,name);return;
				}
				
				if(p.PropertyType==typeof(Dictionary<string,bool>))
				{
					setPropertyDictionaryBool(obj,p,root,name);return;
				}
				
				if(p.PropertyType==typeof(Dictionary<string,object>))
				{
					setPropertyDictionaryObject(obj,p,root,name);return;
				}
				
				//custom class	
				System.Type customtype;
				if(p.PropertyType.IsArray)
				{
					customtype=p.PropertyType.GetElementType();
				}else
				{
					customtype = p.PropertyType;
				}
				var customclass=System.Activator.CreateInstance(customtype);
				if (customclass is ColibrySerializeHelperCustomClass&&!p.PropertyType.IsArray)
				{
					setPropertyCustomClass(obj,p,root,name);return;
				}else
				if(customclass is ColibrySerializeHelperCustomClass&&p.PropertyType.IsArray)
				{
					setPropertyCustomClassArray(obj,p,root,name);return;
				}	
			}
			catch 
        	{
            	Debug.Log("Exception caught.");
				p.SetValue(obj,null,null);return;
        	}
				
		}
		
		public static void setPropertyCustomClass(object obj,PropertyInfo p,JSON root,string name)
		{
			
			System.Type customtype = p.PropertyType;
			var customclass=System.Activator.CreateInstance(customtype);
			JSON newroot=new JSON();
			newroot=root.ToJSON(name);
			//custom class
			customclass=(customclass as ColibrySerializeHelperCustomClass).JSONtoMyClass(newroot);
			p.SetValue(obj,customclass,null);
		}
		
		public static void setPropertyCustomClassArray(object obj,PropertyInfo p,JSON root,string name)
		{
			//custom class array
			JSON[] array=root.ToArray<JSON>(name);
				
			System.Type customtype = p.PropertyType.GetElementType();
			var customclass=System.Activator.CreateInstance(customtype);
								
			p.SetValue(obj,(customclass as ColibrySerializeHelperCustomClass).Array(array),null);
		}
		
		public static void setPropertyDictionaryString(object obj,PropertyInfo p,JSON root,string name)
		{
			Dictionary<string,string> a = new Dictionary<string,string>();
			
			root=root.ToJSON(name);		
			var keys = root.fields.Keys;
			foreach (string key in keys)
			{
				a.Add (key,root.ToString(key));
			}
			p.SetValue(obj,a,null);
		}
		
		public static void setPropertyDictionaryInt(object obj,PropertyInfo p,JSON root,string name)
		{
			Dictionary<string,int> a = new Dictionary<string,int>();
			
			root=root.ToJSON(name);		
			var keys = root.fields.Keys;
			foreach (string key in keys)
			{
				a.Add (key,root.ToInt(key));
			}
			p.SetValue(obj,a,null);
		}
		
		public static void setPropertyDictionaryFloat(object obj,PropertyInfo p,JSON root,string name)
		{
			Dictionary<string,float> a = new Dictionary<string,float>();
			
			root=root.ToJSON(name);		
			var keys = root.fields.Keys;
			foreach (string key in keys)
			{
				a.Add (key,root.ToFloat(key));
			}
			p.SetValue(obj,a,null);
		}
		
		public static void setPropertyDictionaryBool(object obj,PropertyInfo p,JSON root,string name)
		{
			Dictionary<string,bool> a = new Dictionary<string,bool>();
			
			root=root.ToJSON(name);		
			var keys = root.fields.Keys;
			foreach (string key in keys)
			{
				a.Add (key,root.ToBoolean(key));
			}
			p.SetValue(obj,a,null);
		}
		
		public static void setPropertyDictionaryObject(object obj,PropertyInfo p,JSON root,string name)
		{
			Dictionary<string,object> a = new Dictionary<string,object>();
			
			root=root.ToJSON(name);		
			var keys = root.fields.Keys;
			foreach (string key in keys)
			{
				a.Add (key,root.ToObject(key));
			}
			p.SetValue(obj,a,null);
		}
		
		public static void setPropertyArrayList(object obj,PropertyInfo p,JSON root,string name)
		{
			ArrayList a=new ArrayList();
			object[] objArray=root.ToArray<object>(name);
			foreach (object objs in objArray)
			{
				a.Add (objs);
			}
			p.SetValue(obj,a,null);
		}
	
	}
}
