---
layout: post
title: Unity 手游逆向工程
date:  2019-10-21 22:28:51 +0800
categories: Unity
tags: 
img: https://i.loli.net/2019/10/22/zPVqNFsTH64gSWb.png
author: oOtroyOo
lastupdate : 2019-10-23 22:00:00 +0800
---
# 说明文档


##  [安装包下载](https://github.com/oOtroyOo/blog-mdui/releases)
##  [安装包下载](https://github.com/oOtroyOo/blog-mdui/releases)
##  [安装包下载](https://github.com/oOtroyOo/blog-mdui/releases)
##  [安装包下载](https://github.com/oOtroyOo/blog-mdui/releases)
##  [安装包下载](https://github.com/oOtroyOo/blog-mdui/releases)
##  [安装包下载](https://github.com/oOtroyOo/blog-mdui/releases)
- 如果打不开下载链接，请长按/右键 - 新标签打开/新窗口打开
- 首次安装需要卸载原版App
- 首次语言选择会退出一次App
- 如果官方更新了App，就得重新破解，如果只是更新资源是没问题的
- 会不会封号？ 我也不知道~~
 
---

# 运行环境

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
本片教程安装包为v3.0第一版。
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
- 从拿到的代码中，找到所需要的代码
- 每次反编译或者重新编译，**可能出现代码有些许差别**，视情况进行编写
- 右键 编辑方法(C#) 或 编辑类(C#)
  ![批注 2019-10-22 231556](https://i.loli.net/2019/10/22/zPVqNFsTH64gSWb.png)
- 修改完毕之后 编译
  ![批注 2019-10-22 231738](https://i.loli.net/2019/10/22/GPcraJjAfi4uFgK.png)
- 保存dll模块
  ![批注 2019-10-22 231821](https://i.loli.net/2019/10/22/pqV4iCJWAhek2Gu.png)

## 重新打包
- 用好压打开 apk
- 删除 `META-INF`
  
  ![批注 2019-10-16 013818](https://i.loli.net/2019/10/16/aY45Vx8qM9lRoN6.png)
- 添加并覆盖`Assembly-CSharp.dll`
  
  ![批注 2019-10-16 013938](https://i.loli.net/2019/10/16/BFWqERNDMefgGho.png)
- 准备好签名文件
    - 如果你有准备好的签名文件，请使用适合你准备好的签名命令
    - 如果你从前文下载的签名，执行以下命令 （自己看清楚文件名）
  ``` (cmd)
  java -jar "signapk.jar" testkey.x509.pem testkey.pk8 project-qt_268.apk project-qt_268_sign.apk
  ```

# 代码片段
- 每次反编译都**可能出现代码有些许差别**，如果有区别，视情况编写
- 如果出现`Debug`类型出错，就修改为`UnityEngine.Debug` ，下文中不再提到
## 开启伴侣动画
- `UserCompanionRecord`
  - 新增构造方法
  ``` C#
  //此处更改需要先保存模块，否则下文有一条更改会报错
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
  ``` C#
  Dictionary<string, int> value = keyValuePair.Value.JSONDict.ToDictionary((KeyValuePair<string, JSONNode> kvp) => kvp.Key, (KeyValuePair<string, JSONNode> kvp) => kvp.Value.AsInt);
  》》》
   Dictionary<string, int> value = keyValuePair.Value.JSONDict.ToDictionary((KeyValuePair<string, JSONNode> kvp) => kvp.Key, (KeyValuePair<string, JSONNode> kvp) => 1);
  ```
- `EpisodeMainPage`
  ``` C#
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
    ``` C#
  public static CompanionAnimationSetting.AnimationStatus GetEpisodeAnimationStatus(string companionId, int episodeId, int subEpisodeId)

  >>>
  public static CompanionAnimationSetting.AnimationStatus GetEpisodeAnimationStatus(string companionId, int episodeId, int subEpisodeId)
  {
      return CompanionAnimationSetting.AnimationStatus.Collected;  
  }
  ```
- `CompanionBanner`
  （此修改可能会使下次反编出现较大差异）

  ``` C#
    }
    this.ConditionGrid.Reposition();
    this.unlockedContent.SetActive(false);
    this.lockedContent.SetActive(true);
    this.completedContent.SetActive(false);
  }


  >>>
    }
    this.ConditionGrid.Reposition();
    this.unlockedContent.SetActive(false);
    this.lockedContent.SetActive(true);
    this.completedContent.SetActive(false);
    
    Transform vid = unlockedContent.transform.Find("VideoBtn");
    if (vid != null)
    {
        vid.SetParent(transform, true);
    }
  }

  ```
  ## 切换语言
- `Localization`
  ``` C#
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
（此修改可能会使下次反编出现较大差异）
``` C#
  public IEnumerator CheckSystem()
  {
    bool wait = true;
    bool anyErr = false;
    if (this.checkingMessage != null)
    {
      this.checkingMessage.gameObject.SetActive(true);
    }
  》》》
  public IEnumerator CheckSystem()
  {
    Config.LangAbleChange = true;
    bool wait = true;
    bool anyErr = false;
    if (this.checkingMessage != null)
    {
        this.checkingMessage.gameObject.SetActive(true);
    }
```
//因为不明原因，首次选择中文，会造成无限下载资源，暂时没有找到完美的解决方法。此处修改即强制退出App
  ``` C#
    if (!string.IsNullOrEmpty(Config.Language))
    {
      this.StartCoroutine(this.CheckDownload());
    }
    else
    {
      this.StartCoroutine(this.CheckSystem());
    }
  });
  
  》》》
    UnityEngine.Application.Quit();
  });
  ```

  ## 开启所有人物的事件相簿按钮
（目前仅有Erika有这个按钮，其他角色都为灰色）
- `PetDetailPage_v2`
  ``` C#
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
  ``` C#
  btn_cardView.gameObject.SetActive(****)
  》》》全部更改为 
  btn_cardView.gameObject.SetActive(true)
  ```

  ## 解锁人物5星卡片
- `PetDetailFullViewButton`
  ``` C#
  for (int i = 0; i < this.button.Length; i++)
  {
    bool flag = i > star;
    this.button[i].isEnabled = !flag;
    this.button[i].normalSprite = "Common Button_Square_Big2";
    this.lockSprite[i].gameObject.SetActive(flag);
  }
  >>>
  for (int i = 0; i < this.button.Length; i++)
  {
    bool flag = false;
    this.button[i].isEnabled = !flag;
    this.button[i].normalSprite = "Common Button_Square_Big2";
    this.lockSprite[i].gameObject.SetActive(flag);
  }
  ```
  ## 解锁活动CG 
  （目前仅有Erika有这个CG，其他角色都没）
 - `EventRewardPopup`
  ``` C#
  this.scenesLockBlock[num].SetActive(EventManager.CurrentEventLevel() < levelSetting[i].level);        
  >>>
  this.scenesLockBlock[num].SetActive(false);
  ```
- `EventAlbumPage`
  ``` C#
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
## 收集册
- `AlbumPage`
``` C#
  catch (Exception)
  {
      this.petSerializedData = null;
  }
 >>>
  catch (Exception)
  {
    this.petSerializedData = petConfig.CreateRawPetData(1, 5, 0);
  }
```
``` C#
  foreach (KeyValuePair<string, UserCompanionRecord> keyValuePair2 in CompanionManager.Instance.UserCompanions)
  {
    if (keyValuePair2.Value.id == keyValuePair.Value.id)
    {
      this.unlockNum++;
      item = keyValuePair2.Value;
      break;
    }
  }
  list.Add(item);
>>>

  foreach (KeyValuePair<string, UserCompanionRecord> keyValuePair2 in CompanionManager.Instance.UserCompanions)
  {
    if (keyValuePair2.Value.id == keyValuePair.Value.id)
    {
      this.unlockNum++;
      item = keyValuePair2.Value;
      break;
    }
  }
  //注 ：此处需要先修改上文 UserCompanionRecord 构造方法
  if (item == string.Empty)
  {
    item = new UserCompanionRecord(keyValuePair.Value.id);
  }
  list.Add(item);
```

