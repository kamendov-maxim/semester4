module BinaryTree

/// Node of a binary tree
type 'T Node =
    | Node of 'T * 'T Node * 'T Node
    | Leaf

/// Map function for the binary tree implementation described above
/// function f will be applied to all values of all nodes in the tree
let rec map f node =
    match node with
    | Node(value, left, right) -> Node(f value, map f left, map f right)
    | Leaf -> Leaf
