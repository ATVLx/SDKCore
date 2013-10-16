using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WyrmTale;
using Colibry;
using System.Reflection;


namespace Colibry {
	public class Entity: ColibrySerializeHelperCustomClass
	{	
		private string _id=null;		
		public string id
	    {
	        get { return _id; }
	        set { _id = value; }
	    }
		
		//unfortinatele due to language you must provide empty constructor
		public Entity(){}
		
		// serialize this class to JSON
		virtual public JSON ToJSON()
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
		virtual public ColibrySerializeHelperCustomClass JSONtoMyClass(JSON root)
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
		virtual public ColibrySerializeHelperCustomClass[] Array(JSON[] array)
		{
			List<Entity> tc = new List<Entity>();
			for (int i=0; i<array.Length; i++)
			{
				tc.Add((new Entity().JSONtoMyClass(array[i]) as Entity));
			}
			return tc.ToArray();
		}
		
		public JSON getValue(string fieldname,object fieldvalue)
		{
			
			var propertyInfo=this.GetType().GetProperty(fieldname);
			if(propertyInfo!=null)
			{
				propertyInfo.SetValue(this, fieldvalue,null);
				JSON root=new JSON();
				JSON roottosend=new JSON();
				
				ColibrySerializeHelper.getProperty(this,propertyInfo,root,propertyInfo.Name);
				roottosend["field"]=fieldname;
				roottosend["value"]=root[propertyInfo.Name];
				return roottosend;
			}
			else
			{	
				Debug.Log ("No such property "+fieldname);
				return null;
			}
		}
		
		public JSON unsetValue(string fieldname)
		{
			
			var propertyInfo=this.GetType().GetProperty(fieldname);
			if(propertyInfo!=null)
			{
				propertyInfo.SetValue(this,null,null);
				JSON root=new JSON();
				JSON roottosend=new JSON();
				
				ColibrySerializeHelper.getProperty(this,propertyInfo,root,propertyInfo.Name);
				roottosend["field"]=fieldname;
				roottosend["value"]=root[propertyInfo.Name];
				return roottosend;
			}
			else
			{	
				Debug.Log ("No such property "+fieldname);
				return null;
			}
		}
		
		public void setValue(JSON root)
		{
			var keys = root.fields.Keys;
			string propertyname="";
			foreach (string key in keys)
			{
				propertyname=key;
				break;
			}
			
			Debug.Log ("propertyname:" + propertyname);
			
			if(propertyname=="")
			{
				Debug.Log ("No property on server!");
				return;
			}
			
			PropertyInfo propertyInfo=this.GetType().GetProperty(propertyname);
			
			if(propertyInfo!=null)
			{
				ColibrySerializeHelper.setProperty(this,propertyInfo,root,propertyname);
			}
			else
			{	
				Debug.Log ("No such property "+propertyname);
			}
		}
		
		public JSON push(string fieldname,object fieldvalue)
		{
			var propertyInfo=this.GetType().GetProperty(fieldname);
			if(propertyInfo!=null)
			{
				if(propertyInfo.PropertyType==typeof(ArrayList))
				{
					ArrayList a=(ArrayList)propertyInfo.GetValue(this,null);
					a.Add (fieldvalue);
					propertyInfo.SetValue(this,a,null);
					
					JSON roottosend=new JSON();
	
					roottosend["field"]=fieldname;
					roottosend["value"]=fieldvalue;
					return roottosend;
					
				}
				else
				{	
					Debug.Log ("Property is not ArrayList"+fieldname);
					return null;
				}
			}
			else
			{	
				Debug.Log ("No such property "+fieldname);
				return null;
			}		
		}
		
		public JSON pull(string fieldname,object fieldvalue)
		{
			var propertyInfo=this.GetType().GetProperty(fieldname);
			if(propertyInfo!=null)
			{
				if(propertyInfo.PropertyType==typeof(ArrayList))
				{
					ArrayList a=(ArrayList)propertyInfo.GetValue(this,null);
					
					while(a.IndexOf(fieldvalue)>=0) {a.Remove(fieldvalue);}
					propertyInfo.SetValue(this,a,null);
					
					JSON roottosend=new JSON();
	
					roottosend["field"]=fieldname;
					roottosend["value"]=fieldvalue;
					return roottosend;
				}
				else
				{	
					Debug.Log ("Property is not ArrayList"+fieldname);
					return null;
				}
			}
			else
			{	
				Debug.Log ("No such property "+fieldname);
				return null;
			}		
		}
	}
}
