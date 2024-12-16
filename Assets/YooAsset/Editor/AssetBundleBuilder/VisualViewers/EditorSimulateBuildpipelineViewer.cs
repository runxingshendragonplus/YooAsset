#if UNITY_2019_4_OR_NEWER
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace YooAsset.Editor
{
    internal class EditorSimulateBuildPipelineViewer : BuildPipelineViewerBase
    {
        public EditorSimulateBuildPipelineViewer(string packageName, BuildTarget buildTarget, VisualElement parent)
            : base(packageName, EBuildPipeline.RawFileBuildPipeline, buildTarget, parent)
        {
            var compressionField = Root.Q<EnumField>("Compression");
            UIElementsTools.SetElementVisible(compressionField, false);

            var encryptionContainer = Root.Q<VisualElement>("EncryptionContainer");
            UIElementsTools.SetElementVisible(encryptionContainer, false);

            var fileNameStyleField = Root.Q<EnumField>("FileNameStyle");
            UIElementsTools.SetElementVisible(fileNameStyleField, false);

            var copyBuildinFileOptionField = Root.Q<EnumField>("CopyBuildinFileOption");
            UIElementsTools.SetElementVisible(copyBuildinFileOptionField, false);

            var CopyBuildinFileParamField = Root.Q<TextField>("CopyBuildinFileParam");
            UIElementsTools.SetElementVisible(CopyBuildinFileParamField, false);
        }

        /// <summary>
        /// 执行构建
        /// </summary>
        protected override void ExecuteBuild()
        {
            var buildMode = AssetBundleBuilderSetting.GetPackageBuildMode(PackageName, BuildPipeline);
            var fileNameStyle = AssetBundleBuilderSetting.GetPackageFileNameStyle(PackageName, BuildPipeline);
            var buildinFileCopyOption = AssetBundleBuilderSetting.GetPackageBuildinFileCopyOption(PackageName, BuildPipeline);
            var buildinFileCopyParams = AssetBundleBuilderSetting.GetPackageBuildinFileCopyParams(PackageName, BuildPipeline);

            EditorSimulateBuildParameters buildParameters = new EditorSimulateBuildParameters();
            buildParameters.BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
            buildParameters.BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();
            buildParameters.BuildPipeline = BuildPipeline.ToString();
            buildParameters.BuildTarget = BuildTarget;
            buildParameters.BuildMode = buildMode;
            buildParameters.PackageName = PackageName;
            buildParameters.PackageVersion = GetPackageVersion();
            buildParameters.VerifyBuildingResult = true;
            buildParameters.FileNameStyle = fileNameStyle;
            buildParameters.BuildinFileCopyOption = buildinFileCopyOption;
            buildParameters.BuildinFileCopyParams = buildinFileCopyParams;
            buildParameters.EncryptionServices = CreateEncryptionInstance();

            EditorSimulateBuildPipeline pipeline = new EditorSimulateBuildPipeline();
            var buildResult = pipeline.Run(buildParameters, true);
            if (buildResult.Success)
                EditorUtility.RevealInFinder(buildResult.OutputPackageDirectory);
        }

        protected override List<Enum> GetSupportBuildModes()
        {
            List<Enum> buildModeList = new List<Enum>();
            buildModeList.Add(EBuildMode.ForceRebuild);
            return buildModeList;
        }
    }
}
#endif