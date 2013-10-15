using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Colibry;
using AOT;

public class TestUser : ColibryUser {	
	
	private MyClass _MyClassInst;
	public MyClass MyClassInst
    {
        get { return _MyClassInst; }
        set { _MyClassInst = value; }
    }
	
	private MyClass[] _MyClassArray;
	public MyClass[] MyClassArray
    {
        get { return _MyClassArray; }
        set { _MyClassArray = value; }
    }
	
	private Vector3 _vector3Inst;
	public Vector3 vector3Inst
    {
        get { return _vector3Inst; }
        set { _vector3Inst = value; }
    }
	
	private Vector4 _vector4Inst;
	public Vector4 vector4Inst
    {
        get { return _vector4Inst; }
        set { _vector4Inst = value; }
    }
	
	private int _intProperty;
    public int intProperty
    {
        get { return _intProperty; }
        set { _intProperty = value; }
    }
	
	private bool _boolProperty;
	public bool boolProperty
    {
        get { return _boolProperty; }
        set { _boolProperty = value; }
    }
	
	private float _floatProperty;
	public float floatProperty
    {
        get { return _floatProperty; }
        set { _floatProperty = value; }
    }
	
	private string _stringProperty;
	
    public string stringProperty
    {
        get { return _stringProperty; }
        set { _stringProperty = value; }
    }
	
	
	private MyClass2 _MyClassInst2;
	public MyClass2 MyClassInst2
    {
        get { return _MyClassInst2; }
        set { _MyClassInst2 = value; }
    }
	
	private MyClass2[] _MyClass2Array;
	public MyClass2[] MyClass2Array
    {
        get { return _MyClass2Array; }
        set { _MyClass2Array = value; }
    }
	
	
    private int[] _intArray;
	
	public int[] intArray
    {
        get { return _intArray; }
        set { _intArray = value; }
    }
    
    private bool[] _boolArray;
	
	public bool[] boolArray
    {
        get { return _boolArray; }
        set { _boolArray = value; }
    }
    
    
	private string[] _stringArray;
	
	public string[] stringArray
    {
        get { return _stringArray; }
        set { _stringArray = value; }
    }
	
	
	private ArrayList _XXXArrayList=new ArrayList();
	
	public ArrayList XXXArrayList
    {
        get { return _XXXArrayList; }
        set { _XXXArrayList = value; }
    }
	
	private Dictionary<string,string> _MyDictionaryString=new Dictionary<string,string>();
	
	public Dictionary<string,string> MyDictionaryString
    {
        get { return _MyDictionaryString; }
        set { _MyDictionaryString = value; }
    }
	
	private Dictionary<string,int> _MyDictionaryInt=new Dictionary<string,int>();
	
	public Dictionary<string,int> MyDictionaryInt
    {
        get { return _MyDictionaryInt; }
        set { _MyDictionaryInt = value; }
    }
	
	private Dictionary<string,object> _MyDictionaryObject=new Dictionary<string,object>();
	
	public Dictionary<string,object> MyDictionaryObject
    {
        get { return _MyDictionaryObject; }
        set { _MyDictionaryObject = value; }
	}
		
	protected TestUser() {}
		
	new public static  TestUser instance(){
		if (_instance == null)
		{
			_instance = new TestUser();
		}
		return (TestUser)_instance;
	}
	
	/*[MonoPInvokeCallback (typeof (SDKCorePluginCallbackDelegateStruct))]
	//[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	new	public static void sdkResponse(BaseCallbackInfo ininfo){
		Debug.Log ("Response");
		_instance.sdkResponseCustom(ininfo);
	}*/

}
