open System
open System.Globalization
open System.IO
open Aciq.CodeGeneration.Modules
open CommandLine
open FSharp.Compiler.Syntax
open FSharp.Data
open FSharp.Data.Runtime.StructuralInference
open FSharp.Data.Runtime.StructuralTypes
open Json2Fsharp
open Json2Fsharp.Generator

[<Verb(name = "gen", isDefault = true)>]
type Options =
    { [<Option('x', "noxmltags", HelpText = "Omit examples in xml tags")>]
      NoXmlTags: bool
      [<Option('n', "namespace", Default = "MyNamespace", HelpText = "Namespace")>]
      ``namespace``: string
      [<Value(0, MetaName = "input", Required = true, HelpText = "Input file path (e.g. file.json)")>]
      input: string
    }



open Aciq.CodeGeneration
let debugTest() =
    let inputJson = __SOURCE_DIRECTORY__ + "/samples/sample.json" |> File.ReadAllText
    let rootNode = JsType.getRootNode inputJson
    
    let rootRecord = 
        match rootNode with 
        | InferedType.Record(name, props, optional) -> 
            {|
                Name = name
                Props = props
                Optional = optional
            |}
        | n -> 
            failwith $"fail {n}"
    
    let prop2 = rootRecord.Props[1]
            
    let synTypeOfInferedType (it:InferedType) =
        match it with
        | InferedType.Primitive(``type``, typeOption, optional, shouldOverrideOnMerge) ->
            if ``type`` = typeof<string> then SynType.String()
            elif ``type`` = typeof<int> then SynType.Int()
            else failwith $"invalid type {``type``}" 
        | t -> failwith $"invalid type {t}"              
    
    let loop (currentRecord:InferedType) =
        match currentRecord with 
        | InferedType.Record(name, props, optional) ->
            let name = name.Value
            let fields =
                props
                |> List.map (fun f ->
                    Fa.Field.Create f.Name (synTypeOfInferedType f.Type)
                )
            SynTypeDefn.CreateRecord( Ident.create name, fields = fields )
          
        | _ -> failwith $"invalid type {rootNode}" 
        
    
//    let fields = SynField.Create(SynType.String(), Ident.create "Typename")
//    let typedecl1 = SynTypeDefn.CreateRecord( Ident.create "RecordName", fields = [fields] )
    
    let newrecord = loop prop2.Type
    let ns2 = Fa.Namespace.ofTypes "NsName" [newrecord]
    
    
    
    let implFile2 = Fa.ImplFile.Create [ ns2 ]

    let outputCode2 = 
        Fa.ImplFile.ToFormattedStringAsync implFile2
        |> Async.RunSynchronously
    
    let rdy = 1    
    ()
    
    
[<EntryPoint>]
let main argv =
    debugTest()
    let parser =
        new Parser(fun f -> f.HelpWriter <- Console.Out)
    
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
