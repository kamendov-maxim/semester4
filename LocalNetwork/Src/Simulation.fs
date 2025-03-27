namespace HomeWork5

type Simulation(map: System.Collections.Generic.IDictionary<Computer, Computer list>) =
    member val map = map

    member v.NextStep() =
        let rand = new System.Random()

        let infectedComputers =
            map.Keys |> Seq.toList |> List.filter (fun (x: Computer) -> x.IsInfected)

        if infectedComputers.Length = (map.Keys |> Seq.toList).Length then
            false
        else
            for i in infectedComputers do
                for j in map[i] do
                    if not j.IsInfected then
                        j.Infect rand

            true

    member v.RunSimulation() =
        v.Print

        while v.NextStep() do
            v.Print

    member v.Print =
        for i in map.Keys |> Seq.toList do
            printf "Computer %d with OS %s infection status: %b\n" i.ID i.OS.Name i.IsInfected
