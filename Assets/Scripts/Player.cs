using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance = null;

    public Camera mCamera;

    public CharacterController mCharCtrl;

    public Transform mGunPos;

    private Animator mAnimator;

    private bool mUseTeam0 = true;

    #region 内置函数

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () 
    {
        mAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
        UpdatePos();

        if (Input.GetKeyDown(KeyCode.U))
        {
            mUseTeam0 = !mUseTeam0;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            mAnimator.SetBool("Shoot", true);
            CreatePaint();
        }
    }

    #endregion

    #region 函数

    private void UpdatePos()
    {
        float deltax = Input.GetAxis("Horizontal");
        float deltay = Input.GetAxis("Vertical");
        if (Mathf.Abs(deltax) <= 0.01f && Mathf.Abs(deltay) <= 0.01f)
        {
            mAnimator.SetBool("Speed", false);
            return;
        }

        Vector3 realdir = new Vector3(deltax, 0.0f, deltay);
        realdir = Quaternion.AngleAxis(mCamera.transform.eulerAngles.y, Vector3.up) * realdir;

        float angle = Vector3.Angle(transform.forward, realdir);
        realdir = Vector3.Slerp(transform.forward, realdir, Mathf.Clamp01(180 * Time.deltaTime * 5 / angle));
        transform.LookAt(transform.position + realdir);

        mCharCtrl.SimpleMove(realdir * 5);
        mAnimator.SetBool("Speed", true);
    }

    public void CreatePaint()
    {
        int coltype = mUseTeam0 ? (int)xEnumDefine.TeamFlag.Team_0 : (int)xEnumDefine.TeamFlag.Team_1;
        Color color = mUseTeam0 ? CustomColorData_0.Color_Team_0 : CustomColorData_0.Color_Team_1;

        RaycastHit rayhit;
        Ray ray = new Ray(mGunPos.position, -Vector3.up);
        if (Physics.Raycast(ray, out rayhit, 10.0f))
        {
            CameraRender.HitObj obj = new CameraRender.HitObj();
            obj.obj = rayhit.collider.gameObject;
            obj.texcoord = rayhit.textureCoord2;
            obj.coltype = coltype;
            obj.color = color;
            obj.decalname = "Gun_H100";
            obj.decalwidth = 2.5f;
            obj.decalheight = 2.5f;
            obj.decalrot = 0.0f;

            CameraRender.Instance.M_StaticObjArray.Add(obj);
        }
    }

    #endregion
}
