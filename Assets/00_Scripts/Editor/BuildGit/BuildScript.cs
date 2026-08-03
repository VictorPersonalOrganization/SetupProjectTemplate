using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class BuildScript
{
    private static string[] Scenes =>
        EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

    public static void BuildWindows()
    {
        string buildDir = GetArg("-customBuildDir") ?? "Builds/Windows";
        string exeName = GetArg("-customExeName") ?? "Game.exe";
        string outputPath = $"{buildDir}/{exeName}";

        BuildReport report = BuildPipeline.BuildPlayer(
            Scenes,
            outputPath,
            BuildTarget.StandaloneWindows64,
            BuildOptions.None
        );

        if (report.summary.result != BuildResult.Succeeded)
        {
            EditorApplication.Exit(1);
            return;
        }

        EditorApplication.Exit(0);
    }

    private static string GetArg(string name)
    {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == name)
            {
                return args[i + 1];
            }
        }
        return null;
    }
}