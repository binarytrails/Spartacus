using Spartacus.Modes.PROXY;
using Spartacus.Spartacus.CommandLine;
using System;
using System.IO;

namespace Spartacus.Modes.LOCAL
{
    class ModeLocal : ModeBase
    {
        public override void Run()
        {
            Logger.Info("Running is local mode...");
            Logger.Info("DLL path for local mode:" + RuntimeData.DLLPath);
            CreateSingleSolutionForDLL(RuntimeData.DLLPath);

            Logger.Info("End of running local mode.");
            return;
        }

        protected void CreateSingleSolutionForDLL(string dllPath)
        {
            string solution = Path.Combine(RuntimeData.Solution, Path.GetFileNameWithoutExtension(dllPath));
            string dllFile = Helper.LookForFileIfNeeded(dllPath);

            ProxyGeneration proxyMode = new();
            if (String.IsNullOrEmpty(dllPath) || String.IsNullOrEmpty(dllPath) || !File.Exists(dllPath))
            {
                Logger.Warning(" - No DLL Found", true, false);
                return;
            }
            else
            {
                Logger.Info(" - Found", true, false);
            }

            if (!proxyMode.ProcessSingleDLL(dllPath, solution))
            {
                Logger.Error("Could not generate proxy DLL for: " + dllFile);
            }
        }

        public override void SanitiseAndValidateRuntimeData()
        {
            // dllpath.
            if (String.IsNullOrEmpty(RuntimeData.DLLPath))
            {
                throw new Exception("--dllpath is missing, will skip DLL proxy generation");
            }

            // Solution folder.
            if (String.IsNullOrEmpty(RuntimeData.Solution))
            {
                Logger.Debug("--solution is missing, will skip DLL proxy generation");
            }
            else if (Directory.Exists(RuntimeData.Solution))
            {
                Logger.Debug("--solution directory already exists");
            }
            else
            {
                Logger.Debug("--solution directory does not exist - creating now");
                if (!Helper.CreateTargetDirectory(RuntimeData.Solution))
                {
                    throw new Exception("Could not create --solution directory: " + RuntimeData.Solution);
                }
            }
        }
    }
}
