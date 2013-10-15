using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AOT;
using WyrmTale;
using Colibry;

enum Sostoyanie {NotLoged=0, Logged};


public class TestSceneController : Abstract,SDKCoreDelegate {
	
	TestUser user=null;
	ColibriIAP colibriIAP;
	TestEntities1 apples;
	ColibryCoreNative colibryCoreNative;
	
	//string uuid = "da737fec-358d-463b-862c-c9a7a225df5f";
	string uuid = "";
	//string url = "http://api.cas.gs";
	//string url = "http://172.27.7.65:3000";
	string url = "http://10.27.1.53:3000";
	//string url = "http://api.sggs.eu";
	
	Entity object1;
	
	
	public string inappreciepeios;
	public string inappreciepeandroid;

	public GUIText ResultText;
	static private TestSceneController inst=null;
	string playerName = "UnityPlayer";
	float buttonwidth=100*2;
	float buttonheight=20*2;
	float centerxsmex=110;
	float between=3;
	Sostoyanie curSost=Sostoyanie.NotLoged;
	
	void Update()
	{
	}

	// Use this for initialization
	void Start () {
		if(!inst)
		{
			inst=this;
			
			colibryCoreNative=ColibryCoreNative.instance();
			colibryCoreNative.setUrl(url);
			colibryCoreNative.setUuid(uuid);
			colibryCoreNative.setOptions((int)SDKCorePluginOptions.SDKCorePluginOptionHTTP|(int)SDKCorePluginOptions.SDKCorePluginOptionQueued|(int)SDKCorePluginOptions.SDKCorePluginOption5SecTimeOut);
			colibryCoreNative.resetOptions((int)SDKCorePluginOptions.SDKCorePluginOptionQueued);
			colibryCoreNative.initialize();
			
			colibriIAP=ColibriIAP.instance();
			user=TestUser.instance();
			
			user.addDelegate(this);
			
			//entities
			
			apples=new TestEntities1();
			apples.collectionname="apples";
			apples.collectiontype=typeof(MyClass2);
			apples.initialize();			
			AddSerialize();
		}
	}
	
	void AddSerialize () {
		/*user.stringProperty="string44";
			user.intProperty=10;
			user.boolProperty=true;
			user.floatProperty=0.2f;
			user.vector3Inst=new Vector3(10,10,10);
			user.vector4Inst=new Vector4(1,2,3,4);	
			
			user.intArray=new int[] {10,15,25};
			
			user.stringArray=new string[] {"string3","string4"};
			ArrayList MyList=new ArrayList();
			MyList.Add ("string11");
			MyList.Add (1);
			
			ArrayList MyList2=new ArrayList();
			MyList2.Add ("string21");
			MyList2.Add (1);
			
			MyList.Add(MyList2);
			
			user.XXXArrayList.Add("string5");
			user.XXXArrayList.Add("string4");
			user.XXXArrayList.Add(1);
			user.XXXArrayList.Add(MyList);
			
						
			user.MyDictionaryString.Add("key1","value1");
			user.MyDictionaryString.Add("key2","value2");
			
			user.MyDictionaryInt.Add("key1",1);
			user.MyDictionaryInt.Add("key2",2);
			
			user.MyDictionaryObject.Add("key1",1);
			user.MyDictionaryObject.Add("key2","string");
			user.MyDictionaryObject.Add("key3",MyList);
			user.MyDictionaryObject.Add("key4",2.5);
			
			//user.MyClassInst=new MyClass("object.0", new Vector3(20,21,22), new Vector3(2,2,2), Quaternion.identity, null);
			user.MyClassInst2=new MyClass2("MyClass2.object.1", new Vector3(1,1,1), new Vector3(1,1,1), new Quaternion(100,100,100,100),new int[] {1,2,3,4,5});
			user.MyClassInst2=new MyClass2("MyClass2.object.1", new Vector3(1,1,1), new Vector3(1,1,1), new Quaternion(100,100,100,100),null);

			user.MyClassArray=new MyClass[]{new MyClass("object.1",new Vector3(20,20,20), new Vector3(2,2,2), Quaternion.identity,new int[] {1,2,3,4,5}),
				new MyClass("object.2",new Vector3(30,30,30), new Vector3(3,3,3), Quaternion.identity,new int[] {1,2,3,4,5})};*/
			
			
			user.MyClass2Array=new MyClass2[]{new MyClass2("object.1",new Vector3(20,20,20), new Vector3(2,2,2), Quaternion.identity,null),
				new MyClass2("object.2",new Vector3(30,30,30), new Vector3(3,3,3), Quaternion.identity,null)};
	}
	
