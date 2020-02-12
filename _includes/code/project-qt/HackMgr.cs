using System.Linq;
using UnityEngine;

public class HackMgr : MonoBehaviour
{
    //实例
    private static HackMgr _instance;

    //启用无限攻击力
    public static bool AtkActive
    {
        get { return PlayerPrefs.GetInt("xx_atk_active", 0) == 1; }
        set { PlayerPrefs.SetInt("xx_atk_active", value ? 1 : 0); }
    }

    //设置攻击力值
    public static float AtkValue
    {
        get { return PlayerPrefs.GetFloat("xx_atk_value", 9999); }
        set { PlayerPrefs.SetFloat("xx_atk_value", Mathf.Clamp(value, 0, int.MaxValue)); }
    }

    //启用生命不减
    public static bool HpActive
    {
        get { return PlayerPrefs.GetInt("xx_hp_active", 0) == 1; }
        set { PlayerPrefs.SetInt("xx_hp_active", value ? 1 : 0); }
    }

    //启用同色砖块
    public static bool BrickActive
    {
        get { return PlayerPrefs.GetInt("xx_brick_active", 0) == 1; }
        set { PlayerPrefs.SetInt("xx_brick_active", value ? 1 : 0); }
    }

    //启用VIP等级
    public static bool VipLevelActive
    {
        get { return PlayerPrefs.GetInt("xx_viplevel_active", 0) == 1; }
        set { PlayerPrefs.SetInt("xx_viplevel_active", value ? 1 : 0); }
    }

    //设置VIP等级
    public static int VipLevel
    {
        get { return PlayerPrefs.GetInt("xx_viplevel", 0); }
        set { PlayerPrefs.SetInt("xx_viplevel", value); }
    }

    //直接结束游戏
    public static bool GameClear
    {
        get { return PlayerPrefs.GetInt("xx_gameclear_active", 0) == 1; }
        set { PlayerPrefs.SetInt("xx_gameclear_active", value ? 1 : 0); }
    }

