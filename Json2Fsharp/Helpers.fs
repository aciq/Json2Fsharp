[<Microsoft.FSharp.Core.AutoOpen>]
module Json2Fsharp.Helpers

open System
open System.Collections.Generic
open System.Text
open Aciq.FsCodegen.Fau.Common
open FSharp.Compiler.Syntax
open FSharp.Data.Runtime.StructuralTypes
open Aciq.FsCodegen

[<RequireQualifiedAccess>]
module SB =
    let appendLine (txt: string) (sb: StringBuilder) = sb.AppendLine(txt)
    let append (txt: string) (sb: StringBuilder) = sb.Append(txt)



    
module SType =
    
    
    let ofInferedType (it:InferedType) =
        match it with
        | InferedType.Primitive(``type``, typeOption, optional, shouldOverrideOnMerge) ->
            if ``type`` = typeof<string> then SynType.String()
            elif ``type`` = typeof<int> then SynType.Int()
            elif ``type`` = typeof<int64> then SynType.Int64()
            elif ``type`` = typeof<obj> then SynType.Option("obj")
            elif ``type`` = typeof<Bit0> then SynType.Int()
            elif ``type`` = typeof<Bit1> then SynType.Int()
            elif ``type`` = typeof<bool> then SynType.Bool()
            elif ``type`` = typeof<float> then SynType.Float()
            elif ``type`` = typeof<decimal> then SynType.Decimal()
            elif ``type`` = typeof<DateTime> then SynType.DateTime()
            else failwith $"invalid type {``type``}"
        | InferedType.Record(stringOption, inferedProperties, optional) ->
            SynType.CreateLongIdent stringOption.Value
        | InferedType.Collection(inferedTypeTags, inferedTypeTagMap) ->
            let list = SynType.Create "list"
            let rname =
                inferedTypeTags
                |> Seq.head
                |> (fun f -> f.Code.Substring(f.Code.IndexOf("@") + 1))
                |> function
                | "String" -> "string" 
                | n -> n
            let inner = SynType.Create rname 
            let list = SynType.CreateApp(list, [ inner ], true)
            list
        | t -> failwith $"invalid type {t}"     
    let lastName (fieldType) =
        match fieldType with
        | SynType.LongIdent synLongIdent -> 
            synLongIdent.LongIdent
            |> List.last
            |> (fun f -> f.idText)
        | _ -> failwith "todo"
        
    