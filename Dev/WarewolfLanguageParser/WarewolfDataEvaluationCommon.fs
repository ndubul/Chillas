﻿module WarewolfDataEvaluationCommon


open LanguageAST
//open LanguageEval
open Microsoft.FSharp.Text.Lexing
open DataASTMutable
open WarewolfParserInterop

// this method will given a language string return an AST based on FSLex and FSYacc

let mutable ParseCache = Map.empty : Map<string,LanguageExpression>

let PositionColumn = "WarewolfPositionColumn#"

type WarewolfEvalResult = 
    | WarewolfAtomResult of WarewolfAtom
    | WarewolfAtomListresult of WarewolfParserInterop.WarewolfAtomList<WarewolfAtomRecord> 
    | WarewolfRecordSetResult of WarewolfRecordset

let AtomtoString (x:WarewolfAtom )=
    match x with 
        | Float a -> a.ToString()
        | Int a -> a.ToString()
        | DataString a -> a
        | Nothing -> ""
let WarewolfAtomRecordtoString (x:WarewolfAtomRecord )=
    match x with 
        | Float a -> a.ToString()
        | Int a -> a.ToString()
        | DataString a -> a
        | Nothing -> ""

let EvalResultToString (a:WarewolfEvalResult) = 
    match a with
    | WarewolfAtomResult x -> AtomtoString x
    | WarewolfAtomListresult x -> Seq.map WarewolfAtomRecordtoString x |> (Seq.fold (+) "")





type PositionValue =
    | IndexFoundPosition of int
    | IndexDoesNotExist

let getRecordSetIndex (recset:WarewolfRecordset) (position:int) =
    match recset.Optimisations with
    | Ordinal -> if recset.LastIndex <  position then
                    IndexDoesNotExist
                 else
                    IndexFoundPosition (position-1)
    | _->
            let indexes = recset.Data.[PositionColumn]
            let positionAsAtom = Int position
            try  Seq.findIndex (fun a->  a=positionAsAtom) indexes |> IndexFoundPosition
            with     
            | :? System.Collections.Generic.KeyNotFoundException as ex -> IndexDoesNotExist

let evalRecordSetIndex (recset:WarewolfRecordset) (identifier:RecordSetIdentifier) (position:int) =
    let index = getRecordSetIndex recset position
    match index with 
    | IndexFoundPosition a -> recset.Data.[identifier.Column].[a]
    | IndexDoesNotExist -> raise (new Dev2.Common.Common.NullValueInVariableException("index not found",identifier.Name))

let evalRecordSetStarIndex (recset:WarewolfRecordset) (identifier:RecordSetIdentifier)  =
    recset.Data.[identifier.Column]

let evalRecordSetLastIndex (recset:WarewolfRecordset) (identifier:RecordSetIdentifier)  =
    if Seq.isEmpty recset.Data.[PositionColumn] then
        Nothing
    else
        let data = Seq.max recset.Data.[PositionColumn] 
        let index = Seq.findIndex (fun a -> a=data) recset.Data.[PositionColumn] 
        recset.Data.[identifier.Column].[index]







    

let evalScalar (scalarName:ScalarIdentifier) (env:WarewolfEnvironment) =
    if env.Scalar.ContainsKey scalarName
    then     ( env.Scalar.[scalarName])
    else Nothing

let rec IndexToString (x:Index) =
    match x with 
        | IntIndex a -> a.ToString()
        | Star -> "*"
        | Last -> ""
        | IndexExpression a-> LanguageExpressionToString a

and EvalIndex  ( env:WarewolfEnvironment) (exp:string)=
    let getIntFromAtom (a:WarewolfAtom) =
        match a with
        | Int x -> x
        |_ ->failwith "invalid recordset index was not an int"
    let evalled = Eval env exp
    match evalled with
    | WarewolfAtomResult a -> getIntFromAtom a 
    |_ ->failwith "invalid recordset index was a list"



and  LanguageExpressionToString  (x:LanguageExpression) =
    match x with
        | RecordSetExpression a -> sprintf "[[%s(%s).%s]]" a.Name (IndexToString a.Index ) a.Column  
        | ScalarExpression a -> sprintf "[[%s]]" a
        | WarewolfAtomAtomExpression a -> AtomtoString a
        | ComplexExpression a -> List.fold (fun c d -> c + LanguageExpressionToString d ) "" a
        | RecordSetNameExpression a -> sprintf "[[%s(%s)]]" a.Name (IndexToString a.Index ) 

