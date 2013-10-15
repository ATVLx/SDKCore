using UnityEngine;
using System.Collections;

namespace Colibry {
	public interface SDKCoreDelegate{
		void sdkResponse(BaseCallbackInfo ininfo);
	}
}
