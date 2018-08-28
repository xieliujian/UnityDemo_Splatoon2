using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class xEnumDefine//枚举定义类
{
    public enum TeamFlag
    {
        Invalid = -1,
        Team_0 = 0,
        Team_1 = 1,
    }
}

public class CustomColorData_0
{
    public static Color Color_Team_0 = new Color(93 / 255.0f, 25 / 255.0f, 231 / 255.0f);
    public static Color Color_Team_1 = new Color(233 / 255.0f, 106 / 255.0f, 173 / 255.0f);
}

public class CameraRender : MonoBehaviour
{
    public static CameraRender Instance;

    public class HitObj
    {
        public GameObject obj;
        public Vector2 texcoord;
        public Color color;
        public int coltype;
        public string decalname = "Gun_H100";
        public float decalwidth = 1.0f;
        public float decalheight = 1.0f;
        public float decalrot = 0.0f;
    };

    public List<HitObj> M_StaticObjArray
    {
        set
        {
            mStaticObjArray = value;
        }

        get
        {
            return mStaticObjArray;
        }
    }

    private List<HitObj> mStaticObjArray = new List<HitObj>();

    private Material mQuadMaterial;

    public bool M_OnceRender
    {
        get
        {
            return mOnceRender;
        }
        set
        {
            mOnceRender = value;
        }
    }

    private bool mOnceRender = false;
    public  bool M_OpenOnceRender
    {
        get
        {
            return mOpenOnceRender;
        }
        set
        {
            mOpenOnceRender=value;
        }
    }
    private bool mOpenOnceRender = false;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2 ||
            SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
        {
            mQuadMaterial = new Material(Shader.Find("SpraySoldier/Function/MobileDrawDecal"));
        }
        else
        {
            mQuadMaterial = new Material(Shader.Find("SpraySoldier/Function/DrawDecal"));
        }
    }

	// Update is called once per frame
	void Update ()
    {

	}

    void OnPostRender()
    {
        for (int i = 0; i < mStaticObjArray.Count; i++ )
        {
            HitObj obj = mStaticObjArray[i];
            if (obj == null)
                continue;

            StaticObj_Render render = obj.obj.GetComponent<StaticObj_Render>();
            if (render == null)
                continue;

            render.Render(obj.texcoord, mQuadMaterial, obj.color, obj.coltype, obj.decalname, obj.decalwidth, obj.decalheight, obj.decalrot);
        }

        mStaticObjArray.Clear();
    }
}
