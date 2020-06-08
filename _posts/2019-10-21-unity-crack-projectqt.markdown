---
layout: post
title: Unity 手游逆向工程
date:  2019-10-21 22:28:51 +0800
categories: Unity
tags: 
img: https://i.loli.net/2019/10/22/zPVqNFsTH64gSWb.png
author: oOtroyOo
lastupdate : 2020-05-19 00:00:00 +0800
---

# 说明文档
- 适配英文版 Ver 6.0
- 首次安装需要卸载原版App
- 首次语言选择会退出一次App
- 如果官方更新了App，就得重新破解，如果只是更新资源是没问题的
- 会不会封号？ 我也不知道~~
- 这里下载的安装包，包含CG解锁，**不包含 非平衡修改**，如有需要自己添加
- 活动限定CG动画，目前仅有Erica(双枪女仆)、Clara(化学老师)、Flora(小号手)，下文提到的也就是这几个人
  
##  [下载安装包(Github)](https://github.com/oOtroyOo/blog-mdui/releases)
##  [下载安装包(Mega)](https://mega.nz/folder/imYlmADZ#yygn2yqiFlDNxg0Vnvi8Dw)
 
---
<div style="font-size:30px; color:red; line-height:32px">
<p>关于教程和安装包</p>
<p>我没有在发过任何视频，或付费销售！</p>
<p>如果想要充电，去给我充电吧。网站底部有我的B站链接。</p>
</div>

# 工程运行环境

## 游戏结构
- 必须是 Unity 引擎，Mono打包的方式才可以使用此教程破解，即安装包或文件目录有`Assembly-CSharp.dll`。
  如果采用 IL2CPP打包方式则不适用，即目录有`libil2cpp.so`

## 必备软件与用途

1. dnSpy （反编译）
2. 好压 （压缩包的操作，或同类软件）
3. Java SDK （java，安卓必备）
4. Android签名文件 （用于apk包签名）

## 可选备用软件

1. Visual Studio  （代码编写工具 ，但是并不用做编译)
2. Git  （版本差异管理）
3. CodeCompare  （版本差异对比）
   
# 开始工作

