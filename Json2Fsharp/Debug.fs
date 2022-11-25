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
    
    
    let loop (typedict:IDictionary<string,_>) (currentRecord:InferedType) =
        match currentRecord with 
        | InferedType.Record(name, props, optional) ->
            let name = name.Value
            let fields =
                props
                |> List.map (fun f ->
                    let name = f.Name
                    name,(SType.ofInferedType f.Type)
                )
            let mems =
                Fa.Member.CreateStatic(
                    "Default",
                    [],
                    Some(Fa.RetnInfo.Create(SynType.Create name)),
                    SynExpr.CreateRecord(
                        [
                            let rfs : (RecordFieldName * SynExpr option) list  = 
                                fields |> List.map (fun (nam,typ) ->
                                    let v = 1
                                    RecordFieldName(Ident.createLongSyn [nam], true),
                                    let idents = SType.idents typ
                                    match typedict.TryGetValue(idents.Head) with
                                    | true, props when idents.Length = 1 && optional  ->
                                        let defaultv = SynExpr.CreateLongIdent(Ident.createLongSyn [nam ; "Default"])
                                        SynExpr.CreateApp(SynExpr.CreateIdentString("Some"),defaultv) |> Some
                                    | true, props when idents.Length = 1 ->
                                        SynExpr.CreateLongIdent(Ident.createLongSyn [nam ; "Default"]) |> Some  
                                    | _ -> (Fa.Defaults.typeValueDict typedict typ) |> Some
                                )
                            yield! rfs
                        ]    
                    ) 
                )
                |> List.singleton
                
            let fsfields = fields |> List.map (fun (nam,typ) ->
                let idents = SType.idents typ
                match typedict.TryGetValue(idents.Head) with
                | true, props when idents.Length = 1 ->
                    if optional then
                        Fa.Field.Create nam (SynType.Option(typ,true))
                    else
                        Fa.Field.Create nam typ
                | _ -> 
                    Fa.Field.Create nam typ
            )
            SynTypeDefn.CreateRecord( Ident.create name, fields = fsfields, members = mems  )
            
          
        | _ -> failwith $"invalid type {rootNode}" 

    
    
    let jsTypes =
        JsType.collectRecords rootNode
        |> List.distinctBy (fun f -> f.Name,f.Props)
    
    let typeDict =
        jsTypes
        |> List.map (fun f -> f.Name.Value,  f.Props |> List.map (fun f -> f.Name, SType.ofInferedType f.Type)) 
        |> dict
    
    let fsTypes =
        jsTypes
        |> List.map (fun f -> (loop typeDict f.InferedType) )
                
    
    let jts = 1
    
    
    let ns2 = Fa.Namespace.ofTypes "NsName" fsTypes
    
    
    let implFile2 = Fa.ImplFile.Create [ ns2 ]

    let outputCode2 = 
        Fa.ImplFile.ToFormattedStringAsync implFile2
        |> Async.RunSynchronously
    
    File.WriteAllText("/home/ian/f/publicrepos/json2fsharp/Json2Fsharp/samples/out.fsx", outputCode2)  
    ()
    