and evalRecordsSet (recset:RecordSetIdentifier) (env: WarewolfEnvironment)  =
    if  not (env.RecordSets.ContainsKey recset.Name)       then 
        raise (new Dev2.Common.Common.NullValueInVariableException(recset.Index.ToString(),recset.Name))
            
    else
            match recset.Index with
                | IntIndex a -> new WarewolfParserInterop.WarewolfAtomList<WarewolfAtomRecord>(WarewolfAtomRecord.Nothing,[ evalRecordSetIndex env.RecordSets.[recset.Name] recset a])
                | Star ->  evalRecordSetStarIndex env.RecordSets.[recset.Name] recset
                | Last -> new WarewolfParserInterop.WarewolfAtomList<WarewolfAtomRecord>(WarewolfAtomRecord.Nothing,[evalRecordSetLastIndex env.RecordSets.[recset.Name] recset])
                | IndexExpression a -> new WarewolfParserInterop.WarewolfAtomList<WarewolfAtomRecord>(WarewolfAtomRecord.Nothing,[ evalRecordSetIndex env.RecordSets.[recset.Name] recset ( LanguageExpressionToString a|>(EvalIndex env)) ])
                | _ -> failwith "Unknown evaluation type"

and evalRecordSetAsString (env: WarewolfEnvironment) (a:RecordSetIdentifier) = 
    match a.Index with
    | IntIndex i -> (evalRecordsSet a env).[0]
    |_ ->(evalRecordsSet a env) |> Seq.map (AtomtoString ) |> (Seq.fold (+) "") |> DataString

and  Clean (buffer :LanguageExpression) =
    match buffer with
        | RecordSetExpression a -> RecordSetExpression a
        | ScalarExpression a -> ScalarExpression a
        | RecordSetNameExpression a -> RecordSetNameExpression a
        | WarewolfAtomAtomExpression a -> WarewolfAtomAtomExpression a
        | ComplexExpression  a ->  (List.filter (fun b -> "" <> (LanguageExpressionToString b)) a) |> (fun a -> if (List.length a) =1 then Clean a.[0] else ComplexExpression a)

and ParseLanguageExpression  (lang:string) : LanguageExpression=
    let exp = ParseCache.TryFind lang
    match exp with 
    | Some a ->  a
    | None -> 
                let lexbuf = LexBuffer<string>.FromString lang 
                let buffer = Parser.start Lexer.tokenstream lexbuf
                let res = buffer |> Clean
                ParseCache<-ParseCache.Add(lang,res)
                res

and EvalDataSetExpression (env: WarewolfEnvironment)  (name:string) =
    if env.RecordSets.ContainsKey( name) then
        WarewolfRecordSetResult env.RecordSets.[name]
    else
        raise (new Dev2.Common.Common.NullValueInVariableException("Recordset not found",name))

          
and  Eval  (env: WarewolfEnvironment) (lang:string) : WarewolfEvalResult=
    let EvalComplex (exp:LanguageExpression list) = 
        if((List.length exp) =1) then
            match exp.[0] with
                | RecordSetExpression a ->  evalRecordSetAsString env a
                | ScalarExpression a ->  (evalScalar a env)
                | WarewolfAtomAtomExpression a ->  a
                | _ ->failwith "you should not get here"
        else    
            let start = List.map LanguageExpressionToString  exp |> (List.fold (+) "")
            let evaled = (List.map (LanguageExpressionToString >> (Eval  env)>>EvalResultToString)  exp )|> (List.fold (+) "")
            if( evaled = start) then
                DataString evaled
            else DataString (Eval env evaled|>  EvalResultToString)
    
    let exp = ParseCache.TryFind lang
    let buffer =  match exp with 
                    | Some a ->  a
                    | _->    
                        let temp = ParseLanguageExpression lang
                        temp
    match buffer with
        | RecordSetExpression a -> WarewolfAtomListresult(  (evalRecordsSet a env) )
        | ScalarExpression a -> WarewolfAtomResult (evalScalar a env)
        | WarewolfAtomAtomExpression a -> WarewolfAtomResult a
        | RecordSetNameExpression x ->EvalDataSetExpression env x.Name
        | ComplexExpression  a -> WarewolfAtomResult (EvalComplex ( List.filter (fun b -> "" <> (LanguageExpressionToString b)) a)) 

