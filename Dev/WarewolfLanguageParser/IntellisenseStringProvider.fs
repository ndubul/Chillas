module IntellisenseStringProvider
open LanguageAST
//open LanguageEval

open WarewolfDataEvaluationCommon

// take a list of variables and cartesian product of the options. 
// can take a bias at some point
let rec Combine (variables:LanguageExpression seq) (level:int) = 
    List.collect (fun a-> CombineExpressions level (List.ofSeq variables) a) (List.ofSeq variables) |> List.sortBy (fun (a:string) -> a.ToLower()) // clean up multiple enumerations

and CombineExpressions  (level:int) (variables:LanguageExpression list) (variable:LanguageExpression)  =
    match variable with
    | ScalarExpression a -> CombineScalar a 
    | RecordSetExpression b  -> CombineRecset b level  variables
    | RecordSetNameExpression c  -> CombineRecsetName c level  variables
    | WarewolfAtomAtomExpression _ -> List.empty
    | ComplexExpression _ -> List.empty // cant have complex expressions in intellisense because the variable list is made up of simple expressions

and CombineScalar (a:ScalarIdentifier)  =
    [ ScalarExpression a |> LanguageExpressionToString]

and CombineRecset (a:RecordSetIdentifier) (level:int)  (variables:LanguageExpression list)  =
    match level with
    | 0 -> [ RecordSetExpression a |> LanguageExpressionToString]
    | _ -> let indexes = CombineIndex level variables
           List.map (fun x -> "[["+ a.Name + "(" + x + ")." + a.Column + "]]") indexes

and CombineRecsetName (a:RecordSetName) (level:int)  (variables:LanguageExpression list)  =
    match level with
    | 0 -> [ RecordSetNameExpression a |> LanguageExpressionToString]
    | _ -> let indexes = CombineIndex level variables
           List.map (fun x -> "[["+ a.Name + "(" + x + ")" + "]]") indexes

and CombineIndex (level:int) (variables:LanguageExpression list)  =
    let newLevel = level - 1
    let combined = Combine variables newLevel
    "*" :: "":: combined