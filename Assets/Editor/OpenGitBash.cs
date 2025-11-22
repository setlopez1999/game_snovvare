using UnityEditor;
using UnityEngine;
using System.Diagnostics;

public class OpenGitBash
{
    [MenuItem("Git/Abrir Git Bash")]
    public static void OpenBash()
    {
        string projectPath = Application.dataPath.Replace("/Assets", "");
        Process.Start("C:\\Program Files\\Git\\git-bash.exe", "--cd=\"" + projectPath + "\"");
    }
}
