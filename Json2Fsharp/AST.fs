module Json2Fsharp.AST
//
//open FSharp.Compiler.EditorServices
//open FSharp.Compiler.Text
//open FSharp.Compiler.Syntax
//open FSharp.Compiler.Xml
//open Fantomas
//open Myriad.Core.Ast
//open Myriad.Core
//open Fantomas.Core
//
//
//module Ident =
//
//    let ofString str =
//        Ident(str, range.Zero)
//
//    let toLongIdent ident : LongIdent =
//        [ ident ]
//
////    let toLongIdentWithDots ident =
////        SynLongIdent.SynLongIdent (toLongIdent ident, [] , [])
//
//
//let test1  =
//    SynComponentInfo.SynComponentInfo([], None, [],
//            "City"
//                |> Ident.ofString
//                |> Ident.toLongIdent,
//            PreXmlDoc.Empty,
//            false,
//            None,
//            range.Zero)
//
//
//
//
//let formatAst ast =
//    let cfg = { FormatConfig.FormatConfig.Default with StrictMode = true } // no comments
//    let noOriginalSourceCode = "//"
//    CodeFormatter.FormatASTAsync(ast,noOriginalSourceCode,cfg)
//    
//open FSharp.Compiler.Syntax
//open FSharp.Compiler.Text.Range
//open FSharp.Compiler.Text.Range
//open Myriad.Core
//open SynLongIdentHelpers
//let createOuter  =
//        
//        
//        
//        let ident =
//                Ident.ofString "Yello"
//                |> Ident.toLongIdent
//        let openTarget = SynOpenDeclTarget.ModuleOrNamespace(ident, range0)
//        let openParent = SynModuleDecl.Open(openTarget,Range.Zero)
//        
//        let recId =
//                
//                Ident.ofString "aaaaasd"
//                |> Ident.toLongIdent
//
//        
//        let decls = [ yield openParent ]
//        let info = SynComponentInfo.Create recId
//        let mdl = SynModuleDecl.CreateNestedModule(info, decls)
//        
//        mdl
//
//let ns =
//    let li = Ident.ofString "Json2FSharp" |> Ident.toLongIdent
//    SynModuleOrNamespace.CreateNamespace(li, isRecursive = true, decls = [ createOuter ])
//    
//let testGenerate() =
//    let cfg2 =
//        { Fantomas.FormatConfig.FormatConfig.Default with StrictMode = true } // no comments
//    let noOriginalSourceCode = "//"
//    
//    let ast = Myriad.Core.Output.Ast [ ns ]
//     
//    let parseTree = ParsedInput.ImplFile(ParsedImplFileInput.CreateFs("weewoo.fsx", modules = [ns]))
//    Fantomas.CodeFormatter.FormatASTAsync(parseTree, "myriad.fsx", [], None, cfg2)
//        
