I made this tool to allow me to automate mouse actions in clicker games.
Typically you'd record a macro, such as 'click here, here and there, reset (prestige)',
and then play the macro overnight to gain multiplier in such games.

Technically, it's a Windows Forms (boo) GUI with a few buttons, and calling user32.dll 
low level mouse hooks (SetWindowsHookEx) to record mouse clicks and wheel scroll.

The recorded chain of actions is serialized as XML (as I wanted to edit the recording in a text editor),
and played back using SendInput from user32.dll and a timer.

Have fun cheating in clicker games!