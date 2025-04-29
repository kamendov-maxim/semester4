module LambdaInterpreter

type Expression =
    | Var of string
    | Abstraction of string * Expression
    | Application of Expression * Expression

let FindFreeVars expr =
    let rec findFreeVars expr =
        match expr with
        | Var x -> Set.singleton x
        | Abstraction(x, e) -> Set.remove x (findFreeVars e)
        | Application(e1, e2) -> Set.union (findFreeVars e1) (findFreeVars e2)

    findFreeVars expr

let FindBoundVars expr =
    let rec loop expr =
        match expr with
        | Var _ -> Set.empty
        | Abstraction(x, e) -> Set.add x (loop e)
        | Application(e1, e2) -> Set.union (loop e1) (loop e2)

    loop expr

let AlphaConversion var usedVars =
    let rec loop n =
        let newVar = $"{var}{n}"

        if Set.contains newVar usedVars then
            loop (n + 1)
        else
            newVar

    if Set.contains var usedVars then loop 0 else var

let Substitude var what where =
    let rec subst var what where =
        match where with
        | Var x when x = var -> what
        | Var x -> Var x
        | Abstraction(x, e) when x = var -> Abstraction(x, e)
        | Abstraction(x, e) ->
            let freeVarsInSubExpr = FindBoundVars what

            if not (Set.contains x freeVarsInSubExpr) then
                Abstraction(x, subst var what e)
            else
                let newVar = AlphaConversion x (Set.union (FindBoundVars what) (FindFreeVars where))
                let newExp = subst x (Var newVar) e
                Abstraction(newVar, subst var what newExp)
        | Application(e1, e2) -> Application(subst var what e1, subst var what e2)

    subst var what where

let Reduce expression =
    let rec loop expression =
        match expression with
        | Application(Abstraction(x, e1), e2) -> Substitude x e2 e1 |> loop
        | Application(e1, e2) ->
            match loop e1 with
            | Abstraction _ as reduced -> loop (Application(reduced, e2))
            | reduced -> Application(reduced, e2)
        | Abstraction(x, e) -> Abstraction(x, loop e)
        | Var _ -> expression

    loop expression
