#r @"packages/FAKE/tools/FakeLib.dll"
open System.IO
open Fake.Core
open Fake.IO
open Fake.DotNet

let directoriesToClean () =
    DirectoryInfo.getSubDirectories (DirectoryInfo.ofPath "./src/")
    |> Array.collect (fun x -> [|Path.Combine(x.FullName, "bin"); Path.Combine(x.FullName, "obj")|])

Target.create "Clean" (fun _ ->
    Trace.log "--- Cleaning stuff ---"
    Shell.cleanDirs (directoriesToClean())
)

Target.create "Restore" (fun _ ->
    Shell.Exec (Path.Combine(".paket", "paket.exe"), "restore", "") |> ignore
)

Target.create "Build" (fun _ -> 
    Trace.log "--- Building the app ---"
    DotNet.build id (Path.Combine("src", "PaketDemo"))
)

Target.create "Deploy" (fun _ -> Trace.log "--- Deploying the app ---")

open Fake.Core.TargetOperators

"Clean" ==> "Restore" ==> "Build" ==> "Deploy"

Target.runOrDefault "Deploy"
