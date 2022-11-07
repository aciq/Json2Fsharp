[<Microsoft.FSharp.Core.AutoOpen>]
module Json2Fsharp.Helpers

open System.Text
open FSharp.Data.Runtime.StructuralTypes


[<RequireQualifiedAccess>]
module SB =
    let appendLine (txt: string) (sb: StringBuilder) = sb.AppendLine(txt)
    let append (txt: string) (sb: StringBuilder) = sb.Append(txt)



type JsType =
    {
        Name : string option
        Props : InferedProperty list
        IsOptional : bool
    }