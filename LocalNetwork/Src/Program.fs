// module Program

open HomeWork5

let comp1 = new Computer(1, new Linux(), true)
let comp2 = new Computer(2, new Windows(), false)
let comp3 = new Computer(3, new Windows(), true)
let comp4 = new Computer(4, new Windows(), false)
let comp5 = new Computer(5, new MacOs(), false)

let map =
    dict[comp1, [ comp2; comp3; comp4 ]
         comp2, [ comp1; comp4 ]
         comp3, [ comp1; comp2; comp5 ]
         comp4, [ comp1; comp2 ]
         comp5, [ comp3 ]]

let sim = new Simulation(map)
sim.RunSimulation()
