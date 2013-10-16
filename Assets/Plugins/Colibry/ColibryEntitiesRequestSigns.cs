using UnityEngine;
using System.Collections;

namespace Colibry {
	
	public class ColibryEntitiesRequestSigns
	{
	    public static string ENTITY_CREATE = "createEntity";
		public static string ENTITY_DELETE = "deleteEntity";
    	public static string ENTITY_SAVE = "saveEntity";
		public static string ENTITY_FIND = "findEntity";
		public static string ENTITY_SET = "setEntity";
		public static string ENTITY_GET = "getEntity";
		public static string ENTITY_UNSET = "unsetEntity";
		public static string ENTITIES_FIND = "findEntities";
		public static string ENTITIES_CREATE = "createEntities";
		
		public static string ENTITY_PUSH = "pushEntity";
		public static string ENTITY_PULL = "pullEntity";
	};
	
	
	public class ColibryEntitiesCmds
	{		
		public static string ENTITY_CREATE = "entity.create";
		public static string ENTITY_DELETE = "entity.delete";
    	public static string ENTITY_SAVE = "entity.save";
		public static string ENTITY_FIND = "entity.find";
		public static string ENTITY_SET = "entity.set";
		public static string ENTITY_GET = "entity.get";
		public static string ENTITY_UNSET = "entity.unset";
		public static string ENTITIES_FIND = "entities.find";
		public static string ENTITIES_CREATE = "entities.create";
		
		public static string ENTITY_PUSH = "entity.push";
		public static string ENTITY_PULL = "entity.pull";
	};
}
