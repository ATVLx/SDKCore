using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Colibry {	

	public delegate void SDKCorePluginCallbackDelegateStruct(BaseCallbackInfo ininfo);
	public delegate void SDKCorePluginCustomCallbackDelegate(string sign, string jsonstring,bool error);
	
	
	/// CallBack
	[StructLayout(LayoutKind.Sequential)]
	public class BaseCallbackInfo 
	{
		public string jsonresult;
		public string signature;
		public SDKCorePluginCustomCallbackDelegate customcallback;
	};
	
	public enum SDKCorePluginOptions : int
	{
	    SDKCorePluginOptionQueued=0x0001,
	    SDKCorePluginOptionHTTP=0x0002,
	    SDKCorePluginOption30SecTimeOut=0x0004,
	    SDKCorePluginOption15SecTimeOut=0x0008,
	    SDKCorePluginOption10SecTimeOut=0x0010,
	    SDKCorePluginOption5SecTimeOut=0x0020
	};
}
