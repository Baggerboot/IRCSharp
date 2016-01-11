﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using IRCSharp.IRC;

namespace IRCSharp.IrcCommandProcessors.Quirks
{
	public class SlackIrcProcessor : DataProcessor
	{
		private string RecursiveReplace(string input, string pattern, string replacement)
		{
			string currentMessage;
			string newMessage = input;
			do
			{
				currentMessage = newMessage;
				newMessage = Regex.Replace(currentMessage, pattern, replacement);
			} while (currentMessage != newMessage);
			return newMessage;
		}

		public override IrcMessage ProcessMessage(IrcMessage message)
		{
			var newMessage = RecursiveReplace(message.Message, @"^(.*?)(https?:\/\/)(.*?)( )(.*)$", "$1slack-workaround://$3$5");
			newMessage = newMessage.Replace("slack-workaround://", "http://");
			newMessage = RecursiveReplace(newMessage, @"^(.*? |)(.+)( https?:\/\/\2)(.*)$", "$1$2$4");

			return new IrcMessage
			{
				Action = message.Action,
				Channel = message.Channel,
				Message = newMessage,
				Sender = message.Sender
			};
		}
	}
}
