---
layout: post
title: 群晖Synology利用QuickConnect内网穿透获取直链下载
date: 2025-08-11 09:39:34 +0800
categories: Synology
tags: 
img:  https://iili.io/FQRjHiX.png ##https://freeimage.host/i/FQRjHiX
---

# 群晖Synology利用QuickConnect内网穿透获取直链下载
  *[原文地址](https://www.troy-web.top/blog-mdui/synology/2025/08/11/synology-quickconnect-directlink.html)*

<script>
  var $currentScript = Prism.util.currentScript();
  
  if (document.URL.indexOf("troy-web.top") > -1 || document.URL.indexOf("127.0.0.1") > -1) {

    $('.container').ready(() => {
      $currentScript.prev("p").css("display", "none");
      $currentScript.remove();
    });
  }

</script>

  - 有些时候需要直接访问ipv4文件URL地址，比如图片链接、或在线视频播放器，需要 `http:***xxx.mp4`的链接
  - 但是群晖Synology的QuickConnect内网穿透只能获取到一个中转页面，并不能直接访问
  - 所以需要利用底层机制他找到能够直接访问的直链，这样在线播放器就可以访问视频

  
## 前期准备



  - 参考网址
    - [通过群晖的 QuickConnect 访问第三方应用](https://null.studio/2020/02/16/access-thirdparty-web-via-synology-quickconnect/)
    - [使用群晖 QuickConnect 访问内网任意/第三方TCP服务](https://blog.lyc8503.net/post/10-all-in-quickconnect/)

  - 配置资源
    - 假设你能已经根据以上教程，配置好了视频，使用浏览器通过访问
     
    `https://[qc-id].quickconnect.cn/media/video.mp4`
    - 能正常访问，说明前期准备就绪
    - **建议使用手机流量尝试，可以确定是公网** （如果跳转进了局域网，那没意义）。
  - ipv6本身就无需穿透，这里不讨论

## 步骤详解
  *这里使用Postman或类似工具，进行实验*
  
  - 模拟请求
  
  1. 国内账户使用 `https://global.quickconnect.cn/Serv.php`
  2. 全球账户使用 `https://global.quickconnect.to/Serv.php`
  
  - **POST**  `https://global.quickconnect.cn/Serv.php` 
  
    - Json Body  (替换自己的 QuickConnect ID)
  ~~~ json5
    [
        {  // *HTTP的body*
            "version": 1,
            "command": "get_server_info",
            "stop_when_error": false,
            "stop_when_success": false,
            "id": "mainapp_http",
            "serverID": "[QuickConnect ID]",
            "is_gofile": false,
            "path": ""
        },
        {   // *HTTPS的body*
            "version": 1,
            "command": "get_server_info",
            "stop_when_error": false,
            "stop_when_success": false,
            "id": "mainapp_https",
            "serverID": "[QuickConnect ID]",
            "is_gofile": false,
            "path": ""
        }
    ]
  ~~~

  - 发送请求，返回结果
  ~~~ json5
    [
        {
            // ...
            "service": {
              // ...
                "relay_ip": "*ip*",
                "relay_port": *port*,
                // ...
            },
            // ...
        },
        {
        // ...
        }
    ]
  ~~~

  - 如果能拿到以上信息，然后再次打开以上链接: `http://*ip*:*port*/media/video.mp4`  说明解析成功可以访问

## 编写解析器
  - 通过以上方式，获得了直链的ip和端口，然后header返回301+location就完成了。
  - 暂时只使用http，https因为还有证书问题，后续再研究。

  1. nodejs 实现
  
  使用nodejs是因为他可以单文件即可运行，很方便的实现http服务。

  当然如果知晓其原理，也可以用其他语言实现。
  - 安装步骤略
  
  - 编写 `index.js`


~~~ javascript
{% include code/synology-quickconnect-directlink/index.js %}
~~~

<script>
  var $currentScript = Prism.util.currentScript();

  $('.container').ready(() => {
     /*var $currentScript = $('#synology-quickconnect-directlink');$(document.currentScript);*/
    if ($currentScript) {
      $currentScript.prev(".language-javascript").find("pre")
        .attr({
          'data-download-link': '',
          'data-src': "https://github.tianrld.top/https://raw.githubusercontent.com/oOtroyOo/blog-mdui/master/_includes/code/synology-quickconnect-directlink/index.js"
        });
      /*$currentScript.remove();*/
    }
  });
</script>

  - 执行
    - `node ./index.js`
  - 测试访问 `http://127.0.0.1:9000/[QuickConnect ID]/media/video.mp4`
  如果可以正常播放，那么就算完成了

## 部署服务端（可选）
  - 你可以采用任何支持nodejs的服务器。没有具体限制
  - 这里采用阿里云，云函数FC3.0 + nodejs容器 实现。
  - 账户相关略
  - 打开 https://fcnext.console.aliyun.com/
  - 在顶部选择合适的地区
  - 进入 <函数/创建函数/创建Web 函数>
    - 运行环境选择 nodejs 版本随意，其他随意
    - 使用hello示例代码
    - （日志可以关了，不影响使用，浪费资源）
    - 创建完成
  - 进入在线编辑器
    - 替换`index.js`代码为以上代码
    - 保存部署
  - 找到【触发器】
    - 编辑触发器
    - 认证方式 ✅无需认证
    - 抄下**公网访问地址URL**
  - 测试访问
    - 访问 `https://[公网访问地址URL]/[QuickConnect ID]/media/video.mp4`
    - 正常播放，说明完成

