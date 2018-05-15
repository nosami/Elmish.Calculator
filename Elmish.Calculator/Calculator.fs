// Copyright 2018 Elmish.XamarinForms contributors. See LICENSE.md for license.

namespace Elmish.Calculator

open Elmish
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Elmish.XamarinForms.DynamicViews.SimplerHelpers
open Xamarin.Forms

type Operator = Add | Subtract | Multiply | Divide 

type Model =
    { operand1: double
      operand2: double
      operator: Operator option }

/// Represents a calculator button press
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
        let mkButton text command row column =
            Xaml.Button(text = text, command=(fun () -> dispatch command))
                .GridRow(row)
                .GridColumn(column)
                .FontSize("36px")

        let mkNumberButton number row column =
            (mkButton (string number) (Digit number) row column)
                .BackgroundColor(Color.White)
                .TextColor(Color.Black)

        let orange = Color.FromRgb(0xff, 0xa5, 0)
        let gray = Color.FromRgb(0x80, 0x80, 0x80)

        let mkOperatorButton text operator row column =
            (mkButton text (Operator operator) row column)
                .BackgroundColor(orange)
                .TextColor(Color.Black)

        Xaml.ContentPage(
            Xaml.Grid(rowdefs=[ "2*"; "*"; "*"; "*"; "*"; "*"; "*" ], coldefs=[ "*"; "*"; "*"; "*" ],
                children=[
                    Xaml.Label(fontSize = "48px", fontAttributes = FontAttributes.Bold, backgroundColor = Color.Black, textColor = Color.White, horizontalTextAlignment = TextAlignment.End, verticalTextAlignment = TextAlignment.Center).GridColumnSpan(4)
                    mkNumberButton 7 1 0; mkNumberButton 8 1 1; mkNumberButton 9 1 2
                    mkNumberButton 4 2 0; mkNumberButton 5 2 1; mkNumberButton 6 2 2
                    mkNumberButton 1 3 0; mkNumberButton 2 3 1; mkNumberButton 3 3 2
                    (mkNumberButton 0 4 0).GridColumnSpan(3)
                    mkOperatorButton "÷" Divide 1 3
                    mkOperatorButton "×" Multiply 2 3
                    mkOperatorButton "-" Subtract 3 3
                    mkOperatorButton "+" Add 4 3
                    (mkButton "C" Clear 5 0).BackgroundColor(gray)
                    (mkButton "=" Equals 5 1).BackgroundColor(orange).GridColumnSpan(3)
                ]
            )
        )

    let runner = 
        Program.mkSimple init update view
        |> Program.withConsoleTrace
        |> Program.withDynamicView app
        |> Program.run
        