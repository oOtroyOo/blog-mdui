---
layout: post
title: Unity 使用Android原生Camera预览摄像头
date:  2020-01-21 17:26:28 +0800
categories: Unity
tags: 
img: 
---
//尚未写完
#Unity 使用Android原生Camera预览摄像头 
不使用WebCamTex，而在安卓层传递摄像头画面

## 原理

- Android开启Camera，
- Camera默认的数据编码为yuv，需要byte[]从yuv转rbga
- Unity准备一个Texture2D，传递到安卓
- 将rbga数据渲染到Texture2D

## 安卓部分
``` Java

```

## Unity部分

- 场景中准备一个RawImage
- ![批注 2020-01-21 174158](https://i.loli.net/2020/01/21/tQMZbeDkPxdsRy9.png)


