using UnityEngine;
using System.Collections;

/// <summary>
/// Base class.
/// </summary>
public abstract class Abstract : MonoBehaviour {

	private Transform _singleTransform = null;
	private Renderer _singleRenderer = null;
	
    public Transform singleTransform {
        get {
            return _singleTransform;
        }
        protected set {
            _singleTransform = value;
        }
    }
	
    public Renderer singleRenderer {
        get {
            return _singleRenderer;
        }
        protected set {
            _singleRenderer = value;
        }
    }
	
	public virtual void Awake(){
		singleTransform = transform;
		singleRenderer = renderer;
	}
	
	public Transform GetParent(int number){
		Transform current = singleTransform;
		for(int i=0;i<number;i++){
			current = current.parent;
			if(current == null){
				return null;
			}
		}
		return current;
	}
	
	public void RemoveAllChildren(){
		int count = singleTransform.GetChildCount();
		for(int i=0;i<count;i++){
			Destroy(singleTransform.GetChild(0).gameObject);
		}
	}	
}


