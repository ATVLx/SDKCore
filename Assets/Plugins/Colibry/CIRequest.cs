using UnityEngine;
using System.Collections;
using WyrmTale;

namespace Colibry {
	public enum RequestType : int
	{
	    App = 0,//app level
	    Init,//app level, generates id
	    Del, //user level, deletes id
	    User, //user level, needs id
	    Users //users level, don't needs id
	};
	
	public enum RequestMethodToSend : int
	{
	    PostJSON=0,//default full JSON
	    HTTPJSON//HTTP+JSON
	};
	
	public class CIRequest : MonoBehaviour {
		//for WaitTime TimeOut
		private float elapsedTime = 0.0f;
		public float waitTime = 0f;
		bool isDownloading = false;
		WWW response = null;
		
		//global options for instance
		public string requestURLEditor;
		public string UUID;
		public SDKCoreEditorCallBack RequestEndedCallBack;
		
		public RequestMethodToSend requestMethodToSend=RequestMethodToSend.PostJSON;
		public RequestType requestType;
		public SDKCorePluginCallbackDelegateStruct cb;
		
		//request paramters
		public string sign;
		public string cmd;
		public string data;
		public string token=null;
		public	SDKCorePluginCustomCallbackDelegate customcallback;
		public bool queued=false;
		public MakeFromQueueDelegate MakeFromQueue=null;
		public bool WaitForQueue=false;
		
		private Hashtable headers;
		private string url;
		private string result;
		private JSON root;
		
		private bool flagDisposed=false;
		
		void Update () {
		    if(isDownloading&&!WaitForQueue){
				elapsedTime += Time.deltaTime;
				//force stop waiting response
				if(elapsedTime >= waitTime && waitTime!=0)
				{
		        	StopCoroutine("SendWWWRequestCoRoutine");
					result="{\"error\":{\"code\":\"-5\",\"name\":\"NoResponse\",\"message\":\"No response from server!!!\"}}";
					RequestEndedCallBack(this,result);
					flagDisposed=true;
		        	response.Dispose();
				}
			}
			
			if(WaitForQueue&&MakeFromQueue!=null)
			{
				MakeFromQueue();
			}
	    }
		
		
		public void StartRequest()
		{
			StartCoroutine(SendWWWRequestCoRoutine());
		}	
		
		private void SetURL()
		{
			string newurl="";
	    	newurl+=requestURLEditor;
	       
		    if(requestMethodToSend==RequestMethodToSend.HTTPJSON)
		    {
		        newurl+="/";
		        newurl+=UUID;
				if(UUID!="")
				{
		        	newurl+="/";
				}
		        
				newurl+=cmd;
		    }
		    
		    url=newurl;
		    Debug.Log(url);
		}
		
		private void SetHeaders()
		{
			headers=new Hashtable();
			headers.Add("Content-Type", "application/json");
		}
		
		private bool SetRequestData()
		{
			root=new JSON();
			
			JSON dataroot=new JSON();
			
			if(requestMethodToSend==RequestMethodToSend.PostJSON)
		    {
				root["uuid"]=UUID;
				root["cmd"]=cmd;
		    }
					
			if(requestMethodToSend==RequestMethodToSend.HTTPJSON)
		    {
		        
		    }
			
			if(data!=""&&data!="{}")
			{
				dataroot.serialized = data;
				//ошибка
				if(dataroot.serialized=="{}")
				{
					Debug.Log ("Failed to parse data JSON");
					return false;
				}
				root["data"]=dataroot;
			}
			else
			{
				root["data"]=dataroot;
			}
	    
	
		    if(requestMethodToSend==RequestMethodToSend.HTTPJSON)
		    {
		        root=dataroot;
		        if(requestType==RequestType.User||requestType==RequestType.Del)
		        {
					root["token"]=token;
		        }
			}
			return true;
		}
		
		private IEnumerator SendWWWRequestCoRoutine()
		{					
			SetURL();
			SetHeaders();
			string jsonString="";
			if(requestType==RequestType.User||requestType==RequestType.Del)
			{
		        if(token==null)
		        {
		            result="{\"error\":{\"code\":-1,\"name\":\"NoToken\",\"message\":\"You're not singed in/up!!!\"}}";
					Debug.Log (result);
		            RequestEndedCallBack(this,result);
					yield break;
		        }
	    	}
			
			
			if(SetRequestData())
			{
				jsonString = root.serialized;
			}
			else
			{
				result="{\"error\":{\"code\":-2,\"name\":\"InvalidData\",\"message\":\"The data you're send is invalid!!!\"}}";
				RequestEndedCallBack(this,result);
				yield break;
			}
	
	
			var encoding = new System.Text.UTF8Encoding();
			Debug.Log(jsonString);
			isDownloading=true;
			response = new WWW(url, encoding.GetBytes(jsonString), headers); 
			
			//WaitFor
			yield return response;
			
			isDownloading=false;
			if(flagDisposed)
			{
				flagDisposed=false;
				yield break;
			}
			
	    	if(string.IsNullOrEmpty(response.error))
			{
				result = response.text;
			}
			else
			{
				result = "{\"error\":\""+response.error+"\"}";
			}
			
			//if emptyresponse
			if(string.IsNullOrEmpty(result))
			{
				result="{\"error\":{\"code\":-3,\"name\":\"EmptyResponse\",\"message\":\"Empty response from server!!!\"}}";
				RequestEndedCallBack(this,result);	
			}
			
			RequestEndedCallBack(this,result);
		}
		
		public void DestroyGameObject()
		{
			Destroy(gameObject);
		}
	}
}