and getRecordSetPositionsAsInts (recset:WarewolfRecordset) =
    let AtomToInt (a:WarewolfAtom) =
        match a with
            | Int a -> a
            | _->failwith "the position column contains non ints"
    let positions = recset.Data.[PositionColumn];
    Seq.map AtomToInt positions |> Seq.sort

and evalRecordSetIndexes (env:WarewolfEnvironment) (recset:RecordSetName) : int seq =
    match recset.Index with
    | IntIndex a -> {1..1}
    | Star -> getRecordSetPositionsAsInts env.RecordSets.[recset.Name]
    | Last -> getRecordSetPositionsAsInts env.RecordSets.[recset.Name]
    | IndexExpression a -> {1..1}

and  EvalIndexes  (env: WarewolfEnvironment) (lang:string) =
    let EvalComplex (exp:LanguageExpression list) = 
        if((List.length exp) =1) then
            match exp.[0] with
                | RecordSetNameExpression a -> evalRecordSetIndexes env a
                | _ ->failwith "not a recordset"
        else    
            failwith "not a recordset"
    let exp = ParseCache.TryFind lang
    let buffer =  match exp with 
                    | Some a ->  a
                    | _->    
                        let temp = ParseLanguageExpression lang
                        temp
    match buffer with
        | ComplexExpression  a ->  EvalComplex ( List.filter (fun b -> "" <> (LanguageExpressionToString b)) a)
        | RecordSetNameExpression a -> evalRecordSetIndexes env a
        | _ ->failwith "not a recordset"


// assigns happen here all tools will use these functions to do assigns. Must be wrapped in c# class

//let Eval (env:Environment) (Tools: Tool list)
let AddToScalars (env:WarewolfEnvironment) (name:string) (value:WarewolfAtom)  =
    let rem = Map.remove name env.Scalar |> Map.add name value 
    {       Scalar=rem;
            RecordSets = env.RecordSets
    }

let AddToRecordSets (env:WarewolfEnvironment) (name:string) (value:WarewolfRecordset) =
    let rem = Map.remove name env.RecordSets |> Map.add name value 
    {       Scalar=env.Scalar;
            RecordSets = rem
    }

let AddColumnValueToRecordset (destination:WarewolfRecordset) (name:string) (values: WarewolfParserInterop.WarewolfAtomList<WarewolfAtomRecord>) =
     let data = destination.Data
     let columnData =  values
     let added = data.Add( name, columnData  )
     {
        Data = added
        Optimisations = destination.Optimisations;
        LastIndex= destination.LastIndex;
        Count= destination.Count; 
        Frame = 0;
     }

let CreateEmpty (length:int) (count:int) =
   new WarewolfParserInterop.WarewolfAtomList<WarewolfAtomRecord> (WarewolfAtomRecord.Nothing,seq {for x in 1 .. length do yield Nothing },count);

let AddToList (lst:WarewolfParserInterop.WarewolfAtomList<WarewolfAtomRecord>) (value:WarewolfAtomRecord) =
    lst.AddSomething value
    lst    

let AddNothingToList (lst:WarewolfParserInterop.WarewolfAtomList<WarewolfAtomRecord>) (value:WarewolfAtomRecord) =
    lst.AddNothing()
    lst    



