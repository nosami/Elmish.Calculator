namespace Elmish.Calculator

open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms

type Operator = Add | Subtract | Multiply | Divide 

/// Represents a calculator button press
type Msg =
    | Operator of Operator
    | Digit of int
    | Equals
    | Clear

type Operand = double

// We can't represent an invalid state with this model.
// This greatly reduces the amount of validation required.
type State =
    | Initial
    | Operand of Operand // 1
    | OperandOperator of Operand * Operator // 1 +
    | OperandOperatorOperand of Operand * Operator * Operand // 1 + 1
    | Result of double // 2
    | Error

module App =
    let calculateOperation op1 op2 operator =
        match operator with
        | Add -> op1 + op2
        | Subtract -> op1 - op2
        | Multiply -> op1 * op2
        | Divide -> op1 / op2

    let calculate model msg =
        match model with
        | OperandOperatorOperand (_, Divide, 0.0) -> Error
        | OperandOperatorOperand (op1, operator, op2) ->
            let res = calculateOperation op1 op2 operator
            match msg with
            | Equals -> Result(res)
            | Operator operator ->
                // pass the result in as the start of a new calculation (1 + 1 + -> 2 +)
                OperandOperator(res, operator)
            | _ -> model
        | _ -> model

    let update msg model =
        match msg with
        | Clear -> Initial
        | Digit digit ->
            match model with
            | Initial | Error | Result _ -> Operand (double digit)
            | Operand op -> Operand (double (string op + string digit))
            | OperandOperator (operand, operator) -> OperandOperatorOperand (operand, operator, double digit)
            | OperandOperatorOperand (op1, operator, op2) -> OperandOperatorOperand (op1, operator, double (string op2 + string digit))
        | Operator operator ->
            match model with
            | Initial | Error -> model
            | Result operand // previously calculated result is now the first operand
            | Operand operand | OperandOperator (operand, _) -> OperandOperator(operand, operator) 
            | OperandOperatorOperand _ -> calculate model msg
        | Equals -> calculate model msg

    let display model =
        match model with
        | Initial -> "0"
        | Operand op | OperandOperator (op, _) | OperandOperatorOperand (_, _, op) -> string op
        | Result res -> string res
        | Error -> "Error"

    let view (model: State) dispatch =
        let mkButton text command row column =
            Xaml.Button(text = text, command=(fun () -> dispatch command))
                .GridRow(row)
                .GridColumn(column)
                .FontSize(36.0)
                //.ButtonCornerRadius(0)

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
            Xaml.Grid(rowdefs=[ "*"; "*"; "*"; "*"; "*"; "*" ], coldefs=[ "*"; "*"; "*"; "*" ],
                children=[
                    Xaml.Label(text = display model, fontSize = 48.0, fontAttributes = FontAttributes.Bold, backgroundColor = Color.Black, textColor = Color.White, horizontalTextAlignment = TextAlignment.End, verticalTextAlignment = TextAlignment.Center).GridColumnSpan(4)
                    mkNumberButton 7 1 0; mkNumberButton 8 1 1; mkNumberButton 9 1 2
                    mkNumberButton 4 2 0; mkNumberButton 5 2 1; mkNumberButton 6 2 2
                    mkNumberButton 1 3 0; mkNumberButton 2 3 1; mkNumberButton 3 3 2
                    (mkNumberButton 0 4 0).GridColumnSpan(3)
                    mkOperatorButton "÷" Divide 1 3
                    mkOperatorButton "×" Multiply 2 3
                    mkOperatorButton "-" Subtract 3 3
                    mkOperatorButton "+" Add 4 3
                    (mkButton "C" Clear 5 0).BackgroundColor(gray).TextColor(Color.White)
                    (mkButton "=" Equals 5 1).BackgroundColor(orange).GridColumnSpan(3).TextColor(Color.White)
                ], rowSpacing = 1.0, columnSpacing = 1.0, backgroundColor = gray
            )
        )

    let program = 
        Program.mkSimple (fun() -> Initial) update view
        |> Program.withConsoleTrace

type App() as app =
    inherit Application()

    let runner = App.program |> Program.runWithDynamicView app

    #if DEBUG
    do runner.EnableLiveUpdate ()
    #endif