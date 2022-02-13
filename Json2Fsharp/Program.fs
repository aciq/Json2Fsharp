

open System
open System.IO
open CommandLine
open Json2Fs
open Xamasoft.JsonClassGenerator

[<Verb(name="gen",isDefault=true)>]
type Options = {
  [<Option('e', "noexamples", HelpText = "Dont write examples in documentation (e.g. \"2021-01-13 15:19:15\")")>] ``noexamples`` : bool;
  [<Option('n', "ns", Default="MyNamespace", HelpText = "Namespace")>] ``namespace`` : string;
  [<Value(0, MetaName="input",Required=true, HelpText = "Input file path")>] input : string;
  [<Value(1, MetaName="output",Required=true, HelpText = "Output file path")>] output : string;
}


[<EntryPoint>]
let main argv =
    
    let parser = new Parser(fun f -> f.HelpWriter <- Console.Out )
    let result = parser.ParseArguments<Options>(argv)
        
    match result with
    | :? CommandLine.Parsed<Options> as command ->
        let value = command.Value
        let path       = value.input
        let outputPath = value.output
        let input      = File.ReadAllText(path);
        let writer = FSharpCodeWriter()
        let generator = JsonClassGenerator()
        generator.ExamplesInDocumentation <- not value.noexamples
        generator.Namespace <- value.``namespace``
        generator.AttributeLibrary <- JsonLibrary.SystemTextJson
        generator.CodeWriter <- writer
        
        
        match generator.GenerateClasses(input) with
        | _, error when error <> "" -> failwith error
        | generatedFile , _ -> File.WriteAllText(outputPath ,generatedFile.ToString())
        
        printfn $"successfully created {value.output}"
        
    | :? CommandLine.NotParsed<Options> as notparsed -> ()
    | _ -> failwithf $"unknown command: %A{result}"
    0