let AddAtomToRecordSet (rset:WarewolfRecordset) (columnName:string) (value: WarewolfAtom) (position:int) =
    let col = rset.Data.TryFind columnName
    let rsAdded= match col with 
                    | Some a -> rset
                    | None-> { rset with Data=  Map.add columnName (CreateEmpty rset.Data.[PositionColumn].Length rset.Data.[PositionColumn].Count)  rset.Data    }
    if position = rsAdded.Count+1    
        then
                  
            let addedAtEnd =  Map.map (fun k v -> if k=columnName then  ( AddToList v value) else (AddNothingToList v Nothing)) rsAdded.Data
            let len = addedAtEnd.[PositionColumn].Count 
            
            addedAtEnd.[PositionColumn].[len-1] <- Int position
            { rsAdded with Data=addedAtEnd ; LastIndex = rsAdded.LastIndex+1; Count = rsAdded.Count+1 ; Optimisations = if rsAdded.Count = rsAdded.LastIndex &&  rsAdded.Optimisations <> WarewolfAttribute.Fragmented &&  rsAdded.Optimisations <> WarewolfAttribute.Sorted then WarewolfAttribute.Ordinal else rsAdded.Optimisations }
        else
            if position > rsAdded.Count+1
                then
                let addedAtEnd =  Map.map (fun k v -> if k=columnName then  ( AddToList v value) else (AddNothingToList v Nothing)) rsAdded.Data
                let len = addedAtEnd.[PositionColumn].Count 
            
                addedAtEnd.[PositionColumn].[len-1] <- Int position
                { rsAdded with Data=addedAtEnd ; LastIndex = position; Count = len+1 ; Optimisations = if  rsAdded.Optimisations = WarewolfAttribute.Ordinal then WarewolfAttribute.Sorted else rsAdded.Optimisations }

            else
                let lstval = rsAdded.Data.[PositionColumn]
                if Seq.exists (fun vx -> vx=(Int position)) lstval then
                    let index = Seq.findIndex (fun vx -> vx= (Int position)) lstval
                    lstval.[index] <- value
                    rsAdded
                else 
                      let addedAtEnd =  Map.map (fun k v -> if k=columnName then  ( AddToList v value) else (AddNothingToList v Nothing)) rsAdded.Data
                      let len = addedAtEnd.[PositionColumn].Count 
            
                      addedAtEnd.[PositionColumn].[len-1] <- Int position
                      { rsAdded with Data=addedAtEnd ; LastIndex = rsAdded.LastIndex; Count = len+1 ; Optimisations = WarewolfAttribute.Fragmented }


let getPositionFromRecset (rset:WarewolfRecordset) (columnName:string) =
    if rset.Data.ContainsKey( columnName) then
        let posValue =  rset.Data.[columnName].[rset.Data.[columnName].Count-1]
        match posValue with
            |   Nothing -> rset.Frame
            | _-> rset.LastIndex  + 1    
    else
        match rset.Frame with
        | 0 ->(rset.LastIndex)+1
        |_ ->max (rset.LastIndex) (rset.Frame)
        

let AddAtomToRecordSetWithFraming (rset:WarewolfRecordset) (columnName:string) (value: WarewolfAtom) (pos:int) (isFramed:bool) =
    let  position = pos
    let rsAdded = if rset.Data.ContainsKey( columnName) 
                  then  rset
                  else 
                       { rset with Data=  Map.add columnName (CreateEmpty rset.Data.[PositionColumn].Length rset.Data.[PositionColumn].Count )  rset.Data    }
    let frame = if isFramed then position else 0;
    if (position = rsAdded.Count+1)    
        then                  
            let addedAtEnd =  Map.map (fun k v -> if k=columnName then  ( AddToList v value) else (AddNothingToList v Nothing)) rsAdded.Data
            let len = addedAtEnd.[PositionColumn].Count 
            
            addedAtEnd.[PositionColumn].[len-1] <- Int position
            { rsAdded with Data=addedAtEnd ; LastIndex = rsAdded.LastIndex+1; Count = rsAdded.Count+1; Frame = frame ; Optimisations = if rsAdded.Count = rsAdded.LastIndex &&  rsAdded.Optimisations <> WarewolfAttribute.Fragmented &&  rsAdded.Optimisations <> WarewolfAttribute.Sorted then WarewolfAttribute.Ordinal else rsAdded.Optimisations  }
        else
            if (position = rsAdded.Count+1) || (position = rsAdded.Frame && isFramed)   
            then                  
                let len = rsAdded.Data.[PositionColumn].Count 
                rsAdded.Data.[columnName].[len-1] <- value
                rsAdded
            else
            if position > rsAdded.Count
                then
                let addedAtEnd =  Map.map (fun k v -> if k=columnName then  ( AddToList v value) else (AddNothingToList v Nothing)) rsAdded.Data
                let len = addedAtEnd.[PositionColumn].Count 
            
                addedAtEnd.[PositionColumn].[len-1] <- Int position
                { rsAdded with Data=addedAtEnd ; LastIndex = position; Frame = frame ; Count = len+1 ; Optimisations = if  rsAdded.Optimisations = WarewolfAttribute.Ordinal then WarewolfAttribute.Sorted else rsAdded.Optimisations }

            else
                let lstval = rsAdded.Data.[PositionColumn]
                if Seq.exists (fun vx -> vx=(Int position)) lstval then
                    let index = Seq.findIndex (fun vx -> vx= (Int position)) lstval
                    rsAdded.Data.[columnName].[index] <- value
                    rsAdded
                else 
                      let addedAtEnd =  Map.map (fun k v -> if k=columnName then  ( AddToList v value) else (AddNothingToList v Nothing)) rsAdded.Data
                      let len = addedAtEnd.[PositionColumn].Count 
            
                      addedAtEnd.[PositionColumn].[len-1] <- Int position
                      { rsAdded with Data=addedAtEnd ; LastIndex = rsAdded.LastIndex; Count = len+1 ; Frame = frame ; Optimisations = WarewolfAttribute.Fragmented }





