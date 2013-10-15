using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AOT;

using System.Reflection;
using WyrmTale;

namespace Colibry {
	public class ColibryEntities:ColibryBaseEntities
	{	
		//узнать какая из коллекций сушностей
		protected static Dictionary<string,object> EntitiesDictionary=new Dictionary<string,object>();
		public string collectionname;
		public System.Type collectiontype=null;
		
		public void initialize() {
			EntitiesDictionary.Add (collectionname,this);
    	}
		
		//словарь всех объектов
		public Dictionary<string,Entity> EntityDictionary=new Dictionary<string,Entity>();
		
		protected string AddToSign(string insignature)
		{
			if(collectionname=="")
			{
				Debug.Log ("Empty Entity Name!!!");
			}
			string result=collectionname+"."+insignature;
			return result;
		}
		
		protected string AddToSignObjectHashCode(object inobject,string insignature)
		{
			if(inobject==null)
			{
				Debug.Log ("Empty object!!!");
			}
			string result=inobject.GetHashCode()+"."+insignature;
			return result;
		}
		
		protected string GetClearedSign(string insignature)
		{
			string result=insignature;
			
			if(collectionname=="")
			{
				Debug.Log ("Empty Entity Name!!!");
			}
			
			if(insignature.Contains(collectionname+"."))
			{
				result=insignature.Replace(collectionname+".","");
			}
			return result;
		}
		
		protected static string GetFirstPartSign(string insignature)
		{
			string result="";			
			if(insignature.Contains("."))
			{
				int position=insignature.IndexOf(".");
				
				result=insignature.Substring(position);
				result=insignature.Replace(result,"");
			}
			return result;
		}
		
		protected static string RemoveFirstPartSign(string insignature)
		{
			string result="";
			
			if(insignature.Contains("."))
			{
				int position=insignature.IndexOf(".");
				result=insignature.Substring(position+1);
			}
			return result;
		}
		
		// destructor
		~ColibryEntities()
    	{
			EntitiesDictionary.Remove(collectionname);
       	}
				
		public ColibryEntities() {}		
		
		
		public void entitiesfind(SDKCorePluginCustomCallbackDelegate fp,string searchstring)
		{
			JSON roottosend=new JSON();
			if(searchstring!=""&&searchstring!=null)
			{
				roottosend.serialized=searchstring;
			}
			
			string requeststring;
			
			requeststring="{\"collection\":\""+collectionname+"\",\"object\":"+roottosend.serialized+"}";
			
			string customsign=AddToSign(ColibryEntitiesRequestSigns.ENTITIES_FIND);							
			SDKCorePlugin.SDKCorePluginSendRequest(customsign,ColibryEntitiesCmds.ENTITIES_FIND, requeststring,sdkResponse,fp);
		}
		
		public void entitiessave(SDKCorePluginCustomCallbackDelegate fp)
		{
			JSON roottosend=new JSON();

			
			string requeststring;
			
			requeststring="{\"collection\":\""+collectionname+"\",\"object\":"+roottosend.serialized+"}";
			
			string customsign=AddToSign(ColibryEntitiesRequestSigns.ENTITIES_SAVE);							
			SDKCorePlugin.SDKCorePluginSendRequest(customsign,ColibryEntitiesCmds.ENTITIES_SAVE, requeststring,sdkResponse,fp);
		}
		
		//for entity
		public void entitycreate(SDKCorePluginCustomCallbackDelegate fp, Entity inobject)
		{			
			string customsign=AddToSignObjectHashCode(inobject,ColibryEntitiesRequestSigns.ENTITY_CREATE);
				
			customsign=AddToSign(customsign);
			string requeststring="";
			string objectstring=inobject.ToJSON().serialized;
			EntityDictionary.Add(customsign,inobject);
			
			requeststring="{\"collection\":\""+collectionname+"\",\"object\":"+objectstring+"}";
			SDKCorePlugin.SDKCorePluginSendRequest(customsign,ColibryEntitiesCmds.ENTITY_CREATE, requeststring,sdkResponse,fp);
		}	
		
