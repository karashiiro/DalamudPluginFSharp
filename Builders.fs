namespace DalamudPluginProjectTemplateFSharp

[<AutoOpen>]
module internal Builders =
    open System

    //#region Public domain code from FSharpx.Extras' Option.fs
    type MaybeBuilder() =
        member _.Return(x) = Some x

        member _.ReturnFrom(m: 'T option) = m

        member _.Bind(m, f) = Option.bind f m

        member _.Zero() = None

        member _.Combine(m, f) = Option.bind f m

        member _.Delay(f: unit -> _) = f

        member _.Run(f) = f ()

        member this.TryWith(m, h) =
            try
                this.ReturnFrom(m)
            with
            | e -> h e

        member this.TryFinally(m, compensation) =
            try
                this.ReturnFrom(m)
            finally
                compensation ()

        member this.Using(res: #IDisposable, body) =
            this.TryFinally(
                body res,
                (fun () ->
                    if not (isNull (box res)) then
                        res.Dispose())
            )

        member this.While(guard, f) =
            if not (guard ()) then
                Some()
            else
                do f () |> ignore
                this.While(guard, f)

        member this.For(sequence: seq<_>, body) =
            this.Using(
                sequence.GetEnumerator(),
                fun enum -> this.While(enum.MoveNext, this.Delay(fun () -> body enum.Current))
            )
    //#endregion

    /// The maybe monad defined on 'T option.
    /// Has extensions to support binding to non-Option values that still have Option-like semantics,
    /// like nullable values and references.
    let maybe = MaybeBuilder()

    // extensions to make it more ergonomic to bind to not-Option values
    type MaybeBuilder with
        // overload to transparently handle "let! foo = nullable ref"
        member _.Bind(m, f) = Option.bind f (Option.ofObj m)
        // overload to transparently handle "let! foo = nullable val"
        member _.Bind(m, f) = Option.bind f (Option.ofNullable m)
