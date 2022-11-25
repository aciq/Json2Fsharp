module Json2Fsharp.Debug

open System.Collections.Generic
open System.IO
open Aciq.FsCodegen.Fau.Common
open Aciq.FsCodegen.Modules
open Aciq.FsCodegen.Extensions
open Aciq.FsCodegen.Extensions.Extensions
open Aciq.FsCodegen
open FSharp.Compiler.Syntax
open FSharp.Data.Runtime.StructuralTypes
open Json2Fsharp.Generator


let run() =
    let inputJson = __SOURCE_DIRECTORY__ + "/samples/sample2.json" |> File.ReadAllText
    let rootNode = JsType.getRootNode inputJson
    
    let jsTypes =
        JsType.collectRecords rootNode
        |> List.distinctBy (fun f -> f.Name,f.Props)
    
    let typeDict =
        jsTypes
        |> List.map (fun f -> f.Name.Value,  f.Props |> List.map (fun f -> f.Name, SType.ofInferedType f.Type)) 
        |> dict
    
    let fsTypes =
        jsTypes
        |> List.map (fun f -> (convertType typeDict f.InferedType) )
                
    
    let jts = 1
    
    
    let ns2 = Fa.Namespace.ofTypes "NsName" fsTypes
    
    
    let implFile2 = Fa.ImplFile.Create [ ns2 ]

    let outputCode2 = 
        Fa.ImplFile.ToFormattedStringAsync implFile2
        |> Async.RunSynchronously
    
    File.WriteAllText("/home/ian/f/publicrepos/json2fsharp/Json2Fsharp/samples/out.fsx", outputCode2)  
    ()
    