		public void entitysave(SDKCorePluginCustomCallbackDelegate fp, Entity inobject)
		{
			string customsign=inobject.id+"."+ColibryEntitiesRequestSigns.ENTITY_SAVE;	
			customsign=AddToSign(customsign);
			string requeststring="";
			string objectstring=inobject.ToJSON().serialized;
						
			requeststring="{\"collection\":\""+collectionname+"\",\"object\":"+objectstring+"}";
			SDKCorePlugin.SDKCorePluginSendRequest(customsign,ColibryEntitiesCmds.ENTITY_SAVE, requeststring,sdkResponse,fp);
		}
		
		public void entityset(SDKCorePluginCustomCallbackDelegate fp,Entity inobject,string fieldname,object fieldvalue)
		{
			JSON roottosend=inobject.getValue(fieldname,fieldvalue);
			if(roottosend!=null)
			{
				string customsign=inobject.id+"."+ColibryEntitiesRequestSigns.ENTITY_SET;	
				customsign=AddToSign(customsign);
				string requeststring="";
				roottosend["id"]=inobject.id;
				string objectstring=roottosend.serialized;
							
				requeststring="{\"collection\":\""+collectionname+"\",\"object\":"+objectstring+"}";
				
				SDKCorePlugin.SDKCorePluginSendRequest(customsign,ColibryEntitiesCmds.ENTITY_SET, requeststring,sdkResponse,fp);
			}
			else
			{
				Debug.Log ("No such property "+fieldname);
			}
		}
		
		public void entityunset(SDKCorePluginCustomCallbackDelegate fp,Entity inobject,string fieldname)
		{
			JSON roottosend=inobject.unsetValue(fieldname);
			if(roottosend!=null)
			{
				string customsign=inobject.id+"."+ColibryEntitiesRequestSigns.ENTITY_UNSET;	
				customsign=AddToSign(customsign);
				string requeststring="";
				roottosend["id"]=inobject.id;
				string objectstring=roottosend.serialized;
							
				requeststring="{\"collection\":\""+collectionname+"\",\"object\":"+objectstring+"}";
				
				SDKCorePlugin.SDKCorePluginSendRequest(customsign,ColibryEntitiesCmds.ENTITY_UNSET, requeststring,sdkResponse,fp);
			}
			else
			{
				Debug.Log ("No such property "+fieldname);
			}
		}
				
		public void entityget(SDKCorePluginCustomCallbackDelegate fp,Entity inobject,string fieldname)
		{
			JSON roottosend=new JSON();
			string customsign=inobject.id+"."+ColibryEntitiesRequestSigns.ENTITY_SET;	
			customsign=AddToSign(customsign);
			string requeststring="";
			
			roottosend["id"]=inobject.id;
			roottosend["field"]=fieldname;
			string objectstring=roottosend.serialized;
							
			requeststring="{\"collection\":\""+collectionname+"\",\"object\":"+objectstring+"}";
				
			SDKCorePlugin.SDKCorePluginSendRequest(customsign,ColibryEntitiesCmds.ENTITY_GET, requeststring,sdkResponse,fp);
		}
		
		public void entitydelete(SDKCorePluginCustomCallbackDelegate fp, Entity inobject)
		{			
			JSON roottosend=new JSON();
			string customsign=inobject.id+"."+ColibryEntitiesRequestSigns.ENTITY_SET;	
			customsign=AddToSign(customsign);
			string requeststring="";
			
			roottosend["id"]=inobject.id;
			string objectstring=roottosend.serialized;
							
			requeststring="{\"collection\":\""+collectionname+"\",\"object\":"+objectstring+"}";
			
			SDKCorePlugin.SDKCorePluginSendRequest(customsign,ColibryEntitiesCmds.ENTITY_DELETE, requeststring,sdkResponse,fp);
		}	
		
		public void entitypush(SDKCorePluginCustomCallbackDelegate fp,Entity inobject,string fieldname,object fieldvalue)
		{
			JSON roottosend=inobject.push(fieldname,fieldvalue);
			if(roottosend!=null)
			{
				string customsign=inobject.id+"."+ColibryEntitiesRequestSigns.ENTITY_PUSH;	
				customsign=AddToSign(customsign);
				string requeststring="";
				roottosend["id"]=inobject.id;
				string objectstring=roottosend.serialized;
							
				requeststring="{\"collection\":\""+collectionname+"\",\"object\":"+objectstring+"}";
				
				SDKCorePlugin.SDKCorePluginSendRequest(customsign,ColibryEntitiesCmds.ENTITY_PUSH, requeststring,sdkResponse,fp);
			}
		}
		
