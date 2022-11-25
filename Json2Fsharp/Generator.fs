module Json2Fsharp.Generator

open System.Collections.Generic
open System.Globalization
open System.IO
open System.Runtime.Serialization.Formatters.Binary
open System.Text.Json
open Aciq.FsCodegen.Fau.Common
open Aciq.FsCodegen.Modules
open FSharp.Compiler.Syntax
open FSharp.Data
open FSharp.Data.Runtime.StructuralInference
open FSharp.Data.Runtime.StructuralTypes
open Newtonsoft.Json
open Aciq.FsCodegen


let convertType (typedict:IDictionary<string,(string * SynType) list>) (currentRecord:InferedType) =
    match currentRecord with 
    | InferedType.Record(name, props, optional) ->
        let name = name.Value
        let fields = props |> List.map (fun f ->
            let name = f.Name
            name,(SType.ofInferedType f.Type)
        )
        
        let fieldExprs =
            fields |> List.map (fun (nam,typ) ->
                nam,
                let idents = SType.idents typ
                match typedict.TryGetValue(idents.Head) with
                | true, _ when idents.Length = 1 && optional  ->
                    let defaultv = SynExpr.CreateLongIdent(Ident.createLongSyn [nam ; "Default"])
                    SynExpr.CreateApp(SynExpr.CreateIdentString("Some"),defaultv) 
                | true, _ when idents.Length = 1 ->
                    SynExpr.CreateLongIdent(Ident.createLongSyn [nam ; "Default"])  
                | _ -> (Fa.Defaults.typeValueDict typedict typ) 
            )

        let createrec = Fa.Expr.createRecord fieldExprs
        
        let createDefault =
            Fa.Member.CreateStatic(
                "Default",
                [],
                Some(Fa.RetnInfo.Create(SynType.Create name)),
                createrec
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
        SynTypeDefn.CreateRecord( Ident.create name, fields = fsfields, members = createDefault  )
    | _ -> failwith "not impl"
        

let generateFSharp (inputJson:string) =
    let rootNode = JsType.getRootNode inputJson

    let tostr = JsonConvert.SerializeObject rootNode
    let rootjson =  __SOURCE_DIRECTORY__ + "/samples/typed.json"
    
    File.WriteAllText(rootjson,tostr)
    
    let formatter = new BinaryFormatter()
    
    
    let js2 = rootNode 
    

    let outpath =  __SOURCE_DIRECTORY__ + "/samples/out.fsx" 

//    let a1 = JsType.collectRecords rootNode
    
//    let generatedCode = AST.testGenerate()  |> Async.RunSynchronously
    let v = 1
    ()
//    File.WriteAllText(outpath,generatedCode)
    
    