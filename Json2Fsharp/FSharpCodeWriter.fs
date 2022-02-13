namespace Json2Fsharp

open System
open System.Collections.Generic
open System.Text
open Xamasoft.JsonClassGenerator

type NamespaceComparer() =
    let (|Prefix|_|) (p: string) (s: string) =
        if s.StartsWith(p, StringComparison.Ordinal) then
            Some(s.Substring(p.Length))
        else
            None

    interface IComparer<string> with
        member this.Compare(x, y) =
            match x, y with
            | "System", _ -> -1
            | _, "System" -> 1
            | Prefix "System." _, Prefix "System." _ -> StringComparer.Ordinal.Compare(x, y)
            | Prefix "System." restofx, y -> -1
            | x, Prefix "System." restofy -> 1
            | x, y -> StringComparer.Ordinal.Compare(x, y)


type FSharpCodeWriter() =
    let _namespaceComparer = NamespaceComparer()
    let appendLine (txt: string) (sb: StringBuilder) = sb.AppendLine(txt)
    let append (txt: string) (sb: StringBuilder) = sb.Append(txt)

    let getCollectionTypeName (elementTypeName: string) (outType: OutputCollectionType) =
        match outType with
        | OutputCollectionType.Array -> elementTypeName + "[]"
        | OutputCollectionType.MutableList -> "List<" + elementTypeName + ">"
        | OutputCollectionType.IReadOnlyList -> "IReadOnlyList<" + elementTypeName + ">"
        | OutputCollectionType.ImmutableArray -> "ImmutableArray<" + elementTypeName + ">"
        | _ -> failwithf $"unknown type: %A{outType}"

    let _reservedKeywords =
        seq {
            "abstract"
            "as"
            "base"
            "bool"
            "break"
            "byte"
            "case"
            "catch"
            "char"
            "checked"
            "class"
            "const"
            "continue"
            "decimal"
            "default"
            "delegate"
            "do"
            "double"
            "else"
            "enum"
            "event"
            "explicit"
            "extern"
            "false"
            "finally"
            "fixed"
            "float"
            "for"
            "foreach"
            "goto"
            "if"
            "implicit"
            "in"
            "int"
            "interface"
            "internal"
            "is"
            "lock"
            "long"
            "namespace"
            "new"
            "null"
            "object"
            "operator"
            "out"
            "override"
            "params"
            "private"
            "protected"
            "public"
            "readonly"
            "ref"
            "return"
            "sbyte"
            "sealed"
            "short"
            "sizeof"
            "stackalloc"
            "static"
            "string"
            "struct"
            "switch"
            "this"
            "throw"
            "true"
            "try"
            "typeof"
            "uint"
            "ulong"
            "unchecked"
            "unsafe"
            "ushort"
            "using"
            "virtual"
            "void"
            "volatile"
            "while"
        }
        |> HashSet<string>


    let getTypeIndent (config: IJsonClassGeneratorConfig) (typeIsRoot: bool) =
        if (typeIsRoot) then
            "" // nothing
        else
            "" //



    member this.ShouldApplyNoPruneAttribute(config: IJsonClassGeneratorConfig) : bool =
        config.ApplyObfuscationAttributes
        && (config.OutputType = OutputTypes.MutableClass
            && config.MutableClasses.Members = OutputMembers.AsPublicFields)

    member this.ShouldApplyNoRenamingAttribute(config: IJsonClassGeneratorConfig) : bool =
        config.ApplyObfuscationAttributes
        && not config.UsePascalCase


    member this.GetCamelCaseName(camelCaseFromJson: byref<string>) =
        // TODO: rewrite in a coherent way
        
        if (String.IsNullOrEmpty(camelCaseFromJson)) then
            failwith "??"
        else

            let mutable name = camelCaseFromJson

            if (name.Length >= 3) then
                if (Char.IsUpper(name[0])
                    && Char.IsUpper(name[1])
                    && Char.IsLower(name[2])) then
                    // "ABc" --> "abc" // this may be wrong in some cases, if the first two letters are a 2-letter acronym, like "IO".
                    name <-
                        name.Substring(0, 2).ToLowerInvariant()
                        + name.Substring(2)
                else if (Char.IsUpper(name[0])) then
                    // "Abc" --> "abc"
                    // "AbC" --> "abC"
                    name <-
                        Char.ToLower(name[0]).ToString()
                        + name.Substring(1)
            else if (name.Length = 2) then
                if (Char.IsUpper(name[0])) then
                    name <- name.ToLowerInvariant()
            else name <- name.ToLowerInvariant()

            if (not (Char.IsLetter(name[0]))) then
                name <- "_" + name
            else if ((this :> ICodeBuilder).IsReservedKeyword(name)) then
                name <- "@" + name

            name

    member this.GetPascalCaseName(inputname: string) =
        let mutable name = inputname

        if (this :> ICodeBuilder).IsReservedKeyword name then
            name <- "@" + name
        // Check if property name starts with number
        if (not (String.IsNullOrEmpty(name))
            && Char.IsDigit(name[0])) then
            name <- "_" + name

        name

    member this.WriteClassConstructor
        (
            config: IJsonClassGeneratorConfig,
            sw: StringBuilder,
            ``type``: JsonType,
            indentMembers: string,
            indentBodies: string
        ) =
        // Write an empty constructor on a single-line:
        if (``type``.Fields.Count = 0) then
            sw.AppendFormat(indentMembers + "public {0}() {{}}{1}", ``type``.AssignedName, Environment.NewLine)
            |> ignore

            ()

        // Constructor signature:
        match config.AttributeLibrary with
        | JsonLibrary.NewtonsoftJson
        | JsonLibrary.SystemTextJson ->
            sw.AppendLine(indentMembers + "[JsonConstructor]")
            |> ignore
        | _ -> failwithf $"unknown library %A{config.AttributeLibrary}"

        sw.AppendFormat(indentMembers + "public {0} = {({1}", ``type``.AssignedName, Environment.NewLine)
        |> ignore

        let lastField = ``type``.Fields |> Seq.last 

        for field in ``type``.Fields do
            // Writes something like: `[JsonProperty("foobar")] string foobar,`

            let ctorParameterName =
                this.GetCamelCaseName(ref field.MemberName)

            let isLast = Object.ReferenceEquals(field, lastField)
            let comma = if isLast then "" else ","

            //

            sw.Append(indentBodies) |> ignore


            let attribute = config.GetCSharpJsonAttributeCode(field)

            if (attribute.Length > 0) then
                sw.Append(attribute) |> ignore

                sw.Append(' ') |> ignore


            // e.g. `String foobar,\r\n`
            sw.AppendFormat("{0} {1}{2}{3}", field.Type.GetTypeName(), ctorParameterName, comma, Environment.NewLine)
            |> ignore


    interface ICodeBuilder with
        member this.DisplayName = "F#"
        member this.FileExtension = ".fs"

        member this.GetTypeName(``type``: JsonType, config) =
            match ``type``.Type with
            | JsonTypeEnum.Anything -> "obj"
            | JsonTypeEnum.Array ->
                getCollectionTypeName
                    ((this :> ICodeBuilder)
                        .GetTypeName(``type``.InternalType, config))
                    config.CollectionType
            | JsonTypeEnum.Dictionary ->
                "Dictionary<string, "
                + ((this :> ICodeBuilder)
                    .GetTypeName(``type``.InternalType, config)
                   + ">")
            | JsonTypeEnum.Boolean -> "bool"
            | JsonTypeEnum.Float -> "double"
            | JsonTypeEnum.Integer -> "int"
            | JsonTypeEnum.Long -> "long"
            | JsonTypeEnum.Date -> "DateTime"
            | JsonTypeEnum.NonConstrained -> "obj"
            | JsonTypeEnum.NullableBoolean -> "bool?"
            | JsonTypeEnum.NullableFloat -> "double?"
            | JsonTypeEnum.NullableInteger -> "int?"
            | JsonTypeEnum.NullableLong -> "long?"
            | JsonTypeEnum.NullableDate -> "DateTime?"
            | JsonTypeEnum.NullableSomething -> "obj"
            | JsonTypeEnum.Object -> ``type``.NewAssignedName
            | JsonTypeEnum.String -> "string"
            | _ -> failwithf $"unknown type %A{``type``}"

        member this.IsReservedKeyword(word) =
            match word with
            | null -> false
            | str -> _reservedKeywords.Contains(str)

        member this.ReservedKeywords = _reservedKeywords

        member this.WriteClass(config, sw, ``type``) =
            let indentTypes = getTypeIndent config ``type``.IsRoot
            let indentMembers = indentTypes + "    "
            let indentBodies = indentMembers + "    "
            let visibility = "" //let visibility = " private";
            let className = ``type``.AssignedName

            // fsharp CLIMutable record
            if config.OutputType = OutputTypes.MutableClass then
                sw
                |> appendLine $"{indentTypes}[<CLIMutable>]"
                |> appendLine $"{indentTypes}type{visibility} {className} = {{"
                |> (fun sb ->
                    (this :> ICodeBuilder)
                        .WriteClassMembers(config, sw, ``type``, indentMembers)

                    sb)
                |> appendLine (indentTypes + $"}}")
                |> ignore


            // fsharp default record
            if (config.OutputType = OutputTypes.ImmutableRecord) then
                sw
                |> appendLine (indentTypes + $"type{visibility} {className} = {{")
                |> ignore
            else

                if (config.OutputType = OutputTypes.ImmutableClass) then
                    this.WriteClassConstructor(config, sw, ``type``, indentMembers, indentBodies)

                if (config.OutputType = OutputTypes.ImmutableRecord) then
                    sw.AppendLine(indentTypes + ");") |> ignore



                sw.AppendLine() |> ignore



        member this.WriteClassMembers(config, sw, ``type``: JsonType, indentMembers) =
            let mutable first = true

            for field: FieldInfo in ``type``.Fields do
                let classPropertyName = this.GetPascalCaseName(field.MemberName)
                let propertyAttribute = config.GetCSharpJsonAttributeCode(field)

                if config.ExamplesInDocumentation then
                    let exampleText =
                        field.GetExamplesText()
                        |> (fun f -> f.Substring(min f.Length 100)) // max 100 chars
                    sw
                    |> appendLine $"{indentMembers}/// e.g. {exampleText}"
                    |> ignore

                if (propertyAttribute.Length > 0) then
                    sw.Append(indentMembers) |> ignore
                    sw.Append(propertyAttribute) |> ignore

                    if (config.OutputType <> OutputTypes.ImmutableRecord) then
                        sw.AppendLine() |> ignore

                // record types is not compatible with UseFields, so it comes first
                if (config.OutputType = OutputTypes.ImmutableRecord) then
                    // NOTE: not adding newlines here, that happens at the start of the loop. We need this so we can lazily add commas at the end.
                    if (field.Type.Type = JsonTypeEnum.Array) then
                        sw.AppendFormat(
                            " IReadOnlyList<{0}> {1}",
                            (this :> ICodeBuilder)
                                .GetTypeName(field.Type.InternalType, config),
                            classPropertyName
                        )
                        |> ignore
                    else
                        sw.AppendFormat(" {0} {1}", field.Type.GetTypeName(), classPropertyName)
                        |> ignore

                else if (config.OutputType = OutputTypes.MutableClass) then
                    if (config.MutableClasses.Members = OutputMembers.AsPublicFields) then
                        // Render a field like `public int Foobar;`:

                        let useReadonlyModifier =
                            config.OutputType = OutputTypes.ImmutableClass

                        let ternary =
                            if useReadonlyModifier then
                                "readonly "
                            else
                                ""

                        sw.AppendFormat(
                            indentMembers + "public {0}{1} {2};{3}",
                            ternary,
                            field.Type.GetTypeName(),
                            classPropertyName,
                            Environment.NewLine
                        )
                        |> ignore

                    else if (config.MutableClasses.Members = OutputMembers.AsProperties) then
                        // fsharp record field
                        sw
                        |> appendLine $"{indentMembers}{classPropertyName} : {field.Type.GetTypeName()}"
                        |> ignore
                    else
                        let PATH =
                            nameof (config)
                            + "."
                            + nameof (config.MutableClasses)
                            + "."
                            + nameof (config.MutableClasses.Members)

                        let MSG_FMT =
                            "Invalid "
                            + nameof (OutputMembers)
                            + " enum value for "
                            + PATH
                            + ": {0}"

                        failwith MSG_FMT
                else if (config.OutputType = OutputTypes.ImmutableClass) then
                    if (field.Type.Type = JsonTypeEnum.Array) then
                        // TODO: Respect config.CollectionType
                        sw.AppendFormat(
                            indentMembers
                            + "public IReadOnlyList<{0}> {1} {{ get; }}{2}",
                            (this :> ICodeBuilder)
                                .GetTypeName(field.Type.InternalType, config),
                            classPropertyName,
                            Environment.NewLine
                        )
                        |> ignore

                    else
                        sw.AppendFormat(
                            indentMembers + "public {0} {1} {{ get; }}{2}",
                            field.Type.GetTypeName(),
                            classPropertyName,
                            Environment.NewLine
                        )
                        |> ignore

                else
                    raise
                        (InvalidOperationException(
                            "Invalid "
                            + nameof (OutputTypes)
                            + " value: "
                            + config.OutputType.ToString()
                        )
                            )

                first <- false

            // emit a final newline if we're dealing with record types
            if (config.OutputType = OutputTypes.ImmutableRecord) then
                sw.AppendLine() |> ignore

        member this.WriteDeserializationComment(config, sw) = ()
        member this.WriteFileEnd(config, sw) = ()

        member this.WriteFileStart(config, sw) =
            if config.UseNestedClasses then
                failwithf $"%s{nameof config.UseNestedClasses} is not supported"
            if not (String.IsNullOrWhiteSpace(config.SecondaryNamespace)) then
                failwithf $"%s{nameof config.SecondaryNamespace} is not supported"

            sw
            |> appendLine $"namespace {config.Namespace}"
            |> appendLine ""
            |> appendLine ""
            |> ignore

            
            let comparer = NamespaceComparer() :> IComparer<string>

            if (config.HasNamespace()) then
                let importNamespaces =
                    [ "System"
                      "System.Collections.Generic" ]
                    |> ResizeArray

                if (this.ShouldApplyNoPruneAttribute(config)
                    || this.ShouldApplyNoRenamingAttribute(config)) then
                    importNamespaces.Add("System.Reflection")

                match config.AttributeLibrary with
                | JsonLibrary.NewtonsoftJson ->
                    importNamespaces.Add("Newtonsoft.Json")
                    importNamespaces.Add("Newtonsoft.Json.Linq")
                | JsonLibrary.SystemTextJson -> importNamespaces.Add("System.Text.Json")
                | _ -> failwithf $"unknown library %A{config.AttributeLibrary}"
                
                importNamespaces.Sort(comparer)

                for ns in importNamespaces do
                    sw |> appendLine $"open {ns}" |> ignore
                    
                sw |> appendLine "" |> ignore
                


        member this.WriteNamespaceEnd(config, sw, root) = ()
        member this.WriteNamespaceStart(config, sw, root) = ()