		public void entitypull(SDKCorePluginCustomCallbackDelegate fp,Entity inobject,string fieldname,object fieldvalue)
		{
			JSON roottosend=inobject.pull(fieldname,fieldvalue);
			if(roottosend!=null)
			{
				string customsign=inobject.id+"."+ColibryEntitiesRequestSigns.ENTITY_PULL;	
				customsign=AddToSign(customsign);
				string requeststring="";
				roottosend["id"]=inobject.id;
				string objectstring=roottosend.serialized;
							
				requeststring="{\"collection\":\""+collectionname+"\",\"object\":"+objectstring+"}";
				
				SDKCorePlugin.SDKCorePluginSendRequest(customsign,ColibryEntitiesCmds.ENTITY_PULL, requeststring,sdkResponse,fp);
			}
		}
		
		//Responses		
		private void entitycreateResponse(JSON root,string objectHashCode)
		{
			if(EntityDictionary.ContainsKey(objectHashCode)&&root.fields.ContainsKey("id"))
			{
				Entity curobject=EntityDictionary[objectHashCode];
				EntityDictionary.Add (root.ToString("id"),curobject);
				EntityDictionary.Remove(objectHashCode);
				curobject.id=root.ToString("id");
			}
			else
			{
				Debug.Log ("Unknown object sign hashcode="+objectHashCode);
			}
		}
		
		private void entitysaveResponse(JSON root,string objectHashCode)
		{
		    if(EntityDictionary.ContainsKey(objectHashCode))
			{
				//Entity curobject=EntityDictionary[objectHashCode];
			}
			else
			{
				Debug.Log ("Unknown object sign id="+objectHashCode);
			}
		}
		
		private void entitysetResponse(JSON root,string objectHashCode)
		{
		    //do something
		    if(EntityDictionary.ContainsKey(objectHashCode))
			{
				//Entity curobject=EntityDictionary[objectHashCode];
			}
			else
			{
				Debug.Log ("Unknown object sign id="+objectHashCode);
			}
		}
		
		private void entityunsetResponse(JSON root,string objectHashCode)
		{
		    //do something
		    if(EntityDictionary.ContainsKey(objectHashCode))
			{
				//Entity curobject=EntityDictionary[objectHashCode];
			}
			else
			{
				Debug.Log ("Unknown object sign id="+objectHashCode);
			}
		}
		
		private void entitygetResponse(JSON root,string objectHashCode)
		{
		    if(EntityDictionary.ContainsKey(objectHashCode))
			{
				Entity curobject=EntityDictionary[objectHashCode];
				curobject.setValue(root);
			}
			else
			{
				Debug.Log ("Unknown object sign id="+objectHashCode);
			}
		}
		
		private void entitiesfindResponse(JSON root)
		{
			if(collectiontype==null)
			{
				Debug.Log ("Collection type not set!!!");
				return;
			}
		    Debug.Log (root.serialized);
		    EntityDictionary.Clear ();
			
			//custom class array
			JSON[] array=root.ToArray<JSON>("");
				
			System.Type customtype = collectiontype;
			var customclass=System.Activator.CreateInstance(customtype);
			Entity[] objectarray=((customclass as ColibrySerializeHelperCustomClass).Array(array) as Entity[]);	
			for(int i=0;i<objectarray.Length;i++)
			{
				EntityDictionary.Add (objectarray[i].id,objectarray[i]);
			}
		}
		
		private void entitiessaveResponse(JSON root)
		{
		}
		
		private void entitydeleteResponse(JSON root,string objectHashCode)
		{
		    Debug.Log (root.serialized);
			Debug.Log (objectHashCode);
		    if(EntityDictionary.ContainsKey(objectHashCode))
			{
				EntityDictionary.Remove(objectHashCode);
			}
			else
			{
				Debug.Log ("Unknown object sign id="+objectHashCode);
			}
		}
		
