---
layout: post
title: Unity 解锁游戏CG
date:  2022-05-10 23:00:00 +0800
categories: Hacking
tags: Unity
img: https://cdn.cloudflare.steamstatic.com/steam/apps/823910/header.jpg
author: oOtroyOo
lastupdate : 2022-05-10 00:00:00 +0800
---

# 功能概述
- 解锁所有CG
- 移除int数值加密，使得可以使用修改器


# 工程运行环境

## 必备软件与用途

1. dnSpy （反编译）

## 可选备用软件

1. Visual Studio  （代码编写工具 ，但是并不用做编译)
2. Git  （版本差异管理）
3. CodeCompare  （版本差异对比）
   
# 开始工作

## 准备安装包
- 找到安装目录的`assets\bin\Data\Managed`
- 使用dnSpy打开`Assembly-CSharp.dll`
- (可选步骤) 从dnSpy导出 sln工程，建立git库，使用Visual Studio编写代码
  
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
  
# 代码片段
- 每次反编译都**可能出现代码有些许差别**，如果有区别，视情况编写
- 如果出现`Debug`类型出错，就修改为`UnityEngine.Debug` ，下文中不再提到
## 开启所有CG
- `GameManager`

``` csharp
  //此处更改需要及时保存模块，下文有一条更改会引用这里
  private void Awake()
  {
    ////找到这里
    this.Load();
    //添加下面的内容
    for (int j = 0; j < this._gallerySceneList.Count; j++)
    {
        string text2 = this._gallerySceneList[j];
        if (text2.IndexOf("_Yoke") < 0 && (text2.IndexOf("Defeated_") == 0 || text2.IndexOf("Collect_") == 0 || text2.IndexOf("Encounter_") == 0 || text2.IndexOf("AnimCombineEventer_") == 0 || text2.IndexOf("Intimacy_") == 0 || text2.IndexOf("Scene_") == 0 || text2.IndexOf("CutScene_") == 0) && !GameManager._data.GalleryRegScene.Contains(text2))
        {
            GameManager._data.GalleryRegScene.Add(text2);
        }
    }
    for (int k = 0; k < GameDefine.GetGalleryImageCount(); k++)
    {
        string galleryImageTid = GameDefine.GetGalleryImageTid(k);
        if (!GameManager._data.GalleryRegEncounter.Contains(galleryImageTid))
        {
            GameManager._data.GalleryRegEncounter.Add(galleryImageTid);
        }
    }
  }
```

## 关闭Int加密
- `CInt`

  将两处 ` ^ 1899263634` 删除 
  （代码就省略了)