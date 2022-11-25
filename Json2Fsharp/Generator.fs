module Json2Fsharp.Generator

open System.Globalization
open System.IO
open System.Runtime.Serialization.Formatters.Binary
open System.Text.Json
open FSharp.Data
open FSharp.Data.Runtime.StructuralInference
open FSharp.Data.Runtime.StructuralTypes
open Newtonsoft.Json





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
    
    