		private void entitypushResponse(JSON root,string objectHashCode)
		{
		    //do something
		    if(EntityDictionary.ContainsKey(objectHashCode))
			{
				//Entity curobject=EntityDictionary[objectHashCode];
			}
			else
			{
				Debug.Log ("Unknown object sign id="+objectHashCode);
			}
		}
		
		private void entitypullResponse(JSON root,string objectHashCode)
		{
		    //do something
			Debug.Log (root.serialized);
			Debug.Log (objectHashCode);
		    if(EntityDictionary.ContainsKey(objectHashCode))
			{
				//Entity curobject=EntityDictionary[objectHashCode];
			}
			else
			{
				Debug.Log ("Unknown object sign id="+objectHashCode);
			}
		}
		
		private void errorResponse(JSON root)
		{
		    //do something
			Debug.Log ("Error c# = "+root.serialized);
		}
		
		private void sdkResponseCustom(BaseCallbackInfo ininfo){
			string clearedsignature=GetClearedSign(ininfo.signature);
			
			string objecthashcode=GetFirstPartSign(clearedsignature);
			//isobjectsignature
			if(objecthashcode!="")
			{
				clearedsignature=RemoveFirstPartSign(clearedsignature);
			}
			Debug.Log ("ininfo.sign="+ininfo.signature);
			Debug.Log ("objecthashcode="+objecthashcode);
			
			JSON root=new JSON();
			root.serialized=ininfo.jsonresult;
			bool infoerror=false;
			//есть ошибки
			if(root.fields.ContainsKey("error"))
			{
				infoerror=true;
			}
			
			if(infoerror)
			{
				errorResponse(root);			
			}
			else
			{
				
				if(clearedsignature==ColibryEntitiesRequestSigns.ENTITY_CREATE)
			    {
			        entitycreateResponse(root,ininfo.signature);
			    }
				
				if(clearedsignature==ColibryEntitiesRequestSigns.ENTITY_SAVE)
			    {
			        entitysaveResponse(root,objecthashcode);
			    }
				
				if(clearedsignature==ColibryEntitiesRequestSigns.ENTITY_SET)
			    {
			        entitysetResponse(root,objecthashcode);
			    }
				
				if(clearedsignature==ColibryEntitiesRequestSigns.ENTITY_UNSET)
			    {
			        entityunsetResponse(root,objecthashcode);
			    }
				
				if(clearedsignature==ColibryEntitiesRequestSigns.ENTITY_GET)
			    {
			        entitygetResponse(root,objecthashcode);
			    }
				
				if(clearedsignature==ColibryEntitiesRequestSigns.ENTITY_PUSH)
			    {
			        entitypushResponse(root,objecthashcode);
			    }
				
				if(clearedsignature==ColibryEntitiesRequestSigns.ENTITY_PULL)
			    {
			        entitypullResponse(root,objecthashcode);
			    }
				
				if(clearedsignature==ColibryEntitiesRequestSigns.ENTITIES_FIND)
			    {
			        entitiesfindResponse(root);
			    }
				
				if(clearedsignature==ColibryEntitiesRequestSigns.ENTITIES_SAVE)
			    {
			        entitiessaveResponse(root);
			    }
				
				if(clearedsignature==ColibryEntitiesRequestSigns.ENTITY_DELETE)
			    {
			        entitydeleteResponse(root,objecthashcode);
			    }
				
			}
	
			for(int i=0;i<delegatesArray.Count;i++)
		    {
		        ((SDKCoreDelegate)delegatesArray[i]).sdkResponse(ininfo);
		    }
			
			if(ininfo.customcallback!=null)
			{
				ininfo.customcallback(clearedsignature,root.serialized,infoerror);
			}
		}
		
		[MonoPInvokeCallback (typeof (SDKCorePluginCallbackDelegateStruct))]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public static void sdkResponse(BaseCallbackInfo ininfo){
			ColibryEntities curentity=null;
			if(EntitiesDictionary.ContainsKey(GetFirstPartSign(ininfo.signature)))
			{
				curentity=EntitiesDictionary[GetFirstPartSign(ininfo.signature)] as ColibryEntities;
				curentity.sdkResponseCustom(ininfo);
			}
			else
			{
				Debug.Log ("Invalid Signature!!!");
			}
		}
	}
	
}
