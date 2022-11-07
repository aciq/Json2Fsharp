#r "nuget: Fantomas.Core, 5.0.6"

#r @"C:\Users\kast\source\myrepos\CodeGeneration\Aciq.CodeGeneration\bin\Debug\net7.0\Aciq.CodeGeneration.dll"
#r @"bin\Debug\net7.0\Json2Fsharp.dll"

open FSharp.Compiler.Syntax
open Fantomas.Core

let outpath =  __SOURCE_DIRECTORY__ + "/samples/out.fsx"

open Fantomas.Core.FormatConfig

let cfg = { FormatConfig.Default with StrictMode = true }

open Aciq.CodeGeneration
open Aciq.CodeGeneration.Modules

let nsm2 = SynModuleOrNamespace.CreateNamespace( Ident.createLong "LongName" )

let implFile = 
    Fa.ImplFile.Create [ nsm2 ]

let outputCode = 
    Fa.ImplFile.ToFormattedStringAsync implFile
    |> Async.RunSynchronously


#r "nuget: Newtonsoft.Json"
#r "nuget: FSharp.Data"
open System.IO
open Newtonsoft.Json
open Json2Fsharp
open FSharp.Data.Runtime.StructuralTypes



let sample1Text = __SOURCE_DIRECTORY__ + "/samples/sample.json" |> File.ReadAllText
    
let wad2 = Generator.JsType.getRootNode sample1Text    

wad2

let rootRecord = 
    match wad2 with 
    | InferedType.Record(name, props, optional) -> 
        {|
            Name = name
            Props = props
            Optional = optional
        |}
    | n -> 
        failwith $"fail {n}"


let basicProps = rootRecord.Props[1]


// Record
//        (Some "Class2",
//         [{ Name = "SomePropertyOfClass2"
//            Type = Primitive (System.String, None, false, false) }], false) }


let fields = SynField.Create(SynType.String(), Ident.create "Typename")

// seq<SynField>

let typedecl1 = SynTypeDefn.CreateRecord( Ident.create "RecordName", fields = [fields] )

let typelist = SynModuleDecl.Types([typedecl1], FSharp.Compiler.Text.Range.range0)

let newNamespace = 
    SynModuleOrNamespace.CreateNamespace( 
        Ident.createLong "LongName", 
        decls = [ typelist ] )

let implFile2 = Fa.ImplFile.Create [ newNamespace ]


let outputCode2 = 
    Fa.ImplFile.ToFormattedStringAsync implFile2
    |> Async.RunSynchronously

