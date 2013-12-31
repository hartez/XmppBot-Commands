# Commands Plugin for XmppBot

This is a simple plugin for [XmppBot For HipChat](https://github.com/patHyatt/XmppBot-for-HipChat). It loads up a `.json` file of commands and responses.

## Installation

Copy the .dlls (Bender.dll, SimpleConfig.dll, XmppBot.Common.dll, and XmppBot-Commands.dll) into the /plugins folder. 
 
## Configuration

Add the following to the `<configSections>` element of XmppBot's configuration file:

    <section name="commandsConfig" type="SimpleConfig.Section, SimpleConfig, Version=1.0.29.0, Culture=neutral, PublicKeyToken=null"/>

Then add the following to the `configuration ` element:

	<commandsConfig CommandFilePath="plugins/Commands.json">
    </commandsConfig>

Where `CommandFilePath` is the path to the `.json` file containing the command definitions.

## File Format

The `.json` file should contain an array of command definitions. A command definition consists of a Value (the command itself), the FormatString for the response, and an array of Parameters for the FormatString. Optionally, the command definition can also contain a help message. For example: 

	[

	{FormatString:"{0} dances.", Parameters:[{ParameterType: "User"}], Value: "dance"},

	{FormatString:"{0} hits {1}", Parameters:[{ParameterType: "User"},{ParameterType: "AllArguments", Index: 0, Default: "himself"}], Value: "hit", Help: "!hit [thing]"},
	
	{FormatString:"{0} {1}", Parameters:[{ParameterType: "User"},{ParameterType: "Argument", Index: 0, Default: "gesticulates wildly"}], Value: "me", Help: "!me [thing to do]"},

	{FormatString:"{0}", Parameters:[{ParameterType: "Raw"},], Value: "echo"},

	{FormatString:"http://i.imgur.com/r7ZNHvz.gif", Parameters:[], Value: "slowclap"}
	]

The first command is "dance"; it can be invoked by entering "!dance" into the chat. The bot will reply with "{0} dances.", where '{0}' is replaced by the first parameter in the Parameters array. In this case, the first (and only) parameter is of ParameterType 'User' - the format string will be fed the username of whoever issued the command.

The second command is similar, but has a second parameter of type 'AllArguments'. This means that all of the arguments after the command (starting at Index 0) will be passed in as a single string. So a user named "Bob" issuing "!hit the drums and the cowbell" will result in "Bob hits the drums and the cowbell". If the index were changed to 1, the result would be "Bob hits the drums and the cowbell". The default for the parameter is also set - if there are no arguments, this is what will be used. So "Bob" issuing "!hit" will result in "Bob hits himself".

The second command also specifies a help string - if a user issues the command "!help", the bot will respond with a list of the commands it knows. If no help string is specified, the bot just lists the raw command. 

The third command uses ParameterType "Argument", which gets a single argument; "Bob" issuing "!me wails loudly" will result in "Bob wails".

The fourth command uses a "Raw" ParameterType - it will pass in the raw command as entered. In this case, issuing "!echo everything" will result in "!echo everything".

The last command doesn't use any parameters at all.


[weather]: http://www.eclectrics.com/2009/09/getting-the-current-weather-conditions/