# LoLScoreboard
Takes a screenshot of the League of Legends scoreboard whenever TAB has been pressed.  
Why? Because I want a scoreboard on my second monitor without it blocking my view.

# Requirements
* Microsoft Visual Studio 2015 or better
* Windows 7 or better
* .NET Framework 4.5.2 or better (might work with 4.0?)

# Configuration
Open `Worker.cs` and make sure the debug define has been set  
`#DEFINE DEBUG`  
Run the application in Debug mode. Place the cursor on the **top left** corner of the scoreboard and hit TAB. Note the output.  
Repeat the same, but this time for **bottom right** corner of the scoreboard.  

The four values you have gotten (top left and bottom right tuple) are the coordinates.  
Adjust them accordingly like so:  
Suppose we have gotten `X: 310, Y: 243` as the first tuple (top left) and `X: 1131, Y: 570` as the second one (bottom right).
The variables would then be:
```cs
// Coordinates
int[] topLeft       = { 310, 243 };
int[] bottomRight   = { 1131, 570 };
```

Now, restart the application and confirm it's working as intended. Continue adjusting the coordinates until you are satisfied with the result.