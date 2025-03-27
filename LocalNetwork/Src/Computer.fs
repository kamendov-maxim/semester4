namespace HomeWork5

type Computer(id: int, os: OperatingSystem, isInfected: bool) =
    member val ID = id
    member val OS = os

    member val IsInfected = isInfected with get, set

    member v.Infect(random: System.Random) =
        if not v.IsInfected && random.Next(1, 100) <= v.OS.InfectionChance then
            v.IsInfected <- true
