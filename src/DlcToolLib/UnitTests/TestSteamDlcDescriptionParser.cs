using NUnit.Framework;
using NUnit.Framework.Internal;
using Shouldly;

namespace DlcToolLib.UnitTests
{
	[TestFixture]
	public class TestSteamDlcDescriptionParser
	{
		[Test]
		public void IfRocksmithSingleSongThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith - Pantera - Cowboys From Hell");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBeEmpty();
			result.Artist.ShouldBe("Pantera");
			result.SongName.ShouldBe("Cowboys From Hell");
		}

		[Test]
		public void IfRocksmithSingleSongArtistWithSpacesThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith - Fall Out Boy - Dance, Dance");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBeEmpty();
			result.Artist.ShouldBe("Fall Out Boy");
			result.SongName.ShouldBe("Dance, Dance");
		}

		[Test]
		public void IfRocksmithSingleSongWithPunctuationThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith - Blue Oyster Cult - Burnin' for You");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBeEmpty();
			result.Artist.ShouldBe("Blue Oyster Cult");
			result.SongName.ShouldBe("Burnin' for You");
		}

		[Test]
		public void IfRocksmithSingleSongArtistWithPunctuationThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith - blink-182 - What's My Age Again?");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBeEmpty();
			result.Artist.ShouldBe("blink-182");
			result.SongName.ShouldBe("What's My Age Again?");
		}

		[Test]
		public void IfRocksmithSongPackThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith - Allman Brothers Band Song Pack");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBe("Allman Brothers Band Song Pack");
			result.Artist.ShouldBeEmpty();
			result.SongName.ShouldBeEmpty();
		}

		[Test]
		public void IfRocksmithSongPackFormattedLikeSongThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith - Nickelback - 3 Song Pack");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBe("Nickelback - 3 Song Pack");
			result.Artist.ShouldBeEmpty();
			result.SongName.ShouldBeEmpty();
		}

		[Test]
		public void IfRocksmithSongPackFormattedLikeSongAndHyphenThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith - blink-182 - 3-Song Pack");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBe("blink-182 - 3-Song Pack");
			result.Artist.ShouldBeEmpty();
			result.SongName.ShouldBeEmpty();
		}

		[Test]
		public void IfRocksmith2014SingleThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith® 2014 – Dio - “Holy Diver”");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBeEmpty();
			result.Artist.ShouldBe("Dio");
			result.SongName.ShouldBe("Holy Diver");
		}

		[Test]
		public void IfRocksmith2014SingleComplexArtistThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith® 2014 – Tom Petty and the Heartbreakers - “Learning to Fly”");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBeEmpty();
			result.Artist.ShouldBe("Tom Petty and the Heartbreakers");
			result.SongName.ShouldBe("Learning to Fly");
		}

		[Test]
		public void IfRocksmith2014SingleComplexSongTitleThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith® 2014 – The White Stripes - “You Don’t Know What Love Is (You Just Do As You’re Told)”");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBeEmpty();
			result.Artist.ShouldBe("The White Stripes");
			result.SongName.ShouldBe("You Don’t Know What Love Is (You Just Do As You’re Told)");
		}

		[Test]
		public void IfRocksmith2014SongPackThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith® 2014 – Bullet For My Valentine Song Pack");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBe("Bullet For My Valentine Song Pack");
			result.Artist.ShouldBeEmpty();
			result.SongName.ShouldBeEmpty();
		}

		[Test]
		public void IfRocksmith2014EdSongPackThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith® 2014 Edition – Remastered – Stevie Ray Vaughan & Double Trouble Song Pack");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBe("Stevie Ray Vaughan & Double Trouble Song Pack");
			result.Artist.ShouldBeEmpty();
			result.SongName.ShouldBeEmpty();
		}

		[Test]
		public void IfRocksmith2014EdSingleComplexSongTitleThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith® 2014 Edition – Remastered – Yes - “I’ve Seen All Good People”");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBeEmpty();
			result.Artist.ShouldBe("Yes");
			result.SongName.ShouldBe("I’ve Seen All Good People");
		}

		[Test]
		public void IfRocksmith2014EdSingleComplexArtistThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith® 2014 Edition - Remastered – Panic! At The Disco - “Ballad of Mona Lisa”");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBeEmpty();
			result.Artist.ShouldBe("Panic! At The Disco");
			result.SongName.ShouldBe("Ballad of Mona Lisa");
		}

		[Test]
		public void IfRocksmith2014EdSingleThenParses()
		{
			var sut = new SteamDlcDescriptionParser();
			var result = sut.ParseDlcLinkText("Rocksmith® 2014 Edition - Remastered – Cinderella - “Nobody’s Fool”");
			result.ShouldNotBeNull();
			result.ParsedSuccessfully.ShouldBe(true);
			result.SongPack.ShouldBeEmpty();
			result.Artist.ShouldBe("Cinderella");
			result.SongName.ShouldBe("Nobody’s Fool");
		}
	}
}