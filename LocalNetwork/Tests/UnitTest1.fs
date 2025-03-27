module Tests

open NUnit.Framework
open Moq
open HomeWork5

[<Test>]
let ``Computer should not be infected when chance is 0`` () =
    let mock = Mock<OperatingSystem>()
    mock.Setup(fun os -> os.InfectionChance).Returns 0 |> ignore
    let comp1 = Computer(1, mock.Object, true)
    let comp2 = Computer(2, mock.Object, false)

    let map =
        dict[comp1, [ comp2 ]
             comp2, [ comp1 ]]

    let sim = new Simulation(map)

    for _ in [ 1..100 ] do
        sim.NextStep() |> ignore
        Assert.That(sim.NextStep(), Is.EqualTo true)
        Assert.That(comp2.IsInfected, Is.EqualTo false)


[<Test>]
let ``Computer should be infected after first step when chance is 100 and sim should stop`` () =
    let mock = Mock<OperatingSystem>()
    mock.Setup(fun os -> os.InfectionChance).Returns 100 |> ignore
    let comp1 = Computer(1, mock.Object, true)
    let comp2 = Computer(2, mock.Object, false)

    let map =
        dict[comp1, [ comp2 ]
             comp2, [ comp1 ]]

    let sim = new Simulation(map)
    printf "FUCK1\n"
    sim.NextStep() |> ignore
    Assert.That(comp2.IsInfected, Is.EqualTo true)
    Assert.That(sim.NextStep(), Is.EqualTo false)

[<Test>]
let ``Simulation should finish`` () =
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

    for i in map.Keys |> Seq.toList do
        Assert.That(i.IsInfected, Is.EqualTo true)
