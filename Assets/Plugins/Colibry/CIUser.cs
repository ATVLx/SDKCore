using UnityEngine;
using System.Collections;
using WyrmTale;

namespace Colibry {
	public delegate void MakeFromQueueDelegate();
	
	public class CIUser{
		//unity says that arraylist faster then geheric types, believe it!!!
		//request queue
		private static ArrayList RequestQueue=new ArrayList();
	    private static CIRequest queueMaker=null;
	
		public static string url;
		public static string UUID;
		public static SDKCoreEditorCallBack RequestEndedCallBackEditor=null;
		private static string token=null;
		
		public static CIRequest MakeRequest(string sign,string cmd, string data,SDKCorePluginCallbackDelegateStruct fp,SDKCorePluginCustomCallbackDelegate customcallback)
		{
			GameObject newobj= new GameObject();
			newobj.name="www";
			newobj.AddComponent("CIRequest");
			CIRequest CIRequestComponent=newobj.GetComponent<CIRequest>();
			
			//общие данные
			CIRequestComponent.UUID=UUID;
			CIRequestComponent.requestURLEditor=url;
			CIRequestComponent.token=token;
			
			
			CIRequestComponent.RequestEndedCallBack=RequestEndedCallBack;
			CIRequestComponent.cb=fp;
			
			//данные конкретного запроса
			CIRequestComponent.sign=sign;
			CIRequestComponent.cmd=cmd;
			CIRequestComponent.data=data;
			CIRequestComponent.customcallback=customcallback;
			
			SetRequestType(CIRequestComponent,cmd);
			
			return CIRequestComponent;
		}
		
		public static void PutRequestIntoQueue(CIRequest inrequest)
		{
			if(queueMaker!=null)
		    {
		        inrequest.queued=true;
		        RequestQueue.Add (inrequest);
		    }
		    else
		    {
		        //ничего из очереди ещё не выполнялось
		        inrequest.queued=true;
				inrequest.MakeFromQueue=MakeFromQueue;
		        queueMaker=inrequest;
		        queueMaker.StartRequest();
		    }
		}
		
		public static void ForceResetQueue()
		{
			if(queueMaker!=null)
	    	{
				queueMaker.DestroyGameObject();
				queueMaker=null;
	    	}
	    
			CIRequest request;
		    //clear queue
		    while(RequestQueue.Count!=0)
		    {
				request=RequestQueue[0] as CIRequest;
				RequestQueue.RemoveAt(0);
		        request.DestroyGameObject();
		    }
		}
		
			
		private static void SetRequestType(CIRequest CIRequestComponent,string cmd)
		{
			//инициализация типа запроса
		    if(cmd.Contains(".signup")||cmd.Contains(".signin"))
		    {
		        CIRequestComponent.requestType=RequestType.Init;
		    }
		    else if(cmd.Contains("user.delete")||cmd.Contains("user.signout"))
		    {
		        CIRequestComponent.requestType=RequestType.Del;
		    }else
		    if(cmd.Contains("user."))
		    {
		        CIRequestComponent.requestType=RequestType.User;
		    }
		    else
		    {
		        CIRequestComponent.requestType=RequestType.User;
		    }
		}
		
		
		public static void RequestEndedCallBack(CIRequest request,string response)
		{
			JSON js = new JSON();
			js.serialized = response;
			
			if(string.IsNullOrEmpty(js.serialized))
			{
				js.serialized="{\"error\":{\"code\":-4,\"name\":\"NotJsonResponse\",\"message\":\"Response is invalid!!!\"}}";
				response=js.serialized;
			}
			
			if(request.requestType==RequestType.Init)
	        {
				if(js.fields.ContainsKey("token")&&js["token"]!=null)
				{
					token=js.ToString("token");
					js["token"]="";
					response=js.serialized;
				}
	        }
	        
	        //удалим пользователя
	        if(request.requestType==RequestType.Del)
	        {
	            token=null;
	        }       
			
			if(RequestEndedCallBackEditor!=null)
			{
				RequestEndedCallBackEditor(request,response);
			}
			
			
			//возможно это поток из очереди
		    if(request.queued)
		    {        
		        if(RequestQueue.Count == 0)
				{
		           request.WaitForQueue=true;
				}
		        else
		        {
					//в очереди есть ещё задачи на выполнение
		            MakeFromQueue();
		        }
		
		    }
			else
			{
				request.DestroyGameObject();
			}
		}
		
		public static void MakeFromQueue()
		{
			if(RequestQueue.Count>0)
			{
				queueMaker.WaitForQueue=false;
				CIRequest vspMaker=queueMaker;
				vspMaker.DestroyGameObject();
			    //выполняем следующий запрос
			    queueMaker=RequestQueue[0] as CIRequest;
				RequestQueue.RemoveAt(0);
				
				queueMaker.MakeFromQueue=MakeFromQueue;
				queueMaker.StartRequest();
			}
		}
	}
}