	// Update is called once per frame
	static SDKCorePluginCustomCallbackDelegate fpsignInResponse=new SDKCorePluginCustomCallbackDelegate(signInResponse);
	static SDKCorePluginCustomCallbackDelegate fpsignUpResponse=new SDKCorePluginCustomCallbackDelegate(signUpResponse);
	static SDKCorePluginCustomCallbackDelegate fpsaveResponse=new SDKCorePluginCustomCallbackDelegate(saveResponse);
	static SDKCorePluginCustomCallbackDelegate fploadResponse=new SDKCorePluginCustomCallbackDelegate(loadResponse);
	static SDKCorePluginCustomCallbackDelegate fpsignOutResponse=new SDKCorePluginCustomCallbackDelegate(signOutResponse);
	static SDKCorePluginCustomCallbackDelegate fpdeleteResponse=new SDKCorePluginCustomCallbackDelegate(deleteResponse);
	
	static SDKCorePluginCustomCallbackDelegate fppurchaseiOSResponse=new SDKCorePluginCustomCallbackDelegate(purchaseiOSResponse);
	static SDKCorePluginCustomCallbackDelegate fppurchaseAndroidResponse=new SDKCorePluginCustomCallbackDelegate(purchaseAndroidResponse);
	
	static SDKCorePluginCustomCallbackDelegate fpsetValueResponse=new SDKCorePluginCustomCallbackDelegate(setResponse);
	static SDKCorePluginCustomCallbackDelegate fpgetValueResponse=new SDKCorePluginCustomCallbackDelegate(getResponse);
	static SDKCorePluginCustomCallbackDelegate fppushResponse=new SDKCorePluginCustomCallbackDelegate(pushResponse);
	static SDKCorePluginCustomCallbackDelegate fppullResponse=new SDKCorePluginCustomCallbackDelegate(pullResponse);
	static SDKCorePluginCustomCallbackDelegate fpusersfindResponse=new SDKCorePluginCustomCallbackDelegate(usersfindResponse);
	static SDKCorePluginCustomCallbackDelegate fpchangeUserNamesPasswordResponse=new SDKCorePluginCustomCallbackDelegate(changeUserNamesPasswordResponse);
	
	//universal
	static SDKCorePluginCustomCallbackDelegate fpuniversalResponse=new SDKCorePluginCustomCallbackDelegate(universalResponse);
	
	//entities
	static SDKCorePluginCustomCallbackDelegate fpentitycreateResponse=new SDKCorePluginCustomCallbackDelegate(entitycreateResponse);
	
