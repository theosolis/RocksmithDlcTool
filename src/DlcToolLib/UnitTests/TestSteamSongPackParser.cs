using NUnit.Framework;
using Shouldly;

namespace DlcToolLib.UnitTests
{
	[TestFixture]
	public class TestSteamSongPackParser
	{
		private const string SingleArtist3PackDesc =
			@"Play ""Remedy"", ""Fake It"", and ""Broken"" by Seether on any electric guitar or bass. Each song includes a new authentic tone.";

		private const string SingleArtist3PackDescParsing =
			@"""Remedy"", ""Fake It"", and ""Broken"" by Seether";

		private const string MultiPackNoArtistDesc =
			@"Play ""Who Do You Love?"", “Jungle Love”, and ""Ever Fallen in Love (With Someone You Shouldn’t’ve)"" on any electric guitar or bass. Each song includes a new authentic tone.";

		private const string MultiPackNoArtistDescParsing =
			@"""Who Do You Love?"", ""Jungle Love"", and ""Ever Fallen in Love (With Someone You Shouldn’t’ve)""";

		private const string BigSingleArtistPackDesc =
			@"Play ""Folsom Prison Blues"", “Jackson”, “Big River”, “Hey Porter”, and ""Give My Love to Rose"" by Johnny Cash on any electric guitar or bass.Each song includes a new authentic tone.";

		private const string BigSingleArtistPackDescParsing = @"""Folsom Prison Blues"", ""Jackson"", ""Big River"", ""Hey Porter"", and ""Give My Love to Rose"" by Johnny Cash";

		private const string MultiPackWithArtistsDesc =
			@"Play ""I Fought the Law” by The Clash, ""Emerald” by Thin Lizzy, and “Little Green Bag” by George Baker Selection on any electric guitar or bass. Each song includes a new authentic tone.";

		private const string MultiPackWithArtistsDescParsing =
			@"""I Fought the Law"" by The Clash, ""Emerald"" by Thin Lizzy, and ""Little Green Bag"" by George Baker Selection";

		private const string BadlyFormattedDesc = @"Play "" on any electric guitar or bass. Each song includes a new authentic tone.";

		[Test]
		public void IfInputMatchesSingleArtist3PackThenOutputIsCorrect()
		{
			var sut = new SteamSongPackParser();
			var result = sut.ParseSteamString(SingleArtist3PackDesc);

			result.Parsing.ShouldBe(SingleArtist3PackDescParsing);
			result.SongsFound.Count.ShouldBe(3);

			result.SongsFound[0].Song.ShouldBe("Remedy");
			result.SongsFound[0].Artist.ShouldBe("Seether");

			result.SongsFound[1].Song.ShouldBe("Fake It");
			result.SongsFound[1].Artist.ShouldBe("Seether");

			result.SongsFound[2].Song.ShouldBe("Broken");
			result.SongsFound[2].Artist.ShouldBe("Seether");
		}

		[Test]
		public void IfInputMatchesMultiPackNoArtistThenParsingSubstringIsCorrect()
		{
			var sut = new SteamSongPackParser();
			var result = sut.ParseSteamString(MultiPackNoArtistDesc);

			result.Parsing.ShouldBe(MultiPackNoArtistDescParsing);
			result.SongsFound.Count.ShouldBe(3);

			result.SongsFound[0].Song.ShouldBe("Who Do You Love?");
			result.SongsFound[0].Artist.ShouldBeEmpty();

			result.SongsFound[1].Song.ShouldBe("Jungle Love");
			result.SongsFound[1].Artist.ShouldBeEmpty();

			result.SongsFound[2].Song.ShouldBe("Ever Fallen in Love (With Someone You Shouldn’t’ve)");
			result.SongsFound[2].Artist.ShouldBeEmpty();

		}

		[Test]
		public void IfInputMatchesBigSingleArtistPackThenParsingSubstringIsCorrect()
		{
			var sut = new SteamSongPackParser();
			var result = sut.ParseSteamString(BigSingleArtistPackDesc);

			result.Parsing.ShouldBe(BigSingleArtistPackDescParsing);
			result.SongsFound.Count.ShouldBe(5);

			result.SongsFound[0].Song.ShouldBe("Folsom Prison Blues");
			result.SongsFound[0].Artist.ShouldBe("Johnny Cash");

			result.SongsFound[1].Song.ShouldBe("Jackson");
			result.SongsFound[1].Artist.ShouldBe("Johnny Cash");

			result.SongsFound[2].Song.ShouldBe("Big River");
			result.SongsFound[2].Artist.ShouldBe("Johnny Cash");

			result.SongsFound[3].Song.ShouldBe("Hey Porter");
			result.SongsFound[3].Artist.ShouldBe("Johnny Cash");

			result.SongsFound[4].Song.ShouldBe("Give My Love to Rose");
			result.SongsFound[4].Artist.ShouldBe("Johnny Cash");
		}

		[Test]
		public void IfInputMatchesMultiPackWithArtistsThenParsingSubstringIsCorrect()
		{
			var sut = new SteamSongPackParser();
			var result = sut.ParseSteamString(MultiPackWithArtistsDesc);

			result.Parsing.ShouldBe(MultiPackWithArtistsDescParsing);
			//I Fought the Law"" by The Clash, ""Emerald"" by Thin Lizzy, and ""Little Green Bag"" by George Baker Selection
			result.SongsFound.Count.ShouldBe(3);

			result.SongsFound[0].Song.ShouldBe("I Fought the Law");
			result.SongsFound[0].Artist.ShouldBe("The Clash");

			result.SongsFound[1].Song.ShouldBe("Emerald");
			result.SongsFound[1].Artist.ShouldBe("Thin Lizzy");

			result.SongsFound[2].Song.ShouldBe("Little Green Bag");
			result.SongsFound[2].Artist.ShouldBe("George Baker Selection");
		}

		[Test]
		public void IfInputDoesNotMatchThenParsingSubstringIsEmpty()
		{
			var sut = new SteamSongPackParser();
			var result = sut.ParseSteamString("Play This is a test string “Little Green Bag” by George Baker Selection");

			result.Parsing.ShouldBe(string.Empty);
		}

		[Test]
		public void IfBadPartialMatchThenNoExceptions()
		{
			var sut = new SteamSongPackParser();
			var result = sut.ParseSteamString(BadlyFormattedDesc);
			result.Parsing.ShouldBe("\"");
			result.SongsFound.Count.ShouldBe(0);
		}

		[Test]
		public void IfInputContainsLotsOfDelimitersParsingSucceeds()
		{
			var sut = new SteamSongPackParser();
			var result = sut.ParseSteamString(@"Play ""do, da do, dah day"" by silly ben, and friends, and others, ""freedom"" by george michael, and ""benny and the jets"" by the one and only elton john on any electric guitar or bass. Each song includes a new authentic tone.");
			result.Parsing.ShouldBe(@"""do, da do, dah day"" by silly ben, and friends, and others, ""freedom"" by george michael, and ""benny and the jets"" by the one and only elton john");
			result.SongsFound.Count.ShouldBe(3);

			result.SongsFound[0].Song.ShouldBe("do, da do, dah day");
			result.SongsFound[0].Artist.ShouldBe("silly ben, and friends, and others");

			result.SongsFound[1].Song.ShouldBe("freedom");
			result.SongsFound[1].Artist.ShouldBe("george michael");

			result.SongsFound[2].Song.ShouldBe("benny and the jets");
			result.SongsFound[2].Artist.ShouldBe("the one and only elton john");
		}
	}
}