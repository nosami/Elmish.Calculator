namespace Elmish.Calculator.MacOS
open System
open Foundation
open AppKit
open Xamarin.Forms
open Xamarin.Forms.Platform.MacOS

[<Register ("AppDelegate")>]
type AppDelegate () =
    inherit FormsApplicationDelegate ()

    let style = NSWindowStyle.Closable ||| NSWindowStyle.Resizable ||| NSWindowStyle.Titled
    let rect = new CoreGraphics.CGRect(nfloat 200.0, nfloat 1000.0, nfloat 1024.0, nfloat 768.0)
    let window = new NSWindow(rect, style, NSBackingStore.Buffered, false, TitleVisibility = NSWindowTitleVisibility.Hidden)

    override this.MainWindow = window

    override this.DidFinishLaunching(notification) =
        Forms.Init()
        this.LoadApplication (new Elmish.Calculator.App())
        base.DidFinishLaunching(notification)
