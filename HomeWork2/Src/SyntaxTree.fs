module SyntaxTree

/// An implementation of arithmetic expression syntax tree that supports operation of multiplication, addition and substraction
type Node =
    | Var of int
    | Add of Node * Node
    | Mul of Node * Node
    | Sub of Node * Node

/// Evaluate arithmetic expression described with syntax tree nodes implemented above
let rec eval tree =
    match tree with
    | Var x -> x
    | Add(x, y) -> eval x + eval y
    | Mul(x, y) -> eval x * eval y
    | Sub(x, y) -> eval x - eval y
