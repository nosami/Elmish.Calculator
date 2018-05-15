// Copyright 2018 Elmish.XamarinForms contributors. See LICENSE.md for license.

namespace Elmish.Calculator

open Elmish
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Elmish.XamarinForms.DynamicViews.SimplerHelpers
open Xamarin.Forms

type Operator = | Add | Subtract | Multiply | Divide 

type Model =
    { operand1: double
      operand2: double
      operator: Operator option }

/// Represents a calculator button
type Msg =
    | Operator of Operator
    | Digit of int
    | Equals
    | Clear

type App() as app =
    inherit Application()

    let init() = { operand1 = 0.0; operand2 = 0.0; operator = None }

    let update msg model =
        match msg with
        | _ -> model

    let view model dispatch =
        let mkNumberButton number row column =
            Xaml.Button(text = string number, command=(fun () -> dispatch (Digit number)))
                .GridRow(row)
                .GridColumn(column)
                .BackgroundColor(Color.White)
                .TextColor(Color.Black)
                .FontSize("36px")

        Xaml.ContentPage(
            Xaml.Grid(rowdefs=[ "2*"; "*"; "*"; "*"; "*"; "*"; "*" ], coldefs=[ "*"; "*"; "*"; "*" ],
                children=[
                    Xaml.Label(fontSize = "48px", fontAttributes = FontAttributes.Bold, backgroundColor = Color.Black, textColor = Color.White, horizontalTextAlignment = TextAlignment.End, verticalTextAlignment = TextAlignment.Center, gridColumnSpan = 4)
                    mkNumberButton 7 1 0; mkNumberButton 8 1 1; mkNumberButton 9 1 2
                    mkNumberButton 4 2 0; mkNumberButton 5 2 1; mkNumberButton 6 2 2
                    mkNumberButton 1 3 0; mkNumberButton 2 3 1; mkNumberButton 3 3 2
                    (mkNumberButton 0 4 0).GridColumnSpan(3)
                ]
            )
        )

    let runner = 
        Program.mkSimple init update view
        |> Program.withConsoleTrace
        |> Program.withDynamicView app
        |> Program.run
        