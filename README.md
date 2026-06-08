# TexasHoldemGameEngine

Texas Hold'em game engine for .NET. This repository gives you:

- A reusable poker engine in `TexasHoldem.Logic`
- Sample AI players in `TexasHoldem.AI.DummyPlayer` and `TexasHoldem.AI.SmartPlayer`
- A console UI sample in `TexasHoldem.UI.Console`
- Hand-evaluation helpers for cards, rankings, comparisons, and best-hand detection

If you only want to use the engine, the main thing to implement is `IPlayer` or, more commonly, inherit from `BasePlayer` and override `PostingBlind` and `GetTurn`.

## Packages and projects

- NuGet package: `TexasHoldemGameEngine`
- Main library target: `.NET Standard 2.0`
- Console sample target: `.NET Core 3.1`

Install from NuGet:

```bash
dotnet add package TexasHoldemGameEngine --version 2.0.0
```

Or reference the project directly:

```xml
<ProjectReference Include="..\TexasHoldem.Logic\TexasHoldem.Logic.csproj" />
```

## What the engine does

The engine runs complete Texas Hold'em hands and games:

- Deals hole cards and community cards
- Posts blinds
- Runs betting rounds for `PreFlop`, `Flop`, `Turn`, and `River`
- Tracks stacks, pot, main pot, and side pots
- Evaluates hands at showdown
- Ends when one player remains with money

The public entry point is `TexasHoldemGame`.

## Quick start

The smallest useful integration is: create players, create a game, and call `Start()`.

```csharp
using System.Collections.Generic;
using TexasHoldem.AI.DummyPlayer;
using TexasHoldem.AI.SmartPlayer;
using TexasHoldem.Logic.GameMechanics;
using TexasHoldem.Logic.Players;

var players = new List<IPlayer>
{
    new SmartPlayer(),
    new DummyPlayer(),
    new SmartPlayer(),
};

ITexasHoldemGame game = new TexasHoldemGame(players, initialMoney: 200);
IPlayer winner = game.Start();

Console.WriteLine($"Winner: {winner.Name}");
Console.WriteLine($"Hands played: {game.HandsPlayed}");
```

You can also create a heads-up game:

```csharp
var game = new TexasHoldemGame(new SmartPlayer(), new DummyPlayer(), initialMoney: 1000);
var winner = game.Start();
```

## The player model

To plug in your own bot, implement `IPlayer` or inherit from `BasePlayer`.

`BasePlayer` is the easiest option because it already stores:

- `FirstCard`
- `SecondCard`
- `CommunityCards`

and provides empty virtual implementations for lifecycle callbacks you may not care about.

Example custom player:

```csharp
using System;
using TexasHoldem.Logic.Players;

public class TightPlayer : BasePlayer
{
    public override string Name { get; } = "TightPlayer_" + Guid.NewGuid();

    // -1 means "use the game's initialMoney value"
    public override int BuyIn { get; } = -1;

    public override PlayerAction PostingBlind(IPostingBlindContext context)
    {
        return context.BlindAction;
    }

    public override PlayerAction GetTurn(IGetTurnContext context)
    {
        if (context.CanCheck)
        {
            return PlayerAction.CheckOrCall();
        }

        if (context.MoneyToCall <= context.SmallBlind * 2)
        {
            return PlayerAction.CheckOrCall();
        }

        return PlayerAction.Fold();
    }
}
```

Use it in a game:

```csharp
var game = new TexasHoldemGame(new IPlayer[]
{
    new TightPlayer(),
    new TightPlayer(),
    new SmartPlayer(),
});

var winner = game.Start();
```

## Player lifecycle

Your player receives callbacks in this order:

1. `StartGame`
2. `StartHand`
3. `StartRound` for preflop
4. `PostingBlind` if this player is small blind or big blind
5. `GetTurn` zero or more times during betting
6. `EndRound`
7. `StartRound` for flop, then betting, then `EndRound`
8. `StartRound` for turn, then betting, then `EndRound`
9. `StartRound` for river, then betting, then `EndRound`
10. `EndHand`
11. After the whole game finishes: `EndGame`

