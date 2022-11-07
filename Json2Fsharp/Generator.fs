module Json2Fsharp.Generator

open System.Globalization
open System.IO
open System.Runtime.Serialization.Formatters.Binary
open System.Text.Json
open FSharp.Data
open FSharp.Data.Runtime.StructuralInference
open FSharp.Data.Runtime.StructuralTypes
open Newtonsoft.Json


module JsType =
    let rec collectRecords (root:FSharp.Data.Runtime.StructuralTypes.InferedType) = 
        let rec loop acc (currType:InferedType) =
            match currType with
            | InferedType.Record(stringOption, inferedProperties, optional) ->
                let curr = 
                    {
                        Name = stringOption
                        Props = inferedProperties
                        IsOptional = optional
                    }
                [
                    for prop in inferedProperties do
                        yield! collectRecords prop.Type
                    yield curr
                    yield! acc
                ]
//            | InferedType.Collection(inferedTypeTags, inferedTypeTagMap) ->
                // todo: implement

            | _ -> acc
        loop [] root

    let getRootNode (input:string) =
        let uom = Unchecked.defaultof<FSharp.Data.Runtime.StructuralInference.IUnitsOfMeasureProvider>
        let im = InferenceMode'.ValuesAndInlineSchemasOverrides
        let ci = CultureInfo.InvariantCulture
        let pn = "Root"
        let jv = JsonValue.Parse input
        let rootNode = Inference.inferType uom im ci pn jv
        rootNode





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
    
    