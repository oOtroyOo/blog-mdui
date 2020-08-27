---
layout: post
title: 安卓游戏抓包过程
date: 2020-08-27 21:56:05 +0800
categories: Unity
tags: 
img: https://i.loli.net/2020/08/27/syHFSzKueDYQAj2.jpg
---

# Fiddler 安卓游戏抓包过程
文章正在更新中。


## 前言
  - 游戏联网通常有几种形式，http，socket，蓝牙连接等。
  - 无论那种网络形式，都有数据包的发送。
  - Fiddler就是用来截取http连接的内容的。能够获得发送的数据，修改获得的内容等等。
  - 恰当的修改数据包的内容，就可以欺骗客户端，拿到你的假数据
  
## 安装与配置Fiddler
  - 安装 
    可以从[这里下载](http://www.dayanzai.me/fiddler.html)，注意不是用最新的 Fiddler Everywhere 。

  - 设备

    - 电脑一台
    - 因为网络抓包存在不安全性，你可以用手机Wifi连接，建议用模拟器，我使用的Mumu
    
  - 基本配置
   
    由于过程较为繁琐，建议自行搜索关于Fiddler与安卓抓包的文章 ，例如[这里](https://www.jianshu.com/p/6858a25674b4) 。

    关键就是要设置正确局域网Wifi代理地址，下载Fiddler提供的证书

  - 代理配置
    由于游戏可能需要VPN，如果你有电脑版的Http代理器，可以参考以下配置

    - Fiddler 工具-选项-网关

    ![Fiddler网关](https://i.loli.net/2020/08/27/hEAdnGzQSHmXFYt.jpg)
    
    - 关闭系统代理

    ![关闭系统代理](https://i.loli.net/2020/08/27/7clCrsWOVHPDjE9.jpg)

# 导入脚本
  - 
  
