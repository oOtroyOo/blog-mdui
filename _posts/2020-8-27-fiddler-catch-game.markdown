---
layout: post
title: 安卓游戏抓包过程
date: 2021-05-05 20:00:00 +0800
categories: Hacking
tags: Unity
img: https://i.loli.net/2020/08/27/syHFSzKueDYQAj2.jpg
---

# Fiddler 安卓游戏抓包过程
适配理论上所有版本，源码稍后发布

 **注意**服务端增设了CG验证机制，后续更新之后，可能都无法下载活动CG或觉醒立绘。
    活动CG 例如vivian(侦探),Una(冰女)
    觉醒立绘 例如 Juliana(调教官)
# [脚本下载](#导入脚本)
  - v1.5 
    - 加入 Una(冰女)约会活动适配
     
    - 优化修改器版本的逻辑，只会将已拥有角色练到满级，避免使用未解锁的角色
  - v1.4 
    提供修改器版的脚本
  - v1.3 
    解锁房间伴侣，适配优化
  - v1.2 
    开启角色的CG页面，开启CG活动玩法
  - v1.1
    支持繁中版，修正一些问题
  - v1.0
    支持美版， 开启了全部角色解锁，并满星，包括8星突破

# 前言
  - 游戏联网通常有几种形式，http，socket，蓝牙连接等。
  - 无论那种网络形式，都有数据包的发送。
  - Fiddler就是用来截取http连接的内容的。能够获得发送的数据，修改获得的内容等等。
  - 恰当的修改数据包的内容，就可以欺骗客户端，拿到你的假数据
  - 美版游戏与中版游戏的版本不同，故游戏内容多少差点。
  
<div style="font-size:30px; color:red; line-height:32px">
<p>关于教程</p>
<p>我暂时没有在发过任何教学视频，或付费服务</p>
<p>如果想要充电，去给我充电吧。网站底部有我的B站链接。</p>
</div>
# 安装与配置Fiddler

## 安装 Fiddler

  可以从[这里下载](http://www.dayanzai.me/fiddler.html)，注意不是用最新的 Fiddler Everywhere 。

## 设备

  - 电脑一台，最好能开Http代理
  - 手机需要与电脑在统一局域网
  - 可以使用Mumu模拟器，其他模拟器自行尝试
  - **注意** 由于抓包不安全，且高版本手机必须Root安装系统级证书，不建议使用手机，建议用模拟器
    
## 基本配置
   
  由于过程较为繁琐，建议自行搜索关于Fiddler与安卓抓包的文章   
  例如[这里](https://blog.csdn.net/gld824125233/article/details/52588275)。  
  关键就是要设置正确局域网Wifi代理地址, 通常是 

  地址：192.168.x.xx  
  端口：8888 

  Mumu模拟器是在设置里，长按那个Wifi连接就可以填代理 
  然后重要的是要安装证书

  根据教程下载Fiddler提供的证书

  - 点亮菜单上的“解码”按钮

  ![批注 2020-09-05 025853](https://i.loli.net/2020/09/05/feMc2o4xyPaAkZ8.jpg)

## 代理配置

  - 手机或模拟器本身不能开VPN，否则无效
  - 关闭启动充当系统代理
  
  ![批注 2020-08-28 230428](https://i.loli.net/2020/08/28/3kLjKU9MzGQNm7b.jpg)
  
  - 关闭系统的其他代理

  ![关闭系统代理](https://i.loli.net/2020/08/27/7clCrsWOVHPDjE9.jpg)

  - 由于游戏可能需要VPN 
  
    如果你没有电脑端Http代理，或者不需要，**请跳过此步骤** 
    
    如：ShadowSocks，Trojan，等工具，可以参考以下配置 **<u> 请根据你自己的地址填写</u>**  不要直接复制。

    - Fiddler 工具-选项-网关 

    ![Fiddler网关](https://i.loli.net/2020/08/27/hEAdnGzQSHmXFYt.jpg)
  
  

## 导入脚本
  - 下载脚本 
    - [Github](https://github.com/oOtroyOo/blog-mdui/releases/tag/qt)
  - 将FiddlerQT.dll , LitJson.dll 放在 `Fiddler安装目录\Scripts\`
   （以后更新可以不下载LitJson）
  - 若想使用修改器版，则更换为 FiddlerQT_Hack.dll
  - 重新开启Fiddler

## 问题排查
  - 可以从“日志”页查看脚本异常，通常没反应可能就是出错了
  - 遇到技术性难题可以向我私信

# 开始游戏
 应该不出意外就能破解了 

## 解锁角色动画
  - 所有角色都将进入已召唤
  - 所有角色均5星及以上 点击右上方放大镜，切换卡片姿势
  
  - 8星突破角色       
    **注意** Juliana(调教官) 等，之后更新的觉醒角色由于服务端判定而**无法下载**，所以无法点进去，这个问题待处理。
    包括 Venus，Elva，Giselle，Alberta，Vivian，Geneva，Una 或更多

  - 推出过活动CG收藏角色，从角色右上角，或首页右上角的收藏图鉴按钮可以查看   
    **注意** Una(冰女)，vivian(侦探) 等，之后更新的活动CG可能由于服务端判定而**无法下载**
    包括 Erica埃里卡(双枪女仆)，Clara克拉拉(化学老师)，Amy艾米(运动兽娘)，Flora菲歐娜(小号手)，Venus維納斯(神秘学姐)，Alberta艾伯塔(女皇)，Alice愛麗絲(电锯)，Elva埃爾瓦(赛博人)，Geneva(幽灵)，Giselle(巫女)，Lucy(蛋糕师)，Winni(潜水员)
    或更多

## 调教活动/猜拳活动 玩法
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
  
