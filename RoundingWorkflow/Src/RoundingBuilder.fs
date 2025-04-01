module Rounding

type IntRepresentation(digitsAfterPoint: int, value: int64) =
    member val DigitsAfterPoint = digitsAfterPoint
    member val Value = value

    static member (+)(a: IntRepresentation, b: IntRepresentation) =
        new IntRepresentation(a.DigitsAfterPoint, a.Value + b.Value)

    static member (-)(a: IntRepresentation, b: IntRepresentation) =
        new IntRepresentation(a.DigitsAfterPoint, a.Value - b.Value)

    static member (*)(a: IntRepresentation, b: IntRepresentation) =
        new IntRepresentation(a.DigitsAfterPoint, a.Value * b.Value / pown (int64 10) a.DigitsAfterPoint)

    static member (/)(a: IntRepresentation, b: IntRepresentation) =
        new IntRepresentation(a.DigitsAfterPoint, a.Value * pown (int64 10) a.DigitsAfterPoint / b.Value)

type RoundingBuilder(digitsAfterPoint: int) =

    member v.Bind(x: float, f: IntRepresentation -> float) =
        x
        |> fun y -> 
            new IntRepresentation(digitsAfterPoint,
                int64 (y * pown 10.0 digitsAfterPoint))
        |> f

    member v.Return(x: IntRepresentation) : float =
        x.Value |> float |> fun y -> y / pown 10.0 x.DigitsAfterPoint

let rounding n = RoundingBuilder n
