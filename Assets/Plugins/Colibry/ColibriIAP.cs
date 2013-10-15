using UnityEngine;
using System.Collections;
using AOT;

using System.Reflection;
using WyrmTale;

namespace Colibry {
	public class ColibriIAP:ColibryBaseEntities
	{		
		protected static ColibriIAP _instance=null;
		
		protected ColibriIAP() {}
		
		public static  ColibriIAP instance(){
			if (_instance == null)
			{
				_instance = new ColibriIAP();
			}
			return _instance;
		}
		
		public void testpurchaseiOS(SDKCorePluginCustomCallbackDelegate fp, string receipt,string productid)
		{
			
			string modifiedString=receipt.Replace("\"","'");
			string testString="{\"receipt-data\":\""+modifiedString+"\",\"product-id\":\""+productid+"\",\"bid\":\""+SDKCorePlugin.SDKCoreGetBundleIdentifier()+"\"}"; 
					
			SDKCorePlugin.SDKCorePluginSendRequest(ColibryIapRequestSigns.TEST_PURCHASE_IOS,ColibryIapCmds.TEST_PURCHASE_IOS, testString,sdkResponse,fp);
		}	
		
		public void testpurchaseAndroid(SDKCorePluginCustomCallbackDelegate fp, string key,string signature,string receipt)
		{
			string testString="{\"key\":\""+key+"\",\"signature\":\""+signature+"\",\"receipt\":"+receipt+"}"; 
			Debug.Log (testString);
					
			SDKCorePlugin.SDKCorePluginSendRequest(ColibryIapRequestSigns.TEST_PURCHASE_ANDROID,ColibryIapCmds.TEST_PURCHASE_ANDROID, testString,sdkResponse,fp);
		}
		
		private void testpurchaseiOSResponse(JSON root)
		{
		    //do something
		}
		
		private void testpurchaseAndroidResponse(JSON root)
		{
		    //do something
		}
		
		private void errorResponse(JSON root)
		{
		    //do something
			Debug.Log ("Error c# = "+root.serialized);
		}	
		
		private void sdkResponseCustom(BaseCallbackInfo ininfo)
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
				
				if(ininfo.signature==ColibryIapRequestSigns.TEST_PURCHASE_IOS)
			    {
					if(!root.fields.ContainsKey("status")||root.ToInt ("status")!=0)
					{
						infoerror=true;
					}
			        testpurchaseiOSResponse(root);
			    }
				
				
				if(ininfo.signature==ColibryIapRequestSigns.TEST_PURCHASE_ANDROID)
			    {
					if(!root.fields.ContainsKey("status")||root.ToInt ("status")!=0)
					{
						infoerror=true;
					}
			        testpurchaseAndroidResponse(root);
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
