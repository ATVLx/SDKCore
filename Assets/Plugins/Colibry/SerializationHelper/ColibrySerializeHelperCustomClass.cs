using UnityEngine;
using System.Collections;
using WyrmTale;

namespace Colibry {
	public interface ColibrySerializeHelperCustomClass{
		
	// serialize this class to JSON
  	JSON ToJSON();
 
  	// JSON to class conversion
  	ColibrySerializeHelperCustomClass JSONtoMyClass(JSON value);
	
	// convert a JSON array to a MyClass Array
	ColibrySerializeHelperCustomClass[] Array(JSON[] array);
	}
 
}