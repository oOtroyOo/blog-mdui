---
layout: post
title: Unity 手游破解流程
date:  2019-10-21 22:28:51 +0800
categories: Unity
tags: 
img: 
author: oOtroyOo
---
# Project QT 说明文档


###  [安装包下载](./) (最新版尚待开发)

---

（以下是破解文档 ，尚未完成书写，后续将补充）
# 运行环境

## 必备软件与用途

1. dnSpy （反编译）
2. 好压 （压缩包的操作，或同类软件）
3. Java SDK （java，安卓必备）
4. 签名文件 （用于签名）

## 可选备用软件

1. Visual Studio  （代码编写工具 ，但是并不用做编译 ，仅用dnSpy也是可以的)
2. Git  （版本差异管理）
3. CodeCompare  （版本差异对比）
   
# 准备工作
- 下载安装包
    本片教程安装包对应的名字应该 `project-qt_268.apk` 。为v3.0d第一版

    今后的版本不出意外的话，应该同样可以参考本教程
- 右键用好压打开
   解压`assets\bin\Data\Managed`
  
  ![批注 2019-10-16 012408](https://i.loli.net/2019/10/16/QlCarOkHoLh1IsY.png)
- 使用dnSpy打开`Assembly-CSharp.dll`

- 安卓签名文件
  如果你有准备好的签名文件，可以跳过下载
  [签名文件下载](http://www.greenxf.com/soft/95740.html)
- (可选步奏) 从dnSpy导出 sln工程，建立git库，使用Visual Studio编写代码
  
  ![批注 2019-10-16 012723](https://i.loli.net/2019/10/16/pBbm1DchtE4XaKC.png)

# 反编代码
   这一节为简介，稍后列出详细的代码段
- 从拿到的代码中，找到所需要的代码
  ![image]()
- 右键 编辑 方法/类 (C#)
  ![image]()
- 修改完毕之后 编译
  ![image]()
- 保存dll模块
  ![image]()

# 重新打包
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