Important detail: `StartHand` is where your hole cards are set, and `StartRound` is where community cards become visible to your player.

## `IPlayer` and what each method does

### Required properties

- `Name`: the player's display name. Treat names as unique.
- `BuyIn`: starting stack for this player. Use `-1` to inherit the game's `initialMoney`.

### Lifecycle methods

- `StartGame(IStartGameContext context)`: called when a player enters the game or is automatically rebought.
- `StartHand(IStartHandContext context)`: receives the two hole cards and hand metadata.
- `StartRound(IStartRoundContext context)`: called at the beginning of each betting street with the current community cards and pot info.
- `PostingBlind(IPostingBlindContext context)`: called when the player posts a blind.
- `GetTurn(IGetTurnContext context)`: your main decision point for check/call/fold/raise.
- `EndRound(IEndRoundContext context)`: called after a betting round finishes.
- `EndHand(IEndHandContext context)`: called after the hand finishes. Showdown cards are available here.
- `EndGame(IEndGameContext context)`: called when the overall winner is known.

## Context objects

These are the objects your player receives during the lifecycle.

### `StartGameContext`

- `PlayerNames`: names of everyone currently at the table
- `StartMoney`: stack assigned for this game entry

### `StartHandContext`

- `FirstCard`
- `SecondCard`
- `HandNumber`
- `MoneyLeft`
- `SmallBlind`
- `FirstPlayerName`: the player with action priority for the hand

### `StartRoundContext`

- `RoundType`: `PreFlop`, `Flop`, `Turn`, `River`
- `CommunityCards`
- `MoneyLeft`
- `CurrentPot`
- `CurrentMainPot`
- `CurrentSidePots`

### `PostingBlindContext`

- `BlindAction`: the already-constructed blind action, typically return this unchanged
- `CurrentPot`
- `CurrentStackSize`

### `GetTurnContext`

- `RoundType`
- `PreviousRoundActions`: actions already taken this betting round
- `SmallBlind`
- `MoneyLeft`
- `CurrentPot`
- `MyMoneyInTheRound`
- `CurrentMaxBet`
- `MinRaise`
- `MainPot`
- `SidePots`
- `CanCheck`
- `CanRaise`
- `MoneyToCall`
- `IsAllIn`

This is the most important context type. In practice:

- `MoneyToCall` is what you must put in to continue
- `CanCheck` means `MoneyToCall == 0`
- `CanRaise` means the engine currently allows a legal raise
- `MinRaise` is the minimum raise increment, not the table's final total bet size

### `EndRoundContext`

- `RoundActions`: all actions taken in the round

### `EndHandContext`

- `ShowdownCards`: dictionary of `player name -> hole cards` for players who reached showdown

### `EndGameContext`

- `WinnerName`

## Actions

Use `PlayerAction` to respond during blinds and turns.

### `PlayerAction.Fold()`

Folds the current hand.

### `PlayerAction.CheckOrCall()`

- Checks if nothing needs to be called
- Calls otherwise

### `PlayerAction.Raise(int withAmount)`

Raises by an increment amount, not by the final total amount placed in the round.

Examples:

- If `MoneyToCall = 10` and you return `Raise(20)`, the player first calls 10 and then raises 20 more.
- If `withAmount <= 0`, the engine treats it like `CheckOrCall()`.
- If `withAmount` is larger than the available stack, the engine converts it to all-in.
- If `withAmount` is below the minimum legal raise, the engine still forces at least the minimum legal raise when possible.

### `PlayerAction.Post(int blind)`

Used by the engine to post blinds.

## Main engine classes

### `TexasHoldemGame`

Namespace: `TexasHoldem.Logic.GameMechanics`

This is the main game runner.

Constructors:

