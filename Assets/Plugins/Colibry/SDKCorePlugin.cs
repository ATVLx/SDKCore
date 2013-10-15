//#define SDKCORE_MAKE_ALL_IN_WWW

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using WyrmTale;

namespace Colibry {	
	public class SDKCorePlugin
	{ 		
		//now UNITY_ANDROID is not supported so using www
	#if UNITY_EDITOR || SDKCORE_MAKE_ALL_IN_WWW || UNITY_ANDROID
		private static void _SDKCorePluginInitialize (string url,string uuid)
		{
			SDKCoreEditor._initSDKCoreEditor(url,uuid);
		}
		
		private static void _SDKCorePluginSetOption (int option)
		{
			SDKCoreEditor._SetWWWSetOption(option);
		}
		
		private static void _SDKCorePluginResetOption (int option)
		{
			SDKCoreEditor._SetWWWResetOption(option);
		}
		
		private static void _SDKCorePluginSendRequest(string sign,string cmd,string data,SDKCorePluginCallbackDelegateStruct fp,SDKCorePluginCustomCallbackDelegate customcallback)
		{
			SDKCoreEditor._SendWWWRequest(sign,cmd,data,fp,customcallback);
		}		
		
		private static string _SDKCoreGetBundleIdentifier ()
		{
			return SDKCoreEditor._WWWGetBundleIdentifier();
		}
		
		private static void _SDKCorePluginForceResetQueue()
		{
			SDKCoreEditor._WWWForceResetQueue();
		}
		
		private static void _SDKCorePluginForceSetToken(string token)
		{
			Debug.Log ("SDKCorePlugin: Application.platform==RuntimePlatform.OSXEditor||RuntimePlatform.AndroidEditor!!!");
		}
		
		
		private static string _SDKCorePluginGetVersion()
		{
			return SDKCoreEditor._SDKCorePluginGetVersion();
		}
	#else
		//UNITY_ANDROID is not supported now
		#if UNITY_ANDROID
		private static extern void _SDKCoreTestPurchaseiOS (string sign,string data,SDKCorePluginCustomCallbackDelegate fp);
		{
			Debug.Log ("Not Supported!");
		}
			[DllImport ("unity_bridge")]
		#elif UNITY_IPHONE
		[DllImport ("__Internal",CallingConvention = CallingConvention.Cdecl)]
		private static extern void _SDKCorePluginInitialize (string url,string uuid);
		[DllImport ("__Internal",CallingConvention = CallingConvention.Cdecl)]
		private static extern void _SDKCorePluginSetOption (int option);
		[DllImport ("__Internal",CallingConvention = CallingConvention.Cdecl)]
		private static extern void _SDKCorePluginResetOption (int option);
		[DllImport ("__Internal",CallingConvention = CallingConvention.Cdecl)]
		private static extern void _SDKCorePluginSendRequest (string sign,string cmd,string data,SDKCorePluginCallbackDelegateStruct fp,SDKCorePluginCustomCallbackDelegate customcallback);		
		
		
		[DllImport ("__Internal",CallingConvention = CallingConvention.Cdecl)]
		private static extern string _SDKCoreGetBundleIdentifier();
		
		
		[DllImport ("__Internal",CallingConvention = CallingConvention.Cdecl)]
		private static extern void _SDKCorePluginForceResetQueue ();
		[DllImport ("__Internal",CallingConvention = CallingConvention.Cdecl)]
		private static extern void _SDKCorePluginForceSetToken (string token);
		
		[DllImport ("__Internal",CallingConvention = CallingConvention.Cdecl)]
		private static extern string _SDKCorePluginGetVersion();
		#endif
	#endif
	
		public static void SDKCorePluginInitialize (string requestURL,string uuid)
		{	
			_SDKCorePluginInitialize(requestURL,uuid);
		}
		
		public static void SDKCorePluginSetOption(int inoption)
		{
			_SDKCorePluginSetOption(inoption);
		}
		
		public static void SDKCorePluginResetOption(int inoption)
		{
			_SDKCorePluginResetOption(inoption);
		}
		
		public static void SDKCorePluginSendRequest(string sign,string cmd,string data,SDKCorePluginCallbackDelegateStruct fp,SDKCorePluginCustomCallbackDelegate customcallback)
		{
			_SDKCorePluginSendRequest(sign,cmd,data,fp,customcallback);
		}	
		
		public static string SDKCoreGetBundleIdentifier ()
		{
			return _SDKCoreGetBundleIdentifier();
		}
		
		
		public static void SDKCorePluginForceResetQueue()
		{
			_SDKCorePluginForceResetQueue();
		}
		
		public static void SDKCorePluginForceSetToken(string token)
		{
			_SDKCorePluginForceSetToken(token);
		}
		
		public static string SDKCorePluginGetVersion()
		{
			return _SDKCorePluginGetVersion();
		}
		
	}
}
