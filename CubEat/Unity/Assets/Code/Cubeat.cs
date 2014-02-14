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
    public UIToolkit textManager;

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
    List<UIHorizontalLayout> listHLayout = new List<UIHorizontalLayout>();

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

        //--- Conteneur
        var scrollable = new UIScrollableVerticalLayout(10);

        scrollable.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;
        scrollable.position = new Vector3(Screen.width * 0.05f, -Screen.height * 0.1f, 0);
        var width = UI.scaleFactor * 150;
        scrollable.setSize(width, Screen.height * 0.8f);
        //---

        //UIGridLayout grid = new UIGridLayout(4, map.Layers.Count, 0);
        //grid.edgeInsets = new UIEdgeInsets(0);
        //grid.alignMode = UIAbstractContainer.UIContainerAlignMode.Left;
        //grid.layoutType = UIAbstractContainer.UILayoutType.AbsoluteLayout
        int i = 0;
        foreach (Layer layer in map.Layers)
        {
            UIHorizontalLayout hLayout = new UIHorizontalLayout(0);

            listHLayout.Add(hLayout);

            //int posY = 10 + i * 20;
            //int margeX = 10;
            //int deltaX = 10;
            //int spaceX = 5;

            //UIText text = new UIText(textManager, "prototype", "prototype.png");

            UIButton btnBrowse = UIButton.create(ui, "Browse.png", "Browse.png", 0, 0, 2);
            UIButton btnMute = UIButton.create(ui, "Mute.png", "Mute.png", 0, 0, 2);
            UIButton btnSolo = UIButton.create(ui, "Solo.png", "Solo.png", 0, 0, 2);
            UIButton btnQuarter = UIButton.create(ui, "Quarter.png", "Quarter.png", 0, 0, 2);
            UIButton btnHalf = UIButton.create(ui, "Half.png", "Half.png", 0, 0, 2);
            UIButton btnNormal = UIButton.create(ui, "Normal.png", "Double.png", 0, 0, 2);
            UIButton btnDouble = UIButton.create(ui, "Double.png", "Double.png", 0, 0, 2);
            UIButton btnFourth = UIButton.create(ui, "Fourth.png", "Fourth.png", 0, 0, 2);


            btnBrowse.setSize(buttonSize, buttonSize);
            btnMute.setSize(buttonSize, buttonSize);
            btnSolo.setSize(buttonSize, buttonSize);
            btnQuarter.setSize(buttonSize, buttonSize);
            btnHalf.setSize(buttonSize, buttonSize);
            btnNormal.setSize(buttonSize, buttonSize);
            btnDouble.setSize(buttonSize, buttonSize);
            btnFourth.setSize(buttonSize, buttonSize);

            hLayout.addChild(btnBrowse, btnMute, btnSolo, btnQuarter, btnHalf, btnNormal, btnDouble, btnFourth);

            hLayout.positionFromTopLeft(i * 0.1f, -0.5f);

            i++;
        }
    }

    void btnMargeArrow_onTouchUpInside(UIButton obj)
    {
        margeCollapsed = !margeCollapsed;

        if (margeCollapsed)
        {
            btnMargeArrow.positionTo(0.5f, new Vector3(0.01f * Screen.width, 0.01f * Screen.height), Easing.Sinusoidal.easeOut);
            spriteMarge.scaleTo(0.5f, new Vector3(1f, 1f, 1f), Easing.Sinusoidal.easeOut);

            foreach (UIHorizontalLayout hLayout in listHLayout)
            {
                hLayout.positionTo(0.5f, new Vector3(-Screen.width / 2, hLayout.localPosition.y, -20), Easing.Sinusoidal.easeOut);
            }
        }
        else
        {
            btnMargeArrow.positionTo(0.5f, new Vector3(0.5f * Screen.width, 0.01f * Screen.height), Easing.Sinusoidal.easeIn);
            spriteMarge.scaleTo(0.5f, new Vector3(50f, 1f, 1f), Easing.Sinusoidal.easeIn);

            foreach (UIHorizontalLayout hLayout in listHLayout)
            {
                hLayout.positionTo(0.5f, new Vector3(0.01f * Screen.width, hLayout.localPosition.y, -20), Easing.Sinusoidal.easeIn);
            }
        }
    }

    internal static void ComputePartition(Cube cubeTouched)
    {
        throw new System.NotImplementedException();
    }
}
