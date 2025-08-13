
# 前言

-   为何如此创建

    在Unity 开发中ILRuntime的引入并不陌生了。

    以往的教程，都需要使用VS配置ILRuntime的工程，进行编译热更dll。这样在没有VS环境的情况，或者非开发人员使用，等等情况会造成不便捷。

-   优点

    1.  无需开启VS来编译，仅需使用Unity打包。且
    1.  Unity主工程，无需开启unsafe特性，提高稳定性

-   缺点

    在 [原理解释](#%E5%8E%9F%E7%90%86%E8%A7%A3%E9%87%8A)

-   我比较懒。注释你们就自己琢磨吧

# 创建步骤

## Unity工程

-   Unity最低什么版本暂未证实，总之需要Unity2018以上且有如下功能

-   从package manager 导入ILRuntime

-   分别创建2个 Assembly Definition

    ![屏幕截图 2021-04-05 221648](https://i.loli.net/2021/04/05/BvhQORYdT1uN5eW.jpg)

    -   `GameScript` 用来写ILRuntime外部代码
    -   `InterScript` 用来写内部代码

-   `InterScript`

    ![屏幕截图 2021-04-05 222309](https://i.loli.net/2021/04/05/VEiBoPcf7rZNAmD.jpg)

    1.  引入必要的其他Assembly。如 `ILRuntime` ，`UnityEngine.UI`
    1.  启动unsfe特性
    1.  不要引入`GameScript`

-   `GameScript`

    ![屏幕截图 2021-04-05 222829](https://i.loli.net/2021/04/05/DPdr2vTRUgxk7bz.jpg)

    1.  引入必要的其他Assembly。如 `ILRuntime` ，`UnityEngine.UI`
    1.  引入 **`InterScript`**
    1.  无需启动unsfe特性。如果你要用，则视情况勾选
    1.  平台只勾选Editor，这一步是为了打包时剔除这个库

## 原理解释

1.  Unity会自动编译Assembly产生的`.dll`和`.pdb`文件到 `项目路径\Library\ScriptAssemblies`之中。

1.  在Unity Editor里，我们可以设计3种代码载入方式

    -   Editor模式

        使用系统加载`Library\ScriptAssemblies`中的文件
         `assembly =System.AppDomain.CurrentDomain.Load(dll, pdb);`
         从`assembly`中去调用方法。
         这样就跟ILRuntime没联系、打印行号也是匹配的

    -   Runtime模式

        使用 `ILRuntime.Runtime.Enviorment.AppDomain`
         `domain.LoadAssembly(dllStream, pdbStream, new PdbReaderProvider());`
         加载`Library\ScriptAssemblies`中的文件
         使用 `domain`去调用方法

    -   Release模式
         将dll（加密[可选]）复制到运行时路径。
         再由 `ILRuntime` 加载

1.  生成ILRuntime的委托和CLR绑定

```
将代码生成到`InterScript`里。因为这个配置使用了unsafe特性。且Unity项目和热更代码均能访问到
```

1.  热更代码将**无法直接使用**Unity项目代码 即`Assembly-CSharp`

```
由于Untiy特性。子级的 Assembly Definition 均无法访问`Assembly-CSharp` 
而ILRuntime本来是支持的。我们的最终目的是使用ILRuntime的，所以仍然可以用其他方式使用代码，例如用反射,基类,委托等方式来使用。

```

## 创建代码

### 运行部分

-   `ILRuntimeLoader`
    1.  在`InterScript`里创建一个加载类 `ILRuntimeLoader`
    1.  并声明一个枚举`ILRuntimeLoader.LoadType`，用于在编辑器中切换几种加载方式。
    1.  实现这几种加载方式

``` csharp   
public class ILRuntimeLoader
{

    public enum LoadType
    {
        Editor, Runtime, Release
    }

    public static LoadType loadType
    {
#if UNITY_EDITOR
        get => (LoadType)PlayerPrefs.GetInt(nameof(ILRuntimeLoader) + "." + nameof(LoadType), 1);
        set => PlayerPrefs.SetInt(nameof(ILRuntimeLoader) + "." + nameof(LoadType), (int)value);
#else
        get => LoadType.Release;
#endif
    }
}

    public static async Task Load(string fixUrl, string asseblyName)
    {
        iLDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
#if DEBUG && !NO_PROFILER
        iLDomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;
#endif
        await StartLoad(fixUrl, asseblyName);
    }



    private static async Task StartLoad(string fixUrl, string asseblyName)
    {
        Stream dllStream = null;
        Stream pdbStream = null;

        string dllFile = $"{fixUrl}/{asseblyName}.dll";
        string pdbFile = $"{fixUrl}/{asseblyName}.pdb";
        if (File.Exists(dllFile))
        {
            dllStream = LoadFile(dllFile);

        }
        else
        {
            dllStream = await LoadAsyncUrl(dllFile);
            dllStream.Seek(0, 0);
        }
        if (loadType == LoadType.Release)
        {
            using (var cryptoTransform = ILRuntimeEncriptFactory.CreateDecrypt(Encryptkey))
            {
                CryptoStream cryptoStream = new CryptoStream(dllStream, cryptoTransform, CryptoStreamMode.Read);
                using (cryptoStream)
                {
                    byte[] temp = new byte[4096];
                    dllStream = new MemoryStream();
                    int count;
                    while ((count = cryptoStream.Read(temp, 0, temp.Length)) > 0)
                    {
                        dllStream.Write(temp, 0, count);
                    }
                }
            }
            dllStream.Seek(0, 0);
        }
        if (loadType != LoadType.Release)
        {
            if (File.Exists(pdbFile))
            {
                pdbStream = LoadFile(pdbFile);
            }
            else
            {
                pdbStream = await LoadAsyncUrl(pdbFile);
            }

            pdbStream.Seek(0, 0);
        }
        if (dllStream != null)
        {
            if (loadType == LoadType.Editor)
            {
                byte[] dllBytes = new byte[dllStream.Length];
                dllStream.Read(dllBytes, 0, dllBytes.Length);
                byte[] pdBytes;
                if (pdbStream != null)
                {
                    pdBytes = new byte[pdbStream.Length];
                    pdbStream.Read(pdBytes, 0, pdBytes.Length);
                }
                else
                {
                    pdBytes = null;
                }
                dllAssembly = System.AppDomain.CurrentDomain.Load(dllBytes, pdBytes);
            }
            else
            {
                iLDomain.LoadAssembly(dllStream, pdbStream, new PdbReaderProvider());
            }
        }
        else
        {
            Debug.LogException(new NullReferenceException("ILRuntime  加载失败"));
            return;
        }

        InitBindings();
        Application.quitting += Shutdown;

    }

```

-   `ILRuntimeConfig`
    作为配置文件

``` csharp
public class ILRuntimeConfig
{
    public static string HotFixUrl
    {
        get
        {
            string path =
#if UNITY_EDITOR
              ILRuntimeLoader.loadType == ILRuntimeLoader.LoadType.Release ? PlayerPath : EditorPath;
#else
                PlayerPath;

#endif
            return (Directory.Exists(path) ? "file://" : "") + path;
        }
    }


    public static string PlayerPath
    {
        get
        {
            return Application.persistentDataPath + "/Patch";
        }
    }
    public static string EntryType = "GameScript.Entry";
    public static string AssemblyName = "GameScript";


#if UNITY_EDITOR
    public static string EditorPath
    {
        get
        {
            return Environment.CurrentDirectory.Replace('\\', '/') + "/Library/ScriptAssemblies";
        }
    }
    public static string GenCodePath
    {
        get
        {
            DirectoryInfo directory = Directory.CreateDirectory(@"Assets\Script\InterScript\ILRuntime\Generated");
            return directory.FullName;
        }
    }
#endif
}

```

### Editor部分

-   `ILRuntimeMenu`
    这是菜单
    效果是可以选择3种模式

    ![屏幕截图 2021-04-05 232731](https://i.loli.net/2021/04/05/ERuThgmHYoIb1nB.jpg)

``` csharp
public class ILRuntimeMenu
{
    [InitializeOnLoadMethod]
    static void Onload()
    {
        SwitchType();
    }

    private const string ModeMenuRoot= "ILRuntime/Editor加载方式/";
    private static void SwitchType()
    {
        foreach (ILRuntimeLoader.LoadType value in Enum.GetValues(typeof(ILRuntimeLoader.LoadType)))
        {
            Menu.SetChecked(ModeMenuRoot + value, value == ILRuntimeLoader.loadType);
        }
    }

    [MenuItem(ModeMenuRoot + nameof(ILRuntimeLoader.LoadType.Editor))]
    static void EditorLoadType1(MenuCommand command)
    {
        ILRuntimeLoader.loadType = ILRuntimeLoader.LoadType.Editor;
        SwitchType();
    }
    [MenuItem(ModeMenuRoot + nameof(ILRuntimeLoader.LoadType.Runtime))]
    static void EditorLoadType2(MenuCommand command)
    {
        ILRuntimeLoader.loadType = ILRuntimeLoader.LoadType.Runtime;
        SwitchType();
    }
    [MenuItem(ModeMenuRoot + nameof(ILRuntimeLoader.LoadType.Release))]
    static void EditorLoadType3(MenuCommand command)
    {
        ILRuntimeLoader.loadType = ILRuntimeLoader.LoadType.Release;
        SwitchType();
    }
}
```

-   `OnILRuntimeBuild`

这个代码是用于自动复制Release模式的代码
  实现`IPreprocessBuildWithReport` 和 `EditorApplication.playModeStateChanged`

``` csharp

[InitializeOnLoad]
public class OnILRuntimeBuild : IPreprocessBuildWithReport
{
    public int callbackOrder => 1;

    static OnILRuntimeBuild()
    {
        EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
    }

    static void EditorApplication_playModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            if (ILRuntimeLoader.loadType == ILRuntimeLoader.LoadType.Release)
            {
                CopyScript();
            }
        }
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        CopyScript();
    }

    static void CopyScript()
    {
        Debug.Log("Copy ILRuntime Script");
        if (!Directory.Exists(ILRuntimeConfig.PlayerPath))
        {
            Directory.CreateDirectory(ILRuntimeConfig.PlayerPath);
        }

        Directory.CreateDirectory(ILRuntimeConfig.PlayerPath);
        string[] files = Directory.GetFiles(ILRuntimeConfig.EditorPath, ILRuntimeConfig.AssemblyName + "*");
        foreach (string file in files)
        {
            if (Path.GetExtension(file) == ".pdb" && !EditorUserBuildSettings.allowDebugging)
            {
                continue;
            }

            string destFileName = Path.Combine(ILRuntimeConfig.PlayerPath, Path.GetFileName(file));
            Encrypt(file, destFileName);
            //File.WriteAllBytes(destFileName, data);
        }

    }
    static void Encrypt(string path, string destFileName)
    {

        using (var cryptoTransform = ILRuntimeEncriptFactory.CreateEncrypt(ILRuntimeLoader.Encryptkey))
        {
            FileStream fileStream = File.OpenRead(path);
            using (fileStream)
            {
                using (FileStream writeFileStream = File.Create(destFileName))
                {
                    CryptoStream cryptoStream = new CryptoStream(writeFileStream, cryptoTransform, CryptoStreamMode.Write);
                    using (cryptoStream)
                    {
                        fileStream.CopyTo(cryptoStream);
                    }
                }
            }
        }
    }
} 
```

-   生成委托和CLR绑定

具体代码，视项目需求各有不同，就不贴了。
   关于

`ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode`

`ILRuntime.Runtime.Enviorment.CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode`

所使用的路径为 `InterScript\ILRuntime\Generated`

### 额外补充

虽然说不能直接使用项目代码，但也是可以尝试一些方案。

例如可以在`GameScript.csproj`中引用 `Assembly-CSharp`, `Assembly-CSharp-firstpass` ,在VS中就可以加载类型。

`.csproj` 本质也是是xml文件。以下是一个自动化脚本，用于识别和配置 .csproj加入引用

吐槽一点。`OnGeneratedCSProject`这个事件在官方文档都没写。而[AssetPostprocessor 源码](https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/AssetPostprocessor.cs#L135) 写了，而且上面也提到了“未写文档，不建议使用”

``` csharp

public class OnILRuntimeImport : AssetPostprocessor
{
    private static string[] projectNames = new[] { "Assembly-CSharp", "Assembly-CSharp-firstpass" };
    private static string OnGeneratedCSProject(string path, string content)
    {
        DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(path));
        if (Path.GetFileNameWithoutExtension(path) == typeof(GameScript.Entry).Assembly.GetName().Name)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(content);
            XmlElement Project = document.DocumentElement;
            bool[] contains = new bool[projectNames.Length];
            foreach (XmlNode itemNode in Project.SelectNodes("ItemGroup"))
            {
                foreach (XmlNode projRef in itemNode.SelectNodes("ProjectReference"))
                {
                    if (projRef.Attributes.Count > 0)
                    {
                        string include = projRef.Attributes["Include"].Value;

                        for (int i = 0; i < projectNames.Length; i++)
                        {
                            if (include == projectNames[i])
                            {
                                contains[i] = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (!Array.TrueForAll(contains, b => b))
            {
                XmlNode itemNode = document.CreateElement("ItemGroup", Project.NamespaceURI);
                for (int i = 0; i < projectNames.Length; i++)
                {
                    if (!contains[i])
                    {
                        if (File.Exists(directory.FullName + "/" + projectNames[i] + ".csproj"))
                        {
                            Debug.Log("为" + Path.GetFileName(path) + "添加" + projectNames[i] + "引用");

                            XmlNode projRef = document.CreateElement("ProjectReference", Project.NamespaceURI);
                            var include = document.CreateAttribute("Include");
                            include.Value = projectNames[i];
                            projRef.Attributes.Append(include);
                            itemNode.AppendChild(projRef);
                        }
                    }
                }

                Project.AppendChild(itemNode);
                StringWriter writer = new StringWriter();
                XmlTextWriter xmlTextWriter = new XmlTextWriter(writer);
                xmlTextWriter.Formatting = Formatting.Indented;
                document.WriteTo(xmlTextWriter);
                return writer.ToString();
            }
        }
        return content;
    }
}

```



Reference:

