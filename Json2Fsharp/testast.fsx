

#r @"C:\Users\kast\.nuget\packages\fsharp.compiler.service\41.0.6\lib\netstandard2.0\FSharp.Compiler.Service.dll"
#r "nuget: FSharp.Compiler.Service"

open FSharp.Compiler.Text
open FSharp.Compiler.Syntax
open FSharp.Compiler.Xml




module Ident =

    let ofString str =
        Ident(str, range.Zero)

    let toLongIdent ident : LongIdent =
        [ ident ]

    let toLongIdentWithDots ident =
        SynLongIdent.SynLongIdent (toLongIdent ident, [] , [])



module Field =

    let create name typ =
        SynField.SynField (
            [],
            false,
            name
                |> Ident.ofString
                |> Some,
            SynType.LongIdent (
                typ
                    |> Ident.ofString
                    |> Ident.toLongIdentWithDots),
            false,
            PreXmlDoc.Empty,
            None,
            range.Zero)

let test1  =
    SynComponentInfo.SynComponentInfo([], None, [],
            "City"
                |> Ident.ofString
                |> Ident.toLongIdent,
            PreXmlDoc.Empty,
            false,
            None,
            range.Zero)


