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