[<AutoOpen>]
module Json2Fsharp.Common

open System.Globalization
open FSharp.Compiler.Syntax
open FSharp.Data
open FSharp.Data.Runtime.StructuralInference
open FSharp.Data.Runtime.StructuralTypes
open Aciq.FsCodegen

type JsType =
    {
        Name : string option
        Props : InferedProperty list
        IsOptional : bool
        InferedType : InferedType
    }
    

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
                        InferedType = currType
                    }
                [
                    for prop in inferedProperties do
                        yield! collectRecords prop.Type
                    yield curr
                    yield! acc
                ]
            | InferedType.Collection(inferedTypeTags, inferedTypeTagMap) ->
                let inner =
                    inferedTypeTagMap
                    |> Seq.collect (fun f -> loop [] (snd f.Value))
                    |> Seq.toList
                inner @ acc
            | _ -> acc
        loop  [] root

    let getRootNode (input:string) =
        let uom = Unchecked.defaultof<FSharp.Data.Runtime.StructuralInference.IUnitsOfMeasureProvider>
        let im = InferenceMode'.ValuesAndInlineSchemasOverrides
        let ci = CultureInfo.InvariantCulture
        let pn = "Root"
        let jv = JsonValue.Parse input
        let rootNode = Inference.inferType uom im ci pn jv
        rootNode


