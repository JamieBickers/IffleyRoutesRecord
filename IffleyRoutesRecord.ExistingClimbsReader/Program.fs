open System.IO
open Newtonsoft.Json

type Hold = {Hold: string; Rules: List<string> option}
type Holds = {StandingStartHolds: List<Hold> option; OtherHolds:List<Hold>}
type Grade = {PrimaryGrade: string; AdditionalGrades: List<string> option}
type Climb = {Name: string; Grade: Grade; Holds: Holds; Page: int; Rules: List<string>}

type ParsedHold = {Hold: string; Rules: List<string>; IsStandingStartHold: bool}
type ParsedClimb = {Name: string; Grades: List<string>; Holds: List<ParsedHold>; Rules: List<string>}

let readExistingClimbsJson() =
    JsonConvert.DeserializeObject<List<Climb>>(File.ReadAllText(@"C:\Users\bicke\OneDrive\Desktop\IffleyRoutesRecord\ExistingProblems.json"))

let parseGrades (grade: Grade) =
    (grade.PrimaryGrade)::(match grade.AdditionalGrades with
                           | None -> []
                           | Some grades -> grades)

let parseRules rules =
    match rules with
    | None -> []
    | Some rs -> rs

let parseHold isStandingStartHold (hold: Hold) =
    {Hold = hold.Hold; Rules = parseRules hold.Rules; IsStandingStartHold = isStandingStartHold}

let parseHolds (holds: Holds) =
    match holds.StandingStartHolds with
    | None -> List.map (parseHold false) holds.OtherHolds
    | Some standingStartHolds -> List.concat [List.map (parseHold true) standingStartHolds; List.map (parseHold false) holds.OtherHolds]

let parseClimb (climb: Climb) =
    {Name = climb.Name; Grades = parseGrades climb.Grade; Holds = parseHolds climb.Holds; Rules = climb.Rules}

let saveParsedClimbsJson climbs =
    File.WriteAllText(@"C:\Users\bicke\OneDrive\Desktop\IffleyRoutesRecord\ParsedExistingProblems.json", JsonConvert.SerializeObject(climbs))

[<EntryPoint>]
let main argv =
    readExistingClimbsJson()
    |> List.map parseClimb
    |> saveParsedClimbsJson
    |> ignore
    0