- `TexasHoldemGame(IPlayer firstPlayer, IPlayer secondPlayer, int initialMoney = 1000)`
- `TexasHoldemGame(IList<IPlayer> players, int initialMoney = 200)`

Behavior:

- Supports 2 to 10 players
- Rejects non-positive stacks and stacks above `200000`
- Calls `Start()` to play until one player remains with money
- Exposes `HandsPlayed`
- Returns the winning `IPlayer` from `Start()`

### `BasePlayer`

Namespace: `TexasHoldem.Logic.Players`

Convenience base class for bot implementations.

Use this if you want:

- Stored hole cards in `FirstCard` and `SecondCard`
- Stored board cards in `CommunityCards`
- Empty default implementations for non-essential callbacks

### `PlayerDecorator`

Namespace: `TexasHoldem.Logic.Players`

Wraps another `IPlayer`. Useful for:

- Logging
- Telemetry
- UI decoration
- Debug output

The console sample uses this pattern in `ConsoleUiDecorator`.

## Card and deck classes

### `Card`

Namespace: `TexasHoldem.Logic.Cards`

Immutable value-like object representing one card.

Members:

- `Suit`
- `Type`
- `DeepClone()`
- `ToString()`: returns friendly text with rank and suit symbol
- `FromHashCode(int hashCode)`: rebuilds a card from the engine's hash format

### `CardSuit`

Suit enum:

- `Club`
- `Diamond`
- `Heart`
- `Spade`

### `CardType`

Rank enum:

- `Two` through `Ace`

### `Deck` and `IDeck`

Namespace: `TexasHoldem.Logic.Cards`

`Deck` is a shuffled 52-card deck. `GetNextCard()` deals one card and throws `InternalGameException` if the deck is empty.

Useful member:

- `Deck.AllCards`: read-only list of all 52 cards

## Hand evaluation and ranking

If you only need hand strength logic without running a full game, use the helper APIs.

### `HandRankType`

Possible hand categories:

- `HighCard`
- `Pair`
- `TwoPairs`
- `ThreeOfAKind`
- `Straight`
- `Flush`
- `FullHouse`
- `FourOfAKind`
- `StraightFlush`

### `BestHand`

Namespace: `TexasHoldem.Logic.Helpers`

Represents the best 5-card hand the evaluator found.

Members:

- `RankType`
- `Cards`: the five card ranks that define the hand
- `CompareTo(BestHand other)`: compares two best hands

### `IHandEvaluator` and `HandEvaluator`

Use `GetBestHand(IEnumerable<Card> cards)` to evaluate a hand from hole cards plus community cards.

Example:

```csharp
using System;
using TexasHoldem.Logic.Cards;
using TexasHoldem.Logic.Helpers;

IHandEvaluator evaluator = new HandEvaluator();

var bestHand = evaluator.GetBestHand(new[]
{
    new Card(CardSuit.Spade, CardType.Ace),
    new Card(CardSuit.Heart, CardType.Ace),
    new Card(CardSuit.Club, CardType.King),
    new Card(CardSuit.Diamond, CardType.Queen),
    new Card(CardSuit.Spade, CardType.Jack),
    new Card(CardSuit.Heart, CardType.Ten),
    new Card(CardSuit.Club, CardType.Two),
});

Console.WriteLine(bestHand.RankType); // Straight
```

### `Helpers`

Namespace: `TexasHoldem.Logic.Helpers`

Convenience static methods:

- `GetHandRank(ICollection<Card> cards)`: returns a `HandRankType`
- `CompareCards(IEnumerable<Card> firstPlayerCards, IEnumerable<Card> secondPlayerCards)`: compares two 5+ card sets
- `GetHandRankValue(IEnumerable<Card> player, IEnumerable<IEnumerable<Card>> opponents, IEnumerable<Card> communityCards)`: multi-opponent strength helper used by the engine

## Utility classes

### `CardExtensions`

Friendly formatting helpers:

- `CardSuit.ToFriendlyString()`
- `CardType.ToFriendlyString()`

