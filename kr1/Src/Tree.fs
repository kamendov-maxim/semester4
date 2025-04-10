module BinaryTree

/// Node of a binary tree
type 'T Node =
    | Node of 'T * 'T Node * 'T Node
    | Leaf

/// Filter function for the binary tree implementation described above
/// will return all values satisfying the predicate
let filter (predicate: 'T -> bool) (tree: 'T Node) =
    let rec loop node = 
        seq {
            match node with
            | Node (value, left, right) -> 
                if predicate value then
                    yield value 
                yield! loop left
                yield! loop right
            | Leaf -> ()
        }
    loop tree

