using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


/// <summary>
/// MOD的根配置
///
/// 本运行框架下，所有的可游玩内容都对等为一个MOD。
/// </summary>
public class MODRootConfig : Editor
{

    [MenuItem("Build/生成配置文件", priority = 0)]
    static void GenerateConfigs()
    {
        string dataPath = Path.Combine("Assets/Core/Scripts/Config/", "xlsx/", "Datas.bytes");
        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
        }
        ExcelTools.GenerateConfigsFromExcel<ConfigBase>($"Assets/Core/Scripts/Config/xlsx");
        AssetDatabase.Refresh();
    }
}