## 准备安装包
- 本片教程安装包截图为v3.0， 文字补充~v6.0
- 右键用好压打开
   解压`assets\bin\Data\Managed`
  
  ![批注 2019-10-16 012408](https://i.loli.net/2019/10/16/QlCarOkHoLh1IsY.png)
- 使用dnSpy打开`Assembly-CSharp.dll`

- 安卓签名文件
  如果你有准备好的签名文件，可以跳过下载
  [签名文件下载](http://www.greenxf.com/soft/95740.html)
- (可选步奏) 从dnSpy导出 sln工程，建立git库，使用Visual Studio编写代码
  
  ![批注 2019-10-16 012723](https://i.loli.net/2019/10/16/pBbm1DchtE4XaKC.png)

## 反编代码
   这一节为简介，稍后列出详细的代码段
- 使用dnSpy打开`Assembly-CSharp.dll`
- 找到所需要的代码
- 每次反编译或者重新编译，**可能出现代码有些许差别**，视情况进行编写
- 右键 编辑方法(C#)、编辑类(C#)、添加类(C#)、添加类成员(C#)
  
  ![批注 2019-10-22 231556](https://i.loli.net/2019/10/22/zPVqNFsTH64gSWb.png)

- 修改完毕之后 编译
  
  ![批注 2019-10-22 231738](https://i.loli.net/2019/10/22/GPcraJjAfi4uFgK.png)

- 保存dll模块

  ![批注 2019-10-22 231821](https://i.loli.net/2019/10/22/pqV4iCJWAhek2Gu.png)

- 如果出现大量报错，尝试保存-关闭-再打开
  
## 重新打包
- 用好压打开 apk
- 删除 `META-INF`
  
  ![批注 2019-10-16 013818](https://i.loli.net/2019/10/16/aY45Vx8qM9lRoN6.png)
- 添加并覆盖`Assembly-CSharp.dll`
  
  ![批注 2019-10-16 013938](https://i.loli.net/2019/10/16/BFWqERNDMefgGho.png)
- 准备好签名文件
    - 此处需要安装Java SDK
    - 如果你有准备好的签名文件，请使用适合你准备好的签名命令
    - 如果你从前文下载的签名，可以使用签名工具
    - 也可以参考以下命令 （自己看清楚文件名）

``` bash
  java -jar "signapk.jar" testkey.x509.pem testkey.pk8 project-qt_268.apk project-qt_268_sign.apk
```

<div style="font-size:30px; color:red; line-height:32px">
<p>关于教程和安装包</p>
<p>我没有在发过任何视频，或付费销售！</p>
<p>如果想要充电，去给我充电吧。网站底部有我的B站链接。</p>
</div>

# 代码片段
- 每次反编译都**可能出现代码有些许差别**，如果有区别，视情况编写
- 如果出现`Debug`类型出错，就修改为`UnityEngine.Debug` ，下文中不再提到
## 开启伴侣动画
- `UserCompanionRecord`
  - 新增构造方法
  
``` csharp
  //此处更改需要及时保存模块，下文有一条更改会引用这里
  public UserCompanionRecord(string id)
  {
    this.id = id;
    this.level = new ReactiveProperty<int>(1);
    this.loyalty = new ReactiveProperty<int>(0);
    this.skillPoint = new ReactiveProperty<int>(0);
    this.skillLevels = new ReactiveProperty<Dictionary<string, int>>(new Dictionary<string, int>());
    this.animationStatus = new ReactiveProperty<Dictionary<int, Dictionary<string, int>>>(new Dictionary<int, Dictionary<string, int>>());
  }
```
  - 修改`UserCompanionRecord(JSONNode json)`和`Update(JSONNode json)`2处Dictionary-Value分配
  
``` csharp
  Dictionary<string, int> value = keyValuePair.Value.JSONDict.ToDictionary((KeyValuePair<string, JSONNode> kvp) => kvp.Key, (KeyValuePair<string, JSONNode> kvp) => kvp.Value.AsInt);
  》》》
   Dictionary<string, int> value = keyValuePair.Value.JSONDict.ToDictionary((KeyValuePair<string, JSONNode> kvp) => kvp.Key, (KeyValuePair<string, JSONNode> kvp) => 1);
```
- `EpisodeMainPage`


``` csharp
  private void SetupAnimationElement(string companionId)

  >>>

  private void SetupAnimationElement(string companionId)
  {
    CompanionSetting companionSetting = CompanionManager.Instance.CompanionSettings[companionId];
    if (CompanionManager.Instance.CompanionAnimationSetting.ContainsKey(companionId))
    {
      Dictionary<int, List<CompanionAnimationSetting>> dictionary = CompanionManager.Instance.CompanionAnimationSetting[companionId];
      foreach (KeyValuePair<int, List<CompanionAnimationSetting>> keyValuePair in dictionary)
      {
        foreach (CompanionAnimationSetting animationSetting in keyValuePair.Value)
          {
            this.episodeElementList[animationSetting.ToBtnIndex()].SetupEpisode(companionSetting, animationSetting, CompanionAnimationSetting.AnimationStatus.Collected);
          }
        }
    }
  }
```
- `CompanionSettingHelper`


``` csharp
  public static CompanionAnimationSetting.AnimationStatus GetEpisodeAnimationStatus(string companionId, int episodeId, int subEpisodeId)

  >>>
  public static CompanionAnimationSetting.AnimationStatus GetEpisodeAnimationStatus(string companionId, int episodeId, int subEpisodeId)
  {
      return CompanionAnimationSetting.AnimationStatus.Collected;  
  }
```
- `CompanionBanner`
  （此修改可能会使下次反编出现较大差异）

``` csharp
  public void importCompanionData(CompanionSetting companionSetting, bool displayUnlock, CompanionBossListPage companionBossListPage)
  {
    .............
    ///在末尾添加
    Transform vid = unlockedContent.transform.Find("VideoBtn");
    if (vid != null)
    {
        vid.SetParent(transform, true);
    }
  }

```

## 切换语言
- `Localization`


``` csharp
  private static string Get(Localization.Language pack, string key)
  {
    key = key.Trim();
    string result = key;
    Dictionary<string, int> keys = Localization.Share.Keys;
    if (pack != null && keys.ContainsKey(key) && !pack.TryGetValue(keys[key], out result))
    {
      result = key;
    }
    return result;
  }
  》》》

  private static string Get(Localization.Language pack, string key)
  {
    key = key.Trim();
    string result = key;
    Dictionary<string, int> keys = Localization.Share.Keys;
    if (pack != null)
    {
        if (keys.ContainsKey(key) && !pack.TryGetValue(keys[key], out result))
        {
            result = key;
            if (pack.Key != "en")
            {
                result = Get(Localization.Share.Packs["en"], key);
            }
        }
    }    
    return result;
  }
```
- `LoginManager`
（此修改可能会使下次反编出现较大差异，请一次修改完成）

``` csharp
  public IEnumerator CheckSystem()
  {
+   Config.LangAbleChange = true;
    bool wait = true;
    bool anyErr = false;
    if (this.checkingMessage != null)
    {
      this.checkingMessage.gameObject.SetActive(true);
    }
//省略部分内容
//////////////
//因为不明原因，首次选择中文，会造成无限下载资源，暂时没有找到完美的解决方法。此处修改即强制退出App

    Config.CurrentSetting[ConfigKey.kLanguage] = new JSONData(lang);
    Config.saveCurrentSetting();
    Config.Language = lang;
    Localization.language = lang;
+    UnityEngine.Application.Quit();

```


## 开启所有人物的事件相簿按钮
仅有开发活动限定CG的角色 可以点击按钮，其他角色都为灰色
- `PetDetailPage_v2`

``` csharp
  UILabel label = this.btnL.transform.FindHiddenComponent(new string[]
  {
    "Label"
  });
  UILocalize.Set(label, "pet_buddy", new object[0]);
  if (this.petShowing == null)
  {
    this.CreatePetBigBG(this.myPet.pet_id);
    yield return base.StartCoroutine(this.CreatePetPanel(this.myPet.pet_id));
    this.petID = this.myPet.pet_id;
    this.story.Load(this.petID);
    this.voice.Load(this.petID);
    if (this.mode == PetDetailPage_v2.DisplayMode.SummonedPet || this.mode == PetDetailPage_v2.DisplayMode.Explore)
    {
      this.eventAlbum.Load(this.myPet);
    }
  }
  
  》》》
  
  UILabel label = this.btnL.transform.FindHiddenComponent<UILabel>(new string[]
  {
    "Label"
  });
  UILocalize.Set(label, "pet_buddy", new object[0]);
  if (this.petShowing == null)
  {
    this.CreatePetBigBG(this.myPet.pet_id);
    yield return base.StartCoroutine(this.CreatePetPanel(this.myPet.pet_id));
    this.petID = this.myPet.pet_id;
    this.story.Load(this.petID);
    this.voice.Load(this.petID);
    if (true)
    {
      this.eventAlbum.Load(this.myPet);
    }
  }
```

## 解锁人物5星卡片

- `PetDetailPage_v2`

``` csharp
  btn_cardView.gameObject.SetActive(****)
  》》》全部更改为 
  btn_cardView.gameObject.SetActive(true)
```

- `PetDetailFullViewButton`

``` csharp

    bool flag = i > star;

  >>>

    bool flag = false;

```


## 收集册
- `AlbumPage`

``` csharp
  catch (Exception)
  {
      this.petSerializedData = null;
  }
 >>>
  catch (Exception)
  {
    this.petSerializedData = petConfig.CreateRawPetData(1, 4, 0);
  }
```

``` csharp
  //注 ：此处需要先修改上文 UserCompanionRecord 构造方法
  public void OnMonstersButtonClick()
  //////////
    foreach (KeyValuePair<string, CompanionSetting> keyValuePair in CompanionManager.Instance.CompanionSettings)
    {
        object item = string.Empty;
        this.totalNum++;
        foreach (KeyValuePair<string, UserCompanionRecord> keyValuePair2 in CompanionManager.Instance.UserCompanions)
        {
            if (keyValuePair2.Value.id == keyValuePair.Value.id)
            {
                this.unlockNum++;
                item = keyValuePair2.Value;
                break;
            }
        }
+    
        if (item == string.Empty)
        {
          item = new UserCompanionRecord(keyValuePair.Value.id);
        }
 +    
        list.Add(item);
    }
 ////////// 
   
```

## 解锁活动CG 
  目前仅有部分角色有活动CG
 - `EventRewardPopup`

``` csharp
  this.scenesLockBlock[num].SetActive(EventManager.CurrentEventLevel() < levelSetting[i].level);        
  >>>
  this.scenesLockBlock[num].SetActive(false);
```
- `EventAlbumPage`

``` csharp
  private bool FindCgId(CharacterBaseInfo_v2 serialData, string cgId)
  {
    if (serialData == null || serialData.cgArray == null)
    {
      return false;
    }
    for (int i = 0; i < serialData.cgArray.Length; i++)
    {
      if (serialData.cgArray[i].Equals(cgId))
      {
        return true;
      }
    }
    return false;
  }        

  >>>
  private bool FindCgId(CharacterBaseInfo_v2 serialData, string cgId)
  {
    return true;
  }
``` 

## 教育活动，可切换造型

- EventMainPage

添加成员
``` csharp
    private static int boosterLevel;

    private void RandIntimacy()
    {
        boosterLevel = boosterLevel + 1;
        if (boosterLevel > EventManager.CurrentEventSetting().intimacyLevelSetting.Count - 1)
        {
            boosterLevel = 1;
        }
        this.intimacyLevel = boosterLevel;
        EventManager.Instance.UserEventData.boosterLevel = boosterLevel;
    }
```

``` csharp
  public bool SetData()
  {
+
      RandIntimacy();
+
      this.eventId = EventManager.CurrentEventId();
      if (!EventManager.Instance.EventSettings.ContainsKey(this.eventId))
      {
          return false;
      }
```
``` csharp
    public override void WillBeBack()
    {
        base.WillBeBack();
+
        RandIntimacy();
+
        NavBar.Instance.backButtonForcePosition = true;
        this.startupUIMode = NavBar.NavBarMode._3HiddenAllBar;
        if (this.petEventAnimationProxy != null)
        {
            this.petEventAnimationProxy.SetAnimation(this.GetAnimationName("idle", false), true);
        }
    }
```

## Clara (化学老师)活动，可切换造型

- `EventMoraManager`

添加成员： 

``` csharp
  int hack =0;
```

``` csharp
  public void UpdateUserEventData(JSONNode node)
  {
      this.userData = new PlayerEventMoraData(node);
      this.hack++;
      this.userData.intimacyTier = this.hack %CurrentEventSetting().eventMoraIntimaciesSettings.Count + 1;
  }
```
## Flora(小号手)活动：WhatSex，解锁对话框

- `WhatSexManager`
  
``` csharp
	public bool GetSubTreadUnlock(string keyID)
  {
      return true;
  }
```

- `WhatSexPage`
  
``` csharp
    public void SetupMessage(WhatSexSubThreadSetting whatSexSubThreadSetting)
    {
+        
        List<string> user_record = WhatSexManager.Instance.whatSexUserData
            .user_records[whatSexSubThreadSetting.event_id.ToString()]
            [whatSexSubThreadSetting.thread_id.ToString()]
            [whatSexSubThreadSetting.sub_thread_id.ToString()]
            .message_record;
        foreach (var kv in whatSexSubThreadSetting.message_json)
        {
            if (kv.Value.post_message.Count > 0)
            {
                foreach (int k in kv.Value.post_message)
                {
                    WhatSexMessageJson message = whatSexSubThreadSetting.message_json[k.ToString()];
                    if (message.message_type == 2 || message.message_type == 3)
                    {
                        if (!user_record.Contains(kv.Key))
                            user_record.Add(kv.Key);
                    }
                }
            }
            if (kv.Value.message_type == 2 || kv.Value.message_type == 3)
            {
                if (!user_record.Contains(kv.Key))
                    user_record.Add(kv.Key);
            }
        }
+        
        this.curWhatSexSubThread = whatSexSubThreadSetting;
```

<p></p>

# 非平衡修改
**以下修改有不平衡内容，未加入安装包中，自己去破解**

- 关于战斗的修改
  - 可用于剧情关卡
  - 可用于打塔
  - 可用于活动关卡
  - pvp竞技场不建议使用。使用会出现奇怪的问题，胜负并不能控制，回放不正确等等
  
## 新增功能菜单类 
(新增内容需要及时保存模块)

- `HackMgr` 

``` csharp
{% include code/project-qt/HackMgr.cs %}
```

- 需要找一个地方启用脚本。
  可以写在`LoginManager`

``` csharp
    private void Awake()
    {
+        HackMgr.GetInstance();
```


## 无限攻击力
  
- `Pet`  

``` csharp
  public float GetAttack(bool withBuff = true, BlockType targetType = BlockType.ANY_COLOR)
  {
+
   if (HackMgr.AtkActive && !this.IsEnemy)
      {
          return (float)HackMgr.AtkValue;
      }
+    
    float num = this.GetAbility(withBuff, null).atk;
    if (this.core != null)
    {

```


## 无限生命
  
- `BattleMode`  

``` csharp
  public virtual int SetPlayerHP(int hp, bool couldOver)
  {
    int playerHPMax = this.GetPlayerHPMax(true);
+
   if (HackMgr.HpActive)
    {
        hp = playerHPMax;
    }
+    
    if (hp < 0)
    {
      hp = 0;

```

``` csharp
  //这一条的意思是让敌人造成的伤害为0。
  //如果仅靠自身生命不减，貌似v5.0在打塔结束计算的时候他仍然会计算失败。
  //尚不清楚有什么不良影响
  protected DamageContainer calculateFinalDamage(CurrentRoundAttack currentRoundAttack, Pet targetPet, Pet sender)
  {
///////////
+
         if (HackMgr.HpActive || HackMgr.GameClear)
          {
              if (sender.IsEnemy && targetPet.IsPlayer)
              {
                  num = 0;
                  num2 = 0;
              }
          }
+          
          return new DamageContainer((float)Mathf.FloorToInt(num), flag2, num3, flag3, num4, num2);
      }
      return DamageContainer.Empty();
  }
```

## 同色砖块

  全场的颜色砖块，都将刷新为队长相同的属性
  
- `CoreGameSystem`

``` csharp
  protected virtual Block pushNewItem(int r, int x, bool checkEmpty = true)
  {
    if (r < 0 || r >= this._blocksPrefab.Length)
    {
        r = 0;
    }
+
    if (HackMgr.BrickActive)
    {
        if (r < (int)BlockType.COLOR_DARK)
        {
            r = (int)this._playerPets[0].GetProperty(true);
            r--;
        }
    }
+    
    GameObject gameObject = null;

    /////////

```

## VIP等级

**这只能看着好玩，如果想点击任何会员功能的话，是会提示错误的** 

- `PlayerData`
  
``` csharp
  public virtual void SetPlayerInfo(JSONNode dataNode)
    /////////

    this.progressionLevel = dataNode["progression_lv"].AsInt;
+
    if (HackMgr.VipLevelActive)
    {
        this.progressionLevel = HackMgr.VipLevel + 1;
    }
+    
    /////////
```

## 立即通关

- `BattleMode`

``` csharp
  public virtual bool IsAllWaveClear()
  {
+
      if (HackMgr.GameClear)
      {
          return true;
      }
+      
      return this.wave > this.waveTotal;
  }
```

- `CoreGameSystem`

``` csharp
  public bool isAllEnemyDead(bool checkAgain = false)
  {
+
      if (HackMgr.GameClear)
      {
          return true;
      }
+
      if (this.reviving)
      {
          return false;
      }
   /////////
```

``` csharp
  protected virtual void battleEnd(Action<JSONNode> callback, string cause = "")
  /////////
    if (this.pauseQuit)
    {
        flag2 = false;
    }
+
    else if (HackMgr.GameClear)
    {
        flag2 = true;
    }
+    
    else if (this.IsPVPTower)
    {
        flag2 = (this.modePVPTower.GetPlayerHP(false) > 0 && this.modePVPTower.GetPlayerHP(false) > this.modePVPTower.GetEnemyHP() && !this.currentMode.IsOverRoundLimit());
    }
  /////////
```
``` csharp
	protected virtual IEnumerator StartBattle(float delay = 0f)
	///////////////////最后
+
    if (HackMgr.GameClear)
    {
        yield return StartCoroutine(GameClear(true));
    }
+    
    yield break;
  }
```

## 技能无CD

- `Pet`

``` csharp
    public int GetSkillCD()
    {
+
        if (HackMgr.SkillCD)
        {
            if (this.IsPlayer)
            {
                SetSkillCD(0);
            }
        }
+
          return this.runtimeSkill.GetSkillCD();
    }
```