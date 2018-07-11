using NUnit.Framework;
using Shouldly;

namespace DlcToolLib.UnitTests
{
	[TestFixture]
	public class TestStringCleaner
	{
		[Test]
		public void WhenStringStartsWithTheItIsRemoved()
		{
			var stringCleaner = new StringCleaner(false, false, false, false);
			stringCleaner.Clean("the who").ShouldBe("who");
			stringCleaner.Clean("thewho").ShouldBe("thewho");
			stringCleaner.Clean("The Who").ShouldBe("Who");
		}

		[Test]
		public void IfUsingCleanPunctuationThenPlainPunctuationRemains()
		{
			var stringCleaner = new StringCleaner(true, false, false, false);
			var str1 = "this.\"has\".-punctuation()";
			stringCleaner.Clean(str1).ShouldBe(str1);
			stringCleaner.Clean("no punctuation in this one").ShouldBe("no punctuation in this one");
		}

		[Test]
		public void IfUsingCleanPunctuationThenCheckForCleaning()
		{
			var stringCleaner = new StringCleaner(true, false, false, false);
			stringCleaner.Clean("apostrophe’s clean").ShouldBe("apostrophe's clean");
			stringCleaner.Clean("“Quote Tidied").ShouldBe("\"Quote Tidied");
			stringCleaner.Clean("Quote Tidied”").ShouldBe("Quote Tidied\"");
			stringCleaner.Clean("em–dash").ShouldBe("em-dash");
			stringCleaner.Clean("benny & the jetts").ShouldBe("benny and the jetts");
		}

		[Test]
		public void IfUsingCleanUnicodeThenPlainTextRemains()
		{
			var stringCleaner = new StringCleaner(false, true, false, false);
			stringCleaner.Clean("this.\"has\".-punctuation()").ShouldBe("this.\"has\".-punctuation()");
			stringCleaner.Clean("no punctuation in this one").ShouldBe("no punctuation in this one");
		}

		[Test]
		public void IfUsingCleanUnicodeThenExtendedBecomePlain()
		{
			var stringCleaner = new StringCleaner(false, true, false, false);
			stringCleaner.Clean("this ùŠ").ShouldBe("this uS");
			stringCleaner.Clean("Mötley Crüe").ShouldBe("Motley Crue");
			stringCleaner.Clean("Mötley!.Crüe").ShouldBe("Motley!.Crue");
		}

		[Test]
		public void WhenStrippingArtistThenCaseChanges()
		{
			var stringCleaner = new StringCleaner(false, false, true, false);
			stringCleaner.MakeArtistSortName("Testing").ShouldBe("testing");
			stringCleaner.MakeArtistSortName("test iNg").ShouldBe("testing");
			stringCleaner.MakeArtistSortName("!strip.this please").ShouldBe("stripthisplease");
		}

		[Test]
		public void WhenStrippingSongThenCaseChanges()
		{
			var stringCleaner = new StringCleaner(false, false, false, true);
			stringCleaner.MakeSongSortName("Testing").ShouldBe("testing");
			stringCleaner.MakeSongSortName("test iNg").ShouldBe("testing");
			stringCleaner.MakeSongSortName("!strip.this please").ShouldBe("stripthisplease");
		}
	}
}