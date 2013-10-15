using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AOT;

using System.Reflection;
using WyrmTale;

namespace Colibry {
	
	public class ColibryCoreNative
	{
		private string uuid=null;
		private string url=null;
		
		public void setUrl(string url){
			this.url = url;
		}
		
		public string getUrl(){
			return this.url;
		}
		
		public void setUuid(string uuid){
			this.uuid = uuid;
		}
		
		public string getUuid(){
			return this.uuid;
		}
		
		public void initialize() {
			SDKCorePlugin.SDKCorePluginInitialize(url,uuid);
    	}
	
		public void setOptions(int inoption){
			SDKCorePlugin.SDKCorePluginSetOption(inoption);
		}
		
		public void resetOptions(int inoption){
			SDKCorePlugin.SDKCorePluginResetOption(inoption);
		}
		
		
		public void forceResetQueue()
		{
			SDKCorePlugin.SDKCorePluginForceResetQueue();
		}
		
		protected static ColibryCoreNative _instance;		
		
		protected ColibryCoreNative() {}
		
		public static  ColibryCoreNative instance(){
			if (_instance == null)
			{
				_instance = new ColibryCoreNative();
			}
			return _instance;
		}
		
	}
}
