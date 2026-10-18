using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class PracticeTools
{
    public const string ScenePath = "Assets/Scenes/Level.unity";
    [MenuItem("Practice/Create base scene")]
    public static void CreateBase()
    {
        EditorSettings.serializationMode = SerializationMode.ForceText;
        UnityEditor.VersionControlSettings.mode = "Visible Meta Files";
        PlayerSettings.companyName = "Study";
        PlayerSettings.productName = "Crystal Lab - Git Practice";
        foreach (string folder in new[] { "Scenes", "Prefabs", "Materials" }) Directory.CreateDirectory("Assets/" + folder);
        AssetDatabase.Refresh();
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        Material ground = Material("Ground", new Color(.075f, .13f, .22f));
        Material cyan = Material("Player", new Color(.1f, .8f, .85f));
        Material gold = Material("Crystal", new Color(1f, .65f, .1f));
        Material border = Material("Border", new Color(.18f, .3f, .42f));
        Cube("Ground", new Vector3(0,-.25f,0), new Vector3(16,.5f,12), ground);
        Cube("Border North", new Vector3(0,.25f,6), new Vector3(16,.5f,.3f), border);
        Cube("Border South", new Vector3(0,.25f,-6), new Vector3(16,.5f,.3f), border);
        Cube("Border East", new Vector3(8,.25f,0), new Vector3(.3f,.5f,12), border);
        Cube("Border West", new Vector3(-8,.25f,0), new Vector3(.3f,.5f,12), border);
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name="Player"; player.transform.position = new Vector3(0,1,-3.5f);
        UnityEngine.Object.DestroyImmediate(player.GetComponent<Collider>());
        player.GetComponent<Renderer>().sharedMaterial=cyan;
        player.AddComponent<CharacterController>(); player.AddComponent<PlayerController>();
        GameObject pickup = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pickup.name="Pickup"; pickup.transform.localScale=Vector3.one * .75f;
        pickup.transform.rotation=Quaternion.Euler(0,45,35);
        pickup.GetComponent<Renderer>().sharedMaterial=gold;
        pickup.GetComponent<Collider>().isTrigger=true;
        pickup.AddComponent<PickupRotator>();
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(pickup,"Assets/Prefabs/Pickup.prefab");
        UnityEngine.Object.DestroyImmediate(pickup);
        var parent=new GameObject("Pickups");
        Vector3[] positions = {new Vector3(-4,1,1),new Vector3(0,1,3),new Vector3(4,1,1)};
        for (int i=0;i<positions.Length;i++) { var instance=(GameObject)PrefabUtility.InstantiatePrefab(prefab); instance.name="Crystal "+(i+1); instance.transform.SetParent(parent.transform); instance.transform.position=positions[i]; }
        new GameObject("LevelValidator").AddComponent<LevelValidator>();
        new GameObject("GameSession").AddComponent<GameSession>();
        var cam = new GameObject("Main Camera").AddComponent<Camera>(); cam.tag="MainCamera";
        cam.transform.position=new Vector3(0,14,-13); cam.transform.LookAt(Vector3.zero);
        cam.orthographic=true; cam.orthographicSize=9; cam.clearFlags=CameraClearFlags.SolidColor; cam.backgroundColor=new Color(.035f,.055f,.095f); cam.gameObject.AddComponent<AudioListener>();
        var light=new GameObject("Directional Light").AddComponent<Light>(); light.type=LightType.Directional; light.intensity=1.3f; light.transform.rotation=Quaternion.Euler(50,-30,0); light.shadows=LightShadows.Soft;
        RenderSettings.ambientLight=new Color(.55f,.6f,.7f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(),ScenePath);
        EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(ScenePath,true)};
        AssetDatabase.SaveAssets(); Debug.Log("[Practice] Base scene created.");
    }
    static Material Material(string name, Color color) { var m=new Material(Shader.Find("Standard")); m.color=color; AssetDatabase.CreateAsset(m,"Assets/Materials/"+name+".mat"); return m; }
    static void Cube(string name,Vector3 position,Vector3 scale,Material material) { var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.position=position;g.transform.localScale=scale;g.GetComponent<Renderer>().sharedMaterial=material; }
    [MenuItem("Practice/Validate final scene")]
    public static void ValidateFinal()
    {
        EditorSceneManager.OpenScene(ScenePath);
        var player=UnityEngine.Object.FindFirstObjectByType<PlayerController>();
        var pickups=UnityEngine.Object.FindObjectsByType<PickupRotator>(FindObjectsSortMode.None);
        if (player==null || player.moveSpeed!=6 || player.health!=100 || pickups.Length!=3) throw new Exception("Final player/count validation failed");
        foreach(var item in pickups) if(item.rotationSpeed!=90 || item.scoreValue!=25) throw new Exception("Final prefab validation failed");
        foreach(var go in UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)) if(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go)>0) throw new Exception("Missing script: "+go.name);
        Debug.Log("[Practice] FINAL VALIDATION PASSED: speed=6 health=100 rotation=90 score=25 pickups=3; no missing scripts.");
    }
}
