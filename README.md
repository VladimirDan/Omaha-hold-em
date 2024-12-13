# Omaha Hold'em

This is a simple Omaha poker game implemented in Unity. The game allows a player to compete against 4 AI players in a poker round with features like placing bets, managing chips, handling community cards, and managing player roles like Small Blind and Big Blind.

## Features
- **AI Opponents**: The player will compete against 4 AI players that simulate betting and decision-making strategies.
- **Poker Hands Management**: Evaluation of poker hands based on community cards and player hands.
- **Blinds Management**: Automatically determines the Small Blind and Big Blind players in each round.
- **Betting System**: Players can raise, call, or fold during the betting rounds.
- **Chip Management**: Each player has a stack of chips that can be increased or decreased based on their actions.

## Controls for Omaha Hold'em Poker

The game uses the following buttons for player actions:

- **Restart poker round** — Restarts the current poker round.  
- **Submit hand** — Confirms the player's selected hand.  
- **Fold** — Folds the cards and exits the current round.  
- **Check** — Passes the turn without betting if allowed.  
- **All-In** — Bets all available chips for the current round.  
- **Call** — Matches the current bet.  
- **Raise** — Increases the bet. The amount can be entered in the input field **to the right** of the Raise button.

## How to Play:
1. **Blinds**: Each betting round starts with the player in the **Small Blind** position. This player must place a small blind bet, which is typically set to 1 chip. The player can simply click **Call** to match the amount of the small blind.
2. **Betting rounds**: After the Small Blind player has acted, the game moves to the other players. They can place bets by selecting options like Fold, Check, Call, All-In, or Raise.
3. **Choosing your hand**: After the final betting round, the player must select the best possible hand by clicking on the cards. You need to choose 3 community cards (on the table) and 2 of your hole cards to form your hand.
4. **Submit hand**: Once you've selected your hand, click on **Submit hand** to confirm your choice.
5. **Reveal hands**: After all players have submitted their hands, the game will determine the winner based on the best combination.
6. **Winner gets the pot**: The player with the best hand wins the pot.
7. **Restart round**: To start a new round, click **Restart poker round**.
8. **End of the game**: If all players except one have run out of chips, the game ends. At this point, you can go back to the main menu and click **Restart Game** to begin a new game.

