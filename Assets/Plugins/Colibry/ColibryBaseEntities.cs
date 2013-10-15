using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AOT;

using System.Reflection;
using WyrmTale;

namespace Colibry {
	public class ColibryBaseEntities
	{					
			
		protected ArrayList delegatesArray = new ArrayList();	
		
		public void addDelegate(SDKCoreDelegate indelegate){
			delegatesArray.Add(indelegate);
		}
		
		public void deleteDelegate(SDKCoreDelegate indelegate){
			delegatesArray.Remove(indelegate);
		}
	}
}
