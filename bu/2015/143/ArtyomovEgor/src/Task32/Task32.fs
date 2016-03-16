﻿module Task32

open Task15
open Task30
open Task31

open FSharp.Charting
open System.Drawing
open System.Windows.Forms

let L1 () = (1, Cons(1, Empty))
let L2 () = (1, Cons(2, Empty))
let L0 () = (1, Cons(0, Empty))

let rec task8Long (n: Num) =
     if n = L1() || n = L2()
     then L1()
     else sum (task8Long (sum n (-1, Cons(1, Empty)))) (task8Long (sum n (-1, Cons(2, Empty))))

let task9Long n =
    let mutable a = (1, Cons(1, Empty))
    let mutable b = (1, Cons(1, Empty))
    let mutable i = Cons(1, Empty)
    match n with
    | sign, lst ->
        while compare i lst <> Equals do
            a <- sum a b
            b <- sum a (mult b (-1, Cons(1, Empty)))
            i <- increment 1 i 1 (Cons(1, Empty)) 0
    b

let task10Long n =
    let rec fibN n fib1 fib2 i =
        match n with
        | sign, lst ->
            if compare lst (Cons(2, Empty)) = Less
            then 1, (Cons(1, Empty))
            elif n = i
            then fib1
            else fibN n (sum fib1 fib2) fib1 (sum i (1, Cons(1, Empty)))
    fibN n (1, Cons(1, Empty)) (1, Cons(1, Empty)) (1, Cons(2, Empty))

let task11Long (n: Num) =
    let mutable a = array2D [ [L1(); L1()]; [L1(); L0()] ]
    let b = array2D [ [L1(); L1()]; [L1(); L0()] ]
    let mutable i = Cons(1, Empty)
    match n with
    | sign, lst ->
        while compare i lst = Less do
            let a00 = sum (mult a.[0, 0] b.[0, 0]) (mult a.[0, 1] b.[1, 0])
            let a01 = sum (mult a.[0, 0] b.[0, 1]) (mult a.[0, 1] b.[1, 1])
            let a10 = sum (mult a.[1, 0] b.[0, 0]) (mult a.[1, 1] b.[1, 0])
            let a11 = sum (mult a.[1, 0] b.[0, 1]) (mult a.[1, 1] b.[1, 1])
            let multarray = array2D [ [a00; a01]; [a10; a11] ]
            a.[0, 0] <- multarray.[0, 0]
            a.[0, 1] <- multarray.[0, 1]
            a.[1, 0] <- multarray.[1, 0]
            a.[1, 1] <- multarray.[1, 1]
            i <- increment 1 i 1 (Cons(1, Empty)) 0
        a.[0, 1]

let multMatrixLong (a: Num[,]) (b: Num[,]) =
    let a00 = sum (mult a.[0, 0] b.[0, 0]) (mult a.[0, 1] b.[1, 0])
    let a01 = sum (mult a.[0, 0] b.[0, 1]) (mult a.[0, 1] b.[1, 1])
    let a10 = sum (mult a.[1, 0] b.[0, 0]) (mult a.[1, 1] b.[1, 0])
    let a11 = sum (mult a.[1, 0] b.[0, 1]) (mult a.[1, 1] b.[1, 1])
    array2D [ [a00; a01]; [a10; a11] ]

let divBy2 dend =
    let rec division num i (carry: int) =
        match dend with
        | sign, lst ->
            match lst with
            | Empty -> Empty
            | Cons(hd, tl) ->
                    if hd < 2 && i < 1
                    then division (1, tl) (i + 1) hd
                    elif carry > 0 && hd >= 2
                    then 
                        let carrynxt = carry * 10 + hd
                        Cons (carrynxt / 2, division (1, tl) (i + 1) (carrynxt % 2))
                    else 
                        Cons(hd / 2, division (1, tl) (i + 1) (hd % 2))
    division dend 0 0
     
let modBy2 (n: Num) = 
    match n with
    | sign, tl ->
        let rec lastDigit lst =
            match lst with
            | Empty -> 0
            | Cons (hd, Empty) -> hd
            | Cons (hd, tl) -> lastDigit tl
        let res = lastDigit tl
        res % 2

let rec matrixPow (n: Num) arr =
    if n = L1()
    then
        arr
    elif modBy2 n = 0
    then
        let pown = matrixPow (1, divBy2 n) arr
        multMatrixLong pown pown
    else
        let pown = matrixPow (1, divBy2 (sum n (-1, Cons(1, Empty)))) arr
        multMatrixLong arr (multMatrixLong pown pown)

let task12Long n =
    let array1 = array2D [ [L1(); L1()]; [L1(); L0()] ]
    let output = matrixPow n array1
    output.[0, 1]

let numToInt n =
    match n with
    | sign, lst ->
        let rec toInt lst a =
            match lst with 
            | Empty -> a
            | Cons (hd, tl) ->
                toInt tl (a * 10 + hd)
        sign * (toInt lst 0)

let task13Long n =
    let len = numToInt n 
    let outArray = Array.zeroCreate len
    match n with
    | sign, lst ->
        if compare lst (Cons(1, Empty)) = More 
        then 
            outArray.[0] <- L1()
            outArray.[1] <- L1()
            for i = 2 to len - 1 do
                outArray.[i] <- sum outArray.[i - 1] outArray.[i - 2]
        else
            outArray.[0] <- L1()
        outArray

let intToNum n =
    let rec sum a i =
        if i = 0
        then a
        else 
            sum (Cons (i % 10, a)) (i / 10)
    if n > 0
    then 1, sum Empty (abs n)
    else -1, sum Empty (abs n)

let time f =
    let start = System.DateTime.Now
    for i = 1 to 5 do f()
    (System.DateTime.Now - start).TotalMilliseconds / 5.00

let charting =
    Chart.Combine
        [
             Chart.Line( [ for i in 1..30 ->     (i, time(fun () -> task8Long (intToNum i) |> ignore) ) ], "8", Color = System.Drawing.Color.Yellow)
             Chart.Line( [ for i in 1..2..100 -> (i, time(fun () -> task9Long (intToNum i) |> ignore) ) ], "9", Color = System.Drawing.Color.Red)
             Chart.Line( [ for i in 1..2..100 -> (i, time(fun () -> task10Long (intToNum i) |> ignore) ) ], "10", Color = System.Drawing.Color.Black)
             Chart.Line( [ for i in 1..2..100 -> (i, time(fun () -> task11Long (intToNum i) |> ignore) ) ], "11", Color = System.Drawing.Color.Gray)
             Chart.Line( [ for i in 1..2..100 -> (i, time(fun () -> task12Long (intToNum i) |> ignore) ) ], "12", Color = System.Drawing.Color.Green)
             Chart.Line( [ for i in 1..100 -> (i, time(fun () -> task13Long (intToNum i) |> ignore) ) ], "13", Color = System.Drawing.Color.Blue)
        ]
do System.Windows.Forms.Application.Run(charting.ShowChart())