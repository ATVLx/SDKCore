using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AOT;

using System.Reflection;
using WyrmTale;

namespace Colibry {	
	public class ColibryUser:ColibryBaseEntities
	{		
		public string _username;
		public string _password;
		public string _email;
		
		public string id=null;
		
		protected static ColibryUser _instance=null;
		
		protected ColibryUser() {}
		
		public static  ColibryUser instance(){
			if (_instance == null)
			{
				_instance = new ColibryUser();
			}
			return (ColibryUser)_instance;
		}
		
		
		private JSON getFieldsValuesJsonObject()
		{	
			JSON root=new JSON();
			
			PropertyInfo[] properties = this.GetType().GetProperties();
			
			foreach (var p in properties)
			{
				ColibrySerializeHelper.getProperty(this,p,root,p.Name);
			}
			return root;	
		}
		
		public void signUp(SDKCorePluginCustomCallbackDelegate fp){
	 		SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.SIGN_UP,ColibryUserCmds.SIGN_UP,"{\"username\":\""+_username+"\",\"password\":\""+_password+"\",\"email\":\""+_email+"\"}",sdkResponse,fp);
		}
		
		public void signIn(SDKCorePluginCustomCallbackDelegate fp)
		{
		    //get id
			SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.SIGN_IN,ColibryUserCmds.SIGN_IN, "{\"username\":\""+_username+"\",\"password\":\""+_password+"\"}",sdkResponse,fp);
		}
		
		public void signOut(SDKCorePluginCustomCallbackDelegate fp)
		{
		    SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.SIGN_OUT, ColibryUserCmds.SIGN_OUT,"",sdkResponse,fp);
		}
		
		public void saveUser(SDKCorePluginCustomCallbackDelegate fp)
		{
		    SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.SAVE_USER, ColibryUserCmds.SAVE_USER,getFieldsValuesJsonObject().serialized,sdkResponse,fp);	
		}
		
		public void saveUser(SDKCorePluginCustomCallbackDelegate fp,params string[] list)
		{				
			JSON root = getFieldsValuesJsonObject();
			JSON newroot = new JSON();
			string result;
													
			for ( int i = 0 ; i < list.Length&&list[i]!=null ; i++ )
			{
				Debug.Log (list[i]);
				//ключ существует
				if(root.fields.ContainsKey(list[i]))
				{
					newroot[list[i]]=root[list[i]];
				}
			}
			
			result=newroot.serialized;
			
			Debug.Log (result);
			
			SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.SAVE_USER,ColibryUserCmds.SAVE_USER, result,sdkResponse,fp);	
		}
		
		public void loadUser(SDKCorePluginCustomCallbackDelegate fp)
		{
		    SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.LOAD_USER,ColibryUserCmds.LOAD_USER, "",sdkResponse,fp);	
		}
		
		public void usersfind(SDKCorePluginCustomCallbackDelegate fp, string searchstring)
		{
		    SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.USERS_FIND,ColibryUserCmds.USERS_FIND, searchstring,sdkResponse,fp);	
		}
		
		
		public void deleteUser(SDKCorePluginCustomCallbackDelegate fp)
		{
		    SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.DELETE_USER,ColibryUserCmds.DELETE_USER, "",sdkResponse,fp);
		}
		
		public void forceSetToken(string token)
		{
		    SDKCorePlugin.SDKCorePluginForceSetToken(token);
		}
		
		
		public void setValue(SDKCorePluginCustomCallbackDelegate fp,string fieldname,object fieldvalue)
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
			
				string stringToSend=roottosend.serialized;
		    	SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.SET_VALUE,ColibryUserCmds.SET_VALUE, stringToSend,sdkResponse,fp);
			}
			else
			{	
				Debug.Log ("No such property "+fieldname);
			}
		}
		
		public void getValue(SDKCorePluginCustomCallbackDelegate fp,string fieldname)
		{
			string stringToSend="{\"field\":\""+fieldname+"\"}";
		    SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.GET_VALUE,ColibryUserCmds.GET_VALUE, stringToSend,sdkResponse,fp);
		}
		
		public void push(SDKCorePluginCustomCallbackDelegate fp,string fieldname,object fieldvalue)
		{
			var propertyInfo=this.GetType().GetProperty(fieldname);
			if(propertyInfo!=null)
			{
				if(propertyInfo.PropertyType==typeof(ArrayList))
				{
					ArrayList a=(ArrayList)propertyInfo.GetValue(this,null);
					a.Add (fieldvalue);
					propertyInfo.SetValue(this,a,null);
					
					string stringToSend="{\"field\":\""+fieldname+"\",\"value\":\""+fieldvalue+"\"}";
		    		SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.PUSH,ColibryUserCmds.PUSH, stringToSend,sdkResponse,fp);
				}
				else
				{	
					Debug.Log ("Property is not ArrayList"+fieldname);
				}
			}
			else
			{	
				Debug.Log ("No such property "+fieldname);
			}		
		}
		
		public void pull(SDKCorePluginCustomCallbackDelegate fp,string fieldname,object fieldvalue)
		{
			var propertyInfo=this.GetType().GetProperty(fieldname);
			if(propertyInfo!=null)
			{
				if(propertyInfo.PropertyType==typeof(ArrayList))
				{
					ArrayList a=(ArrayList)propertyInfo.GetValue(this,null);
					
					while(a.IndexOf(fieldvalue)>=0) {a.Remove(fieldvalue);}
					propertyInfo.SetValue(this,a,null);
					
					
					string stringToSend="{\"field\":\""+fieldname+"\",\"value\":\""+fieldvalue+"\"}";
		    		SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.PULL,ColibryUserCmds.PULL, stringToSend,sdkResponse,fp);
				}
				else
				{	
					Debug.Log ("Property is not ArrayList"+fieldname);
				}
			}
			else
			{	
				Debug.Log ("No such property "+fieldname);
			}		
		}
	
		public void changeUserNamesPassword(SDKCorePluginCustomCallbackDelegate fp)
		{
			string newNames = "{\"username\":\""+_username+"\",\"password\":\""+_password+"\",\"email\":\""+_email+"\"}";
			SDKCorePlugin.SDKCorePluginSendRequest(ColibryUserRequestSigns.CHANGE_USER_NAME_PASSWORD,ColibryUserCmds.SAVE_USER, newNames,sdkResponse,fp);
		}
		
		//responses
		public void signUpResponse(JSON root)
		{   
			if(root.fields.ContainsKey("id"))
			{
				id=root.ToString("id");
			}
		}
	
		private void signInResponse(JSON root)
		{
		    if(root.fields.ContainsKey("id"))
			{
				id=root.ToString("id");
			}
		}
		
		private void signOutResponse(JSON root)
		{
		    //do something
		}
		
		private void saveUserResponse(JSON root)
		{
		    //do something
		}
		
		private void loadUserResponse(JSON root)
		{	
			//root=getFieldsValuesJsonObject();
			//Debug.Log (root.serialized);
			//set data back to instance		
			PropertyInfo[] properties = this.GetType().GetProperties();
			foreach (PropertyInfo p in properties)
			{
			    string name = p.Name;
				if(root.fields.ContainsKey(name))
				{
					ColibrySerializeHelper.setProperty(this,p,root,name);
				}
			}	
		}
		
		private void usersfindResponse(JSON root)
		{
			//do something
		}
		
		private void setValueResponse(JSON root)
		{
			//do something
		}
		
		private void getValueResponse(JSON root)
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
		
		private void pushResponse(JSON root)
		{
			//do something
		}
		
		private void pullResponse(JSON root)
		{
			//do something
		}
		
		private void deleteUserResponse(JSON root)
		{
		    //do something
		}
		
		private void errorResponse(JSON root)
		{
		    //do something
			Debug.Log ("Error c# = "+root.serialized);
		}
		
		private void changeUserNamesPasswordResponse(JSON root)
		{
			//do something
		}
		
		public void sdkResponseCustom(BaseCallbackInfo ininfo)
		{
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
				if(ininfo.signature==ColibryUserRequestSigns.SIGN_UP)
			    {
			        signUpResponse(root);
			    }
			    
			    if(ininfo.signature==ColibryUserRequestSigns.SIGN_IN)
			    {
			        signInResponse(root);
			    }
			    
			    if(ininfo.signature==ColibryUserRequestSigns.SIGN_OUT)
			    {
			        signInResponse(root);
			    }
			    
			    if(ininfo.signature==ColibryUserRequestSigns.SAVE_USER)
			    {
			        saveUserResponse(root);
			    }
				
				if(ininfo.signature==ColibryUserRequestSigns.LOAD_USER)
			    {
			        loadUserResponse(root);
			    }
				
				if(ininfo.signature==ColibryUserRequestSigns.USERS_FIND)
			    {
			        usersfindResponse(root);
			    }
				
				if(ininfo.signature==ColibryUserRequestSigns.SET_VALUE)
			    {
			        setValueResponse(root);
			    }
				
				if(ininfo.signature==ColibryUserRequestSigns.GET_VALUE)
			    {
			        getValueResponse(root);
			    }
				
				if(ininfo.signature==ColibryUserRequestSigns.PUSH)
			    {
			        pushResponse(root);
			    }
				
				if(ininfo.signature==ColibryUserRequestSigns.PULL)
			    {
			        pullResponse(root);
			    }
			    
			    if(ininfo.signature==ColibryUserRequestSigns.DELETE_USER)
			    {
			        deleteUserResponse(root);
			    }
				
				if(ininfo.signature==ColibryUserRequestSigns.CHANGE_USER_NAME_PASSWORD)
			    {
			        changeUserNamesPasswordResponse(root);
			    }
	
			}
	
			
			for(int i=0;i<delegatesArray.Count;i++)
		    {
		        ((SDKCoreDelegate)delegatesArray[i]).sdkResponse(ininfo);
		    }
			
			if(ininfo.customcallback!=null)
			{
				ininfo.customcallback(ininfo.signature,root.serialized,infoerror);
			}
		}
		
		[MonoPInvokeCallback (typeof (SDKCorePluginCallbackDelegateStruct))]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public static void sdkResponse(BaseCallbackInfo ininfo){		
			_instance.sdkResponseCustom(ininfo);
		}
	}
}