### `EnumerableExtensions`

Helpers on enumerables:

- `Shuffle<T>()`
- `CardsToString()`

### `RandomProvider`

Thread-local random number provider used by sample players and shuffle logic.

### `InternalGameException`

Engine-specific exception type used for internal rule violations, for example drawing from an empty deck.

### `IDeepCloneable<T>`

Small cloning interface currently used by `Card`.

## Built-in players

### `DummyPlayer`

Random/simple bot:

- Usually checks or calls
- Sometimes folds
- Sometimes minimum-raises
- Rarely jams all-in

Good for smoke tests and simulations.

### `SmartPlayer`

A simple preflop-oriented bot:

- Evaluates only preflop starting hand strength
- Folds weak holdings unless it can check
- Sometimes raises stronger hands
- Defaults to check/call postflop

Important: despite the name, it is intentionally basic and not a strong poker AI.

### `ConsolePlayer`

Human-controlled player for the sample console app.

Keys:

- `C` for check/call
- `R` for raise
- `F` for fold
- `A` for all-in

## Example: logging player actions

One nice pattern is decorating a player rather than editing the bot itself.

```csharp
using System;
using TexasHoldem.Logic.Players;

public class LoggingPlayer : PlayerDecorator
{
    public LoggingPlayer(IPlayer innerPlayer) : base(innerPlayer)
    {
    }

    public override PlayerAction GetTurn(IGetTurnContext context)
    {
        var action = base.GetTurn(context);
        Console.WriteLine($"{Name}: {action}");
        return action;
    }
}
```

Use it like this:

```csharp
var players = new IPlayer[]
{
    new LoggingPlayer(new SmartPlayer()),
    new LoggingPlayer(new DummyPlayer()),
};

var game = new TexasHoldemGame(players);
game.Start();
```

## Repository layout

```text
src/
  TexasHoldem.Logic/              Core engine, cards, helpers, players, game mechanics
  AI/
    TexasHoldem.AI.DummyPlayer/   Very simple sample bots
    TexasHoldem.AI.SmartPlayer/   Slightly smarter sample bot
  UI/
    TexasHoldem.UI.Console/       Interactive console demo
  Tests/
    TexasHoldem.Logic.Tests/      Unit tests for cards, helpers, and mechanics
    TexasHoldem.Tests.GameSimulations/  Simulation runner
```

## Important engine behaviors and caveats

These are worth knowing before you build on top of the library:

- Player count must be between 2 and 10.
- `BuyIn = -1` means "use the game's `initialMoney`".
- Players with zero money are automatically re-entered through `StartGame(...)` with a fresh stack.
- `TexasHoldemGame.Start()` plays until only one player at the table has money.
- The blind escalation table exists in the code, but the current implementation uses the first small blind value only, so blinds stay fixed unless you change the engine.
- `PlayerAction.Raise(amount)` uses raise increment semantics, not "set my total bet to X".
- `EndHandContext.ShowdownCards` only contains players who were still in the hand at showdown.
- `BestHand` compares rank first, then kickers / pair ranks / straight high card depending on the hand type.

## Running the samples

Run the console UI:

```bash
dotnet run --project src/UI/TexasHoldem.UI.Console/TexasHoldem.UI.Console.csproj
```

Run the tests:

```bash
dotnet test src/TexasHoldem.sln
```

Run the simulation project:

```bash
dotnet run --project src/Tests/TexasHoldem.Tests.GameSimulations/TexasHoldem.Tests.GameSimulations.csproj
```

## Which classes should you care about first?

If you are new to the codebase, start here:

1. `TexasHoldemGame`
2. `IPlayer`
3. `BasePlayer`
4. `PlayerAction`
5. `GetTurnContext`
6. `HandEvaluator` and `Helpers`
7. `Card`, `Deck`, `CardType`, `CardSuit`

That is enough to build a custom poker bot, simulate games, and evaluate hands.
