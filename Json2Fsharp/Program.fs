open System
open System.Globalization
open System.IO
open Aciq.FsCodegen.Modules
open CommandLine
open FSharp.Compiler.Syntax
open FSharp.Data
open FSharp.Data.Runtime.StructuralInference
open FSharp.Data.Runtime.StructuralTypes
open Json2Fsharp
open Json2Fsharp.Generator
open Aciq.FsCodegen
open Aciq.FsCodegen

[<Verb(name = "gen", isDefault = true)>]
type Options =
    { [<Option('x', "noxmltags", HelpText = "Omit examples in xml tags")>]
      NoXmlTags: bool
      [<Option('n', "namespace", Default = "MyNamespace", HelpText = "Namespace")>]
      ``namespace``: string
      [<Value(0, MetaName = "input", Required = true, HelpText = "Input file path (e.g. file.json)")>]
      input: string
    }


    
[<EntryPoint>]
let main argv =
    // Debug.run()
    let parser =
        new Parser(fun f -> f.HelpWriter <- Console.Out)
    
#if DEBUG    
    // debugging
    // let argv = [| __SOURCE_DIRECTORY__ + "/samples/sample2.json" |]
#endif    
    // stdout.WriteLine $"reading {argv[0]}"
    let input = argv[0] |> File.ReadAllText
    let rootNode = JsType.getRootNode input
    
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
             
    
    let ns2 = Fa.Namespace.ofTypes "NsName" fsTypes
    
    let implFile2 = Fa.ImplFile.Create [ ns2 ]

    let outputCode2 = 
        Fa.ImplFile.ToFormattedStringAsync implFile2
        |> Async.RunSynchronously
        
    stdout.Write(outputCode2)
    
    match parser.ParseArguments<Options>(argv) with
    | :? CommandLine.Parsed<Options> as command -> ()
    | :? CommandLine.NotParsed<Options> as notparsed -> ()
    | n -> failwithf $"unknown command: %A{n}"

    0