    //实例化
    public static HackMgr GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("HackMgr").AddComponent<HackMgr>();
            GameObject.DontDestroyOnLoad(_instance.gameObject);
        }
        return _instance;
    }


    private bool openMenu = false;
    private GUISkin skin;
    private Texture2D normalTex;
    private Texture2D onTex;
    void OnGUI()
    {
        //初始化菜单样式
        if (skin == null)
        {
            skin = GameObject.Instantiate(GUI.skin);
            skin.button.fontSize = skin.toggle.fontSize = skin.textField.fontSize = 50;
            //skin.toggle.normal.background = skin.toggle.hover.background = skin.toggle.active.background = ScaleTex(GUI.skin.toggle.normal.background, 2);
            //skin.toggle.onNormal.background = skin.toggle.onHover.background = skin.toggle.onActive.background = ScaleTex(GUI.skin.toggle.onNormal.background, 2);
            //skin.toggle.border.left = 28;
            //skin.toggle.border.top = 28;
            normalTex = new Texture2D(2, 2);
            normalTex.SetPixels(Enumerable.Repeat(Color.gray, normalTex.width * normalTex.height).ToArray());
            normalTex.Apply();
            onTex = new Texture2D(2, 2);
            onTex.SetPixels(Enumerable.Repeat(Color.green, onTex.width * onTex.height).ToArray());
            onTex.Apply();

            skin.toggle.normal.textColor = Color.black;
            skin.toggle.onNormal.textColor = Color.green;

            Vector2 contentOffset = skin.toggle.contentOffset;
            contentOffset.x = 50;
            skin.toggle.contentOffset = contentOffset;
        }


        //应用菜单样式
        var skin_Copy = GUI.skin;
        GUI.skin = skin;

        //绘制区域
        GUILayout.BeginArea(new Rect(180, 20, 600, Screen.height));
        if (GUILayout.Button("Hack Menu"))
        {
            openMenu = !openMenu;
        }


        //当按钮打开，绘制内容
        if (openMenu)
        {
            GUILayout.BeginVertical(skin.box);
            DrawAtk();//绘制攻击力修改
            DrawHP();//绘制生命修改
            DrawBrick();//绘制砖块修改
            DrawVIP();//绘制会员修改
            DrawGameClear();//绘制直接通关

            GUILayout.EndVertical();

            DrawAbout();//绘制网站关于
        }
        GUILayout.EndArea();

        //还原样式
        GUI.skin = skin_Copy;
    }

    /// <summary>
    /// 绘制网站关于
    /// </summary>
    private void DrawAbout()
    {
        if (GUILayout.Button("About"))
        {
            Application.OpenURL("https://www.troy-web.top");
        }
    }

    /// <summary>
    /// 绘制攻击力修改
    /// </summary>
    void DrawAtk()
    {
        GUILayout.BeginHorizontal();
        AtkActive = DrawToggle(AtkActive, "ATK");
        string atkstr = GUILayout.TextField(AtkValue.ToString(), 10);
        try
        {
            AtkValue = int.Parse(atkstr);
        }
        catch
        {
        }

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 绘制生命修改
    /// </summary>
    void DrawHP()
    {
        GUILayout.BeginHorizontal();
        HpActive = DrawToggle(HpActive, "HP");
        GUILayout.EndHorizontal();
    }
    /// <summary> 
    /// 绘制直接通关
    /// </summary>
    void DrawGameClear()
    {
        GUILayout.BeginHorizontal();
        GameClear = DrawToggle(GameClear, "Sweep");

        GUILayout.EndHorizontal();
    }
    /// <summary>
    /// 绘制砖块修改
    /// </summary>
    void DrawBrick()
    {
        GUILayout.BeginHorizontal();
        BrickActive = DrawToggle(BrickActive, "Brick");

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 绘制会员修改
    /// </summary>
    void DrawVIP()
    {
        GUILayout.BeginHorizontal();
        VipLevelActive = DrawToggle(VipLevelActive, "VIP");
        string vipstr = GUILayout.TextField(VipLevel.ToString(), 2);
        try
        {
            VipLevel = int.Parse(vipstr);
        }
        catch
        {
        }
        VipLevel = Mathf.Clamp(VipLevel, 0, 12);
        if (VipLevelActive)
        {
            if (MainGameController.RunningGame != null)
            {
                if (MainGameController.RunningGame.playerInfo != null)
                {
                    MainGameController.RunningGame.playerInfo.progressionLevel = VipLevel + 1;
                }
            }
        }

        GUILayout.EndHorizontal();
    }

    //自定义绘制Toggle
    bool DrawToggle(bool value, string name)
    {
        value = GUILayout.Toggle(value, name);
        Rect rect = GUILayoutUtility.GetLastRect();
        rect.height--;
        rect.width = rect.height;
        GUI.DrawTexture(rect, value ? onTex : normalTex, ScaleMode.ScaleAndCrop);
        return value;
    }
    //工具方法。缩放图片大小
    public static Texture2D ScaleTex(Texture2D texorg, float scale)
    {
        Texture2D tex = new Texture2D(texorg.width, texorg.height, texorg.format, false);
        Graphics.CopyTexture(texorg, tex);
        tex.wrapMode = TextureWrapMode.Clamp;
        Texture2D newTex = new Texture2D((int)(tex.width * scale), (int)(tex.height * scale));
        for (int x = 0; x < newTex.width; x++)
        {
            for (int y = 0; y < newTex.height; y++)
            {
                Color c = tex.GetPixelBilinear(x * 1f / (newTex.width), y * 1f / (newTex.height));
                newTex.SetPixel(x, y, c);
            }
        }

        newTex.wrapMode = TextureWrapMode.Clamp;
        newTex.Apply();
        GameObject.Destroy(tex);
        return newTex;
    }
}
