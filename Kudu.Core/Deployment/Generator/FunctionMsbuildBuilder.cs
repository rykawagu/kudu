﻿using Kudu.Contracts.Settings;
using Kudu.Core.Infrastructure;

namespace Kudu.Core.Deployment.Generator
{
    /// <summary>
    /// Builds .Net Core 2.2 or less and .Net Frameworks Function apps using msbuild.exe
    /// </summary>
    class FunctionMsbuildBuilder : MicrosoftSiteBuilder
    {
        // we want to fail early (before running msbuild) if the deployment environment mismatch with the target framework
        public FunctionMsbuildBuilder(IEnvironment environment, IDeploymentSettingsManager settings, IBuildPropertyProvider propertyProvider, string sourcePath, string projectFilePath, string solutionPath)
            : base(environment, settings, propertyProvider, sourcePath, projectFilePath, solutionPath, "--functionApp")
        {
            FunctionAppHelper.ThrowsIfVersionMismatch(projectFilePath);
        }

        public override string ProjectType
        {
            get
            {
                return "MSBUILD FUNCTIONAPP";
            }
        }
    }
}