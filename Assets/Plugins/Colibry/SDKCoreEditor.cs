using UnityEngine;
using System.Collections;
using WyrmTale;


namespace Colibry {
	public delegate void SDKCoreEditorCallBack(CIRequest request,string response);
	
	public class SDKCoreEditor{
		private static int options=0x0000;
		
		public static string _WWWGetBundleIdentifier ()
		{
			return "com.colibrytest";
		}
		
		public static string _SDKCorePluginGetVersion()
		{
			return "0.0.1";
		}
		
		public static void _SendWWWRequest(string sign,string cmd,string data,SDKCorePluginCallbackDelegateStruct fp,SDKCorePluginCustomCallbackDelegate customcallback)
		{
			CIRequest Request=CIUser.MakeRequest(sign,cmd,data,fp,customcallback);
			SetRequestOptions(Request);
			
			if((options&((int)SDKCorePluginOptions.SDKCorePluginOptionQueued))!=0)
	    	{
	        	CIUser.PutRequestIntoQueue(Request);
	    	}
			else
			{
				Request.StartRequest();
			}
		}
		
		public static void _WWWForceResetQueue()
		{
			CIUser.ForceResetQueue();
		}
		
		
		public static void _SetWWWSetOption(int inoption)
		{
			options=options|inoption;
		}
		
		public static void _SetWWWResetOption(int inoption)
		{
			options=options&(~inoption);
		}
		
		public static void _initSDKCoreEditor(string inurl,string uuid)
		{
			CIUser.UUID=uuid;
			CIUser.url=inurl;
			
	    	//set callback
	    	CIUser.RequestEndedCallBackEditor=RequestEndedCallBack;
	   	}
		
		private static void SetRequestOptions(CIRequest CIRequestComponent)
		{
			if((options&((int)SDKCorePluginOptions.SDKCorePluginOptionHTTP))!=0)
			{
				CIRequestComponent.requestMethodToSend=RequestMethodToSend.HTTPJSON;
			}
			
			if((options&((int)SDKCorePluginOptions.SDKCorePluginOption30SecTimeOut))!=0)
	    	{
	        	CIRequestComponent.waitTime=30f;
	    	}
			
			if((options&((int)SDKCorePluginOptions.SDKCorePluginOption15SecTimeOut))!=0)
	    	{
	        	CIRequestComponent.waitTime=15f;
	    	}
			
			if((options&((int)SDKCorePluginOptions.SDKCorePluginOption10SecTimeOut))!=0)
	    	{
	        	CIRequestComponent.waitTime=10f;
	    	}
			
			if((options&((int)SDKCorePluginOptions.SDKCorePluginOption5SecTimeOut))!=0)
	    	{
	        	CIRequestComponent.waitTime=5f;
	    	}
		}
		
		public static void RequestEndedCallBack(CIRequest request,string response)
		{		
			BaseCallbackInfo info=new BaseCallbackInfo();
			info.customcallback=request.customcallback;
			info.signature=request.sign;
			info.jsonresult=response;
			
			Debug.Log(response);
					
			request.cb(info);
		}
	}
}
