using UnityEditor;
using UnityEngine;

namespace YooAsset.Editor
{
    public static class AssetBundleSimulateBuilder
    {
        /// <summary>
        /// 模拟构建
        /// </summary>
        public static SimulateBuildResult SimulateBuild(string buildPipelineName, string packageName)
        {
            var buildParameters = new EditorSimulateBuildParameters();
            buildParameters.BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
            buildParameters.BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();
            buildParameters.BuildPipeline = EBuildPipeline.EditorSimulateBuildPipeline.ToString();
            buildParameters.BuildTarget = EditorUserBuildSettings.activeBuildTarget;
            buildParameters.BuildMode = EBuildMode.ForceRebuild;
            buildParameters.PackageName = packageName;
            buildParameters.PackageVersion = "Simulate";
            buildParameters.FileNameStyle = EFileNameStyle.HashName;
            buildParameters.BuildinFileCopyOption = EBuildinFileCopyOption.None;
            buildParameters.BuildinFileCopyParams = string.Empty;

            var pipeline = new EditorSimulateBuildPipeline();
            BuildResult buildResult = pipeline.Run(buildParameters, false);
            if (buildResult.Success)
            {
                SimulateBuildResult reulst = new SimulateBuildResult();
                reulst.PackageRootDirectory = buildResult.OutputPackageDirectory;
                return reulst;
            }
            else
            {
                return null;
            }
        }
    }
}