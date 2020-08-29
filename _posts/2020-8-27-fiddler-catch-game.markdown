---
layout: post
title: 安卓游戏抓包过程
date: 2020-08-27 21:56:05 +0800
categories: Hacking
tags: Unity
img: https://i.loli.net/2020/08/27/syHFSzKueDYQAj2.jpg
---

# Fiddler 安卓游戏抓包过程
文章正在更新中。

## [脚本下载](#导入脚本)
  - v1.2 
    开启角色的CG页面，开启CG活动玩法
  - v1.1
    支持繁中版，修正一些问题
  - v1.0
    支持美版， 开启了全部角色解锁，并满星，包括8星突破

## 前言
  - 游戏联网通常有几种形式，http，socket，蓝牙连接等。
  - 无论那种网络形式，都有数据包的发送。
  - Fiddler就是用来截取http连接的内容的。能够获得发送的数据，修改获得的内容等等。
  - 恰当的修改数据包的内容，就可以欺骗客户端，拿到你的假数据
  - 美版游戏与中版游戏的版本不同，故游戏内容多少差点。
  
## 安装与配置Fiddler

### 安装 Fiddler

  可以从[这里下载](http://www.dayanzai.me/fiddler.html)，注意不是用最新的 Fiddler Everywhere 。

### 设备

  - 电脑一台，最好能开Http代理
  - 手机需要与电脑在统一局域网
  - 可以使用Mumu模拟器，其他模拟器自行尝试
  - **注意** 由于抓包不安全，不建议使用手机，建议用模拟器
    
### 基本配置
   
  由于过程较为繁琐，建议自行搜索关于Fiddler与安卓抓包的文章   
  例如[这里](https://www.jianshu.com/p/6858a25674b4)。  
  关键就是要设置正确局域网Wifi代理地址, 通常是 

  地址：192.168.x.xx  
  端口：8888 

  Mumu模拟器是在设置里，长按那个Wifi连接就可以填代理  
  根据教程下载Fiddler提供的证书

### 代理配置

  由于游戏可能需要VPN，如果不需要，则可以跳过此步骤

  手机上不能开VPN，否则无效
  
  如果你有电脑版的http代理器，可以参考以下配置 
   - Fiddler 工具-选项-网关

  ![Fiddler网关](https://i.loli.net/2020/08/27/hEAdnGzQSHmXFYt.jpg)
  
  - 关闭启动充当系统代理
  
  ![批注 2020-08-28 230428](https://i.loli.net/2020/08/28/3kLjKU9MzGQNm7b.jpg)
  
  - 关闭系统的其他代理

  ![关闭系统代理](https://i.loli.net/2020/08/27/7clCrsWOVHPDjE9.jpg)

### 导入脚本
  - 下载脚本 
    - [Github](https://github.com/oOtroyOo/blog-mdui/releases/tag/qt)
  - 将FiddlerQT.dll , LitJson.dll 放在 `Fiddler安装目录\Scripts\`
   （以后更新可以不下载LitJson）
  - 重新开启Fiddler

## 开始游戏
 应该不出意外就能破解了 

### 解锁角色动画
  - 所有角色都将进入已召唤
  - 所有角色均5星及以上 点击右上方放大镜，切换卡片姿势
  - 8星突破角色       
    包括 Venus，Elva 或更多
  - 推出过CG收藏角色右上角的收藏按钮可以查看      
    包括 Erica埃里卡(双枪女仆)，Clara克拉拉(化学老师)，Amy艾米(运动兽娘)，Flora菲歐娜(小号手)，Venus(神秘学姐)，Alberta艾伯塔(女王)，Alice愛麗絲(电锯)，Elva埃爾瓦(赛博人)  
    或更多

### 调教活动/猜拳活动 玩法
  - 从主页中间的活动进入，活动结束就没了
  - 刚进入的时候会切换到第一种姿势
    - 调教活动
    1. 点击身体
    2. 点击正下方的头像
    3. 关闭
    - 猜拳活动
    1. 点击任意猜拳
    2. 任何情况都会显示为胜利
  - 然后就会升级姿势动画
  - 重复点击
  - 当达到最后一个姿势时，在奖励页里，就可以解锁所有CG

# 代码部分
  待更新
  