let CreateFilled (count:int) (value: WarewolfAtom):WarewolfColumnData=
   new WarewolfParserInterop.WarewolfAtomList<WarewolfAtomRecord> (WarewolfAtomRecord.Nothing,seq {for x in 1 .. count do yield value }) 


let UpdateColumnWithValue (rset:WarewolfRecordset) (columnName:string) (value: WarewolfAtom)=
    if rset.Data.ContainsKey( columnName) 
    then
        let x = rset.Data.[columnName];
        for i in [0..x.Count-1] do                         
            x.[i]<-value;    
        rset 
    else 
    {rset with Data=  Map.add columnName ( CreateFilled rset.Count value)  rset.Data    }


let DeleteValues (exp:string)  (env:WarewolfEnvironment) =
    let rset = env.RecordSets.TryFind exp
    match rset with 
    | Some x -> {x with Data = Map.map (fun a b -> new WarewolfAtomList<WarewolfAtom>(WarewolfAtom.Nothing)) x.Data  } 
    | None->failwith "recordset does not exist"

let DeleteValue  (exp:string) (index:int)   (env:WarewolfEnvironment) =
    let rset = env.RecordSets.TryFind exp
    match rset with 
    | Some values -> let pos = Seq.find ( fun a-> AtomtoString a = index.ToString())  values.Data.[PositionColumn]
                     let posAsInt = match pos with
                                    | Nothing -> failwith "index does not exist"
                                    | Int a -> a
                                    | _  -> failwith "index does not exist"
                     {values  with Data = Map.map (fun (a:string) (b:WarewolfAtomList<WarewolfAtom>) -> b.DeletePosition( posAsInt ) ) values.Data  } 
    | None->failwith "recordset does not exist"

let GetLastIndexFromRecordSet (exp:string)  (env:WarewolfEnvironment)  =
    let rset = env.RecordSets.TryFind exp
    match rset with 
    | Some values -> values.LastIndex                    
    | None->failwith "recordset does not exist"

let rec DeleteExpressionIndex (exp:RecordSetName) (ind: LanguageExpression)  (env:WarewolfEnvironment)  =
    let data = LanguageExpressionToString ind |> (Eval env) |> EvalResultToString
    EvalDelete( (sprintf "[[%s(%s)]]" exp.Name data)) env

and EvalDelete (exp:string)  (env:WarewolfEnvironment) =
    let left = ParseLanguageExpression exp 
    match left with 
                |   RecordSetNameExpression b ->  match b.Index with
                                                                 | Star -> DeleteValues  b.Name env |> (fun upd-> {env with RecordSets = Map.map (fun ax bx -> if ax=b.Name then upd else bx ) env.RecordSets} )
                                                                 | Last ->  DeleteValue  b.Name (GetLastIndexFromRecordSet  b.Name env) env |> (fun upd-> {env with RecordSets = Map.map (fun ax bx -> if ax=b.Name then upd else bx ) env.RecordSets} )
                                                                 | IntIndex a -> DeleteValue  b.Name a env |> (fun upd-> {env with RecordSets = Map.map (fun ax bx -> if ax=b.Name then upd else bx ) env.RecordSets} )
                                                                 | IndexExpression exp ->  DeleteExpressionIndex b exp env
                                                               

                |_-> failwith "only recordsets can be deleted"
