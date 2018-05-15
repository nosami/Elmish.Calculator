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

type App() as app =
    inherit Application()

    let init() = { operand1 = 0.0; operand2 = 0.0; operator = None }

    let update msg model =
        match msg with
        | _ -> model

    let view model dispatch =
        Xaml.ContentPage()

    let runner = 
        Program.mkSimple init update view
        |> Program.withConsoleTrace
        |> Program.withDynamicView app
        |> Program.run
        