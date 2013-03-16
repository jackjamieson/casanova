﻿// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Casanova
open Casanova.Core
open Casanova.Coroutines
open Casanova.Input
open Casanova.Utilities
open Casanova.Game
open Casanova.Math
open Casanova.Action
open Casanova.Drawing
open Casanova.StandardLibrary
open Casanova.StandardLibrary.Core

    type [<CasanovaWorld>] World = { 
      Bar : Bar
    }

    and [<CasanovaEntity>] Bar = {
         Position : Rule<Vector2<m>>
         Velocity : Rule<Vector2<m/s>>
         Sprite   : DrawableSprite
    } with
      static member Position'(self:Bar,dt:float32<s>) = !self.Position + dt * !self.Velocity 
      static member Velocity'(self:Bar,dt:float32<s>) = !self.Velocity * 0.9f
      static member SpritePosition'(self:Bar) = !self.Position * 1.0f<pixel/m>

let start_game (game:StartGameArgs) =

  let world0 = 
    {
      Bar = 
        {
          Position = Rule.Create(Vector2<m>(490.0f<m>,0.0f<m>))
          Velocity = Rule.Create(Vector2<m/s>.Zero)
          Sprite   = DrawableSprite.Create(game.default_layer, Vector2<pixel>.One, Vector2<pixel>.One * 100.0f, @"p1")
        }
    }
  let inline (!) x = immediate_lookup x
  let main = yield_
  
  let input = 
    [
      wait_key_press Keys.Escape  =>> game.quit()
      wait_key_press Keys.F9      =>> game.save "savefile"
      wait_key_press Keys.F10     =>> game.load "savefile"
     // wait_key_down Keys.D        => co{ world0.Bar.Velocity := !world0.Bar.Velocity + Vector2<m/s>.UnitX * 100.0f }
     // wait_key_down Keys.A        => co{ world0.Bar.Velocity := !world0.Bar.Velocity - Vector2<m/s>.UnitX * 100.0f }
      wait_key_down Keys.W        => co{ world0.Bar.Velocity := !world0.Bar.Velocity - Vector2<m/s>.UnitY * 100.0f }
      wait_key_down Keys.S        => co{ world0.Bar.Velocity := !world0.Bar.Velocity + Vector2<m/s>.UnitY * 100.0f }
    ]
  world0,main,input

[<EntryPoint>]
let main argv = 
  use game = Game.Create(start_game, 1024, 600, false)
  game.Run()
  0