# Commands Plugin for XmppBot

This is a simple plugin for [XmppBot For HipChat](https://github.com/patHyatt/XmppBot-for-HipChat). It loads up a `.json` file which can define commands with formatted responses or commands which execute a PowerShell command.

## Installation

Copy the .dlls (Bender.dll, SimpleConfig.dll, XmppBot.Common.dll, and XmppBot-Commands.dll) into the /plugins folder. 
 
## Configuration

Add the following to the `<configSections>` element of XmppBot's configuration file:

    <section name="commandsConfig" type="SimpleConfig.Section, SimpleConfig, Version=1.0.29.0, Culture=neutral, PublicKeyToken=null"/>

Then add the following to the `configuration ` element:

	<commandsConfig CommandFilePath="plugins/Commands.json">
    </commandsConfig>

Where `CommandFilePath` is the path to the `.json` file containing the command definitions.

## Command File Format

The `.json` file should contain an array of command definitions. There are currently two types of commands: "FormatResponse" and "Shell". A command definition consists of a Value (the command itself), the ChatCommand (used to invoke the command from chat), and an array of Parameters for the command. Optionally, the command definition can also contain a help message. 

### FormatResponse Commands

These are basically just calls to `String.Format`, but with the option to use arguments from the chat message being evaluated.

For example: 

	[
		{
			ChatCommand:"dance", Value:"FormatResponse", 
			Parameters:[{ParameterType:"Predefined", Value:"{0} dances."}, {ParameterType: "User"}]
		},

		{
			ChatCommand:"hit", Value:"FormatResponse", Help: "!hit [thing]"},
			Parameters:[
						{ParameterType:"Predefined", Value:"{0} hits {1}"}, 
						{ParameterType: "User"},
						{ParameterType: "AllArguments", Index: 0, Default: "himself"}] 
		},
	
		{
			ChatCommand:"me", Value: "FormatResponse", Help: "!me [thing to do]",
			Parameters:[
						{ParameterType:"Predefined", Value:"{0} {1}", 
						{ParameterType: "User"},
						{ParameterType: "Argument", Index: 0, Default: "gesticulates wildly"}] 
		},

		{
			ChatCommand:"echo", Value: "FormatResponse",
			Parameters:[{ParameterType: "Predefined", Value:"{0}"}, {ParameterType: "Raw"}]
		},
	
		{
			ChatCommand:"slowclap", Value: "FormatResponse",
			Parameters:[{ParameterType:"Predefined", Value:"http://i.imgur.com/r7ZNHvz.gif"}] 
		}
	]

The first command is a 'FormatResponse' command called 'dance'; it can be invoked by entering "!dance" into the chat. The bot will reply with "{0} dances.", where '{0}' is replaced by the name of the user who issued the command. The first parameter is the format string to use for the response; the ParameterType of 'Predefined' means that it will be passed in as is, with no replacement. The second parameter is of ParameterType 'User', so it will be replaced with the user who issued the command.  

The second command is similar, but has a third parameter of type 'AllArguments'. This means that all of the arguments after the command (starting at Index 0) will be passed in as a single string. So a user named "Bob" issuing "!hit the drums and the cowbell" will result in "Bob hits the drums and the cowbell". If the index were changed to 3, the result would be "Bob hits the cowbell". The default for the parameter is also set - if there are no arguments, this is what will be used. So "Bob" issuing "!hit" will result in "Bob hits himself".

The second command also specifies a help string - if a user issues the command "!help", the bot will respond with a list of the commands it knows. If no help string is specified, the bot just lists the raw command. 

The third command uses ParameterType "Argument", which gets a single argument; "Bob" issuing "!me wails loudly" will result in "Bob wails".

The fourth command uses a "Raw" ParameterType - it will pass in the raw command as entered. In this case, issuing "!echo everything" will result in "!echo everything".

The last command just emits the predefined string.

### Shell Commands

These allow you to define chat commands which run PowerShell commands and return the results from standard out. This is a convenient way to automate things (for example, kicking off a build with something like [psake]) or to pass of chat line evaluation to another program. 

Here's an example that will add a '!weather' command to the bot which will retrieve the current temperature at the [NOAA weather station] in Boulder, CO:

	[{
		ChatCommand : "weather",
		Value : "Shell",
		Parameters : [{
				ParameterType : "Predefined",
				Value : "([xml](Invoke-WebRequest -URI http://w1.weather.gov/xml/current_obs/KBDU.xml).Content).current_observation.temperature_string"
			}]
	}]

Invoking '!weather' in the chat will result in something like "55.0 F (13.0 C)" (this works in PowerShell 3; I haven't tried it in earlier versions of PowerShell). Parameters are passed into the Shell command in the same way as the FormatResponse command, so you can do things like this:

 	[{
		ChatCommand : "scramble",
		Value : "Shell",
		Parameters : [{
				ParameterType : "Predefined",
				Value : "New-Object System.String((Get-Random -InputObject (\"{0}\".ToCharArray()) -Count (\"{0}\".Length)),0,\"{0}\".Length)"
			}, {ParameterType : "User"}]
	}]

This will return a randomly scrambled version of the name of the user invoking the command. The formatting rules for the command work just like they do in the FormatResponse examples above. If a user named Bob invoked "!scramble", the following would be executed in PowerShell:

	New-Object System.String((Get-Random -InputObject ("Bob".ToCharArray()) -Count ("Bob".Length)),0,"Bob".Length)

As always with something as powerful as PowerShell, be very, very careful what you allow for input and who you allow to run commands like these. Pretty much any time you allow arbitrary user input into a command like this you're leaving yourself open to [shell injection]. I would suggest limiting your commands to only 'Predefined' parameters and 'User' parameters; if you do add other parameters, make sure to restrict the access of the account under which the bot runs.  

[NOAA weather station]: http://www.eclectrics.com/2009/09/getting-the-current-weather-conditions/
[psake]: https://github.com/psake/psake
[shell injection]: http://en.wikipedia.org/wiki/Code_injection#Shell_injection