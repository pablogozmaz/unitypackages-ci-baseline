#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using PackageManager = UnityEditor.PackageManager;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace PixelsHub.UnityPackageManagement
{
    [ScriptedImporter(1, extension)]
    public class PackagesGitDependencyAssetImporter : ScriptedImporter
    {
        private struct GitDependency
        {
            public string name;
            public string url;

            public GitDependency(string name, string url) 
            {
                this.name = name;
                this.url = url;
            }
        }

        public const char nameToUrlSeparator = '@';

        public const string extension = "upgd";

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var gitDependencies = FindGitDependencies(ctx.assetPath);

            if(gitDependencies.Count > 0)
            {
                string packageName = GetPackageName(ctx);
                AddGitDependencyPackages(packageName, gitDependencies);
            }
        }

        private List<GitDependency> FindGitDependencies(string assetPath)
        {
            List<GitDependency> result = new();

            HashSet<string> existingPackages = RegisterExistingPackages();

            try
            {
                using StreamReader reader = new(assetPath);

                string line;
                while((line = reader.ReadLine()) != null)
                {
                    if(!TryReadGitDependency(line, out GitDependency gitDependency))
                        continue;

                    if(existingPackages.Contains(gitDependency.name))
                        continue;

                    existingPackages.Add(gitDependency.name);
                    result.Add(gitDependency);
                }
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
            }

            return result;
        }

        private static HashSet<string> RegisterExistingPackages()
        {
            var packages = AssetDatabase.FindAssets("package")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(x => AssetDatabase.LoadAssetAtPath<TextAsset>(x) != null)
                .Select(PackageInfo.FindForAssetPath).ToList();

            HashSet<string> register = new();

            for(int i = 0; i < packages.Count; i++)
            {
                if(packages[i] == null || register.Contains(packages[i].name))
                    continue;
                
                register.Add(packages[i].name);
            }

            return register;
        }

        private bool TryReadGitDependency(string line, out GitDependency gitDependency)
        {
            gitDependency = new();

            for(int i = 0; i < line.Length; i++)
            {
                if(gitDependency.name == null)
                {
                    if(line[i] != nameToUrlSeparator)
                        continue;

                    gitDependency.name = line[..i];
                    gitDependency.url = line[(i + 1)..];
                    return true;
                }
            }

            Debug.LogError($"Could not read git dependency: \"{line}\"");
            return false;
        }

        private static string GetPackageName(AssetImportContext ctx)
        {
            try
            {
                return ctx.assetPath.Split('/')[1];
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
                return "MISSING";
            }
        }

        private static void AddGitDependencyPackages(string packageName, List<GitDependency> gitDependencies) 
        {
            Debug.Assert(gitDependencies != null && gitDependencies.Count > 0);

            if(!EditorUtility.DisplayDialog("Package git dependencies", $"Do you wish to install {gitDependencies.Count} git dependency packages for \"{packageName}\"?", "Ok", "Ignore"))
                return;

            EditorApplication.delayCall += () =>
            {
                string info = $"Downloading git packages for \"{packageName}\"";

                EditorUtility.DisplayProgressBar("Git package dependencies", info, 0.2f);

                int progressId = Progress.Start(info, null, Progress.Options.Indefinite);
                Progress.Report(progressId, 0);

                int pendingCount = gitDependencies.Count;

                foreach(var gitDependency in gitDependencies)
                {
                    Debug.Log($"Adding package \"{gitDependency.name}\" as git dependency for package \"{packageName}\"");

                    var packageAddRequest = PackageManager.Client.Add(gitDependency.url);

                    EditorApplication.update += HandleProgress;

                    void HandleProgress()
                    {
                        if(packageAddRequest.IsCompleted)
                        {
                            if(packageAddRequest.Status == PackageManager.StatusCode.Success)
                                Debug.Log("Installed: " + packageAddRequest.Result.packageId);
                            else if(packageAddRequest.Status >= PackageManager.StatusCode.Failure)
                                Debug.Log(packageAddRequest.Error.message);

                            pendingCount--;

                            Progress.Report(progressId, (float)pendingCount / gitDependencies.Count);

                            if(pendingCount == 0)
                            {
                                Progress.Remove(progressId);
                                EditorUtility.ClearProgressBar();
                            }

                            EditorApplication.update -= HandleProgress;
                        }
                    }
                }
            };
        }
    }
}
#endif