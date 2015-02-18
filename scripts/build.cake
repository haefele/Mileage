///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target                  = Argument<string>("target", "Default");
var configuration           = Argument<string>("configuration", "Release");
var incrementVersionType    = Argument<string>("incrementVersionType", "Revision");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var rootDir         = "./..";
var solutions       = GetFiles(rootDir + "/**/*.sln");
var solutionDirs    = solutions.Select(solution => solution.GetDirectory());
var outputDir       = rootDir + "/output";
var serverOutputDir = outputDir + "/Server";
var clientOutputDir = outputDir + "/Client";
var nextVersion     = new Version();
var buildsDir       = rootDir + "/builds";

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
});

Teardown(() =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories(outputDir);

    // Clean solution directories.
    foreach(var solutionDir in solutionDirs)
    {
        Information("Cleaning {0}", solutionDir);
        CleanDirectories(solutionDir + "/**/bin/" + configuration);
        CleanDirectories(solutionDir + "/**/obj/" + configuration);
    }
});

Task("Restore")
    .Does(() =>
{
    // Restore all NuGet packages.
    foreach(var solution in solutions)
    {
        Information("Restoring {0}", solution);
        NuGetRestore(solution);
    }
});

Task("IncrementAssemblyVersion")
    .Does(() =>
{
    // Read current version.
    
    var versionInfoPath = GetFiles(rootDir + "/**/VersionInfo.cs").First();
    Information("Found the VersionInfo.cs file at: " + versionInfoPath);

    var parsedVersionInfo = ParseAssemblyInfo(versionInfoPath);
    var currentVersion = new Version(parsedVersionInfo.AssemblyVersion);
    Information("The current version is " + currentVersion);

    // Increment the version.

    int major = currentVersion.Major;
    int minor = currentVersion.Minor;
    int build = currentVersion.Build;
    int revision = currentVersion.Revision;

    if (incrementVersionType == "Major")
    {
        major++;
        minor = 0;
        build = 0;
        revision = 0;
    }
    else if (incrementVersionType == "Minor")
    {
        minor++;
        build = 0;
        revision = 0;
    }
    else if (incrementVersionType == "Build")
    {
        build++;
        revision = 0;
    }
    else if (incrementVersionType == "Revision")
    {
        revision++;
    }
    else if (incrementVersionType == "None")
    {
        // Don't increment the version if incrementVersionType is "None".
    }
    else
    {
        throw new CakeException("The parameter \"incrementVersionType\" has to be \"Major\", \"Minor\", \"Build\", \"Revision\" or \"None\".");
    }

    nextVersion = new Version(major, minor, build, revision);
    Information("The next version is " + nextVersion);

    var assemblyInfoSettings = new AssemblyInfoSettings
    {
        Version = nextVersion.ToString(),
        FileVersion = nextVersion.ToString(),
        InformationalVersion = nextVersion.ToString()
    };

    CreateAssemblyInfo(versionInfoPath, assemblyInfoSettings);
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("IncrementAssemblyVersion")
    .Does(() =>
{
    // Build all solutions.
    foreach(var solution in solutions)
    {
        Information("Building {0}", solution);
        MSBuild(solution, settings => 
            settings.SetPlatformTarget(PlatformTarget.MSIL)
                .WithProperty("TreatWarningsAsErrors","true")
                .WithTarget("Build")
                .SetConfiguration(configuration));
    }
});

Task("CopyAssembliesToOutputDirectory")
    .IsDependentOn("Build")
    .Does(() => 
{
    var buildOutputDir = rootDir + "/src/03 Server/Hosts/Mileage.Server.Hosts.Console/bin/" + configuration;

    var allAssemblies = GetFiles(buildOutputDir + "/*.dll");
    var exe = GetFiles(buildOutputDir + "/*.exe").First();
    var exeConfig = GetFiles(buildOutputDir + "/*.exe.config").First();

    var assembliesOutputDir = serverOutputDir + "/Assemblies";

    CreateDirectory(outputDir);
    CreateDirectory(serverOutputDir);
    CreateDirectory(assembliesOutputDir);

    CopyFiles(allAssemblies, assembliesOutputDir);
    CopyFile(exe, serverOutputDir + "/MileageServer.exe");
    CopyFile(exeConfig, serverOutputDir + "/MileageServer.exe.config");
});

Task("CopyRavenStudioHtml5")
    .IsDependentOn("CopyAssembliesToOutputDirectory")
    .Does(() => 
{
    var ravenStudioHtml5Path = GetFiles(rootDir + "/**/Raven.Studio.Html5.zip").First();
    var ravenStudioOutputDir = serverOutputDir + "/Database/Files";

    CreateDirectory(ravenStudioOutputDir);
    CopyFileToDirectory(ravenStudioHtml5Path, ravenStudioOutputDir);
});

Task("ZipOutputDir")
    .IsDependentOn("CopyRavenStudioHtml5")
    .Does(() => 
{
    CreateDirectory(buildsDir);
    Zip(outputDir, buildsDir + "/Mileage-" + nextVersion + ".zip");
    DeleteDirectory(outputDir, true);
});

Task("Default")
    .IsDependentOn("ZipOutputDir");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
