// include Fake lib
#r "../../packages/FAKE/tools/FakeLib.dll"

open Fake

// Directories
let outDir = "./out"
let buildDir  = outDir + "/build/"
let testDir   = outDir + "/test/"
let nugetDir  = outDir + "/nuget/"

// tools
let fxCopRoot = "build/Tools/FxCop/FxCopCmd.exe"


Target "Clean" (fun _ -> 
    CleanDirs ["obj"; buildDir; testDir; nugetDir]
)

Target "PrepareBuild" (fun _ ->
    CreateDir buildDir
    CreateDir testDir
    CreateDir nugetDir
)

Target "Compile" (fun _ ->
   !! "./src/**/*.csproj"
    |> MSBuildRelease buildDir "Build"
    |> Log "AppBuild-Output: *"
 )

Target "FxCop" (fun _ ->
    !! (buildDir + "/**/*.dll")
        |> FxCop (fun p ->
            {p with
                ReportFileName = testDir + "FXCopResults.xml"
                ToolPath = fxCopRoot})
)

Target "CreateNuGetPackage"(fun _ ->
    NuGet (fun p -> 
       {p with
            Version = "1.0.0.0"
            OutputPath = nugetDir
            WorkingDir = @"."
            Publish = false })
        "./build/nuget/dng.ResumableUpload.nuspec"
)

"PrepareBuild"
    ==> "Clean"
    ==> "Compile"
    ==> "FxCop"
    ==> "CreateNuGetPackage"

// start build
RunTargetOrDefault "CreateNuGetPackage"