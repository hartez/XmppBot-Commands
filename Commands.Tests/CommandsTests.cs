using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;
using XmppBot.Common;
using XmppBot_Commands;

namespace Commands.Tests
{
    [TestFixture]
    public class CommandsTests
    {
        [Test]
        public void should_slap_with_default()
        {
            var plugin = new CommandPlugin();

            var pl = new ParsedLine("!slap", "Bob");

            plugin.Evaluate(pl).Should().Be("Bob slaps himself");
        }

        [Test]
        public void should_respond_with_slowclap_image_url()
        {
            var plugin = new CommandPlugin();

            var pl = new ParsedLine("!slowclap", "Bob");

            plugin.Evaluate(pl).Should().Be("http://i.imgur.com/r7ZNHvz.gif");
        }

        [Test]
        public void should_slap_with_first_arg()
        {
            var plugin = new CommandPlugin();

            var pl = new ParsedLine("!slap Jim", "Bob");

            plugin.Evaluate(pl).Should().Be("Bob slaps Jim");
        }

        [Test]
        public void should_hug_with_all_args()
        {
            var plugin = new CommandPlugin();

            var pl = new ParsedLine("!hug Jim and his monkey", "Bob");

            plugin.Evaluate(pl).Should().Be("Bob hugs Jim and his monkey");
        }

        [Test]
        public void should_hug_with_default()
        {
            var plugin = new CommandPlugin();

            var pl = new ParsedLine("!hug", "Bob");

            plugin.Evaluate(pl).Should().Be("Bob hugs himself");
        }

        [Test]
        public void should_smack_with_all_args()
        {
            var plugin = new CommandPlugin();

            var pl = new ParsedLine("!smack Jim and his monkey", "Bob");

            plugin.Evaluate(pl).Should().Be("Bob smacks Jim and his monkey around with a large trout");
        }

        [Test]
        public void should_smack_with_default()
        {
            var plugin = new CommandPlugin();

            var pl = new ParsedLine("!smack", "Bob");

            plugin.Evaluate(pl).Should().Be("Bob smacks himself around with a large trout");
        }

        [Test]
        public void should_display_help()
        {
            var plugin = new CommandPlugin();

            var pl = new ParsedLine("!help", "Bob");

            plugin.Evaluate(pl).Should().Contain("the commands I know");

            plugin.Evaluate(pl).Should().Contain("!slap [thing]");

            Debug.Write(plugin.Evaluate(pl));
        }

        [Test]
        public void should_display_directory()
        {
            var plugin = new CommandPlugin();

            var pl = new ParsedLine("!dir", "Bob");

            var result = plugin.Evaluate(pl);

            Debug.WriteLine(result);

            result.Should().Contain("Directory:");
        }
    }
}