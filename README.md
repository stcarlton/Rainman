# Rainman
An AR Card Counting app for iOS.

## Description
Rainman is an augmented reality tool that suggests blackjack betting
strategy and playing strategies based on the cards that have already
been played. Rainman applies computer vision to recognize cards,
utilizing the card positions to deduce whether each card is owned by
the player, the dealer or another player. The popular card counting
strategy "Hi-Lo" is employed to dynamically update betting and
playing suggestions.

Rainman is an iOS app developed with Unity game engine. Source code
is C# and follows MVC design pattern. ARKit package is utilized for
AR functionality.

## Legend
![image](https://user-images.githubusercontent.com/58635162/193425318-e22f0f5d-3f62-45bb-ba63-29ff02e43726.png)

### Top Left
* Count
    * The HI-Lo Cumulative Count Total
    * Cards 2-6 increase the count
    * Cards 7-9 do not affect the count
    * Cards 10 and Face Cards decrease the count
    * Count is adjusted as per ratio of 52 cards reamining in the deck

* Bet Strategy
    * Count < 1 : Bet Minimum
    * Count < 3 : Bet Big
    * Count >= 3 : Bet Maximum

### Lower Left
* Display of Dealer's Assumed Cards
* Display of Your Assumed Hand
* Your Hand Value

### Upper Right
* Display of all cards in the deck
* Cards are greyed out once recognized

### Lower Right
* Suggested move
* Default is Basic Strategy
* Adjusted Dynamically based on cards that have been seen

### Lower Center
* Button to reset counter
