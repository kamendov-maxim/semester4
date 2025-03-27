namespace HomeWork5

type OperatingSystem =
    abstract member InfectionChance: int with get
    abstract member Name: string with get

type Linux() =
    interface OperatingSystem with
        member val InfectionChance = 15 with get
        member val Name = "Linux" with get

type MacOs() =
    interface OperatingSystem with
        member val InfectionChance = 10 with get
        member val Name = "Linux" with get

type Windows() =
    interface OperatingSystem with
        member val InfectionChance = 60 with get
        member val Name = "Linux" with get