	void OnGUI () {
		
		float shag=0;
		float upsmeh=80;
		if(curSost==Sostoyanie.NotLoged){
			// Make the next button. 
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "signup")) {
				ResultText.text="GettingData!!!";
				user.signUp(fpsignUpResponse);
			}
			//Make the next button.
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "signin")) {
				ResultText.text="GettingData!!!";
				user.signIn(fpsignInResponse);	
			}
			shag++;
		}
		//if(curSost==Sostoyanie.Logged)
		{
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "save")) {
				ResultText.text="GettingData!!!";
				//user.saveUser(null,"sdfsd","someProperty2","someProperty1",null);
				user.saveUser(fpsaveResponse);
			}
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "load")) {
				ResultText.text="GettingData!!!";
				user.loadUser(fploadResponse);
			}
			shag++;
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "signout")) {
				ResultText.text="GettingData!!!";
				user.signOut(fpsignOutResponse);
			}
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "delete")) {
				ResultText.text="GettingData!!!";
				user.deleteUser(fpdeleteResponse);
			}
			
			shag++;
			
			//put your reciepe hear
			// Make the next button.
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "IOS inapp")) {
				ResultText.text="GettingData!!!";
				
				//colibriIAP.testpurchaseiOS(fppurchaseiOSResponse,getedreceipe,"com.colibrytest.inapp1");
				colibriIAP.testpurchaseiOS(fppurchaseiOSResponse,inappreciepeios,"com.colibrytest.inapp1");
			}
			
			
			// Make the next button.
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "Android inapp")) {
				ResultText.text="GettingData!!!";
				string key="MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA47m8ZwV+96rb6b+k8NNpl37BpteB10i2KWt/G3CpzOd6/6dSP8PS+TngYtt62sFmtJ4a8YbFsRCXu1M5YhCWnaHhwzZdzJwOszZvOgW1ANDed8QJPXOXEZz5fUqqByjRnEVBv4ndgnHXcEb8Xviwt7r2LIaAUiQbJ0pW7I2Hr4ttFFq3IlKoecSfC/nqw3a6epzMScXnoiWhBlFQ90Ci3uZDKf6Yphj15ItTnWGStNID4Hs0xhtqXzp6LmVFMjNMY3E+ITbiqc14jC2BIdhiXZ79LBMvggeNpORfnFgq0bcXVPhrp49CzRTDH8K5mHJDQJ8spHtsgLTT7xC8+YnGOQIDAQAB";
				string signature="Jn0l3kVvxHbeCfDMxhjphwYiTYQM1EDd47H9Wj4R7aY6nhb4CuryvwqYtfjPV9KmZmophoJjVPy8/05N3qFToYiZTE60Ie060WwIBYkHLCNbwQD2A6eugeGUk0GtWZq75PibKkMGmVd/Lr0qpJDlXi+VjlBkc80aeNNV5Tc+GsNwU1L8zYfxNfMpk8ry9uYFCyfZUXNBoJRqtdIwSe/l0O2baPvRMv+4pCa6tJ+VPsC+ySAvBmPQesRXc28vXK6d2iPQJPURsQLuKKeh6jNx6urGGsr7iIaK/zxdvVwYx6BryzZR1s0hVParBw6hV0qJtCBk3FgQFtJILrIF0D3G0Q==";
				colibriIAP.testpurchaseAndroid(fppurchaseAndroidResponse,key,signature,inappreciepeandroid);
			}
			shag++;
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "set")) {
				ResultText.text="GettingData!!!";
				user.setValue(fpsetValueResponse,"MyClassInst",new MyClass("object.0", new Vector3(20,21,22), new Vector3(2,2,2), Quaternion.identity, null));
			}
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "get")) {
				ResultText.text="GettingData!!!";
				user.getValue(fpgetValueResponse,"MyClassInst");
			}
			shag++;
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "push")) {
				ResultText.text="GettingData!!!";
				user.push(fppushResponse,"XXXArrayList","string3");
			}
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "pull")) {
				ResultText.text="GettingData!!!";
				user.pull(fppullResponse,"XXXArrayList","string3");
			}
			shag++;
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "version")) {
				ResultText.text="GettingData!!!";
				ResultText.text=SDKCorePlugin.SDKCorePluginGetVersion();
			}
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "usersfind")) {
				ResultText.text="GettingData!!!";
				//user.usersfind(fpusersfindResponse,"{\"query\":{\"password\":\"123\"},\"fields\":{\"username\":1},\"limit\":1}");
				user.usersfind(fpusersfindResponse,"{}");
			}
			shag++;
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "changename")) {
				ResultText.text="GettingData!!!";
				user.changeUserNamesPassword(fpchangeUserNamesPasswordResponse);
			}
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "entitycreate")) {
				ResultText.text="GettingData!!!";
				object1=new MyClass2("object.1",new Vector3(20,20,20), new Vector3(2,2,2), Quaternion.identity,null);
				apples.entitycreate(fpentitycreateResponse,object1);
			}
			
			shag++;
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "entitysave")) {
				ResultText.text="GettingData!!!";
				apples.entitysave(fpuniversalResponse,object1);
			}
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "entityset")) {
				ResultText.text="GettingData!!!";
				apples.entityset(fpuniversalResponse,object1,"intarray",new int[]{10,250,25});
			}
			
			shag++;
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "entityget")) {
				ResultText.text="GettingData!!!";
				apples.entityget(fpuniversalResponse,object1,"intarray");
			}
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "entitiesfind")) {
				ResultText.text="GettingData!!!";
				apples.entitiesfind(fpuniversalResponse,"{\"limit\":10}");
				//apples.entitiesfind(fpuniversalResponse,"{}");
			}
			
			shag++;
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "entitydelete")) {
				ResultText.text="GettingData!!!";
				apples.entitydelete(fpuniversalResponse,object1);
			}
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "entitypush")) {
				ResultText.text="GettingData!!!";
				apples.entitypush(fpuniversalResponse,object1,"XXXArrayList","string1");
			}
			
			shag++;
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "entitypull")) {
				ResultText.text="GettingData!!!";
				apples.entitypull(fpuniversalResponse,object1,"XXXArrayList","string1");
			}
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "entityunset")) {
				ResultText.text="GettingData!!!";
				apples.entityunset(fpuniversalResponse,object1,"intarray");
			}
			
			shag++;
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2-centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "entitiescreate")) {
				ResultText.text="GettingData!!!";
				apples.entitiescreate(fpuniversalResponse);
			}
			
			if(GUI.Button(new Rect(Screen.width/2-buttonwidth/2+centerxsmex,upsmeh+shag*(buttonheight+between),buttonwidth,buttonheight), "entitiessaveall")) {
				ResultText.text="GettingData!!!";
				apples.entitiessaveall(fpuniversalResponse);
			}
			
		}
		
		// Make a text field that modifies stringToEdit.
		playerName = GUI.TextField (new Rect(Screen.width/2-100, 20, 200, 40), playerName, 20);
		user._username = playerName;
		user._password = "123";
		user._email = "";
	}
	
	//universal	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void universalResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
			Debug.Log ("universalResponse "+root.serialized);
			inst.ResultText.text="universalResponse "+root.serialized;
	}
	
	//start entities...............
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void entitycreateResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
			Debug.Log ("entitycreateResponse "+root.serialized);
			inst.ResultText.text="entitycreateResponse "+root.serialized;
	}
	
	
	//end entities...............
	
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void purchaseiOSResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		if(!error)
		{
			Debug.Log ("Pucrchase result!!!  "+"com.colibrytest");
			inst.ResultText.text="You purchased!"+root.serialized;
		}
		else
		{
			Debug.Log ("Pucrchase result!!!  "+"com.colibrytest");
			inst.ResultText.text="You NOT purchased!"+root.serialized;
		}
	}
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void changeUserNamesPasswordResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		if(!error)
		{
			Debug.Log ("You changed name!!!");
			inst.ResultText.text="You changed name!"+root.serialized;
		}
		else
		{
			Debug.Log ("You NOT changed name!!!");
			inst.ResultText.text="You NOT changed name!"+root.serialized;
		}
	}
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void purchaseAndroidResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		if(!error)
		{
			Debug.Log ("Pucrchase result!!!  "+"colibry_test_inap");
			inst.ResultText.text="You purchased!"+root.serialized;
		}
		else
		{
			Debug.Log ("Pucrchase result!!!  "+"colibry_test_inap");
			inst.ResultText.text="You NOT purchased!"+root.serialized;
		}
	}
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void loadResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		inst.ResultText.text="You loaded!"+root.serialized;
	}
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void signOutResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		inst.curSost=Sostoyanie.NotLoged;
		inst.ResultText.text="You signed out!"+root.serialized;
	}
	
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void signUpResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		if(!error)
		{
			inst.curSost=Sostoyanie.Logged;
		}
		
		inst.ResultText.text="You signed up!"+root.serialized;
	}
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void signInResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		if(!error)
		{
			inst.curSost=Sostoyanie.Logged;
		}
		inst.ResultText.text="You signed in!"+root.serialized;
	}
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void saveResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		inst.ResultText.text="You saved Player!"+root.serialized;
	}
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void deleteResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		inst.curSost=Sostoyanie.NotLoged;
		inst.ResultText.text="You deleted user!"+root.serialized;
	}
	
	
	//not supported now responses
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void setResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		inst.ResultText.text="setResponse!"+root.serialized;
	}
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void getResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		inst.ResultText.text="getResponse!"+root.serialized;
	}
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void pushResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		inst.ResultText.text="pushResponse!"+root.serialized;
	}
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void pullResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		inst.ResultText.text="pullResponse!"+root.serialized;
	}
	
	[MonoPInvokeCallback (typeof (SDKCorePluginCustomCallbackDelegate))]
	public static void usersfindResponse(string sign, string jsonstring,bool error){
		JSON root=new JSON();
		root.serialized=jsonstring;
		inst.ResultText.text="usersfindResponse!"+root.serialized;
	}
	
	public void sdkResponse(BaseCallbackInfo ininfo)
 	{		
		//здесь можно также обрабатывать все события
	}
}
