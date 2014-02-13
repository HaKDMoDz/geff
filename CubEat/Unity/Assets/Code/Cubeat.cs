using UnityEngine;
using System.Collections;
using Assets.Code;
using System.Collections.Generic;

public class Cubeat : MonoBehaviour
{
    public Map map;
    public double BPM;
    private double lastTime;
    Vector3 theForwardDirection;
    Quaternion _initialRotation;

    void Start()
    {
        map = new Map("Rapman", this);
        BPM = 240.0;

        //theForwardDirection = Camera.main.transform.TransformDirection(Vector3.forward);
        //theForwardDirection.Normalize();

        //Camera.main.transform.Translate(-theForwardDirection * (layer-3)*4);
        //Camera.main.transform.LookAt(Vector3.zero);
        ComputeCameraPosition();

        CreateGUI();
    }

    public void ComputeCameraPosition()
    {
        float halfHeight = map.Size / 2.0f;

        float halfFov = Camera.main.fieldOfView / 2.0f;

        float fovTan = Mathf.Tan(halfFov * Mathf.Deg2Rad);

        theForwardDirection = Camera.main.transform.TransformDirection(Vector3.forward);
        theForwardDirection.Normalize();
        //Camera.main.transform.Translate(-theForwardDirection * halfHeight / fovTan/10f);

        Camera.main.transform.position = new Vector3(1f, map.Size * 0.9f, -2.5f);

        //Debug.Log(halfHeight / fovTan);
        //Debug.Log(theForwardDirection);

        Camera.main.transform.LookAt(Vector3.zero);

        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(Camera.main.transform.rotation.eulerAngles.x, 0f, 0f));

        _initialRotation = Camera.main.transform.rotation;

    }

    public int numBeatsPerSegment = 16;
    public AudioClip[] clips = new AudioClip[2];
    private double nextEventTime;

    void Update()
    {
        double time = AudioSettings.dspTime;
        map.Update(time);
        //if (time - lastTime + 1.0F > Speed / 1000f)
        //{
        //    lastTime = time;
        //    map.Update(time);
        //    Debug.Log(time);
        //}

        //if (Application.platform == RuntimePlatform.Android)
        {
            this.transform.rotation = Quaternion.Euler(_initialRotation.eulerAngles + new Vector3(Input.acceleration.y, Input.acceleration.x, 0f) * 3f);
        }
    }

    UIButton btnMargeArrow;
    UISprite spriteMarge;
    bool margeCollapsed = true;

    private void CreateGUI()
    {
        UIToolkit ui = GameObject.Find("UIToolkit").GetComponent<UIToolkit>();

        float buttonSize = Screen.width * 0.06f;

        btnMargeArrow = UIButton.create(ui, "MargeArrow.png", "MargeArrow.png", 0, 0);
        btnMargeArrow.setSize(buttonSize, buttonSize);
        btnMargeArrow.positionFromTopLeft(0f, 0.01f);
        btnMargeArrow.onTouchUpInside += btnMargeArrow_onTouchUpInside;

        spriteMarge = ui.addSprite("Marge.png", 0, 0, 2);
        spriteMarge.setSize(0.01f * Screen.width, 1 * Screen.height);
        spriteMarge.positionFromTopLeft(0f, 0f);

        //btnValidation.onTouchUpInside += btnValidation_onTouchUpInside;

        //buttonSize = Screen.width * 0.04f;

        //UIButton btnSaveFileLocalAndServer = UIButton.create(ui, "SaveIcon.png", "SaveIcon.png", 0, 0, 0);
        //btnSaveFileLocalAndServer.setSize(buttonSize, buttonSize);
        //btnSaveFileLocalAndServer.positionFromTopRight(0.02f, 0.15f);
        ////btnSaveFileLocalAndServer.onTouchUpInside += btnSaveFileLocalAndServer_onTouchUpInside;

        //UIButton btnSaveFileLocal = UIButton.create(ui, "SaveIcon.png", "SaveIcon.png", 0, 0, 0);
        //btnSaveFileLocal.setSize(buttonSize, buttonSize);
        //btnSaveFileLocal.positionFromTopRight(0.02f, 0.25f);
        //btnSaveFileLocal.color = Color.green;
        ////btnSaveFileLocal.onTouchUpInside += btnSaveFileLocal_onTouchUpInside;

        //UISprite spriteHeaderHolo = ui.addSprite("HeaderHolo.png", (int)(0.6f * Screen.width), (int)(Repository.Instance.HeaderHeight * Screen.height), 1);
        //spriteHeaderHolo.setSize(0.4f * Screen.width, Repository.Instance.HeaderHeight * Screen.height);
        //spriteHeaderHolo.positionFromTopRight(0f, 0f);
        //spriteHeaderHolo.color = new Color(0f, 0.31f, 0.4f);

        //UISprite spriteBackground = ui.addSprite("background.jpg", 0, 0, 2);
        //spriteBackground.positionFromTopLeft(0f, 0f);
        //spriteBackground.setSize(Screen.width, Screen.height);
        //spriteBackground.color = new Color(0f, 0.2f, 0.27f);

        //_eyeCursor = uiCursor.addSprite("eye_cursor.png", 0, 0);
        //float sizeCursor = Repository.Instance.HeaderHeight * Screen.height * 2f;
        //_eyeCursor.setSize(sizeCursor, sizeCursor);

    }

    void btnMargeArrow_onTouchUpInside(UIButton obj)
    {
        margeCollapsed = !margeCollapsed;

        if (margeCollapsed)
        {
            btnMargeArrow.positionFromTopLeft(0f, 0.01f);
            spriteMarge.setSize(0.01f * Screen.width, 1 * Screen.height);
        }
        else
        {
            btnMargeArrow.positionFromTopLeft(0f, 0.5f);
            spriteMarge.setSize(0.5f * Screen.width, 1 * Screen.height);
        }
    }

    internal static void ComputePartition(Cube cubeTouched)
    {
        throw new System.NotImplementedException();
    }
}
