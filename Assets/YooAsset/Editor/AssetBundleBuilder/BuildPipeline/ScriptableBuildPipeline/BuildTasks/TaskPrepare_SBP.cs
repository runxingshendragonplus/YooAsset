using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace YooAsset.Editor
{
    public class TaskPrepare_SBP : IBuildTask
    {
        void IBuildTask.Run(BuildContext context)
        {
            var buildParametersContext = context.GetContextObject<BuildParametersContext>();
            var buildParameters = buildParametersContext.Parameters;

            // 检测基础构建参数
            buildParametersContext.CheckBuildParameters();

            // 检测是否有未保存场景
            if (EditorTools.HasDirtyScenes())
            {
                string message = BuildLogger.GetErrorMessage(ErrorCode.FoundUnsavedScene, "Found unsaved scene !");
                throw new Exception(message);
            }

            // 检测不被支持的构建模式
            if (buildParameters.BuildMode == EBuildMode.ForceRebuild)
            {
                string message = BuildLogger.GetErrorMessage(ErrorCode.BuildPipelineNotSupportBuildMode, $"{nameof(EBuildPipeline.ScriptableBuildPipeline)} not support {nameof(EBuildMode.ForceRebuild)} build mode !");
                throw new Exception(message);
            }

            // 检测包裹输出目录是否存在
            string packageOutputDirectory = buildParameters.GetPackageOutputDirectory();
            if (Directory.Exists(packageOutputDirectory))
            {
                string message = BuildLogger.GetErrorMessage(ErrorCode.PackageOutputDirectoryExists, $"Package outout directory exists: {packageOutputDirectory}");
                throw new Exception(message);
            }

            // 如果输出目录不存在
            string pipelineOutputDirectory = buildParameters.GetPipelineOutputDirectory();
            if (EditorTools.CreateDirectory(pipelineOutputDirectory))
            {
                BuildLogger.Log($"Create pipeline output directory: {pipelineOutputDirectory}");
            }
        }
    }
}