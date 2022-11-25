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
    Debug.run()
    let parser =
        new Parser(fun f -> f.HelpWriter <- Console.Out)
    
    if true then 0 else
    
#if DEBUG    
    // debugging
    let argv =
        [| __SOURCE_DIRECTORY__ + "/samples/sample2.json" |]
#endif    
    
    let result = parser.ParseArguments<Options>(argv)

    match result with
    | :? CommandLine.Parsed<Options> as command ->
        let value = command.Value
        let path = value.input
//        let outputPath = value.output
        let input = File.ReadAllText(path)
        
        let result = Generator.generateFSharp input
                
        let t12 = 1

        ()
//        match generator.GenerateClasses(input) with
//        | _, error when error <> "" -> failwith error
//        | generatedFile, _ ->
//            generatedFile |> string |> stdout.Write 


    | :? CommandLine.NotParsed<Options> as notparsed -> ()
    | _ -> failwithf $"unknown command: %A{result}"

    0
