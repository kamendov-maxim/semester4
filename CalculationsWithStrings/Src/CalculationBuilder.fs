module CalculationsWithStrings

open System

type CalculationBuilder () =
    member v.Bind (x: string, f: float -> option<float>) =
        match x |> System.Double.TryParse with
        | false, _ -> None
        | true, y -> f y

    member v.Return x =
        Some x

let calculate = new CalculationBuilder()
